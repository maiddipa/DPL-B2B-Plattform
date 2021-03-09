namespace Dpl.B2b.Dal.Models
{
    public class LoadCarrierQualityMapping
    {
        public int Id { get; set; }

        public int RefLmsQualityId { get; set; }

        public int LoadCarrierQualityId { get; set; }
        public virtual LoadCarrierQuality LoadCarrierQuality { get; set; }
    }
}