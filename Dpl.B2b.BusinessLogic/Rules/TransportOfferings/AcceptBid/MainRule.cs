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

namespace Dpl.B2b.BusinessLogic.Rules.TransportOfferings.AcceptBid
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>, IRuleWithRessourceSkill
    {
        public MainRule((int transportId, TransportOfferingBidAcceptRequest request) context, IRule parentRule = null)
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
                .Eval(Context.Rules.QueryTransport)
                .Eval(Context.Rules.Required)
                .Eval(Context.Rules.StateValid)
                .Eval(Context.Rules.BidIdValid);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<(int transportId, TransportOfferingBidAcceptRequest request)>
        {
            public ContextModel((int transportId, TransportOfferingBidAcceptRequest request) parent, MainRule rule) : base(parent, rule)
            {

            }

            public Olma.Transport Transport => Rules.QueryTransport.Context.Transport; 
            
            public RulesBundle Rules { get; protected internal set; }

            public int BidId => Parent.request.BidId;

            public Olma.TransportBid WinningBid => Rules.BidIdValid.Context.WinningBid;
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                QueryTransport= new QueryTransportRule(context, rule);
                Required = new ValidOperatorRule<ContextModel>(context, (c) => c.Transport != null, null, rule);
                StateValid= new StateValidRule(context, rule);
                BidIdValid= new ValidBidIdRule(context, rule);
            }

            public readonly QueryTransportRule QueryTransport; 
            public readonly ValidOperatorRule<ContextModel> Required;
            public readonly StateValidRule StateValid;
            public readonly ValidBidIdRule BidIdValid;
        }

        #endregion

        public IValidationRule ResourceRule => Context.Rules.Required;
    }
}