﻿using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class Lmsauswertung
    {
        public int DeliveryId { get; set; }
        public int? AvailabilityId { get; set; }
        public string Palette { get; set; }
        public int? PaletteId { get; set; }
        public string Qualitaet { get; set; }
        public int QualitaetId { get; set; }
        public int Menge { get; set; }
        public int? MengeZugeordnet { get; set; }
        public int? MengeReserviert { get; set; }
        public DateTime? Zustelltermin { get; set; }
        public string ZustellterminZeitVon { get; set; }
        public string ZustellterminZeitBis { get; set; }
        public DateTime? Ladetermin { get; set; }
        public string LadeterminZeitVon { get; set; }
        public string LadeterminZeitBis { get; set; }
        public string Prioritaet { get; set; }
        public bool? Ictransport { get; set; }
        public bool? Timocom { get; set; }
        public int Ladungen { get; set; }
        public int? Kilometer { get; set; }
        public double? Frachtpreis { get; set; }
        public DateTime? ZuordnungLetzteAktualisierung { get; set; }
        public DateTime? ZuordnungAm { get; set; }
        public string ZuordnungVon { get; set; }
        public string LieferKategorie { get; set; }
        public int? LieferKategorieId { get; set; }
        public bool Finished { get; set; }
        public bool IsMixedDelivery { get; set; }
        public bool IsSmallAmount { get; set; }
        public bool? Storniert { get; set; }
        public bool? Fixmenge { get; set; }
        public bool? Jumboladung { get; set; }
        public bool? IstBedarf { get; set; }
        public int? KundeNummer { get; set; }
        public string KundeName { get; set; }
        public string KundePlz { get; set; }
        public string KundeOrt { get; set; }
        public string KundeStrasse { get; set; }
        public int? LieferadresseNummer { get; set; }
        public string LieferadresseName { get; set; }
        public string LieferadressePlz { get; set; }
        public string LieferadresseOrt { get; set; }
        public string LieferadresseStrasse { get; set; }
        public int? LadestelleNummer { get; set; }
        public string LadestelleName { get; set; }
        public string LadestellePlz { get; set; }
        public string LadestelleOrt { get; set; }
        public string LadestelleStrasse { get; set; }
        public int? SpediteurId { get; set; }
        public string SpediteurManuell { get; set; }
        public string Spediteur { get; set; }
        public string AuftragsNrVonVerfuegbarkeit { get; set; }
        public string ArtVonVerfuegbarkeit { get; set; }
        public int? ArtIdvonVerfuegbarkeit { get; set; }
        public DateTime? VerfuegbarkeitDatumVon { get; set; }
        public DateTime? VerfuegbarkeitDatumBis { get; set; }
        public bool IcdrohtVerfuegbarkeit { get; set; }
        public string IcdrohtVonVerfuegbarkeit { get; set; }
        public DateTime? IcdrohtAmVerfuegbarkeit { get; set; }
        public bool IcdurchfuehrenVerfuegbarkeit { get; set; }
        public string IcdurchfuehrenVonVerfuegbarkeit { get; set; }
        public DateTime? IcdurchfuehrenAmVerfuegbarkeit { get; set; }
        public string IcdurchfuehrenVon { get; set; }
        public int? IcGrundId { get; set; }
        public string Icgrund { get; set; }
        public int ClientId { get; set; }
        public int? CountryId { get; set; }
        public DateTime? LieferterminDatumVon { get; set; }
        public DateTime? LieferterminDatumBis { get; set; }
        public bool? IstFixtermin { get; set; }
        public string Comment { get; set; }
        public string AuftragsNummer { get; set; }
        public string LieferscheinNummer { get; set; }
        public string FrachtauftragsNummer { get; set; }
        public string Bestellnummer { get; set; }
        public string TransaktionsNummer { get; set; }
        public bool Rejection { get; set; }
        public bool? Reklamation { get; set; }
        public int? RejectionReasonId { get; set; }
        public int? RejectionQuantity { get; set; }
        public int? RejectionApproachId { get; set; }
        public DateTime? RejectionDoneDate { get; set; }
        public double? RejectionCost { get; set; }
        public string Notiz { get; set; }
        public string LieferterminErstelltVon { get; set; }
        public DateTime LieferterminErstelltAm { get; set; }
        public string LieferterminGeaendertVon { get; set; }
        public DateTime? LieferterminGeaendertAm { get; set; }
        public string VerfuegbarkeitErstelltVon { get; set; }
        public DateTime? VerfuegbarkeitErstelltAm { get; set; }
        public string VerfuegbarkeitGeaendertVon { get; set; }
        public DateTime? VerfuegbarkeitGeaendertAm { get; set; }
        public decimal? Einzelpreis { get; set; }
    }
}