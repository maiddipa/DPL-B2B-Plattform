namespace Dpl.B2b.Contracts.Models
{
    public class BalanceTransfer
    {
        public int Id { get; set; }

        public BalanceTransferStatus Status { get; set; }

        public int SourceAccountId { get; set; }
        public int DestinationAccountId { get; set; }

        public int LoadCarrierId { get; set; }
        public int Quantity { get; set; }


    }

    public enum BalanceTransferStatus
    {
        Pending = 0,
        Completed = 1
    }

    public class BalanceTransferSearchRequest
    {
        public BalanceTransferStatus Status { get; set; }

        public int SourceAccountId { get; set; } 
        public int DestinationAccountId { get; set; } 

        public int LoadCarrierId { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
    }

    public class BalanceTransferCreateRequest
    {
        public string DigitalCode { get; set; }
        public int? SourceAccountId { get; set; }
        public int? DestinationAccountId { get; set; }

        public int? LoadCarrierId { get; set; }
        public int? Quantity { get; set; }
        public string Note { get; set; }

        public EmployeeNoteCreateRequest DplNote { get; set; }
    }

    public class BalanceTransferCancelRequest
    {
        public string Reason { get; set; }
    }
}
