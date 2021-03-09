using System.Linq;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Extensions
{
    public static class ValidationExtensions
    {

        public static bool HasValidMatchingStatus(this Olma.OrderGroup orderGroup)
        {
            return orderGroup.Orders != null && orderGroup.Orders.Any(o => (o.Status == OrderStatus.Pending || o.Status == OrderStatus.Confirmed) && !o.IsDeleted);
        }

        public static bool CheckUpdatedProperties(this OrderGroupsUpdateRequest request, Olma.Order order)
        {
            if (order.LoadCarrierId != request.LoadCarrierId
                || order.LoadingLocationId != request.LoadingLocationId
                || order.LatestFulfillmentDateTime != request.LatestFulfillmentDateTime
                || order.EarliestFulfillmentDateTime != request.EarliestFulfillmentDateTime
                || order.BaseLoadCarrierId != request.BaseLoadCarrierId
                || order.QuantityType != request.QuantityType
                || order.StackHeightMax != request.StackHeightMax
                || order.StackHeightMin != request.StackHeightMin
                || order.SupportsJumboVehicles != request.SupportsJumboVehicles
                || order.SupportsRearLoading != request.SupportsRearLoading
                || order.SupportsSideLoading != request.SupportsSideLoading)
            {
                return true;
            }
            return false;
        }

    }
}
