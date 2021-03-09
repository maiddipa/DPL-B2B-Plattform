using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsDelivery
    {
        public LmsDelivery()
        {
            LmsDeliverydetail = new HashSet<LmsDeliverydetail>();
        }

        public int DiliveryId { get; set; }
        public DateTime? OldDate { get; set; }
        public int Quantity { get; set; }
        public int Quality { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Cw { get; set; }
        public int? ZipCode { get; set; }
        public int TempAllocAvail { get; set; }
        public int? Day { get; set; }
        public bool Finished { get; set; }
        public int ClientId { get; set; }
        public int? CountryId { get; set; }
        public int? CustomerId { get; set; }
        public int? DistributorId { get; set; }
        public int? LoadingPointId { get; set; }
        public int? CarrierId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? UntilDate { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? UntilTime { get; set; }
        public string Comment { get; set; }
        public bool? IsFix { get; set; }
        public int? PalletType { get; set; }
        public string ContractNo { get; set; }
        public string DeliveryNoteNo { get; set; }
        public string TransactionNo { get; set; }
        public int? CategoryId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string DeletedBy { get; set; }
        public bool Rejection { get; set; }
        public bool? Reklamation { get; set; }
        public int? RejectionReasonId { get; set; }
        public string RejectionComment { get; set; }
        public int? RejectionQuantity { get; set; }
        public int? RejectionApproachId { get; set; }
        public string RejectionBy { get; set; }
        public DateTime? RejectionDate { get; set; }
        public bool RejectionDone { get; set; }
        public string RejectionDoneComment { get; set; }
        public string RejectionNextReceiptNo { get; set; }
        public string RejectionDoneBy { get; set; }
        public DateTime? RejectionDoneDate { get; set; }
        public double? RejectionCost { get; set; }
        public string ContactPerson { get; set; }
        public string ContactDetails { get; set; }
        public bool IsMixedDelivery { get; set; }
        public string ManualDistributorAddress { get; set; }
        public bool IsSmallAmount { get; set; }
        public bool ZuAvisieren { get; set; }
        public bool IstAvisiert { get; set; }
        public string AvisiertVon { get; set; }
        public DateTime? AvisiertAm { get; set; }
        public string LsmanAdrAnrede { get; set; }
        public string LsmanAdrName1 { get; set; }
        public string LsmanAdrName2 { get; set; }
        public string LsmanAdrName3 { get; set; }
        public string LsmanAdrPlz { get; set; }
        public string LsmanAdrOrt { get; set; }
        public string LsmanAdrStrasse { get; set; }
        public string LsmanAdrLand { get; set; }
        public string LamanAdrAnrede { get; set; }
        public string LamanAdrName1 { get; set; }
        public string LamanAdrName2 { get; set; }
        public string LamanAdrName3 { get; set; }
        public string LamanAdrPlz { get; set; }
        public string LamanAdrOrt { get; set; }
        public string LamanAdrStrasse { get; set; }
        public string LamanAdrLand { get; set; }
        public string DeliveryTime { get; set; }
        public string FrachtauftragNo { get; set; }
        public string Notiz { get; set; }
        public int? Kilometer { get; set; }
        public double? Frachtpreis { get; set; }
        public string Prioritaet { get; set; }
        public string Bestellnummer { get; set; }
        public bool? Geblockt { get; set; }
        public string GeblocktVon { get; set; }
        public DateTime? GeblocktDatum { get; set; }
        public string GeblocktAufgehobenVon { get; set; }
        public DateTime? GeblocktAufgehobenDatum { get; set; }
        public string GeblocktKommentar { get; set; }
        public string GeblocktFuer { get; set; }
        public bool? Storniert { get; set; }
        public string StorniertVon { get; set; }
        public DateTime? StorniertDatum { get; set; }
        public string StorniertKommentar { get; set; }
        public string InBearbeitungVon { get; set; }
        public DateTime? InBearbeitungDatumZeit { get; set; }
        public int? Revision { get; set; }
        public bool? Fixmenge { get; set; }
        public bool? Jumboladung { get; set; }
        public bool? IstBedarf { get; set; }
        public int? BusinessTypeId { get; set; }
        public bool? IstPlanmenge { get; set; }
        public string Rahmenauftragsnummer { get; set; }
        public bool? FvKoffer { get; set; }
        public bool? FvPlane { get; set; }
        public int? FvStapelhoehe { get; set; }
        public bool? FvVerladerichtungLaengs { get; set; }
        public string DispoNotiz { get; set; }
        public string OrderManagementId { get; set; }
        public decimal? Einzelpreis { get; set; }
        public string ExterneNummer { get; set; }
        public int? LtmsAccountId { get; set; }
        public Guid RowGuid { get; set; }

        public virtual ICollection<LmsDeliverydetail> LmsDeliverydetail { get; set; }
    }
}
