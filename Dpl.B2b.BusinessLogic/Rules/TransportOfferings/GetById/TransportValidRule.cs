using System;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Extensions;
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
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.TransportOfferings.GetById
{
    public class TransportValidRule : BaseValidationWithServiceProviderRule<TransportValidRule, TransportValidRule.ContextModel>
    {
        public TransportValidRule(MainRule.ContextModel request, IRule parentRule = null)
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

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            var repository = ServiceProvider.GetService<IRepository<Olma.Transport>>();
            Context.TransportOffering = repository.GetById<Olma.Transport, TransportOffering>(Context.TransportId, true).Data;
            
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
            public ContextModel(MainRule.ContextModel parent, TransportValidRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }
            
            public TransportOffering TransportOffering{ get; protected internal set; }
            public int TransportId => Parent.TransportId;
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                Required = new ValidOperatorRule<ContextModel>(context, model => model.TransportOffering != null);
            }
            
            public ValidOperatorRule<ContextModel> Required { get; protected internal set; }
        }

        #endregion
    }
}