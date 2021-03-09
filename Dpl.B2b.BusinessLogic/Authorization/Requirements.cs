using Dpl.B2b.Common.Enumerations;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.BusinessLogic.Authorization
{

    public abstract class ActionRequirement
    {
        public abstract ResourceAction Action { get; }
    }
    public class CanCreateVoucherRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.CreateVoucher;
    }
    public class CanReadVoucherRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.ReadVoucher;
    }

    public class CanCancelVoucherRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.CancelVoucher;
    }

    public class CanCreateLoadCarrierReceiptRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.CreateLoadCarrierReceipt;
    }

    public class CanReadLoadCarrierReceiptRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.ReadLoadCarrierReceipt;
    }

    public class CanCancelLoadCarrierReceiptRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.CancelLoadCarrierReceipt;
    }

    public class CanCreateOrderRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.CreateOrder;
    }
    public class CanReadOrderRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.ReadOrder;
    }
    
    public class CanReadOrderLoadRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.ReadOrderLoad;
    }

    public class CanUpdateOrderRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.UpdateOrder;
    }
    public class CanCancelOrderRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.CancelOrder;
    }


    public class CanCreateBalanceTransferRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.CreateBalanceTransfer;
    }

    public class CanReadBalanceTransferRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.ReadBalanceTransfer;
    }

    public class CanCancelBalanceTransferRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.CancelBalanceTransfer;
    }
    public class CanCreateExpressCodeRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.CreateExpressCode;
    }

    public class CanReadExpressCodeRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.ReadExpressCode;
    }

    public class CanCancelExpressCodeRequirement : ActionRequirement, IAuthorizationRequirement
    {
        public override ResourceAction Action => ResourceAction.CancelExpressCode;
    }

    public class AttributeRequirement<TActionRequirement> : ActionRequirement, IAuthorizationRequirement
        where TActionRequirement : ActionRequirement, new()
    {
        private readonly TActionRequirement _requirement;
        public AttributeRequirement()
        {
            _requirement = new TActionRequirement();
        }
        public override ResourceAction Action => _requirement.Action;
    }


    namespace OrderLoadRequirements
    {
        public class CanRead : ActionRequirement, IAuthorizationRequirement
        {
            public override ResourceAction Action => ResourceAction.ReadOrderLoad;
        }

        public class CanCancel : ActionRequirement, IAuthorizationRequirement
        {
            public override ResourceAction Action => ResourceAction.CancelOrderLoad;
        }
    }

    namespace AccountingRecordsRequirements
    {
        public class CanRead : ActionRequirement, IAuthorizationRequirement
        {
            public override ResourceAction Action => ResourceAction.ReadAccountingRecords;
        }
    }

    namespace LivePoolingRequirements
    {
        public class CanCreateSearch : ActionRequirement, IAuthorizationRequirement
        {
            public override ResourceAction Action => ResourceAction.CreateLivePoolingSearch;
        }
        public class CanReadOrders : ActionRequirement, IAuthorizationRequirement
        {
            public override ResourceAction Action => ResourceAction.ReadLivePoolingOrders;
        }
    }

    namespace SortingRequirements
    {
        public class CanCreate : ActionRequirement, IAuthorizationRequirement
        {
            public override ResourceAction Action => ResourceAction.CreateSorting;
        }
    }
}
