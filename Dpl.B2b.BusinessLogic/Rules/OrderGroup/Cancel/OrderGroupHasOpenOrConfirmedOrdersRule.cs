using System;
using System.Linq;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.OrderGroup;

namespace Dpl.B2b.BusinessLogic.Rules.OrderGroup.Cancel
{
    internal class OrderGroupHasOpenOrConfirmedOrdersRule : BaseValidationRule
    {

        private Dal.Models.OrderGroup OrderGroup { get; }


        public OrderGroupHasOpenOrConfirmedOrdersRule(Dal.Models.OrderGroup orderGroup)
        {
            OrderGroup = orderGroup ?? throw new ArgumentNullException(nameof(orderGroup));
        }

        public override void Evaluate()
        {
            // No statement for an empty quantity
            if (OrderGroup.Orders == null) return;

            var validState = OrderGroup.Orders.Any(og =>
                og.Status == Dpl.B2b.Common.Enumerations.OrderStatus.Pending ||
                og.Status == Dpl.B2b.Common.Enumerations.OrderStatus.Confirmed);

            if (!validState)
            {
                RuleState.Add("OrderGroup",
                    new RuleState(OrderGroup)
                        { RuleStateItems = { new OrderGroupHasOpenOrConfirmedOrders() } });
            }
        }

        public override bool IsMatch()
        {
            return OrderGroup.Orders != null;
        }
    }
}