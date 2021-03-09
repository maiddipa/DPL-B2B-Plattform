using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Z.EntityFramework.Plus;
using Ltms = Dpl.B2b.Dal.Ltms;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class PostingAccountBalancesService : BaseService, IPostingAccountBalancesService
    {
        private readonly IRepository<Ltms.Bookings> _ltmsBookingsRepo;
        private readonly IRepository<Ltms.Accounts> _ltmsAccountsRepo;
        private readonly IRepository<Olma.PostingAccount> _olmaPostingAccountRepo;
        private readonly IRepository<Olma.PostingRequest> _olmaPostingRequestRepo;
        private readonly ILtmsReferenceLookupService _ltmsReferenceLookupService;
        private readonly IServiceProvider _serviceProvider;
        public readonly IOptions<CacheOptions> _cacheOptions;

        public PostingAccountBalancesService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Ltms.Bookings> ltmsBookingsRepo,
            IRepository<Ltms.Accounts> ltmsAccountsRepo,
            IRepository<Olma.PostingAccount> olmaPostingAccountRepo,
            IRepository<Olma.PostingRequest> olmaPostingRequestRepo,
            ILtmsReferenceLookupService ltmsReferenceLookupService,
            IServiceProvider serviceProvider,
            IOptions<CacheOptions> cacheOptions
        ) : base(authData, mapper)
        {
            //_olmaPostingRequestRepo = olmaPostingRequestRepo;
            _ltmsBookingsRepo = ltmsBookingsRepo;
            _ltmsAccountsRepo = ltmsAccountsRepo;
            _olmaPostingAccountRepo = olmaPostingAccountRepo;
            _olmaPostingRequestRepo = olmaPostingRequestRepo;
            _ltmsReferenceLookupService = ltmsReferenceLookupService;
            _serviceProvider = serviceProvider;
            _cacheOptions = cacheOptions;
        }

        public async Task<IWrappedResponse> Search(BalancesSearchRequest request)
        {
            var postingAccount = await _olmaPostingAccountRepo.FindAll()
                .Where(p => p.Id == request.PostingAccountId)
                .AsNoTracking().SingleOrDefaultAsync();
            
            if (postingAccount == null)
            {
                return BadRequest<BalancesSummary>(
                    ErrorHandler.Create()
                    .AddMessage(new PostingAccountNotFound())
                    .GetServiceState());
            }

            var queryCacheTag = $"{request}{request.PostingAccountId}-{request.RefLtmsArticleId}";

            if (request.ForceBalanceCalculation)
            {
                QueryCacheManager.ExpireTag(queryCacheTag);
            }
            
            var queryCacheOptions = _cacheOptions.Value.QueryCache;

            var query = _ltmsBookingsRepo.FindAll()
                .Where(b => b.AccountId == postingAccount.RefLtmsAccountId 
                            && b.ArticleId == request.RefLtmsArticleId)
                .AsNoTracking();
           
            var qualityBalancesRequired = false;
            
            var ltmsAccount = await _ltmsAccountsRepo.FindByCondition(a => a.Id == postingAccount.RefLtmsAccountId)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (ltmsAccount.AccountTypeId == "DEP"
                || ltmsAccount.AccountTypeId == "LAG"
                || ltmsAccount.AccountTypeId == "LDL")
            {
                qualityBalancesRequired = true;
            }

            var balancesResult = await query.GroupBy(b => b.Quality.Name)
                .Select(g => new {
                g.Key,
                Balances = new
                {
                    CoordinatedBalance = g.Sum(b => b.Matched ? b.Quantity : 0),
                    ProvisionalCharge = qualityBalancesRequired ?
                        g.Sum(b => !b.Matched && !b.IncludeInBalance && b.Quantity > 0 ? b.Quantity : 0) : 0,
                    ProvisionalCredit = qualityBalancesRequired ?
                        g.Sum(b =>  !b.Matched && !b.IncludeInBalance && b.Quantity < 0 ? b.Quantity : 0) : 0,
                    UncoordinatedCharge = qualityBalancesRequired ?
                        g.Sum(b =>  !b.Matched && b.IncludeInBalance && b.Quantity > 0 ? b.Quantity : 0) : 0,
                    UncoordinatedCredit = qualityBalancesRequired ?
                        g.Sum(b =>  !b.Matched && b.IncludeInBalance && b.Quantity < 0 ? b.Quantity : 0) : 0,
                    ProvisionalChargeFilterd =
                        g.Sum(b => b.BookingTypeId != "POOL" && !b.Matched && !b.IncludeInBalance && b.Quantity > 0 ? b.Quantity : 0),
                    ProvisionalCreditFilterd =
                        g.Sum(b => b.BookingTypeId != "POOL" && !b.Matched && !b.IncludeInBalance && b.Quantity < 0 ? b.Quantity : 0),
                    UncoordinatedChargeFilterd =
                        g.Sum(b => b.BookingTypeId != "POOL" && !b.Matched && b.IncludeInBalance && b.Quantity > 0 ? b.Quantity : 0),
                    UncoordinatedCreditFilterd =
                        g.Sum(b => b.BookingTypeId != "POOL" && !b.Matched && b.IncludeInBalance && b.Quantity < 0 ? b.Quantity : 0)
                }
            }).FromCacheAsync(new MemoryCacheEntryOptions
            {
                AbsoluteExpiration =
                    DateTimeOffset.UtcNow.AddMinutes(queryCacheOptions.AbsoluteExpirationInMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(queryCacheOptions.SlidingExpirationInMinutes)
            }, queryCacheTag);

            var balanceSummary = new BalancesSummary
            {
                IntactBalance = new BalanceOverview(),
                DefectBalance = new BalanceOverview()
            };

            if (qualityBalancesRequired)
            {
                balanceSummary.Balances = balancesResult.Where(br => br.Key != "D" && br.Key != "Schr").Select(br =>
                    new BalanceOverview
                    {
                        Name = br.Key,
                        CoordinatedBalance = br.Balances.CoordinatedBalance.Value,
                        ProvisionalCharge = br.Balances.ProvisionalCharge.Value,
                        ProvisionalCredit = br.Balances.ProvisionalCredit.Value,
                        UncoordinatedCharge = br.Balances.UncoordinatedCharge.Value,
                        UncoordinatedCredit = br.Balances.UncoordinatedCredit.Value
                    });
            }

            foreach (var balance in balancesResult)
            {
                if (balance.Key != "D" && balance.Key != "Schr")
                {
                    balanceSummary.IntactBalance.CoordinatedBalance += balance.Balances.CoordinatedBalance.Value;
                    balanceSummary.IntactBalance.ProvisionalCharge += balance.Balances.ProvisionalChargeFilterd.Value;
                    balanceSummary.IntactBalance.ProvisionalCredit += balance.Balances.ProvisionalCreditFilterd.Value;
                    balanceSummary.IntactBalance.UncoordinatedCharge += balance.Balances.UncoordinatedChargeFilterd.Value;
                    balanceSummary.IntactBalance.UncoordinatedCredit += balance.Balances.UncoordinatedCreditFilterd.Value;
                }
                else
                {
                    balanceSummary.DefectBalance.CoordinatedBalance += balance.Balances.CoordinatedBalance.Value;
                    balanceSummary.DefectBalance.ProvisionalCharge += balance.Balances.ProvisionalChargeFilterd.Value;
                    balanceSummary.DefectBalance.ProvisionalCredit += balance.Balances.ProvisionalCreditFilterd.Value;
                    balanceSummary.DefectBalance.UncoordinatedCharge += balance.Balances.UncoordinatedChargeFilterd.Value;
                    balanceSummary.DefectBalance.UncoordinatedCredit += balance.Balances.UncoordinatedCreditFilterd.Value;
                }
            }

            var postingRequestsQuery = _olmaPostingRequestRepo.FindAll()
                .Where(pr => pr.PostingAccountId == postingAccount.Id
                             && pr.Status == PostingRequestStatus.Pending
                             && pr.LoadCarrier.TypeId == request.LoadCarrierTypeId).AsNoTracking();

            var postingRequestBalanceCredit = postingRequestsQuery.DeferredSum(i =>
                i.Type == PostingRequestType.Credit ? i.LoadCarrierQuantity : 0).FutureValue();
            var postingRequestBalanceCharge = postingRequestsQuery.DeferredSum(i =>
                i.Type == PostingRequestType.Charge ? i.LoadCarrierQuantity : 0).FutureValue();

            balanceSummary.PostingRequestBalance = new BalanceOverview
            {
                ProvisionalCredit = postingRequestBalanceCredit.Value,
                ProvisionalCharge = postingRequestBalanceCharge.Value
            };

            return Ok(balanceSummary);
        }
    }
}