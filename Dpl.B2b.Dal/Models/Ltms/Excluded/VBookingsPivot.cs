using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class VBookingsPivot
    {
        public int Id { get; set; }
        public string BookingTypeId { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public string ExtDescription { get; set; }
        public bool IncludeInBalance { get; set; }
        public bool Matched { get; set; }
        public string MatchedUser { get; set; }
        public DateTime? MatchedTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string DeleteUser { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int AccountId { get; set; }
        public int TransactionId { get; set; }
        public int Dd1a { get; set; }
        public int Dd2a { get; set; }
        public int Dd2aplus { get; set; }
        public int Dd2b { get; set; }
        public int Dd2bg { get; set; }
        public int Dd2bmt { get; set; }
        public int Dd2brmp { get; set; }
        public int DdChep { get; set; }
        public int DdD { get; set; }
        public int DdI { get; set; }
        public int DdNeu { get; set; }
        public int DdSchr { get; set; }
        public int DdU { get; set; }
        public int E1epD { get; set; }
        public int E1epI { get; set; }
        public int E1epNeu { get; set; }
        public int E2epD { get; set; }
        public int E2epI { get; set; }
        public int E2epNeu { get; set; }
        public int E2krD { get; set; }
        public int E2krI { get; set; }
        public int E2krNeu { get; set; }
        public int Eur1a { get; set; }
        public int Eur2a { get; set; }
        public int Eur2b { get; set; }
        public int EurChep { get; set; }
        public int EurD { get; set; }
        public int EurEw { get; set; }
        public int EurI { get; set; }
        public int EurMpal { get; set; }
        public int EurNeu { get; set; }
        public int EurSchr { get; set; }
        public int EurU { get; set; }
        public int Gb1a { get; set; }
        public int GbD { get; set; }
        public int GbEigV { get; set; }
        public int GbI { get; set; }
        public int GbU { get; set; }
        public int H1D { get; set; }
        public int H1DmieteI { get; set; }
        public int H1Dreckig { get; set; }
        public int H1Ew { get; set; }
        public int H1I { get; set; }
        public int H1MK { get; set; }
        public int H1Neu { get; set; }
        public int H1OdmK { get; set; }
        public int H1OdoK { get; set; }
        public int H1OK { get; set; }
        public int H1Sauber { get; set; }
        public int H1Schr { get; set; }
        public int H1U { get; set; }
        public int H1gdI { get; set; }
    }
}
