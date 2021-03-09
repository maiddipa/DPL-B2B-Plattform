using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Common.Enumerations
{
    public enum BaseLoadCarrierInfo
    {
        None = 0,
        Optional = 1,
        Required = 2
    }

    public enum DocumentFileType
    {
        Original = 0,
        Copy = 1,
        Composite = 2,
        Archive = 3,
        Cancellation = 4,
        }

    public enum EmployeeNoteReason
    {
        Mail = 0,
        Phone = 1,
        Fax = 2,
        Other = 3
    }

    public enum EmployeeNoteType
    {
        Create = 0,
        Updated = 1,
        Cancellation = 2,
        CancellationConfirmed = 3
    }

    public enum LoadCarrierReceiptDepotPresetCategory
    {
        External = 0,
        Internal = 1
    }

    public enum OrderMatchStatus
    {
        Pending = 0,
        TransportScheduled = 2,
        Cancelled = 3,
        Fulfilled = 4
    }

    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        PartiallyMatched = 3,
        Matched = 4,
        Fulfilled = 5,
        CancellationRequested = 9,
        Cancelled = 10,
        Expired = 11,
    }

    public enum OrderLoadStatus
    {
        Pending = 0, // this is the case right after a match has been created
        TransportPlanned = 1,
        Fulfilled = 2,
        CancellationRequested = 5,
        Canceled = 6 // Match / Avail2Deli has been canceld / removed
    }

    public enum OrderTransportType
    {
        Self = 0,
        ProvidedByOthers = 1
    }

    public enum OrderType
    {
        Supply = 0,
        Demand = 1
    }

    public enum OrderQuantityType
    {
        Load = 0,
        Stacks = 1,
        LoadCarrierQuantity = 2
    }

    public enum LoadCarrierQualityType
    {
        Intact = 0,
        Defect = 1
    }

    public enum LoadCarrierReceiptTrigger
    {
        OrderMatch = 0,
        Manual = 1
    }

    public enum LoadCarrierReceiptType
    {
        Pickup = 0,
        Delivery = 1,
        Exchange = 2,
    }

    public enum PartnerType
    {
        Default = 0,
        Recipient = 1,
        Supplier = 2,
        Shipper = 3,
        SubShipper = 4
    }

    //Attention!!! Do not change the numbering. This can cause security issues
    public enum PermissionResourceType
    {
        Organization = 0,
        Customer = 1,
        Division = 2,
        PostingAccount = 3
    }

    public enum PersonGender
    {
        None = 0,
        Female = 1,
        Male = 2,
        Other = 3
    }

    public enum DocumentTypeEnum
    {
        VoucherDirectReceipt = 1,
        VoucherDigital = 2,
        VoucherOriginal = 3,
        LoadCarrierReceiptExchange = 4,
        LoadCarrierReceiptPickup = 5,
        LoadCarrierReceiptDelivery = 6,
        TransportVoucher = 7,
    }

    // TODO decide how to handle doc type which needs to be either mapped to a code only enum, or we need to add an additional enum value to each db entry so we dont need to have mappping in the code
    public enum PrintType
    {
        VoucherCommon = 1,
        LoadCarrierReceiptExchange = 2,
        LoadCarrierReceiptPickup = 3,
        LoadCarrierReceiptDelivery = 4,
        TransportVoucher = 5,
    }

    public enum PostingRequestReason
    {
        Transfer = 0,
        Voucher = 1,
        LoadCarrierReceipt = 2,
        Sorting = 3
    }
    public enum PostingRequestStatus
    {
        Pending = 0,
        Confirmed = 1,
        Canceled = 2
    }

    public enum PostingRequestType
    {
        Credit = 0,
        Charge = 1
    }

    public enum TransportBidStatus
    {
        Active = 0,
        Won = 1,
        Lost = 2,
        Accepted = 3,
        Declined = 4,
        Canceled = 5
    }


    public enum UserRole
    {        
        Retailer = 0,
        Warehouse = 1,
        Shipper = 2,
        DplEmployee = 3
    }

    // TODO We need to discuss which voucher status
    public enum VoucherStatus
    {
        Issued = 0,
        Submitted = 1,
        Accounted = 2,
        Expired = 99,
        Canceled = 255,  //Same Id in OPMS
    }

    public enum VoucherType
    {
        Original = 0,
        Digital = 1,
        Direct = 2
    }

    public enum RefLtmsProcessType
    {
        Undefined = 0,
        Migration = 1,
        DepotAcceptance = 2,
        Manual = 3,
        OpgSubmission = 4,
        OpgChargoff = 5,
        DpgSubmission = 6, 
        DplOpgIssuing = 7, //Original Voucher
        BalanceTransfer = 8,
        Excemption = 9,
        DplDpgIssuing = 10, //Digital Voucher
        DirectBooking = 20,
        ExemptionForSale = 30,
        Sorting = 40
    }

    public enum RefLmsDeliveryCategory
    {
        SelfPickup = 3
    }

    public enum SyncErrorStatus
    {
        AutomatedProcessing = 0,
        ManuallyProcessing = 1,
        ProcessAbandoned = 2
    }
    
    public enum BusinessHourExceptionType
    {
        Open = 0,
        Closed = 1
    }
}
