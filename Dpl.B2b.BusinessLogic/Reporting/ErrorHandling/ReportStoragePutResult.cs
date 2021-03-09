namespace Dpl.B2b.BusinessLogic.Reporting.ErrorHandling
{
    public class ReportStoragePutResult
    {
        public ReportStorageResultType Result{ get; }

        public ReportStoragePutResult(ReportStorageResultType result)
        {
            Result = result;
        }
    }
}
