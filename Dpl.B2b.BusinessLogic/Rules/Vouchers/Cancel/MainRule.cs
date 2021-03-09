using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Vouchers;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Vouchers.Cancel
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule((int id, VouchersCancelRequest request) request, IRule parentRule=null)
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

            // Assign rules to the Evaluator
            rulesEvaluator
                .Eval(Context.Rules.ResourceAuthorizationRule)
                .Eval(Context.Rules.IsIssuedState);
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<(int Id, VouchersCancelRequest Request)>
        {
            public ContextModel((int id, VouchersCancelRequest request) parent, MainRule rule) : base(parent, rule)
            {
            }

            public RulesBundle Rules { get; protected internal set; }
            public Olma.Voucher Voucher => Rules.ResourceAuthorizationRule.Context[0];
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                ResourceAuthorizationRule = new ResourceAuthorizationRule<Olma.Voucher, CanCancelVoucherRequirement, DivisionPermissionResource>(context.Parent.Id, rule)
                    .Include(v => v.Positions).Include(v => v.Document);
                IsIssuedState=new ValidOperatorRule<ContextModel>(context, model=>model.Voucher.Status == VoucherStatus.Issued, new NotIssuedState());
            }

            public ResourceAuthorizationRule<Olma.Voucher, CanCancelVoucherRequirement, DivisionPermissionResource> ResourceAuthorizationRule { get; protected internal set; }
            public ValidOperatorRule<ContextModel> IsIssuedState { get; }
        }

        #endregion
    }
}