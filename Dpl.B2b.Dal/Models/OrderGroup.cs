using System;
using System.Collections.Generic;
using System.Text;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Dal.Models
{
    public class OrderGroup: OlmaAuditable
    {
        public int Id { get; set; }

        public virtual ICollection<Order> Orders { get; set; }             
    }
}
