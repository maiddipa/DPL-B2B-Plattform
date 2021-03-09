using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Order.Shared;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Order.Search
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>, IRuleWithAuthorizationSkill
    {
        public MainRule(OrderSearchRequest request)
        {
            // Create Context
            Context = new ContextModel(request, this);
            
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
                .Eval(Context.Rules.CustomerDivisionAuthorizationRule)
                .Eval(Context.Rules.OrderSearchQueryRule)
                .Eval(Context.Rules.OrderValidRule);
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<OrderSearchRequest>
        {
            public ContextModel(OrderSearchRequest parent, IRule rule) : base(parent, rule)
            {

            }

            public OrderSearchRequest OrderSearchRequest => Parent;
            public RulesBundle Rules { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public ResourceAuthorizationRule<Olma.CustomerDivision, CanReadOrderRequirement, DivisionPermissionResource> CustomerDivisionAuthorizationRule { get; protected internal set; }

            public readonly OrderSearchQueryRule OrderSearchQueryRule;
            public readonly OrderValidRule OrderValidRule;

            public RulesBundle(ContextModel context, IRule rule)
            {
                CustomerDivisionAuthorizationRule =
                    new ResourceAuthorizationRule<
                        Olma.CustomerDivision,
                        CanReadOrderRequirement,
                        DivisionPermissionResource>(context.OrderSearchRequest.DivisionId, rule);
                OrderSearchQueryRule = new OrderSearchQueryRule(context.OrderSearchRequest, rule);
                OrderValidRule = new OrderValidRule(context, rule);
            }
        }

        #endregion

        public (IValidationRule AuthorizationRule, IValidationRule ResourceRule) Authorization => (
            Context.Rules.CustomerDivisionAuthorizationRule.Authorization.AuthorizationRule,
            Context.Rules.CustomerDivisionAuthorizationRule.Authorization.ResourceRule);
    }
}