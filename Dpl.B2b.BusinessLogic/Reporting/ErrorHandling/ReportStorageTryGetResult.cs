
using Azure.Storage.Blobs.Models;
using Olma = Dpl.B2b.Dal.Models;
namespace Dpl.B2b.BusinessLogic.Reporting.ErrorHandling
{
    public class ReportStorageTryGetResult
    {
        public ReportStorageResultType Result { get; }
        public Olma.Document SourceDocument { get; }
        public BlobBlock Blob { get; }

        public ReportStorageTryGetResult(ReportStorageResultType result, Olma.Document document, BlobBlock blob)
        {
            Result = result;
            SourceDocument = document;
            Blob = blob;
        }
    }
}
