using System.Collections.Generic;
using System.Linq;
using Dpl.B2b.BusinessLogic.Rules.PostingAccount.Shared;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.PostingAccount.GetBalances
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(PostingAccountsSearchRequest request)
        {
            // Create Context
            Context = new ContextModel(request, this);
        }

        /// <summary>
        ///     Message for RuleState if Rule is invalid
        /// </summary>
        protected override ILocalizableMessage Message => new NotAllowedByRule();

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
            rulesEvaluator.Eval(Context.Rules.PostingAccountBalanceRule);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        ///     Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<PostingAccountsSearchRequest>
        {
            public ContextModel(PostingAccountsSearchRequest parent, IRule rule) : base(parent, rule)
            {
            }

            public IEnumerable<Balance> Balances =>
                Rules.PostingAccountBalanceRule.Context.PostingAccountsBalances.First();

            public RulesBundle Rules { get; protected internal set; }
        }

        /// <summary>
        ///     Bundles of rules
        /// </summary>
        public class RulesBundle
        {
            public readonly PostingAccountBalanceRule PostingAccountBalanceRule;

            public RulesBundle(ContextModel context, IRule rule)
            {
                PostingAccountBalanceRule = new PostingAccountBalanceRule(context.Parent, rule);
            }
        }

        #endregion
    }
}