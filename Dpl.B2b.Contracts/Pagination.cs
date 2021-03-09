using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts
{
    public interface IPagination
    {
        int Page { get; set; }
        int Limit { get; set; }
    }

    public interface IPaginationResult<T>
    {
        int PerPage { get; set; }
        int LastPage { get; set; }
        int CurrentPage { get; set; }
        int Total { get; set; }
        IEnumerable<T> Data { get; set; }
    }

    public interface IAdvancedPaginationResult<T>
    {
        int PerPage { get; set; }
        int LastPage { get; set; }
        int CurrentPage { get; set; }
        int Total { get; set; }
        IEnumerable<T> Data { get; set; }
        string[] Markers { get; set; }
    }

    /// <summary>
    /// The structure of the response is required by Akita (client side state management)
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public class PaginationResult<T> : IPaginationResult<T>
    {
        public int PerPage { get; set; }
        public int LastPage { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }

    public class AdvancedPaginationResult<T> : PaginationResult<T>, IAdvancedPaginationResult<T>
    {
        public string[] Markers { get; set; }
    }
}
