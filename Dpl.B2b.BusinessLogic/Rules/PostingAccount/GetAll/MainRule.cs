using System.Collections.Generic;
using System.Linq;
using Dpl.B2b.BusinessLogic.Rules.PostingAccount.Shared;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.PostingAccount.GetAll
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
            rulesEvaluator.Eval(Context.Rules.PostingAccountBalanceRule)
                .Eval(Context.Rules.PostingAccountConditionRule);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            var postingAccountsConditions =
                Context.Rules.PostingAccountConditionRule.Context.PostingAccountsConditions;
            var postingAccountsBalances = Context.Rules.PostingAccountBalanceRule.Context.PostingAccountsBalances;
            Context.PostingAccountsResult = new List<Contracts.Models.PostingAccount>();

            foreach (var postingAccount in Context.PostingAccounts)
            {
                postingAccount.PickupConditions = postingAccountsConditions.Where(c =>
                    c.RefLtmsAccountId == postingAccount.RefLtmsAccountId &&
                    c.Type == PostingAccountConditionType.Pickup).ToList();
                postingAccount.DropoffConditions = postingAccountsConditions.Where(c =>
                    c.RefLtmsAccountId == postingAccount.RefLtmsAccountId &&
                    c.Type == PostingAccountConditionType.DropOff).ToList();
                postingAccount.DemandConditions = postingAccountsConditions.Where(c =>
                    c.RefLtmsAccountId == postingAccount.RefLtmsAccountId &&
                    c.Type == PostingAccountConditionType.Demand).ToList();
                postingAccount.SupplyConditions = postingAccountsConditions.Where(c =>
                    c.RefLtmsAccountId == postingAccount.RefLtmsAccountId &&
                    c.Type == PostingAccountConditionType.Supply).ToList();
                postingAccount.TransferConditions = postingAccountsConditions.Where(c =>
                    c.RefLtmsAccountId == postingAccount.RefLtmsAccountId &&
                    c.Type == PostingAccountConditionType.Transfer).ToList();
                postingAccount.VoucherConditions = postingAccountsConditions.Where(c =>
                    c.RefLtmsAccountId == postingAccount.RefLtmsAccountId &&
                    c.Type == PostingAccountConditionType.Voucher).ToList();

                postingAccount.Balances = postingAccountsBalances[postingAccount.Id];

                Context.PostingAccountsResult.Add(postingAccount);
            }

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

            public IEnumerable<Contracts.Models.PostingAccount> PostingAccounts => Rules.PostingAccountBalanceRule
                .Context.Rules.PostingAccountValidRule.Context.PostingAccounts;

            public List<Contracts.Models.PostingAccount> PostingAccountsResult { get; protected internal set; }
            public RulesBundle Rules { get; protected internal set; }
        }

        /// <summary>
        ///     Bundles of rules
        /// </summary>
        public class RulesBundle
        {
            public readonly PostingAccountBalanceRule PostingAccountBalanceRule;
            public readonly PostingAccountConditionRule PostingAccountConditionRule;

            public RulesBundle(ContextModel context, IRule rule)
            {
                PostingAccountConditionRule = new PostingAccountConditionRule(context, rule);
                PostingAccountBalanceRule = new PostingAccountBalanceRule(context.Parent, rule);
            }
        }

        #endregion
    }
}