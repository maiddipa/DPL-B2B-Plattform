namespace Dpl.B2b.Dal.Models
{
    public class VoucherReasonType
    {
        public int Id { get; set; }
        public string RefLtmsReasonTypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }
    }
}