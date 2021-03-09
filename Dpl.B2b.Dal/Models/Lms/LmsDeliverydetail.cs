using System;
using System.Collections.Generic;

namespace Dpl.B2b.Dal.Models.Lms
{
    public partial class LmsDeliverydetail
    {
        public int DeliveryDetailId { get; set; }
        public int DeliveryId { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Cw { get; set; }
        public bool Finished { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string DeletedBy { get; set; }

        public virtual LmsDelivery Delivery { get; set; }
    }
}
