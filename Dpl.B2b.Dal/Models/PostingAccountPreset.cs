namespace Dpl.B2b.Dal.Models
{
    public class PostingAccountPreset
    {
        public int Id { get; set; }
        public int ExpressCodeId { get; set; }
        public virtual ExpressCode ExpressCode { get; set; }
        public int? DestinationAccountId { get; set; }
        public virtual PostingAccount DestinationAccount { get; set; }

        public int? LoadCarrierId { get; set; }

        public int? LoadCarrierQuantity { get; set; }
    }
}