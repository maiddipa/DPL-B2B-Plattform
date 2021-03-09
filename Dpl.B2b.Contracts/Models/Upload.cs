namespace Dpl.B2b.Contracts.Models
{
    public class Upload
    {
        public int Id { get; set; }
    }

    public class UploadSearchRequest : PaginationRequest
    {
        public string CustomerName { get; set; }
    }

    public class CreateUploadRequest
    {
        public string Value { get; set; }
    }

    public class UpdateUploadRequest
    {
        public string Value { get; set; }
    }
}
