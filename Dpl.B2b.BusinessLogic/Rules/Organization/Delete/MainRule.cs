using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Organization.Delete
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(OrganizationDeleteRequest request, IRule parentRule = null)
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

            rulesEvaluator
                .Eval(Context.Rules.OrganizationResourceRule);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            Context.Organization =
                Context.Rules.OrganizationResourceRule.Context.Resource;

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<OrganizationDeleteRequest>
        {
            public ContextModel(OrganizationDeleteRequest parent, IRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }

            public OrganizationDeleteRequest Request => Parent;
            public Olma.Organization Organization { get; set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public readonly ResourceRule<Olma.Organization> OrganizationResourceRule;

            public RulesBundle(ContextModel context, IRule rule)
            {
                OrganizationResourceRule = new ResourceRule<Olma.Organization>(context.Request.Id, rule).IgnoreQueryFilter();
            }
        }

        #endregion
    }
}