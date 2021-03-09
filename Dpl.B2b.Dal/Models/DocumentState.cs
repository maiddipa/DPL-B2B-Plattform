namespace Dpl.B2b.Dal.Models
{
    // TODO we need to discuss if we needs document states as is as we wanne track status of processes instead of status of documents
    public class DocumentState
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool CanBeCanceled { get; set; }

        public bool IsCanceled { get; set; }
    }
}