using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Ltms
{
    public partial class Tags
    {
        public Tags()
        {
            TagAccounts = new HashSet<TagAccounts>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string Description { get; set; }
        public Guid RowGuid { get; set; }
        public string Role { get; set; }
        public string Owner { get; set; }
        public string Discriminator { get; set; }

        public virtual ICollection<TagAccounts> TagAccounts { get; set; }
    }
}
