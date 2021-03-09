using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class SortingWorker : OlmaAuditable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ReferenceStaffNumber { get; set; }

        // TODO check if it would make more sense to connect SortingWorker to Division, issue might be that single division sees to many users
        public virtual ICollection<CustomerSortingWorker> CustomerSortingWorkers { get; set; }
    }
}