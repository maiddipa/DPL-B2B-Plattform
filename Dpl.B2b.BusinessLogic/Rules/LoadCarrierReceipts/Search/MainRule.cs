using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.LoadCarrierReceipts.Search
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>, IRuleWithAuthorizationSkill
    {
        public MainRule(LoadCarrierReceiptsSearchRequest request, IRule parentRule = null)
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

            rulesEvaluator.Eval(Context.Rules.LoadCarrierReceiptAuthorizationRule);

            

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<LoadCarrierReceiptsSearchRequest>
        {
            public ContextModel(LoadCarrierReceiptsSearchRequest parent, IRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }

            public LoadCarrierReceiptsSearchRequest Request => Parent;
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public readonly ResourceAuthorizationRule<Olma.CustomerDivision, CanReadLoadCarrierReceiptRequirement,
                DivisionPermissionResource> LoadCarrierReceiptAuthorizationRule;
            public RulesBundle(ContextModel context, IRule rule)
            {
                LoadCarrierReceiptAuthorizationRule = new ResourceAuthorizationRule<Olma.CustomerDivision, CanReadLoadCarrierReceiptRequirement, DivisionPermissionResource>(context.Request.CustomerDivisionId, rule);
            }
        }
        
        public (IValidationRule AuthorizationRule, IValidationRule ResourceRule) Authorization => (
            Context.Rules.LoadCarrierReceiptAuthorizationRule.Authorization.AuthorizationRule,
            Context.Rules.LoadCarrierReceiptAuthorizationRule.Authorization.ResourceRule);

        #endregion
    }
}