namespace Dpl.B2b.BusinessLogic.Reporting.ErrorHandling
{
    public class ReportStorageGetResult
    {
        public ReportStorageResultType Result { get; }
        public byte[] PdfBytes { get; }

        public ReportStorageGetResult(ReportStorageResultType result, byte[] pdfBytes)
        {
            Result = result;
            PdfBytes = pdfBytes;
        }
    }
}
