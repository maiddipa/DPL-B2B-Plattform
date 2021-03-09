using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Order;
using Dpl.B2b.Contracts.Models;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Order.Cancel
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>, IRuleWithAuthorizationSkill
    {
        public MainRule(int id, OrderCancelRequest request)
        {
            Context = new ContextModel(id, request, this);
        }
        

        protected override ILocalizableMessage Message => new OrderCancel();
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);
            
            // Initialized Evaluator
            var rulesEvaluator= RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            rulesEvaluator
                .Eval(Context.Rules.OrderResourceRule)
                .Eval(Context.Rules.AuthorizationRule)
                .Eval(Context.Rules.OrderCancelStatusValidationRule);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();
            
            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }
        
        public class ContextModel : ContextModelBase<OrderCancelRequest>
        {
            public ContextModel(int id, OrderCancelRequest parent, IRule rule) : base(parent, rule)
            {
                Id = id;
            }
            
            public int Id { get;}
            public OrderCancelRequest Request => Parent;
            
            public RulesBundle Rules { get; protected internal set; }
        }
        
        public class RulesBundle
        {
            public readonly OrderCancelStatusValidationRule<Olma.Order, ContextModel> OrderCancelStatusValidationRule;
            public readonly ResourceRule<Olma.Order> OrderResourceRule;
            public readonly AuthorizationRule<Olma.Order, ContextModel, DivisionPermissionResource, CanCancelOrderRequirement> AuthorizationRule;

            public RulesBundle(ContextModel context, IRule rule)
            {
                OrderResourceRule = new ResourceRule<Olma.Order>(context.Id, rule).Include(o => o.Group)
                    .Include(o => o.CreatedBy).Include(u => u.CreatedBy.Person)
                    .Include(i => i.DplNotes);
                
                var getOperatorRule = new GetOperatorRule<ContextModel, Olma.Order>(context, (c) => c.Rules.OrderResourceRule.Context.Resource);
                AuthorizationRule = new AuthorizationRule<Olma.Order, ContextModel, DivisionPermissionResource, CanCancelOrderRequirement>(getOperatorRule, rule);
                OrderCancelStatusValidationRule = new OrderCancelStatusValidationRule<Olma.Order, ContextModel>(getOperatorRule, rule);
            }
        }

        public (IValidationRule AuthorizationRule, IValidationRule ResourceRule) Authorization => (Context.Rules.AuthorizationRule, Context.Rules.OrderResourceRule);
    }
}