using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Olma = Dpl.B2b.Dal.Models;
using Ltms = Dpl.B2b.Dal.Ltms;
using Z.EntityFramework.Plus;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class PostingAccountsService : BaseService, IPostingAccountsService
    {
        private readonly ILtmsReferenceLookupService _ltmsLookup;
        private readonly IRepository<Olma.PostingAccount> _olmaPostingAccountRepo;
        private readonly IRepository<Ltms.Bookings> _ltmsBookingsRepo;
        private readonly IRepository<Ltms.Accounts> _ltmsAccountsRepo;
        private readonly IRepository<Olma.CalculatedBalance> _calculatedBalanceRepo;
        private readonly IRepository<Olma.CalculatedBalancePosition> _calculatedBalancePositionRepo;
        private readonly IServiceProvider _serviceProvider;

        public PostingAccountsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            ILtmsReferenceLookupService ltmsLookup,
            IRepository<Olma.PostingAccount> olmaPostingAccountRepo,
            IRepository<Ltms.Bookings> ltmsBookingsRepo,
            IRepository<Ltms.Accounts> ltmsAccountsRepo,
            IRepository<Olma.CalculatedBalance> calculatedBalanceRepo,
            IRepository<Olma.CalculatedBalancePosition> calculatedBalancePositionRepo,
            IServiceProvider serviceProvider
        ) : base(authData, mapper)
        {
            _ltmsLookup = ltmsLookup;
            _olmaPostingAccountRepo = olmaPostingAccountRepo;
            _ltmsBookingsRepo = ltmsBookingsRepo;
            _ltmsAccountsRepo = ltmsAccountsRepo;
            _calculatedBalanceRepo = calculatedBalanceRepo;
            _calculatedBalancePositionRepo = calculatedBalancePositionRepo;
            _serviceProvider = serviceProvider;
        }

        public async Task<IWrappedResponse> GetLtmsAccountsByCustomerNumber(string number)
        {
            var accounts = await _ltmsAccountsRepo.FindAll()
                .Where(a => a.CustomerNumber == number && !a.Inactive && !a.Locked)
                .IgnoreQueryFilters().ToListAsync();

            var ltmsAccount = Mapper.Map<List<LtmsAccount>>(accounts);

            return Ok(ltmsAccount.AsEnumerable());
        }

        public async Task<IWrappedResponse> GetById(int id)
        {
            return _olmaPostingAccountRepo.GetById<Olma.PostingAccount, PostingAccount>(id, true);
        }

        public async Task<IWrappedResponse> Create(PostingAccountsCreateRequest request)
        {
            var cmd = ServiceCommand<PostingAccountAdministration, Rules.PostingAccount.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.PostingAccount.Create.MainRule(request))
                .Then(CreateAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.PostingAccount.Create.MainRule mainRule)
        {
            var request = mainRule.Context.Request;

            var olmaPostingAccount = Mapper.Map<Olma.PostingAccount>(request);
            _olmaPostingAccountRepo.Create(olmaPostingAccount);
            _olmaPostingAccountRepo.Save();

            var response = _olmaPostingAccountRepo
                .GetById<Olma.PostingAccount, PostingAccountAdministration>(olmaPostingAccount.Id, true);

            return response;
        }

        public async Task<IWrappedResponse> Update(ValuesUpdateRequest request)
        {
            var cmd = ServiceCommand<PostingAccountAdministration, Rules.PostingAccount.Update.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.PostingAccount.Update.MainRule(request))
                .Then(UpdateAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> UpdateAction(Rules.PostingAccount.Update.MainRule mainRule)
        {
            var request = mainRule.Context.Request;
            var olmaPostingAccount = mainRule.Context.PostingAccount;

            JsonConvert.PopulateObject(request.Values, olmaPostingAccount);

            _olmaPostingAccountRepo.Save();

            var postingAccount =
                Mapper.Map<PostingAccountAdministration>(olmaPostingAccount);

            return Updated(postingAccount);
        }

        public async Task<IWrappedResponse> Delete(PostingAccountsDeleteRequest request)
        {
            var cmd = ServiceCommand<PostingAccountAdministration, Rules.PostingAccount.Delete.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.PostingAccount.Delete.MainRule(request))
                .Then(DeleteAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> DeleteAction(Rules.PostingAccount.Delete.MainRule mainRule)
        {
            var postingAccount = mainRule.Context.PostingAccount;
            _olmaPostingAccountRepo.Delete(postingAccount);
            _olmaPostingAccountRepo.Save();
            return Deleted<PostingAccountAdministration>(null);
        }

        public async Task<IWrappedResponse> GetByCustomerId(PostingAccountsSearchRequest request)
        {
            var query = _olmaPostingAccountRepo.FindAll()
                .IgnoreQueryFilters()
                .AsNoTracking().Where(d => !d.IsDeleted);

            var olmaPostingAccounts = await query.Where(d => d.CustomerId == request.CustomerId)
                .Include(a => a.Address).ToListAsync();

            return Ok(Mapper.Map<IEnumerable<PostingAccountAdministration>>(olmaPostingAccounts
                .AsEnumerable()));
        }

        public async Task<IWrappedResponse> Search(PostingAccountsSearchRequest request)
        {
            var query = _olmaPostingAccountRepo.FindAll()
                .IgnoreQueryFilters()
                .AsNoTracking().Where(d => !d.IsDeleted);

            if (request.PostingAccountId.HasValue)
            {
                var olmaPostingAccount = await query.Where(d => d.Id == request.PostingAccountId)
                    .Include(a => a.Address).SingleOrDefaultAsync();
                return Ok(Mapper.Map<PostingAccountAdministration>(olmaPostingAccount));
            }

            if (request.CustomerId.HasValue)
            {
                query = query.Where(d => d.CustomerId == request.CustomerId);
                return Ok(query.ProjectTo<PostingAccountAdministration>(Mapper.ConfigurationProvider));
            }

            return new WrappedResponse
            {
                ResultType = ResultType.BadRequest,
                State = ErrorHandler.Create().AddMessage(new GeneralError()).GetServiceState()
            };
        }
        
        public async Task<IWrappedResponse> GetAll()
        {
            var cmd = ServiceCommand<PostingAccount, Rules.PostingAccount.GetAll.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.PostingAccount.GetAll.MainRule(new PostingAccountsSearchRequest()))
                .Then(GetAllAction);

            return await cmd.Execute();
        }

        public async Task<IWrappedResponse> GetByCustomerId(int customerId)
        {
            var cmd = ServiceCommand<PostingAccount, Rules.PostingAccount.GetAll.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.PostingAccount.GetAll.MainRule(
                    new PostingAccountsSearchRequest {CustomerId = customerId}))
                .Then(GetAllAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> GetAllAction(Rules.PostingAccount.GetAll.MainRule rule)
        {
            var result = rule.Context.PostingAccountsResult.AsEnumerable();
            return Ok(result);
        }

        public async Task<IWrappedResponse> GetBalances(int id)
        {
            var cmd = ServiceCommand<Balance, Rules.PostingAccount.GetBalances.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.PostingAccount.GetBalances.MainRule(new PostingAccountsSearchRequest
                    {PostingAccountId = id}))
                .Then(GetBalancesAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> GetBalancesAction(Rules.PostingAccount.GetBalances.MainRule rule)
        {
            var result = rule.Context.Balances;
            return Ok(result);
        }

        public async Task<IWrappedResponse> GetAllowedDestinationAccounts()
        {
            var partnerAccount = _olmaPostingAccountRepo.FindAll().IgnoreQueryFilters().FirstOrDefault(a =>
                a.CustomerDivisions.Any(c => c.Customer.Organization.Name.Contains("Dpl")));

            var shipperAccount = _olmaPostingAccountRepo.FindAll().IgnoreQueryFilters().Where(a =>
                    a.CustomerDivisions.Any(d => d.Customer.Users.Select(u => u.User).All(u => u.Role == UserRole.Shipper)) && !a.IsDeleted)
                .FirstOrDefault(a => !AuthData.GetPostingAccountIds().Contains(a.Id) && a.Id != partnerAccount.Id);
            var reatailerAccount = _olmaPostingAccountRepo.FindAll().IgnoreQueryFilters().FirstOrDefault(a =>
                a.CustomerDivisions.Any(c => c.Customer.Organization.Name.Contains("Lieferant")));

            //HACK Hardcoded list of allowed destination accounts
            var destinationAccounts = new List<AllowedPostingAccount>();
            if (reatailerAccount != null)
                destinationAccounts.Add(new AllowedPostingAccount()
                {
                    PostingAccountId = reatailerAccount.Id, DisplayName = $"Konto Lieferant"
                }); // [Id: {reatailerAccount.Id}]"});
            if (shipperAccount != null)
                destinationAccounts.Add(new AllowedPostingAccount()
                    {PostingAccountId = shipperAccount.Id, DisplayName = $"Konto Spedition"});
            if (partnerAccount != null)
                destinationAccounts.Add(new AllowedPostingAccount()
                    {PostingAccountId = partnerAccount.Id, DisplayName = $"Konto Partner"});

            return Ok(destinationAccounts.AsEnumerable());
        }

        public async Task GenerateCalculatedBalances()
        {
            var balances = (await GetPostingAccountBalances(postingAccounts: null, useCalculatedBalance: false))
                .ToList();

            var calculatedBalancesDict = _calculatedBalanceRepo.FindAll()
                .IgnoreQueryFilters()
                .Include(i => i.Positions)
                .ToDictionary(i => i.PostingAccountId);

            foreach (var g in balances.GroupBy(i => i.PostingAccountId))
            {
                Olma.CalculatedBalance calculatedBalance;
                if (calculatedBalancesDict.ContainsKey(g.Key))
                {
                    calculatedBalance = calculatedBalancesDict[g.Key];
                }
                else
                {
                    calculatedBalance = new Olma.CalculatedBalance()
                        {PostingAccountId = g.Key, Positions = new List<Olma.CalculatedBalancePosition>()};
                    _calculatedBalanceRepo.Create(calculatedBalance);
                }

                calculatedBalance.LastBookingDateTime = g.Max(i => i.LastBookingDateTime);

                var positionsDict = calculatedBalance.Positions
                    .ToDictionary(i => i.LoadCarrierId);

                foreach (var p in g)
                {
                    Olma.CalculatedBalancePosition position;
                    if (positionsDict.ContainsKey(p.LoadCarrierId))
                    {
                        position = positionsDict[p.LoadCarrierId];
                    }
                    else
                    {
                        position = new Olma.CalculatedBalancePosition() {LoadCarrierId = p.LoadCarrierId};
                        _calculatedBalancePositionRepo.Create(position);
                    }

                    var credit = new[]
                    {
                        p.CoordinatedBalance,
                        p.ProvisionalCredit,
                        p.InCoordinationCredit,
                        p.UncoordinatedCredit,
                        p.PostingRequestBalanceCredit
                    }.Sum();

                    var charge = new[]
                    {
                        p.ProvisionalCharge,
                        p.InCoordinationCharge,
                        p.UncoordinatedCharge,
                        p.PostingRequestBalanceCharge
                    }.Sum();

                    position.CoordinatedBalance = p.CoordinatedBalance;
                    position.ProvisionalCharge = p.ProvisionalCharge;
                    position.ProvisionalCredit = p.ProvisionalCredit;
                    position.InCoordinationCharge = p.InCoordinationCharge;
                    position.InCoordinationCredit = p.InCoordinationCredit;
                    position.UncoordinatedCharge = p.UncoordinatedCharge;
                    position.UncoordinatedCredit = p.UncoordinatedCredit;
                    position.LatestUncoordinatedCharge = p.LatestUncoordinatedCharge;
                    position.LatestUncoordinatedCredit = p.LatestUncoordinatedCredit;
                    position.ProvisionalBalance = credit - charge;
                }
            }

            _calculatedBalanceRepo.Save();
        }

        #region Balance Helper methods

        private async Task<IEnumerable<PostingAccountBalance>> GetPostingAccountBalances(
            IList<Olma.PostingAccount> postingAccounts = null, bool useCalculatedBalance = true)
        {
            IQueryable<Ltms.Bookings> ltmsBookingsQuery = null;
            if (postingAccounts != null)
            {
                if (postingAccounts.Count == 0)
                {
                    return new List<PostingAccountBalance>();
                }

                // build a union clause with all posting accounts and potential date limiters
                foreach (var postingAccount in postingAccounts)
                {
                    IQueryable<Ltms.Bookings> subQuery;
                    if (useCalculatedBalance && postingAccount.CalculatedBalance == null)
                    {
                        subQuery = _ltmsBookingsRepo.FindAll()
                            .AsNoTracking()
                            .Where(i => i.Quantity.HasValue)
                            .Where(i => i.AccountId == postingAccount.RefLtmsAccountId);

                    }
                    else
                    {
                        subQuery = _ltmsBookingsRepo.FindAll()
                            .AsNoTracking()
                            .Where(i => i.Quantity.HasValue)
                            // TODO discuss if created field is the correct field to filter on
                            .Where(i => i.AccountId == postingAccount.RefLtmsAccountId &&
                                        i.CreateTime > postingAccount.CalculatedBalance.LastBookingDateTime);
                    }

                    // TODO discuss if we need to filter on bookings type
                    //subQuery = subQuery.Where(i => i.BookingTypeId == 0)

                    ltmsBookingsQuery =
                        ltmsBookingsQuery == null ? subQuery : ltmsBookingsQuery.Union(subQuery);
                }
            }
            else
            {
                ltmsBookingsQuery = _ltmsBookingsRepo.FindAll()
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .Where(i => i.OlmaPostingAccount != null);
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
                    i.balance.QualityId,
                })
                .Select(g => new
                {
                    g.Key,
                    Balances = new
                    {
                        LastBookingDateTime = g.Max(i => i.balance.BookingDate),
                        CoordinatedBalance = g.Sum(i => i.balance.Matched ? i.balance.Quantity.Value : 0),
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
                                : 0),
                    }
                })
                // HACK Balances from Bookings are cached after first request
                .FromCache()
                .ToList();

            var ltmsAccountIds = dbResult.Select(i => i.Key.AccountId).Distinct().ToArray();
            var accountMap = _ltmsLookup.GetOlmaPostingAccountIds(ltmsAccountIds);

            var ltmsArticleQualityIds = dbResult.Select(i => Tuple.Create(i.Key.ArticleId, i.Key.QualityId)).Distinct()
                .ToArray();
            var loadCarrierMap = _ltmsLookup.GetOlmaLoadCarrierIds(ltmsArticleQualityIds);


            // convert db result to dictionary
            var postingAccountBalances = dbResult
                // HACK Fix mappings Tuple(6,12)
                .Where(i => loadCarrierMap.ContainsKey(Tuple.Create(i.Key.ArticleId, i.Key.QualityId)))
                .Select(i => new PostingAccountBalance()
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
                    UncoordinatedCredit = i.Balances.UncoordinatedCredit,
                });

            return postingAccountBalances;
        }

        #endregion
    }
}
