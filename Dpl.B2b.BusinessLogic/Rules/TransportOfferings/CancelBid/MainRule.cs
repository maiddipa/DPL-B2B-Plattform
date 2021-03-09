using System;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.TransportOfferings.CancelBid
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>, IRuleWithRessourceSkill
    {
        public MainRule((int transportId, int bidId) context, IRule parentRule = null)
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

            // Assign rules to the Evaluator
            rulesEvaluator
                .Eval(Context.Rules.QueryBid)
                .Eval(Context.Rules.Required)
                .Eval(Context.Rules.StateValid);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<(int transportId, int bidId)>
        {
            public ContextModel((int transportId, int bidId) parent, MainRule rule) : base(parent, rule)
            {

            }

            public Olma.TransportBid Bid => Rules.QueryBid.Context.Bid; 
            
            public RulesBundle Rules { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                QueryBid= new QueryBidRule(context, rule);
                Required = new ValidOperatorRule<ContextModel>(context, (c) => c.Bid != null, null, rule);
                StateValid= new StateValidRule(context, rule);
            }

            public readonly QueryBidRule QueryBid; 
            public readonly ValidOperatorRule<ContextModel> Required;
            public readonly StateValidRule StateValid;
        }

        #endregion
        
        public IValidationRule ResourceRule => Context.Rules.Required;
    }
}