namespace Dpl.B2b.Dal.Models
{
    public class LoadingLocationLoadCarrierDimensions : OlmaAuditable
    {
        public int Id { get; set; }

        public int LoadCarrierLocationId { get; set; }
        public virtual LoadingLocation LoadCarrierLocation { get; set; }

        public int LoadCarrierId { get; set; }
        public virtual LoadCarrier LoadCarrier { get; set; }


        public int LayersPerStackMin { get; set; }
        public int LayersPerStackMax { get; set; }
    }
}