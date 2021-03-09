using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models
{
    public class OrderSerie : OlmaAuditable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}