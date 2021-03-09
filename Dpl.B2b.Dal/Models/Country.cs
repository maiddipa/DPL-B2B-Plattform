using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string LicensePlateCode { get; set; }
        public string Iso2Code { get; set; }
        public string Iso3Code { get; set; }


        public virtual ICollection<CountryState> States { get; set; }

        public virtual ICollection<PublicHoliday> PublicHolidays { get; set; }
    }
}