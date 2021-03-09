using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dpl.B2b.Contracts
{
    public interface IWrappedResponse
    {
        ResultType ResultType { get; set; }
        string[] Errors { get; set; }
        string[] Warnings { get; set; }

        object State { get; set; }

        int Id { get; set; }
    }

    public interface IWrappedResponse<TResponseType> : IWrappedResponse
    {
        TResponseType Data { get; set; }

    }

    public class WrappedResponse : IWrappedResponse
    {
        public ResultType ResultType { get; set; }
        public string[] Errors { get; set; }
        public string[] Warnings { get; set; }

        //TODO
        public object State { get; set; }

        public int Id { get; set; }
    }

    public class WrappedResponse<TResponseType> : WrappedResponse, IWrappedResponse<TResponseType>
    {
        public TResponseType Data { get; set; }
    }

    public enum ResultType
    {
        Ok = 0,
        Created = 1,
        Updated = 2,
        Deleted = 3,
        NotFound = 4,
        Failed = 5,
        BadRequest = 6,
        Forbidden = 7
    }
}
