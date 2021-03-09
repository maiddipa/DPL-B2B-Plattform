using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class ExtBelegdaten
    {
        public string Belegnummer { get; set; }
        public string Vorgangsnummer { get; set; }
        public string StandardadresseNummer { get; set; }
        public string Standardadresse { get; set; }
        public string StrasseStandardadresse { get; set; }
        public string PlzStandardadresse { get; set; }
        public string OrtStandardadresse { get; set; }
        public string LandStandardadresse { get; set; }
        public string LieferkundenNummer { get; set; }
        public string Lieferadresse { get; set; }
        public string LieferStrasse { get; set; }
        public string LieferPlz { get; set; }
        public string LieferOrt { get; set; }
        public string LieferLand { get; set; }
        public string LieferOeffnungszeiten { get; set; }
        public string LiefernImAuftragNummer { get; set; }
        public string LiefernImAuftrag { get; set; }
        public string LiefernImAuftragStrasse { get; set; }
        public string LiefernImAuftragPlz { get; set; }
        public string LiefernImAuftragOrt { get; set; }
        public string LiefernImAuftragLand { get; set; }
        public string LadeAdresseNummer { get; set; }
        public string LadeAdresse { get; set; }
        public string LadeAdresseStrasse { get; set; }
        public string LadeAdressePlz { get; set; }
        public string LadeAdresseOrt { get; set; }
        public string LadeAdresseLand { get; set; }
        public string LadeAdresseOeffnungszeiten { get; set; }
        public string LadenImAuftrageNummer { get; set; }
        public string LadenImAuftrage { get; set; }
        public string LadenImAuftrageStrasse { get; set; }
        public string LadenImAuftragePlz { get; set; }
        public string LadenImAuftrageOrt { get; set; }
        public string LadenImAuftrageLand { get; set; }
        public string TransporteurNummer { get; set; }
        public string Transporteur { get; set; }
        public string TransporteurStrasse { get; set; }
        public string TransporteurPlz { get; set; }
        public string TransporteurOrt { get; set; }
        public string TransporteurLand { get; set; }
        public string Ansprechpartner { get; set; }
        public DateTime? Termin { get; set; }
        public string Artikelnummer { get; set; }
        public decimal? Menge { get; set; }
    }
}
