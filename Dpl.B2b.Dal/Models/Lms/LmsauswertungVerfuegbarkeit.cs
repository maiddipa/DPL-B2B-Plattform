using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsauswertungVerfuegbarkeit
    {
        public int AvailabilityId { get; set; }
        public DateTime? DatumVon { get; set; }
        public DateTime? DatumBis { get; set; }
        public int Menge { get; set; }
        public int? Frei { get; set; }
        public int? PaletteId { get; set; }
        public string PaletteName { get; set; }
        public int? QualitaetId { get; set; }
        public string QualitaetName { get; set; }
        public bool Mischladung { get; set; }
        public string Art { get; set; }
        public int MandantId { get; set; }
        public string Land { get; set; }
        public int? KontaktNummer { get; set; }
        public string KontaktName { get; set; }
        public string KontaktPlz { get; set; }
        public string KontaktOrt { get; set; }
        public int? LadeadresseNummer { get; set; }
        public string LadeadresseName { get; set; }
        public string LadeadressePlz { get; set; }
        public string LadeadresseOrt { get; set; }
        public string Abholscheinnummer { get; set; }
        public string Lieferscheinnummer { get; set; }
        public string Vorgangsnummer { get; set; }
        public bool Erledigt { get; set; }
        public int? Ablehnung { get; set; }
        public int? Reklamation { get; set; }
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
        public string Notiz { get; set; }
        public int? Geblockt { get; set; }
        public string GeblocktVon { get; set; }
        public DateTime? GeblocktDatum { get; set; }
        public string GeblocktAufgehobenVon { get; set; }
        public DateTime? GeblocktAufgehobenDatum { get; set; }
        public string GeblocktKommentar { get; set; }
        public string GeblocktFuer { get; set; }
        public int? Storniert { get; set; }
        public string StorniertVon { get; set; }
        public DateTime? StorniertDatum { get; set; }
        public string StorniertKommentar { get; set; }
        public int? Icdroht { get; set; }
        public string IcdrohtVon { get; set; }
        public DateTime? IcdrohtAm { get; set; }
        public int? Icdurchfuehren { get; set; }
        public string IcdurchfuehrenVon { get; set; }
        public DateTime? IcdurchfuehrenAm { get; set; }
        public int? ZuAvisieren { get; set; }
        public int? IstAvisiert { get; set; }
        public string AvisiertVon { get; set; }
        public DateTime? AvisiertAm { get; set; }
        public DateTime ErzeugtAm { get; set; }
        public DateTime? GeaendertAm { get; set; }
        public DateTime? GeloeschtAm { get; set; }
        public string ErzeugtVon { get; set; }
        public string GeaendertVon { get; set; }
        public string GeloeschtVon { get; set; }
    }
}
