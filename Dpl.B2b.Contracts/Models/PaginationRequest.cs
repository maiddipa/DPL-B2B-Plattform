using System;
using System.ComponentModel;

namespace Dpl.B2b.Contracts.Models
{
    public class PaginationRequest : IPagination
    {
        public int Page { get; set; }
        public int Limit { get; set; }
    }

    public class AdvancedPaginationRequest : PaginationRequest
    {
        public string[] Markers { get; set; }
    }

    public class SortablePaginationRequest<TSortEnum> : PaginationRequest
        where TSortEnum : Enum
    {
        public TSortEnum SortBy { get; set; }

        public ListSortDirection SortDirection { get; set; }
    }
}
