using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Ltms = Dpl.B2b.Dal.Ltms;

namespace Dpl.B2b.BusinessLogic
{
    public class UploadsService : BaseService, IUploadsService
    {
        public UploadsService(
            IAuthorizationDataService authData,
            IMapper mapper) : base(authData, mapper)
        {
        }

        public Task<IWrappedResponse> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IWrappedResponse<IEnumerable<string>>> GetBalances(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IWrappedResponse<Upload>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IWrappedResponse<Upload>> Patch(int id, UpdateUploadRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IWrappedResponse<Upload>> Post(CreateUploadRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IWrappedResponse<IPaginationResult<Upload>>> Search(UploadSearchRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
