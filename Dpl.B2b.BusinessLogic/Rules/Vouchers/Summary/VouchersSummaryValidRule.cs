using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Vouchers.Summary
{
    public class VouchersSummaryValidRule : BaseValidationWithServiceProviderRule<VouchersSummaryValidRule, VouchersSummaryValidRule.ContextModel>
    {
        public VouchersSummaryValidRule(MainRule.ContextModel request, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;
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
            Context.DbContext = ServiceProvider.GetService<Dal.OlmaDbContext>();
            
            
            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            // Needs no Validation
            

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            Context.Result = BuildResult();
            
            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        private IEnumerable<VoucherSummary> BuildResult()
        {
            var query = Context.Parent.Rules.VouchersSearchQueryRule.Context.VoucherSearchQuery;
            
            var typeOrderDict = Context.DbContext.LoadCarrierTypes
                .Select(i => new { i.Id, i.Order })
                .ToDictionary(i => i.Id, i => i.Order);

            var summaryByLoadCarrierAndStatusLookup =

            query.SelectMany(i => i.Positions).GroupBy(i => new { i.LoadCarrier.TypeId, i.Voucher.Status })
            .Select(g => new
            {
                g.Key,
                Sum = g.Sum(p => p.LoadCarrierQuantity),
                // TODO Performance optimization - check if splitting counting into seperate call would be more efficient
                Count = query.Where(i => i.Status == g.Key.Status && i.Positions.Any(p => p.LoadCarrier.TypeId == g.Key.TypeId)).Count(),
            })
            .ToLookup(i => i.Key.TypeId);

            var result = summaryByLoadCarrierAndStatusLookup.Select(g => new VoucherSummary()
            {
                LoadCarrierTypeId = g.Key,
                Sum = new VoucherAggregate()
                {
                    Issued = g.Where(i => i.Key.Status == VoucherStatus.Issued).Select(i => i.Sum).SingleOrDefault(),
                    Canceled = g.Where(i => i.Key.Status == VoucherStatus.Canceled).Select(i => i.Sum).SingleOrDefault(),
                    Submitted = g.Where(i => i.Key.Status == VoucherStatus.Submitted).Select(i => i.Sum).SingleOrDefault(),
                    Accounted = g.Where(i => i.Key.Status == VoucherStatus.Accounted).Select(i => i.Sum).SingleOrDefault(),
                    Expired = g.Where(i => i.Key.Status == VoucherStatus.Expired).Select(i => i.Sum).SingleOrDefault(),
                },
                Count = new VoucherAggregate()
                {
                    Issued = g.Where(i => i.Key.Status == VoucherStatus.Issued).Select(i => i.Count).SingleOrDefault(),
                    Canceled = g.Where(i => i.Key.Status == VoucherStatus.Canceled).Select(i => i.Count).SingleOrDefault(),
                    Submitted = g.Where(i => i.Key.Status == VoucherStatus.Submitted).Select(i => i.Count).SingleOrDefault(),
                    Accounted = g.Where(i => i.Key.Status == VoucherStatus.Accounted).Select(i => i.Count).SingleOrDefault(),
                    Expired = g.Where(i => i.Key.Status == VoucherStatus.Expired).Select(i => i.Count).SingleOrDefault(),
                },
                Order = typeOrderDict[g.Key]
            }).OrderBy(vs => vs.Order).AsEnumerable();

            return result;
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel parent, IRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }
            
            public OlmaDbContext DbContext { get; protected internal set; }
            public IEnumerable<VoucherSummary> Result { get; set; }
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