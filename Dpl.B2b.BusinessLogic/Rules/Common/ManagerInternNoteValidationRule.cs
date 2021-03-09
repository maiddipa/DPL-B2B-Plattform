using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Common
{
    public class ManagerInternNoteValidationRule : BaseValidationWithServiceProviderRule<ManagerInternNoteValidationRule
        , ManagerInternNoteValidationRule.ContextModel>
    {
        public ManagerInternNoteValidationRule(EmployeeNoteCreateRequest request)
        {
            // Create Context
            Context = new ContextModel(request, this);
        }

        /// <summary>
        ///     Message for RuleState if Rule is invalid
        /// </summary>
        protected override ILocalizableMessage Message => new ManagerInternNoteRequired();

        /// <summary>
        ///     Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            // {{TODO}}

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        ///     Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<EmployeeNoteCreateRequest>
        {
            public ContextModel(EmployeeNoteCreateRequest parent, IRule rule) : base(parent, rule)
            {
            }

            public RulesBundle Rules { get; protected internal set; }
        }

        /// <summary>
        ///     Bundles of rules
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