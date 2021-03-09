using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Address.Delete
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(AddressesDeleteRequest request, IRule parentRule = null)
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
                .Eval(Context.Rules.AddressResourceRule);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            Context.Address =
                Context.Rules.AddressResourceRule.Context.Resource;

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<AddressesDeleteRequest>
        {
            public ContextModel(AddressesDeleteRequest parent, IRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }

            public AddressesDeleteRequest Request => Parent;
            public Olma.Address Address { get; set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public readonly ResourceRule<Olma.Address> AddressResourceRule;

            public RulesBundle(ContextModel context, IRule rule)
            {
                AddressResourceRule = new ResourceRule<Olma.Address>(context.Request.Id, rule);
            }
        }

        #endregion
    }
}