using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class CountryState
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Iso2Code { get; set; }

        public virtual ICollection<PublicHoliday> PublicHolidays { get; set; }

        public int CountryId { get; set; }
    }
}