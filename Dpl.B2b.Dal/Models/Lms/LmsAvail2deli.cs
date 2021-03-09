using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsAvail2deli
    {
        public int Avail2DeliId { get; set; }
        public int DeliveryId { get; set; }
        public int AvailabilityId { get; set; }
        public int QualityId { get; set; }
        public int PalletTypeId { get; set; }
        public DateTime DateOfRelation { get; set; }
        public int Quantity { get; set; }
        public int? BaseQualityId { get; set; }
        public int? BasePalletTypeId { get; set; }
        public int? BaseQuantity { get; set; }
        public int State { get; set; }
        public string Usr { get; set; }
        public bool IsFix { get; set; }
        public int? SpediteurId { get; set; }
        public string SpediteurManuell { get; set; }
        public int? Kilometer { get; set; }
        public double? Frachtpreis { get; set; }
        public string LieferscheinNr { get; set; }
        public string FrachtauftragsNr { get; set; }
        public bool? FrachtpapiereErstellt { get; set; }
        public string Notiz { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string DeletedBy { get; set; }
        public string Bestellnummer { get; set; }
        public string ZustellterminZeitVon { get; set; }
        public string ZustellterminZeitBis { get; set; }
        public DateTime? LadeterminDatum { get; set; }
        public string LadeterminZeitVon { get; set; }
        public string LadeterminZeitBis { get; set; }
        public string InBearbeitungVon { get; set; }
        public DateTime? InBearbeitungDatumZeit { get; set; }
        public string SpediteurTelefon { get; set; }
        public string SpediteurFax { get; set; }
        public string SpediteurEmail { get; set; }
        public int? Lieferkategorie { get; set; }
        public string AbweichenderArtikel { get; set; }
        public string FrachtvorschriftFelder { get; set; }
        public int? FrachtvorschriftStapelhoehe { get; set; }
        public int? Revision { get; set; }
        public bool? StatusGedrucktFrachtvorschrift { get; set; }
        public bool? StatusGedrucktZuordnung { get; set; }
        public bool? Ictransport { get; set; }
        public int? CategoryId { get; set; }
        public int? AnzahlBelegteStellplaetze { get; set; }
        public bool? Timocom { get; set; }
        public Guid RowGuid { get; set; }
        public string ExpressCode { get; set; }
        public Guid? RefLtmsTransactionRowGuid { get; set; }
        public virtual LmsAvailability Availability { get; set; }
        public virtual LmsDelivery Delivery { get; set; }
    }
}
