using System.Linq;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.OrderLoad.Search
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>,
        IRuleWithAuthorizationSkill
    {
        public MainRule(OrderLoadSearchRequest request)
        {
            // Create Context
            Context = new ContextModel(request, this);
        }

        /// <summary>
        ///     Message for RuleState if Rule is invalid
        /// </summary>
        protected override ILocalizableMessage Message => new NotAllowedByRule();

        public (IValidationRule AuthorizationRule, IValidationRule ResourceRule) Authorization => (
            Context.Rules.CustomerDivisionAuthorizationRule.Authorization.AuthorizationRule,
            Context.Rules.CustomerDivisionAuthorizationRule.Authorization.ResourceRule);

        /// <summary>
        ///     Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            rulesEvaluator
                .Eval(Context.Rules.CustomerDivisionAuthorizationRule);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        ///     Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<OrderLoadSearchRequest>
        {
            public ContextModel(OrderLoadSearchRequest parent, IRule rule) : base(parent, rule)
            {
            }

            public OrderLoadSearchRequest OrderLoadSearchRequest => Parent;

            public RulesBundle Rules { get; protected internal set; }
        }

        /// <summary>
        ///     Bundles of rules
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                CustomerDivisionAuthorizationRule =
                    new ResourceAuthorizationRule<
                        Olma.CustomerDivision,
                        CanReadOrderRequirement,
                        DivisionPermissionResource>(context.OrderLoadSearchRequest.DivisionId, rule);
            }

            public ResourceAuthorizationRule<Olma.CustomerDivision, CanReadOrderRequirement,
                DivisionPermissionResource> CustomerDivisionAuthorizationRule { get; protected internal set; }
        }

        #endregion
    }
}