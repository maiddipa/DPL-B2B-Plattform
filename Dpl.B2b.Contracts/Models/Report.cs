namespace Dpl.B2b.Contracts.Models
{
    public class Report
    {
        public string DocumentOriginalName { get; set; }
        public string DocumentCompositeName { get; set; }
        public string DocumentCopyName { get; set; }
        public string DocumentCancellationName { get; set; }
        public string DocumentArchiveName { get; set; }
        public byte[] Document { get; set; }
    }
}