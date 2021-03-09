using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsauswertungLiefertermin
    {
        public int DeliveryId { get; set; }
        public DateTime? DatumVon { get; set; }
        public DateTime? DatumBis { get; set; }
        public int Menge { get; set; }
        public int? PaletteId { get; set; }
        public string PaletteName { get; set; }
        public int QualitaetId { get; set; }
        public string QualitaetName { get; set; }
        public bool Mischladung { get; set; }
        public string Geschaeftsart { get; set; }
        public int MandantId { get; set; }
        public string Land { get; set; }
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
        public string Auftragsnummer { get; set; }
        public string Lieferscheinnummer { get; set; }
        public string Vorgangsnummer { get; set; }
        public string Frachtauftragsnummer { get; set; }
        public string Bestellnummer { get; set; }
        public bool Erledigt { get; set; }
        public bool Ablehnung { get; set; }
        public bool? Reklamation { get; set; }
        public int? AblehnungMenge { get; set; }
        public string AblehnungGrund { get; set; }
        public string AblehnungKommentar { get; set; }
        public string AblehnungVerfahrensweise { get; set; }
        public string AblehnungVon { get; set; }
        public DateTime? AblehnungAm { get; set; }
        public string RejectionDoneComment { get; set; }
        public string AblehnungFolgebeleg { get; set; }
        public string AblehnungBearbeitetVon { get; set; }
        public DateTime? AblehnungBearbeitetAm { get; set; }
        public double? AblehnungKosten { get; set; }
        public bool Kleinstmengenfreistellung { get; set; }
        public bool? Fixmenge { get; set; }
        public bool? Jumboladung { get; set; }
        public bool? IstBedarf { get; set; }
        public bool? IstPlanmenge { get; set; }
        public string Notiz { get; set; }
        public string Prioritaet { get; set; }
        public bool ZuAvisieren { get; set; }
        public bool IstAvisiert { get; set; }
        public string AvisiertVon { get; set; }
        public DateTime? AvisiertAm { get; set; }
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
        public DateTime ErzeugtAm { get; set; }
        public DateTime? GeaendertAm { get; set; }
        public DateTime? GeloeschtAm { get; set; }
        public string ErzeugtVon { get; set; }
        public string GeaendertVon { get; set; }
        public string GeloeschtVon { get; set; }
        public decimal? Einzelpreis { get; set; }
    }
}
