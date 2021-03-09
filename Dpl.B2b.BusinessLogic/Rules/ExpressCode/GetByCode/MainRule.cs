using System;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.ExpressCode.Shared;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.ExpressCode;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.ExpressCode.GetByCode
{
    /// <summary>
    /// Comment: no authorization, because Information of an valid ExpressCode should be available for everyone
    /// Sprint Meeting: 10.08.2020
    /// </summary>
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
        // , IRuleWithAuthorizationSkill
    {
        public MainRule(ExpressCodesSearchRequest request, IRule parentRule=null)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;
        }

        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message { get; } = new DigitalCodeInvalid();

        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);

            // Initialized Evaluator
            var rulesEvaluatorExpressCode = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            rulesEvaluatorExpressCode
                // .Eval(Context.Rules.CustomerDivisionAuthorizationRule)
                .Eval(Context.Rules.ExpressCodeValidRule);
            
            // Evaluate 
            var ruleResult = rulesEvaluatorExpressCode.Evaluate();
            
            if (ruleResult.IsSuccess)
            {
                Context.DigitalCode = Context.Rules.ExpressCodeValidRule.Context.ExpressCode;
                var partnerExpressCodeCondition = Context.Rules.ExpressCodeValidRule.Context.OlmaRecipientPartner
                    .DefaultPostingAccount.ExpressCodeCondition;
                
                if (Context.DigitalCode?.IsCanceled ?? false)
                    RuleState.Add(ResourceName, new RuleState(Context.DigitalCode) {RuleStateItems = {new DigitalCodeCanceled()}});
                
                if (Context.DigitalCode?.ValidTo < DateTime.Now)
                    RuleState.Add(ResourceName, new RuleState(Context.DigitalCode) {RuleStateItems = {new DigitalCodeExpired()}});

                if (Context.PrintType == PrintType.LoadCarrierReceiptDelivery && !partnerExpressCodeCondition.AllowDropOff
                    || Context.PrintType == PrintType.VoucherCommon && !partnerExpressCodeCondition.AllowReceivingVoucher)
                {
                    RuleState.Add(ResourceName, new RuleState(Context.DigitalCode) {RuleStateItems = {new DigitalCodeInvalid()}});
                }
            }
            else
            {
                var rulesEvaluatorExpressCodeFromOderLoad = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

                // Assign rules to the Evaluator
                rulesEvaluatorExpressCodeFromOderLoad
                    .Eval(Context.Rules.OrderLoadValidRule);
            
                // Evaluate 
                ruleResult = rulesEvaluatorExpressCodeFromOderLoad.Evaluate();

                if (ruleResult.IsSuccess)
                {
                    Context.DigitalCode = Context.Rules.OrderLoadValidRule.Context.ExpressCode;
                }
            }
            
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<ExpressCodesSearchRequest>
        {
            public ContextModel(ExpressCodesSearchRequest parent, IRule rule) : base(parent, rule)
            {
            }

            public RulesBundle Rules { get; protected internal set; }

            public string Code => Parent.ExpressCode;
            public PrintType? PrintType => Parent.PrintType;
            public int CustomerDivisionId => Parent.IssuingCustomerDivisionId;
            public Contracts.Models.ExpressCode DigitalCode { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public readonly ExpressCodeValidRule ExpressCodeValidRule;
            // public readonly IssuingPostingAccountValidRule IssuingPostingAccountValidRule;
            public readonly OrderLoadValidRule OrderLoadValidRule;
            public readonly ResourceAuthorizationRule<Olma.CustomerDivision, CanReadExpressCodeRequirement,
                    DivisionPermissionResource> CustomerDivisionAuthorizationRule;

            public RulesBundle(ContextModel context, IRule rule)
            {
                ExpressCodeValidRule = new ExpressCodeValidRule(context.Parent, rule);
                OrderLoadValidRule = new OrderLoadValidRule(context.Parent, rule);
                // CustomerDivisionAuthorizationRule =
                //     new ResourceAuthorizationRule<
                //         Olma.CustomerDivision,
                //         CanReadExpressCodeRequirement,
                //         DivisionPermissionResource>(context.CustomerDivisionId, rule);
            }
        }

        #endregion

        // public (IValidationRule AuthorizationRule, IValidationRule ResourceRule) Authorization => (
        //     Context.Rules.CustomerDivisionAuthorizationRule.Authorization.AuthorizationRule,
        //     Context.Rules.CustomerDivisionAuthorizationRule.Authorization.ResourceRule);
        }
}