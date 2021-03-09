using System;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.OrderGroup;

namespace Dpl.B2b.BusinessLogic.Rules.OrderGroup.Cancel
{
    internal class OrderGroupHasOrdersRule : BaseValidationRule
    {
        private Dal.Models.OrderGroup OrderGroup { get; }

        public OrderGroupHasOrdersRule(Dal.Models.OrderGroup orderGroup)
        {
            OrderGroup = orderGroup ?? throw new ArgumentNullException(nameof(orderGroup));
        }

        public override void Evaluate()
        {
            var invalidState = OrderGroup.Orders == null || OrderGroup.Orders.Count == 0;

            if (invalidState)
            {
                RuleState.Add("OrderGroup", new RuleState(OrderGroup) {RuleStateItems = {new OrderGroupHasOrders()}});
            }
        }
    }
}