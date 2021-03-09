using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.LoadCarrierReceipts.Cancel
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule((int id, LoadCarrierReceiptsCancelRequest request) request, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;
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
            rulesEvaluator.Eval(Context.Rules.ResourceAuthorizationRule);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        ///     Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<(int Id, LoadCarrierReceiptsCancelRequest Request)>
        {
            public ContextModel((int id, LoadCarrierReceiptsCancelRequest request) parent, IRule rule) : base(parent,
                rule)
            {
            }

            public RulesBundle Rules { get; protected internal set; }
            public Olma.LoadCarrierReceipt LoadCarrierReceipt => Rules.ResourceAuthorizationRule.Context[0];
        }

        /// <summary>
        ///     Bundles of rules
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                ResourceAuthorizationRule =
                    new ResourceAuthorizationRule<Olma.LoadCarrierReceipt, CanCancelLoadCarrierReceiptRequirement,
                            DivisionPermissionResource>(context.Parent.Id, rule)
                        .Include(l => l.Positions)
                        .Include(l => l.Document)
                        .Include(l => l.PostingRequests)
                        .Include(l=> l.OrderLoadDetail);
            }

            public ResourceAuthorizationRule<Olma.LoadCarrierReceipt, CanCancelLoadCarrierReceiptRequirement,
                DivisionPermissionResource> ResourceAuthorizationRule { get; }
        }

        #endregion
    }
}