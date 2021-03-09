namespace Dpl.B2b.Dal.Models
{
    public class DeliveryNote : OlmaAuditable
    {
        public int Id { get; set; }

        public DeliveryNoteStatus Status { get; set; }

        // TODO Check with Dominik where in LTMS or if at all this information is available in LTMS
        public string DeliveryNoteNumber { get; set; }

        //public ICollection<DeliveryNotePosition> DeliveryNotePosition { get; set; }
    }

    //public class DeliveryNotePosition
    //{
    //    public int Id { get; set; }
    //}

    public enum DeliveryNoteStatus
    {
        Submitted = 0,
        Confirmed = 1,
        Rejected = 2
    }
}