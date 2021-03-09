using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Dal.Models
{
    public class AdditionalField : OlmaAuditable
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public PrintType PrintType { get; set; }

        public string Name { get; set; }

        public AdditionalFieldDataType Type { get; set; }
    }

    public enum AdditionalFieldDataType
    {
        Checkbox,
        Number,
        Text
    }
}