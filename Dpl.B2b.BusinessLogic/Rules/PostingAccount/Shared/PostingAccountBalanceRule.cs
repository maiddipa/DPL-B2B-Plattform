using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.PostingAccount;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Contracts.Options;
using Dpl.B2b.Dal.Ltms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Z.EntityFramework.Plus;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.PostingAccount.Shared
{
    public class PostingAccountBalanceRule : BaseValidationWithServiceProviderRule<PostingAccountBalanceRule,
        PostingAccountBalanceRule.ContextModel>
    {
        public PostingAccountBalanceRule(PostingAccountsSearchRequest request, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;
        }

        /// <summary>
        ///     Message for RuleState if Rule is invalid
        /// </summary>
        protected override ILocalizableMessage Message => new PostingAccountBalanceError();

        /// <summary>
        ///     Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);
            Context.PostingRequestRepository = ServiceProvider.GetService<IRepository<Olma.PostingRequest>>();
            Context.BookingsRepository = ServiceProvider.GetService<IRepository<Bookings>>();
            Context.LtmsReferenceLookupService = ServiceProvider.GetService<ILtmsReferenceLookupService>();
            Context.Mapper = ServiceProvider.GetService<IMapper>();
            Context.CacheOptions = ServiceProvider.GetService<IOptions<CacheOptions>>();

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            rulesEvaluator.Eval(Context.Rules.PostingAccountValidRule);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);

            AddMessage(Context.Parent.PostingAccountId != null && !Context.PostingAccountsBalances.Any(), ResourceName,new PostingAccountBalanceNotFound());
            
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        ///     Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<PostingAccountsSearchRequest>
        {
            public ContextModel(PostingAccountsSearchRequest parent, IRule rule) : base(parent, rule)
            {
            }
            
            public RulesBundle Rules { get; protected internal set; }
            public IMapper Mapper { get; protected internal set; }
            public IRepository<Olma.PostingRequest> PostingRequestRepository { get; protected internal set; }
            public IRepository<Bookings> BookingsRepository { get; protected internal set; }
            public ILtmsReferenceLookupService LtmsReferenceLookupService { get; protected internal set; }
            
            public ILookup<int, Balance> PostingAccountsBalances => GetPostingAccountsBalances();
            public IOptions<CacheOptions> CacheOptions { get; protected internal set; }

            private ILookup<int, Balance> GetPostingAccountsBalances()
            {
                var postingAccounts = Rules.PostingAccountValidRule.Context.PostingAccounts.ToList();
                var olmaPostingAccount = Rules.PostingAccountValidRule.Context.OlmaPostingAccounts.ToList();

                // we need to perform the following steps
                // - retrieve calculatedBalances (olma), bookings balances (ltms)  + postingRequest balances without rows already in ltms (olma) 
                // - identify the unique combinations of posting account id + load carrier id
                // - calculate balances based on all three data source

                // execute live grouping against different DBs in parallel
                var getBookingsPositionsTask = GetBookingsPositionsDict(olmaPostingAccount);
                var getPostingRequestBalancesTask = GetPostingRequestBalancesDict(postingAccounts.Select(pa => pa.Id));
                Task.WaitAll(getBookingsPositionsTask, getPostingRequestBalancesTask);

                var calculatedBalancePositionsDict = GetCalculatedBalancesPositionsDict(olmaPostingAccount);
                var bookingsBalancePositionsDict = getBookingsPositionsTask.Result;

                // TODO filter out postingRequest balances that have already been processed
                var postingRequestBalancesDict = getPostingRequestBalancesTask.Result;

                // identifies the unique combinations of posting account id + load carrier id across all data sources
                var uniquePostingAccountLoadCarrierCombinations = calculatedBalancePositionsDict.Keys
                    .Union(bookingsBalancePositionsDict.Keys)
                    .Union(postingRequestBalancesDict.Keys)
                    .Distinct();

                var combinedBalancePositions = uniquePostingAccountLoadCarrierCombinations.Select(key =>
                {
                    var balance = calculatedBalancePositionsDict.ContainsKey(key)
                        ? calculatedBalancePositionsDict[key]
                        : new Balance();

                    if (bookingsBalancePositionsDict.ContainsKey(key))
                    {
                        var bookingsBalance = bookingsBalancePositionsDict[key];
                        balance.CoordinatedBalance += bookingsBalance.CoordinatedBalance;
                        balance.ProvisionalCharge += bookingsBalance.ProvisionalCharge;
                        balance.ProvisionalCredit += bookingsBalance.ProvisionalCredit;
                        balance.InCoordinationCharge += bookingsBalance.InCoordinationCharge;
                        balance.InCoordinationCredit += bookingsBalance.InCoordinationCredit;
                        balance.UncoordinatedCharge += bookingsBalance.UncoordinatedCharge;
                        balance.UncoordinatedCredit += bookingsBalance.UncoordinatedCredit;
                    }

                    if (postingRequestBalancesDict.ContainsKey(key))
                    {
                        var postingRequestBalance = postingRequestBalancesDict[key];
                        balance.PostingRequestBalanceCredit += postingRequestBalance.PostingRequestBalanceCredit;
                        balance.PostingRequestBalanceCharge += postingRequestBalance.PostingRequestBalanceCharge;
                    }

                    var credit = new[]
                    {
                        balance.CoordinatedBalance,
                        balance.ProvisionalCredit,
                        balance.InCoordinationCredit,
                        balance.UncoordinatedCredit,
                        balance.PostingRequestBalanceCredit
                    }.Sum();

                    var charge = new[]
                    {
                        balance.ProvisionalCharge,
                        balance.InCoordinationCharge,
                        balance.UncoordinatedCharge,
                        balance.PostingRequestBalanceCharge
                    }.Sum();

                    balance.ProvisionalBalance = credit - charge;
                    balance.AvailableBalance = 
                        balance.CoordinatedBalance 
                        - balance.ProvisionalCharge 
                        - balance.PostingRequestBalanceCharge 
                        + balance.InCoordinationCredit
                        - balance.InCoordinationCharge
                        + balance.UncoordinatedCredit
                        - balance.UncoordinatedCharge;

                    balance.LoadCarrierId = key.Item2;

                    return new
                    {
                        PostingAccountId = key.Item1,
                        Balance = balance
                    };
                });

                return combinedBalancePositions.ToLookup(i => i.PostingAccountId, i => i.Balance);
            }
            
            private async Task<IDictionary<Tuple<int, int>, PostingAccountBalance>> GetBookingsPositionsDict(
                List<Olma.PostingAccount> postingAccounts)
            {
                return (await GetPostingAccountBalances(postingAccounts))
                    .ToDictionary(i => Tuple.Create(i.PostingAccountId, i.LoadCarrierId));
            }

            private async Task<IDictionary<Tuple<int, int>, Balance>> GetPostingRequestBalancesDict(
                IEnumerable<int> postingAccountIdList)
            {
                var olmaPostingRequestQuery = PostingRequestRepository.FindAll()
                    .AsNoTracking()
                    .Where(i => postingAccountIdList.Contains(i.PostingAccountId))
                    .Where(i => i.Status == PostingRequestStatus.Pending);

                var postingRequestBalances = olmaPostingRequestQuery.GroupBy(r => new
                    {
                        r.PostingAccountId,
                        r.LoadCarrierId
                    })
                    .Select(g => new
                    {
                        g.Key,
                        Balances = new
                        {
                            // TODO discuss if splitting Credit/Charge is actually necessary for balances
                            BookingRequestBalanceCredit = g.Sum(i =>
                                i.Type == PostingRequestType.Credit ? i.LoadCarrierQuantity : 0),
                            BookingRequestBalanceCharge = g.Sum(i =>
                                i.Type == PostingRequestType.Charge ? i.LoadCarrierQuantity : 0)
                        }
                    })
                    .ToList()
                    .ToDictionary(i => new Tuple<int, int>(i.Key.PostingAccountId, i.Key.LoadCarrierId), i =>
                        new Balance
                        {
                            PostingRequestBalanceCharge = i.Balances.BookingRequestBalanceCharge,
                            PostingRequestBalanceCredit = i.Balances.BookingRequestBalanceCredit
                        });
                return postingRequestBalances;
            }

            private async Task<IEnumerable<PostingAccountBalance>> GetPostingAccountBalances(
                IList<Olma.PostingAccount> postingAccounts = null, bool useCalculatedBalance = true)
            {
                var queryCacheOptions = CacheOptions.Value.QueryCache;
                IQueryable<Bookings> ltmsBookingsQuery = null;
                if (postingAccounts != null)
                {
                    if (postingAccounts.Count == 0) return new List<PostingAccountBalance>();

                    // build a union clause with all posting accounts and potential date limiters
                    foreach (var postingAccount in postingAccounts)
                    {
                        IQueryable<Bookings> subQuery;
                        if (useCalculatedBalance && postingAccount.CalculatedBalance == null)
                            subQuery = BookingsRepository.FindAll()
                                .AsNoTracking()
                                .Where(i => i.Quantity.HasValue)
                                .Where(i => i.AccountId == postingAccount.RefLtmsAccountId);
                        else
                            subQuery = BookingsRepository.FindAll()
                                .AsNoTracking()
                                .Where(i => i.Quantity.HasValue)
                                // TODO discuss if created field is the correct field to filter on
                                .Where(i => i.AccountId == postingAccount.RefLtmsAccountId &&
                                            i.CreateTime > postingAccount.CalculatedBalance.LastBookingDateTime);

                        // TODO discuss if we need to filter on bookings type
                        //subQuery = subQuery.Where(i => i.BookingTypeId == 0)

                        ltmsBookingsQuery =
                            ltmsBookingsQuery == null ? subQuery : ltmsBookingsQuery.Union(subQuery);
                    }
                }
                else
                {
                    ltmsBookingsQuery = BookingsRepository.FindAll()
                        .AsNoTracking()
                        .IgnoreQueryFilters()
                        .Where(i => i.OlmaPostingAccount != null && i.DeleteTime == null);
                }

                var inCoordinationQuery = ltmsBookingsQuery
                    .Where(i => i.IncludeInBalance && !i.Matched && i.ReportBookings.Any(rb =>
                        rb.Report.DeleteTime == null && rb.Report.ReportStateId == "U"));


                // perfrom left outer join to add uncordinated info to data before grouping as other ef core will fail
                var joined = from balance in ltmsBookingsQuery
                    join inCoordination in inCoordinationQuery on balance.Id equals inCoordination.Id into
                        inCoordinationMatch
                    from u in inCoordinationMatch.DefaultIfEmpty()
                    select new {balance, isInCoordination = u != null};

                // now we need to group by
                var dbResult = joined
                    .GroupBy(i => new
                    {
                        i.balance.AccountId,
                        i.balance.ArticleId,
                        i.balance.QualityId
                    })
                    .Select(g => new
                    {
                        g.Key,
                        Balances = new
                        {
                            LastBookingDateTime = g.Max(i => i.balance.BookingDate),
                            CoordinatedBalance =
                                g.Sum(i => i.balance.Matched
                                    ? i.balance.Quantity.Value * -1
                                    : 0), //TODO: Workaround to fix the Balances, but with the wrong viewpoint
                            ProvisionalCharge = g.Sum(i =>
                                !i.balance.Matched && !i.isInCoordination && !i.balance.IncludeInBalance &&
                                i.balance.Quantity > 0
                                    ? i.balance.Quantity.Value
                                    : 0),
                            ProvisionalCredit = g.Sum(i =>
                                !i.balance.Matched && !i.isInCoordination && !i.balance.IncludeInBalance &&
                                i.balance.Quantity < 0
                                    ? i.balance.Quantity.Value * -1
                                    : 0),
                            InCoordinationCharge = g.Sum(i =>
                                !i.balance.Matched && i.isInCoordination && i.balance.IncludeInBalance &&
                                i.balance.Quantity > 0
                                    ? i.balance.Quantity.Value
                                    : 0),
                            InCoordinationCredit = g.Sum(i =>
                                !i.balance.Matched && i.isInCoordination && i.balance.IncludeInBalance &&
                                i.balance.Quantity < 0
                                    ? i.balance.Quantity.Value * -1
                                    : 0),
                            UncoordinatedCharge = g.Sum(i =>
                                !i.balance.Matched && !i.isInCoordination && i.balance.IncludeInBalance &&
                                i.balance.Quantity > 0
                                    ? i.balance.Quantity.Value
                                    : 0),
                            UncoordinatedCredit = g.Sum(i =>
                                !i.balance.Matched && !i.isInCoordination && i.balance.IncludeInBalance &&
                                i.balance.Quantity < 0
                                    ? i.balance.Quantity.Value * -1
                                    : 0)
                        }
                    })
                    .FromCache(new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration =
                            DateTimeOffset.UtcNow.AddMinutes(queryCacheOptions.AbsoluteExpirationInMinutes),
                        SlidingExpiration = TimeSpan.FromMinutes(queryCacheOptions.SlidingExpirationInMinutes)
                    })
                    .ToList();

                var ltmsAccountIds = dbResult.Select(i => i.Key.AccountId).Distinct().ToArray();
                var accountMap = LtmsReferenceLookupService.GetOlmaPostingAccountIds(ltmsAccountIds);

                var ltmsArticleQualityIds = dbResult.Select(i => Tuple.Create(i.Key.ArticleId, i.Key.QualityId))
                    .Distinct().ToArray();
                var loadCarrierMap = LtmsReferenceLookupService.GetOlmaLoadCarrierIds(ltmsArticleQualityIds);


                // convert db result to dictionary
                var postingAccountBalances = dbResult
                    // HACK Fix mappings Tuple(6,12)
                    .Where(i => loadCarrierMap.ContainsKey(Tuple.Create(i.Key.ArticleId, i.Key.QualityId)))
                    .Select(i => new PostingAccountBalance
                    {
                        PostingAccountId = accountMap[i.Key.AccountId],
                        LoadCarrierId = loadCarrierMap[Tuple.Create(i.Key.ArticleId, i.Key.QualityId)],
                        LastBookingDateTime = i.Balances.LastBookingDateTime,
                        CoordinatedBalance = i.Balances.CoordinatedBalance,
                        ProvisionalCharge = i.Balances.ProvisionalCharge,
                        ProvisionalCredit = i.Balances.ProvisionalCredit,
                        InCoordinationCharge = i.Balances.InCoordinationCharge,
                        InCoordinationCredit = i.Balances.InCoordinationCredit,
                        UncoordinatedCharge = i.Balances.UncoordinatedCharge,
                        UncoordinatedCredit = i.Balances.UncoordinatedCredit
                    });

                return postingAccountBalances;
            }

            private IDictionary<Tuple<int, int>, Balance> GetCalculatedBalancesPositionsDict(
                IList<Olma.PostingAccount> postingAccounts)
            {
                return postingAccounts
                    .Where(i => i.CalculatedBalance != null)
                    .SelectMany(i => i.CalculatedBalance.Positions.Select(p =>
                        new
                        {
                            PostingAccountId = i.Id,
                            p.LoadCarrierId,
                            Balance = p
                        })).ToDictionary(i => new Tuple<int, int>(i.PostingAccountId, i.LoadCarrierId),
                        i => Mapper.Map<Balance>(i.Balance));
            }
        }
        
        /// <summary>
        ///     Bundles of rules
        /// </summary>
        public class RulesBundle
        {
            public readonly PostingAccountValidRule PostingAccountValidRule;

            public RulesBundle(ContextModel context, IRule rule)
            {
                PostingAccountValidRule = new PostingAccountValidRule(context.Parent, rule);
            }
        }

        #endregion
    }
}