using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Dal.Models
{
    public class DocumentType
    {
        public int Id { get; set; }

        public DocumentTypeEnum Type { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public bool HasReport { get; set; }

        public BusinessDomain BusinessDomain { get; set; }

        public PrintType PrintType { get; set; }

        public int OriginalAvailableForMinutes { get; set; }
    }

    // TODO we need to speak further about what this means
    public enum BusinessDomain
    {
        Pooling = 0,
        Depot = 1
    }


}