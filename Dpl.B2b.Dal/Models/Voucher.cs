using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class Voucher : OlmaAuditable, ILoadCarrierDocument<VoucherPosition>
    {
        public int Id { get; set; }
        
        public Guid RowGuid { get; set; }

        // this refers to the express code that was provided when creating the voucher to preset fields like Booking account etc
        public int? ExpressCodeId { get; set; }
        public virtual ExpressCode ExpressCode { get; set; }

        public VoucherType Type { get; set; }

        public VoucherStatus Status { get; set; }  //TODO DB Discuss Double with Document State

        public DateTime ValidUntil { get; set; }

        public int ReasonTypeId { get; set; }
        public virtual VoucherReasonType ReasonType { get; set; }

        public int RecipientId { get; set; }
        public virtual CustomerPartner Recipient { get; set; }

        public PartnerType RecipientType { get; set; } //TODO DB added type to explicitlty/redundant store the PartnerType because, this can change over time

        public int? SupplierId { get; set; }
        public virtual CustomerPartner Supplier { get; set; }

        public int? ShipperId { get; set; }
        public virtual CustomerPartner Shipper { get; set; }

        public int? SubShipperId { get; set; }
        public virtual CustomerPartner SubShipper { get; set; }


        public bool ProcurementLogistics { get; set; }
        public string? CustomerReference { get; set; }


        // ------ ILoadCarrierDocument props
        public int DocumentId { get; set; }
        public virtual Document Document { get; set; }

        public int CustomerDivisionId { get; set; }
        public virtual CustomerDivision CustomerDivision { get; set; }

        public string Note { get; set; }

        public int? ReceivingPostingAccountId { get; set; } //TODO DB Changed ReceivingPostingAccount FK to nullable
        public virtual PostingAccount ReceivingPostingAccount { get; set; }
        public virtual ICollection<PostingRequest> PostingRequests { get; set; }

        public int? ReceivingCustomerId { get; set; }
        public virtual Customer ReceivingCustomer { get; set; }

        public string TruckDriverName { get; set; }
        public string TruckDriverCompanyName { get; set; }
        public string LicensePlate { get; set; }
        public int? LicensePlateCountryId { get; set; }
        public virtual Country LicensePlateCountry { get; set; }

        public virtual ICollection<VoucherPosition> Positions { get; set; }

        public virtual ICollection<EmployeeNote> DplNotes { get; set; }

    }



    public class VoucherPosition : OlmaAuditable
    {
        public int Id { get; set; }

        public int LoadCarrierId { get; set; }
        public virtual LoadCarrier LoadCarrier { get; set; }
        public int LoadCarrierQuantity { get; set; }

        public int VoucherId { get; set; }
        public virtual Voucher Voucher { get; set; }
    }
}