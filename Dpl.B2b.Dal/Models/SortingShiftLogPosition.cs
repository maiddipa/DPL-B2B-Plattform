namespace Dpl.B2b.Dal.Models
{
    public class SortingShiftLogPosition : OlmaAuditable
    {
        public int Id { get; set; }

        public int LoadCarrierId { get; set; }
        public virtual LoadCarrier LoadCarrier { get; set; }

        public int Quantity { get; set; }
    }
}