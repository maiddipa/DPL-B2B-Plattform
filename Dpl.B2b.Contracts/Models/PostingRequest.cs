using System;
using System.Collections.Generic;
using System.Text;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Contracts.Models
{
    public class PostingRequest
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public int PostingAccountId { get; set; }
        public int LoadCarrierId { get; set; }
        public PostingRequestReason Reason { get; set; }
        public PostingRequestType Type { get; set; }
        public PostingRequestStatus Status { get; set; }
        public bool IsSortingRequired { get; set; }
        public int LoadCarrierQuantity { get; set; }
        public int? VoucherId { get; set; }
        public int? SubmissionId { get; set; }
        public int? OrderMatchId { get; set; }
        public string Note { get; set; }
        public Guid RefLtmsTransactionId { get; set; }
        
        //public EmployeeNote DplNote { get; set; }
    }

    public class PostingRequestsCreateRequest
    {
        public string ReferenceNumber { get; set; }
        public Guid RefLtmsProcessId { get; set; }
        public int RefLtmsProcessTypeId { get; set; }
        public Guid RefLtmsTransactionId { get; set; }
        public PostingRequestReason Reason { get; set; }
        public PostingRequestType Type { get; set; }
        public bool IsSortingRequired { get; set; }
        public int? OrderMatchId { get; set; }
        public int? VoucherId { get; set; }
        public int? SubmissionId { get; set; }
        public int SourceRefLtmsAccountId { get; set; }
        public int DestinationRefLtmsAccountId { get; set; }
        public IEnumerable<PostingRequestPosition> Positions { get; set; }
        public int PostingAccountId { get; set; }
        public string Note { get; set; }
        public int? LoadCarrierReceiptId { get; set; }
        public string DocumentFileName { get; set; }
        public bool? IsSelfService { get; set; }

        public EmployeeNoteCreateRequest DplNote { get; set; }
        public string DigitalCode { get; set; }
        public string DeliveryNoteNumber { get; set; }
        public string PickUpNoteNumber { get; set; }
        public int? RefLmsBusinessTypeId { get; set; }
        public Guid? RefLtmsTransactionRowGuid { get; set; }
    }

    public class PostingRequestPosition
    {
        public int LoadCarrierId { get; set; }
        public int LoadCarrierQuantity { get; set; }
        public Guid RefLtmsBookingRowGuid { get; set; }
        public int RefLtmsPalletId { get; set; }
    }

    public class PostingRequestsUpdateRequest
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public Guid RefLtmsProcessId { get; set; }
        public int RefLtmsProcessTypeId { get; set; }
        public Guid RefLtmsTransactionId { get; set; }
        public PostingRequestReason Reason { get; set; }
        public PostingRequestType Type { get; set; }
        public PostingRequestStatus Status { get; set; }
        public int? OrderMatchId { get; set; }
        public int? VoucherId { get; set; }
        public int? SubmissionId { get; set; }
        public int SourceRefLtmsAccountId { get; set; }
        public int DestinationRefLtmsAccountId { get; set; }
        public int LoadCarrierId { get; set; }
        public int LoadCarrierQuantity { get; set; }
        public string Note { get; set; }
    }

    public class PostingRequestsSearchRequest : PaginationRequest
    {
        public int PostingAccountId { get; set; }
        public int LoadCarrierTypeId { get; set; }
    }

    public class PostingRequestsSyncRequest
    {
        public List<PostingRequestCreateSyncRequest> PostingRequestCreateSyncRequests{get; set;}
        public List<PostingRequestUpdateSyncRequest> PostingRequestUpdateSyncRequests{get; set;}
        public List<PostingRequestRollbackSyncRequest> PostingRequestRollbackSyncRequests { get; set; }
        public List<PostingRequestCancelSyncRequest> PostingRequestCancelSyncRequests { get; set; }
    }

    public class PostingRequestRollbackSyncRequest
    {
        public Guid RefLtmsTransactionId { get; set; }
    }
    public class PostingRequestCancelSyncRequest
    {
        public Guid RefLtmsTransactionId { get; set; }
        public string Reason { get; set; }
    }

    public class PostingRequestCreateSyncRequest
    {
        public int CreditRefLtmsAccountId { get; set; }
        public int DebitRefLtmsAccountId { get; set; }
        public int IssuedByRefLtmsAccountId { get; set; }
        public string ReferenceNumber { get; set; }
        public Guid RefLtmsProcessId { get; set; }
        public RefLtmsProcessType RefLtmsProcessType { get; set; }
        public Guid RefLtmsTransactionId { get; set; }
        public string Note { get; set; }
        public Guid? VoucherRowGuid { get; set; }
        public string DocumentFileName { get; set; }
        public bool? IsSelfService { get; set; }
        public IEnumerable<PostingRequestPosition> Positions { get; set; }
        public string DigitalCode { get; set; }
        public bool IsSortingRequired { get; set; }

        public string DeliveryNoteNumber { get; set; }
        public string PickUpNoteNumber { get; set; }
        public int? RefLmsBusinessTypeId { get; set; }
    }  
    public class PostingRequestUpdateSyncRequest
    {
        public int CreditRefLtmsAccountId { get; set; }
        public int DebitRefLtmsAccountId { get; set; }
        public int IssuedByRefLtmsAccountId { get; set; }
        public string ReferenceNumber { get; set; }
        //public Guid RefLtmsProcessId { get; set; }
        public RefLtmsProcessType RefLtmsProcessType { get; set; }
        public Guid RefLtmsTransactionId { get; set; }
        public string Note { get; set; }
        public Guid? VoucherRowGuid { get; set; }
        public string DocumentFileName { get; set; }
        public bool? IsSelfService { get; set; }
        public IEnumerable<PostingRequestPosition> Positions { get; set; }
        public string DigitalCode { get; set; }

        public string DeliveryNoteNumber { get; set; }
        public string PickUpNoteNumber { get; set; }
        public int? RefLmsBusinessTypeId { get; set; }
    }
}
