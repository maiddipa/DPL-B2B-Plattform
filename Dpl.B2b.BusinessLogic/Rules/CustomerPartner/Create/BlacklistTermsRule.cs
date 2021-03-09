using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.CustomerPartner.Create
{
    public class BlacklistTermsRule : BaseValidationWithServiceProviderRule<BlacklistTermsRule, BlacklistTermsRule.ContextModel>
    {
        public BlacklistTermsRule(MainRule.ContextModel context, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(context, this);
            ParentRule = parentRule;
        }

        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => new BlacklistMatch();

        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            rulesEvaluator
                .Eval(Context.Rules.BlacklistOther);
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }
        
        private bool ValidOther(ContextModel context)
        {
            var blacklistFactory = ServiceProvider.GetService<IBlacklistFactory>();
            
            var blacklistRule = new BlacklistRule(
                new[] {context.CompanyName},
                blacklist: blacklistFactory.CreateCommonFieldsBlacklist(),
                parentRule: this);
            
            return blacklistRule.Validate();
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public string CompanyName => Parent.Parent.CompanyName;
            
            public ContextModel(MainRule.ContextModel parent, BlacklistTermsRule termsRule) : base(parent, termsRule)
            {
                Rules=new RulesBundle(this, termsRule);
            }

            public RulesBundle Rules { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public readonly ValidOperatorRule<ContextModel> BlacklistOther;
            
            public RulesBundle(ContextModel context, BlacklistTermsRule termsRule)
            {
                BlacklistOther = new ValidOperatorRule<ContextModel>(
                    context,
                    termsRule.ValidOther,
                    new BlacklistMatch(),
                    parentRule: termsRule, 
                    nameof(BlacklistOther));
            }
        }

        #endregion
    }
}