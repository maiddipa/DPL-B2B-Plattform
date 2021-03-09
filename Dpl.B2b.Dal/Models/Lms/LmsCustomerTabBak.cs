using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsCustomerTabBak
    {
        public int AdressNr { get; set; }
        public string Anrede { get; set; }
        public string Kontoname { get; set; }
        public string Kontoname2 { get; set; }
        public string Strasse { get; set; }
        public string Lkz { get; set; }
        public string Plz { get; set; }
        public string Postfach { get; set; }
        public string Ort { get; set; }
        public string Land { get; set; }
        public string Telefon { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Www { get; set; }
        public string Azeiten { get; set; }
        public string Notiz { get; set; }
        public double? WinlineAdresse { get; set; }
        public double? Gwadresse { get; set; }
        public double? ZusatzAdresse { get; set; }
        public string WinlineKonto { get; set; }
        public string Gwgguid { get; set; }
        public double? Inaktiv { get; set; }
        public DateTime? Angelegt { get; set; }
        public DateTime? Geaendert { get; set; }
    }
}
