using System;
using System.Linq;
using AutoMapper;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ltms = Dpl.B2b.Dal.Ltms;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.AccountingRecords.GetById
{
    public class QueryAccountingRecordRule : BaseValidationWithServiceProviderRule<QueryAccountingRecordRule, QueryAccountingRecordRule.ContextModel>
    {
        public QueryAccountingRecordRule(MainRule.ContextModel context, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(context, this);
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
            
            MergeFromResult(ruleResult);
        }

        // Main BL 
        private void OnValidEvaluate(object sender, EvaluateInternalEventArgs evaluateInternalEventArgs)
        {
            var ltmsBookingsRepo = ServiceProvider.GetService<IRepository<Ltms.Bookings>>();
            var ltmsReferenceLookupService = ServiceProvider.GetService<ILtmsReferenceLookupService>();

            var booking= ltmsBookingsRepo.FindAll()
                .Include(i => i.Transaction)
                .Include(i => i.ReportBookings).ThenInclude(r => r.Report)
                .AsNoTracking()
                .SingleOrDefault();
            
            if (booking == null)
            {
                throw new Exception("There is no Booking");
            }

            var mapper = ServiceProvider.GetService<IMapper>();
            
            var record = mapper.Map<AccountingRecord>(booking);

            #region Map Cross DB properties

            var accountMap = ltmsReferenceLookupService.GetOlmaPostingAccountIds(new[] { booking.AccountId });

            var articleAndQuality = Tuple.Create(booking.ArticleId, booking.QualityId);
            var loadCarrierMap = ltmsReferenceLookupService.GetOlmaLoadCarrierIds(new[] { articleAndQuality });

            record.PostingAccountId = accountMap[booking.AccountId];
            record.LoadCarrierId = loadCarrierMap[articleAndQuality];

            #endregion
            
            Context.AccountingRecord = record;
            
            AddMessage(Context.AccountingRecord == null, ResourceName, Message);
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

            public AccountingRecord AccountingRecord { get; protected internal set; }
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