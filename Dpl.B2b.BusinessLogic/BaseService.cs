using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace Dpl.B2b.BusinessLogic
{
    public abstract class BaseService
    {
        protected readonly IMapper Mapper;
        protected readonly IAuthorizationDataService AuthData;
        protected readonly IAuthorizationService AuthService;
        protected readonly bool IsDplEmployee;

        protected BaseService(IAuthorizationDataService authData, IMapper mapper)
        {
            AuthData = authData;
            Mapper = mapper;
            IsDplEmployee = authData.GetUserRole() == Common.Enumerations.UserRole.DplEmployee;
        }
        protected BaseService(IAuthorizationDataService authData,IAuthorizationService authService, IMapper mapper)
        {
            AuthData = authData;
            AuthService = authService;
            Mapper = mapper;
            IsDplEmployee = authData.GetUserRole() == Common.Enumerations.UserRole.DplEmployee;
        }

        public virtual IWrappedResponse<TResponse> Ok<TResponse>(TResponse data)
        {
            return new WrappedResponse<TResponse>()
            {
                ResultType = ResultType.Ok,
                Data = data
            };
        }
        public virtual IWrappedResponse<TResponse> Created<TResponse>(TResponse data)
        {
            return new WrappedResponse<TResponse>()
            {
                ResultType = ResultType.Created,
                Data = data
            };
        }

        public virtual IWrappedResponse<TResponse> Updated<TResponse>(TResponse data)
        {
            return new WrappedResponse<TResponse>()
            {
                ResultType = ResultType.Updated,
                Data = data
            };
        }

        public virtual IWrappedResponse<TResponse> Deleted<TResponse>(TResponse data)
        {
            return new WrappedResponse<TResponse>()
            {
                ResultType = ResultType.Deleted,
                Data = data
            };
        }

        public virtual IWrappedResponse<TResponse> NotFound<TResponse>(int? id)
        {
            return new WrappedResponse<TResponse>()
            {
                ResultType = ResultType.NotFound,
                Id = id.HasValue ? id.Value : 0
            };
        }

        public virtual IWrappedResponse<TResponse> Failed<TResponse>(params string[] errors)
        {
            return new WrappedResponse<TResponse>()
            {
                ResultType = ResultType.Failed,
                Errors = errors 
            };
        }

        public virtual IWrappedResponse<TResponse> BadRequest<TResponse>(params string[] errors)
        {
            return new WrappedResponse<TResponse>()
            {
                ResultType = ResultType.BadRequest,
                Errors = errors 
            };
        }

        public virtual IWrappedResponse<TResponse> BadRequest<TResponse>(ServiceState state, params string[] errors)
        {
            return new WrappedResponse<TResponse>
            {
                ResultType = ResultType.BadRequest,
                State = state,
                Errors = errors
            };
        }
        
        public virtual IWrappedResponse<TResponse> Failed<TResponse>(ServiceState state, params string[] errors)
        {
            return new WrappedResponse<TResponse>
            {
                ResultType = ResultType.Failed,
                State = state,
                Errors = errors
            };
        }

        public virtual IWrappedResponse<TResponse> Forbidden<TResponse>(params string[] errors)
        {
            return new WrappedResponse<TResponse>()
            {
                ResultType = ResultType.Forbidden,
                Errors = errors 
            };
        }
    }
}
