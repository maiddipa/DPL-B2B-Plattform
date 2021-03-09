namespace Dpl.B2b.Dal.Models
{
    public class AdditionalFieldValue
    {
        public int Id { get; set; }

        public int AdditionalFieldId { get; set; }
        public virtual AdditionalField AdditionalField { get; set; }

        public string ValueString { get; set; }
        public int? ValueInt { get; set; }
        public float? ValueFloat { get; set; }
    }
}