using System;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.OrderGroup;

namespace Dpl.B2b.BusinessLogic.Rules.OrderGroup.Cancel
{
    public class MainRule : BaseValidationRule
    {
        private Dal.Models.OrderGroup OrderGroup { get; }

        public MainRule(Dal.Models.OrderGroup orderGroup)
        {
            OrderGroup = orderGroup ?? throw new ArgumentNullException(nameof(orderGroup));
        }

        public override void Evaluate()
        {
            var result = RulesEvaluator.Create()
                .Eval(new OrderGroupHasOrdersRule(OrderGroup))
                .Eval(new OrderGroupHasOpenOrConfirmedOrdersRule(OrderGroup))
                .Evaluate();

            if (!result.IsSuccess)
            {
                RuleState.Add("OrderGroup", new RuleState(OrderGroup) {RuleStateItems = {new OrderGroupCancel()}});
            }

            if (result is RuleWithStateResult stateResult)
            {
                
                RuleState.Merge(stateResult.State);
            }
        }
    }
}