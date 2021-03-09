using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsAvailabilityLtv
    {
        public int AvailabilityId { get; set; }
        public string ZipCode { get; set; }
        public int Quantity { get; set; }
        public bool? TempAlloc { get; set; }
        public bool? TempReserved { get; set; }
        public DateTime? Bbd { get; set; }
        public bool? IsManual { get; set; }
        public int TypeId { get; set; }
        public int ClientId { get; set; }
        public int? Reliability { get; set; }
        public int? ReliabilityQuantity { get; set; }
        public int? ReliabilityQuality { get; set; }
        public DateTime? AvailableFromDate { get; set; }
        public DateTime? AvailableUntilDate { get; set; }
        public DateTime? AvailableFromTime { get; set; }
        public DateTime? AvailableUntilTime { get; set; }
        public int? ContactId { get; set; }
        public int? LoadingPointId { get; set; }
        public int? QualityId { get; set; }
        public int? AvailabilityTypeId { get; set; }
        public string CommentText { get; set; }
        public int? CountryId { get; set; }
        public int? AvailabilitySubTypeId { get; set; }
        public int? PalletTypeId { get; set; }
        public string ContactAddressInfo { get; set; }
        public string ContractNo { get; set; }
        public string DeliveryNoteNo { get; set; }
        public string TransactionNo { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string DeletedBy { get; set; }
        public string ContactPerson { get; set; }
        public string ContactDetails { get; set; }
        public string RepositoryHours { get; set; }
        public bool Finished { get; set; }
        public bool IsMixedAvailability { get; set; }
        public string ManAdrAnrede { get; set; }
        public string ManAdrName1 { get; set; }
        public string ManAdrName2 { get; set; }
        public string ManAdrName3 { get; set; }
        public string ManAdrPlz { get; set; }
        public string ManAdrOrt { get; set; }
        public string ManAdrStrasse { get; set; }
        public string ManAdrLand { get; set; }
    }
}
