using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class Organization : OlmaAuditable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<CustomerIpSecurityRule> CustomerIpSecurityRules { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}