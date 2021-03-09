using System;
using System.Linq;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.TransportOfferings.AcceptTransport
{
    public class QueryTransportRule : BaseValidationWithServiceProviderRule<QueryTransportRule, QueryTransportRule.ContextModel>
    {
        public QueryTransportRule(MainRule.ContextModel context, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(context, this);
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

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();
            
            var transportRepo = ServiceProvider.GetService<IRepository<Olma.Transport>>();
            
            Context.Transport = transportRepo.FindAll()
                // TODO handle security seperately as we are disabling query filters to get both demand / supply order
                .IgnoreQueryFilters()
                .Include(i => i.WinningBid)
                .Include(i => i.OrderMatch)
                .Include(i => i.OrderMatch.Demand).ThenInclude(i => i.Detail)
                .Include(i => i.OrderMatch.Supply).ThenInclude(i => i.Detail)
                .SingleOrDefault(i => i.Id == Context.TransportId);

            // Assign rules to the Evaluator
            rulesEvaluator.Eval(Context.Rules.Required);
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel parent, QueryTransportRule rule) : base(parent, rule)
            {

            }

            public int TransportId => Parent.TransportId;

            public Olma.Transport Transport { get; protected internal set; }

            public RulesBundle Rules { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                Required = new ValidOperatorRule<ContextModel>(context, (c) => c.Transport != null, null, rule);
            }
            
            public ValidOperatorRule<ContextModel> Required { get; protected internal set; }
        }

        #endregion
    }
}