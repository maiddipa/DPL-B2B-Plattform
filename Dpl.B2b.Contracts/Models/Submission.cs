namespace Dpl.B2b.Contracts.Models
{
    public class Submission
    {
        public int Id { get; set; }

        public SubmissionType Type { get; set; }
    }

    public enum SubmissionType
    {
        Voucher = 0,
        DeliverySlip = 1
    }
}
