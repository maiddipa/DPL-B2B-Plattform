using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Users
    {
        public Users()
        {
            AccountsKeyAccountManager = new HashSet<Accounts>();
            AccountsResponsiblePerson = new HashSet<Accounts>();
            AccountsSalesperson = new HashSet<Accounts>();
            UserDetails = new HashSet<UserDetails>();
        }

        public string Id { get; set; }
        public Guid RowGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Description { get; set; }
        public string Initial { get; set; }
        public string PersonnelNumber { get; set; }

        public virtual ICollection<Accounts> AccountsKeyAccountManager { get; set; }
        public virtual ICollection<Accounts> AccountsResponsiblePerson { get; set; }
        public virtual ICollection<Accounts> AccountsSalesperson { get; set; }
        public virtual ICollection<UserDetails> UserDetails { get; set; }
    }
}
