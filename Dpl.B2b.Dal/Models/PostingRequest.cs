using System;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Dal.Models
{
    /// <summary>
    ///     Each entry represents a completed fullfillment
    /// </summary>
    public class PostingRequest : OlmaAuditable
    {
        public int Id { get; set; }
        public Guid RowGuid { get; set; }

        public string ReferenceNumber { get; set; }

        // TODO We need a GUID on the process table in LTMS, today only have an int
        public Guid RefLtmsProcessId { get; set; }

        public int RefLtmsProcessTypeId { get; set; }

        public Guid RefLtmsTransactionId { get; set; }

        public PostingRequestType Type { get; set; }
        public PostingRequestReason Reason { get; set; }
        public PostingRequestStatus Status { get; set; }

        public bool IsSortingRequired { get; set; }

        public int? VoucherId { get; set; }
        public virtual Voucher Voucher { get; set; }

        public int? LoadCarrierReceiptId { get; set; }
        public virtual LoadCarrierReceipt LoadCarrierReceipt { get; set; }

        public int? SubmissionId { get; set; }
        public virtual Submission Submission { get; set; }

        public int? OrderMatchId { get; set; }
        public virtual OrderMatch OrderMatch { get; set; }

        public int PostingAccountId { get; set; }
        public virtual PostingAccount PostingAccount { get; set; }
        public int SourceRefLtmsAccountId { get; set; }
        public int DestinationRefLtmsAccountId { get; set; }
        public int LoadCarrierId { get; set; }
        public virtual LoadCarrier LoadCarrier { get; set; }


        /// <summary>
        ///     Positiv for credit + negativ for charge
        /// </summary>
        public int LoadCarrierQuantity { get; set; }

        /// <summary>
        ///     Note from the client, witch will be shown as Booking Description on both sides Credit/Charge (LTMS ExtDescription)
        /// </summary>
        public string Note { get; set; }

        public virtual EmployeeNote DplNote { get; set; }

        public DateTime? SyncDate { get; set; }
        public string SyncNote { get; set; }
    }
}