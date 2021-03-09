using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.OrderGroup;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;


namespace Dpl.B2b.BusinessLogic.Rules.OrderGroup.Create
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule,
        MainRule.ContextModel>, IRuleWithAuthorizationSkill
    {
        public MainRule(OrderGroupsCreateRequest orderGroupsCreateRequest)
        {
            Context = new ContextModel(orderGroupsCreateRequest, this);
        }

        protected override ILocalizableMessage Message => new OrderGroupCreate();

        protected override void EvaluateInternal()
        {
            Context.Rules = new RulesBundle(Context, this);
            
            var rulesEvaluator = RulesEvaluator.Create().CheckAllRules();
            
            rulesEvaluator.Eval(Context.Rules.CustomerDivisionAuthorizationRule)
            .Eval(Context.Rules.LoadCarrierIdValidationRule)
            .Eval(Context.Rules.NumberOfLoadsValidationRule)
            .Eval(Context.Rules.LoadingLocationIdValidationRule)
            .Eval(Context.Rules.BaseLoadCarrierValidationRule);
            
            var ruleResult = rulesEvaluator.Evaluate();
            
            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        public class ContextModel : ContextModelBase<OrderGroupsCreateRequest>
        {
            public ContextModel(OrderGroupsCreateRequest parent, IRule rule) : base(parent, rule)
            {
            }

            public RulesBundle Rules { get; protected internal set; }
            
            public OrderGroupsCreateRequest OrderGroupsCreateRequest => Parent;
        }

        public class RulesBundle
        {
            public readonly ResourceAuthorizationRule<Olma.CustomerDivision, CanCreateOrderRequirement,
                DivisionPermissionResource> CustomerDivisionAuthorizationRule;
            public readonly LoadCarrierIdValidationRule LoadCarrierIdValidationRule;
            public readonly NumberOfLoadsValidationRule NumberOfLoadsValidationRule;
            public readonly LoadingLocationIdValidationRule LoadingLocationIdValidationRule;
            public readonly BaseLoadCarrierValidationRule BaseLoadCarrierValidationRule;

            public RulesBundle(ContextModel context, IRule rule)
            {
                CustomerDivisionAuthorizationRule = new ResourceAuthorizationRule<Olma.CustomerDivision, CanCreateOrderRequirement, DivisionPermissionResource>(context.OrderGroupsCreateRequest.DivisionId, rule);
                LoadCarrierIdValidationRule = new LoadCarrierIdValidationRule(context.OrderGroupsCreateRequest.LoadCarrierId, rule);
                NumberOfLoadsValidationRule = new NumberOfLoadsValidationRule(context, rule);
                LoadingLocationIdValidationRule = new LoadingLocationIdValidationRule(context.OrderGroupsCreateRequest.LoadingLocationId, rule);
                BaseLoadCarrierValidationRule = new BaseLoadCarrierValidationRule(context.OrderGroupsCreateRequest, rule);
            }
        }

        public (IValidationRule AuthorizationRule, IValidationRule ResourceRule) Authorization => (
            Context.Rules.CustomerDivisionAuthorizationRule.Authorization.AuthorizationRule,
            Context.Rules.CustomerDivisionAuthorizationRule.Authorization.ResourceRule);
    }
}