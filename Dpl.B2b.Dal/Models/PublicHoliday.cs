using System;

namespace Dpl.B2b.Dal.Models
{
    public class PublicHoliday
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public int? CountryId { get; set; }
        public int? CountryStateId { get; set; }
    }
}