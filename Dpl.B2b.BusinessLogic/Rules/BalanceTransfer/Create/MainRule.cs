using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.BalanceTransfer;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.BalanceTransfer.Create
{
    /// <summary>
    /// If DigitalCode ist given, PostingAccount, LoadCarrier and Quantity are taken from PostingAccountPreset,
    /// else they are take from the request
    /// </summary>
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(BalanceTransferCreateRequest balanceTransferCreateRequest)
        {
            // Create Context
            Context = new ContextModel(balanceTransferCreateRequest, this);
        }

        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => new BalanceTransferCreate();

        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);
            
            // Initialized Evaluator
            var rulesEvaluator= RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();
            
            // Assign rules to the Evaluator
            rulesEvaluator.EvalIf(Context.Rules.DigitalCodeValidationRule, !string.IsNullOrEmpty(Context.Parent.DigitalCode));

            rulesEvaluator
                //TODO AMINE
                //.Eval(Context.Rules.OrderResourceRule)
                //.Eval(Context.Rules.AuthorizationRule)
                .Eval(Context.Rules.RequiredLoadCarrierIdRule)
                .Eval(Context.Rules.RequiredLoadCarrierQuantityRule)
                .Eval(Context.Rules.ValidSourceAccountRule)
                .Eval(Context.Rules.ValidDestinationAccountRule)
                .Eval(Context.Rules.SufficientBalanceRule);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();
            
            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal
        
        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<BalanceTransferCreateRequest>
        {
            public ContextModel(BalanceTransferCreateRequest parent, IRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }

            public Olma.ExpressCode ExpressCode => Rules.DigitalCodeValidationRule.Context.ExpressCode;
            
            public int? LoadCarrierQuantity => ExpressCode != null
                ? ExpressCode.PostingAccountPreset.LoadCarrierQuantity
                : Parent.Quantity;

            public int? LoadCarrierId => ExpressCode != null
                ? ExpressCode.PostingAccountPreset.LoadCarrierId
                : Parent.LoadCarrierId;

            public Olma.PostingAccount SourceAccount => Rules.ValidSourceAccountRule.Context.SourceAccount;

            public Olma.PostingAccount DestinationAccount => Rules.ValidDestinationAccountRule.Context.DestinationAccount;
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public readonly DigitalCodeValidationRule DigitalCodeValidationRule;
            public readonly RequiredLoadCarrierIdRule RequiredLoadCarrierIdRule;
            public readonly RequiredLoadCarrierQuantityRule RequiredLoadCarrierQuantityRule;
            public readonly ValidDestinationAccountRule ValidDestinationAccountRule;
            public readonly ValidSourceAccountRule ValidSourceAccountRule;
            public readonly SufficientBalanceRule SufficientBalanceRule;
            public readonly AuthorizationRule<Olma.Order, ContextModel, PostingAccountPermissionResource, CanCancelOrderRequirement> AuthorizationRule;

            public RulesBundle(ContextModel context, IRule rule)
            {
                DigitalCodeValidationRule = new DigitalCodeValidationRule(context.Parent.DigitalCode, rule).LoadDigitalCodeNumber();
                RequiredLoadCarrierIdRule = new RequiredLoadCarrierIdRule(context, rule);
                RequiredLoadCarrierQuantityRule = new RequiredLoadCarrierQuantityRule(context, rule);
                ValidSourceAccountRule = new ValidSourceAccountRule(context, rule);
                ValidDestinationAccountRule = new ValidDestinationAccountRule(context, rule);
                SufficientBalanceRule = new SufficientBalanceRule(context, rule);

                // TODO Muss als Beispiel gesehen werden
                // OrderResourceRule = new ResourceRule<Olma.Order>(1, rule);
                // var getOperatorRule = new GetOperatorRule<ContextModel, Olma.Order>(context, (c) => c.Rules.OrderResourceRule.Context.Resource);
                // AuthorizationRule = new AuthorizationRule<Olma.Order, ContextModel, PostingAccountPermissionResource, CanCancelOrderRequirement>(getOperatorRule, rule);
            }
        }
        #endregion
    }
}