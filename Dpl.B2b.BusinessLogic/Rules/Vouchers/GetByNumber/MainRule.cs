using System;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Vouchers.GetByNumber
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(string request, IRule parentRule = null)
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
                .Eval(Context.Rules.VoucherResourceRule)
                .Eval(Context.Rules.VoucherAuthorizationRule);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<string>
        {
            public ContextModel(string parent, MainRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }
            public string Number => Parent;
            public Olma.Voucher Voucher => Rules.VoucherResourceRule.Context.Resource;
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                VoucherResourceRule=new ResourceRule<Olma.Voucher>(x=>x.Document.Number==context.Number, rule);

                var getOperator = new GetOperatorRule<ContextModel, Olma.Voucher>(context,
                    model => model.Rules.VoucherResourceRule.Context.Resource);

                VoucherAuthorizationRule = new AuthorizationRule<
                    Olma.Voucher,
                    ContextModel,
                    DivisionPermissionResource,
                    CanReadVoucherRequirement>(getOperator, rule);
            }
            
            public ResourceRule<Olma.Voucher> VoucherResourceRule { get; protected internal set; }
            public AuthorizationRule<Olma.Voucher, ContextModel, DivisionPermissionResource, CanReadVoucherRequirement> VoucherAuthorizationRule { get; protected internal set; }
        }

        #endregion
    }
}