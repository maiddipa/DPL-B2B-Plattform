using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.Rules.PartnerDirectory;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class PartnerDirectoriesService : BaseService, IPartnerDirectoriesService
    {
        
        
        private readonly IRepository<Olma.PartnerDirectory> _olmaPartnerDirectoryRepo;
        private readonly IServiceProvider _serviceProvider;

        public PartnerDirectoriesService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IServiceProvider serviceProvider,
            IRepository<Olma.PartnerDirectory> olmaPartnerDirectoryRepo
            )
            : base(authData, mapper)
        {
            _serviceProvider = serviceProvider;
            _olmaPartnerDirectoryRepo = olmaPartnerDirectoryRepo;
        }

        
        // Wird im Controller aber nicht im Client verwendet
        public async Task<IWrappedResponse> GetAll()
        {
            var cmd = ServiceCommand<IWrappedResponse<IEnumerable<PartnerDirectory>>, Rules.PartnerDirectory.GetAll.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.PartnerDirectory.GetAll.MainRule())
                .Then(GetAllAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> GetAllAction(Rules.PartnerDirectory.GetAll.MainRule rule)
        {
            var task= Task<IWrappedResponse<IEnumerable<PartnerDirectory>>>.Factory.StartNew(() =>
                Ok(rule.Context.MappedResult)
            );
            return await task;
        }

        public async Task<IWrappedResponse<PartnerDirectory>> GetById(int id)
        {
            #region security

            // TODO add security

            #endregion

            var response = _olmaPartnerDirectoryRepo.GetById<Olma.PartnerDirectory, PartnerDirectory>(id);
            return response;
        }

        public async Task<IWrappedResponse> Delete(int id)
        {
            #region security

            // TODO add security

            #endregion

            var response = _olmaPartnerDirectoryRepo.Delete(id);
            return response;
        }

        public async Task<IWrappedResponse> Create(PartnerDirectoriesCreateRequest request)
        {
            #region security

            // TODO add security

            #endregion

            var response = _olmaPartnerDirectoryRepo.Create<Olma.PartnerDirectory, PartnerDirectoriesCreateRequest, PartnerDirectory>(request);
            return response;
        }

        public Task<IWrappedResponse<PartnerDirectory>> Update(int id, PartnerDirectoriesUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
