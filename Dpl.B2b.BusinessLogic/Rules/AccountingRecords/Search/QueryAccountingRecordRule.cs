using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ltms = Dpl.B2b.Dal.Ltms;
using Olma = Dpl.B2b.Dal.Models;
using Dpl.B2b.Dal;

namespace Dpl.B2b.BusinessLogic.Rules.AccountingRecords.Search
{
    public class QueryAccountingRecordRule : BaseValidationWithServiceProviderRule<QueryAccountingRecordRule, QueryAccountingRecordRule.ContextModel>
    {
        public QueryAccountingRecordRule(MainRule.ContextModel request, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;

            ValidEvaluate += OnValidEvaluate;
        }

        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => new NotAllowedByRule();

        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            // No need to validate deeper
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();
            
            // Add all Message from ruleResult
            MergeFromResult(ruleResult);
            
            // Add Message if ruleResult is not success
            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
        }

        private void OnValidEvaluate(object sender, EvaluateInternalEventArgs evaluateInternalEventArgs)
        {
            var bookingsRepo = ServiceProvider.GetService<IRepository<Ltms.Bookings>>();

            var query = bookingsRepo.FindAll().Where(b =>
                !b.DeleteTime.HasValue && b.BookingTypeId != "POOL" && b.BookingTypeId != "STORNO").AsNoTracking();
            var mapper = ServiceProvider.GetService<IMapper>();

            var accountingRecords = query.ProjectTo<IEnumerable<AccountingRecord>>(mapper.ConfigurationProvider);
            
            var _context = ServiceProvider.GetService<Dal.OlmaDbContext>();

            var _ltmsReferenceLookupService = ServiceProvider.GetService<ILtmsReferenceLookupService>();
            var authData = ServiceProvider.GetService<IAuthorizationDataService>();
            AccountingRecordsSearchRequest request = Context.Parent.Parent;

            var isDplEmployee = authData.GetUserRole() == UserRole.DplEmployee;
            var postingAccountIds = authData.GetPostingAccountIds().ToArray();
            var accountIds = _ltmsReferenceLookupService.GetLtmsAccountIds(postingAccountIds).Select(i => i.Value);

            var ltmsPalletInfos = _ltmsReferenceLookupService
                .GetLtmsPalletInfos();

            // Using SQL is neccessary as efcore does not support full outer joins
            // We could move this into a view but right now this is the only place where it is needed
            IQueryable<LtmsAccoutingRecord> queryOld = _context.LtmsAccoutingRecords.FromSqlRaw(@"
SELECT
	CASE WHEN B.Id is not null THEN 'B' + CAST(B.Id AS VARCHAR(50)) ELSE 'P' + CAST(PR.Id AS VARCHAR(50)) END AS Id,
	B.Id as BookingId,
	PR.Id as PostingRequestId
FROM LTMS.Bookings B
FULL OUTER JOIN dbo.PostingRequests PR on B.RowGuid = PR.RowGuid and PR.Status <> 1
WHERE B.DeleteTime IS NULL AND B.BookingType_ID != 'POOL' AND B.BookingType_ID != 'STORNO' AND (B.Account_ID IN (" +
                        string.Join(",", accountIds.Select(n => n.ToString()).ToArray()) +
                        "))") //TODO: add isDplEmployee -statement
                    .Include(i => i.Booking).ThenInclude(i => i.Transaction).ThenInclude(i => i.Process)
                    .Include(i => i.Booking).ThenInclude(i => i.ReportBookings).ThenInclude(i => i.Report)
                    .Include(i => i.Booking).ThenInclude(i => i.OlmaPostingAccount)
                    .Include(i => i.PostingRequest).ThenInclude(i => i.DplNote)
                    .IgnoreQueryFilters()
                    //.Where(i => i.Booking == null || (i.Booking.DeleteTime == null &&( isDplEmployee || accountIds.Contains(i.Booking.AccountId))))
                    .Where(i => i.PostingRequest == null || (isDplEmployee ||
                                                             (postingAccountIds.Contains(i.PostingRequest
                                                                  .PostingAccountId) ||
                                                              accountIds.Contains(
                                                                  i.PostingRequest.SourceRefLtmsAccountId) ||
                                                              accountIds.Contains(i.PostingRequest
                                                                  .DestinationRefLtmsAccountId))))
                ;


            var queryCount = queryOld.Count();


            Context.AccountingRecordPaginationResult = null;
        }
        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel parent, QueryAccountingRecordRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }
            public IPaginationResult<AccountingRecord> AccountingRecordPaginationResult { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
            }
        }

        #endregion
    }
}