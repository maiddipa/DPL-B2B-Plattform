using System;

namespace Dpl.B2b.Contracts.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Number { get; set; }

        public int TypeId { get; set; }

        public DateTime IssuedDateTime { get; set; }
        public string CancellationReason { get; set; }

        public int DivisionId { get; set; }

        public int? VoucherId { get; set; }

        public int[] FileIds { get; set; }
    }

    public class DocumentSearchRequest : PaginationRequest
    {
        // TODO decide which filters for document searches we will allow (we should at least allow the previous ones => old asp app)
    }

    public class DocumentState
    {
        public int Id { get; set; }

        public bool CanBeCanceled { get; set; }

        public bool IsCanceled { get; set; }
    }
}