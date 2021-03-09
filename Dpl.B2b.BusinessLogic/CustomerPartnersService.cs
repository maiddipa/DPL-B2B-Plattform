using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.Rules.CustomerPartner.Create;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class CustomerPartnersService : BaseService, ICustomerPartnersService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<Olma.CustomerPartner> _olmaCustomerPartnerRepo;
        private readonly IRepository<Olma.CustomerPartnerDirectoryAccess> _customerPartnerDirectoryAccessRepo;
        
        public CustomerPartnersService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IServiceProvider serviceProvider,
            IRepository<Olma.CustomerPartner> olmaCustomerPartnerRepo,
            IRepository<Olma.CustomerPartnerDirectoryAccess> customerPartnerDirectoryAccessRepo) : base(authData, mapper)
        {
            _serviceProvider = serviceProvider;
            _olmaCustomerPartnerRepo = olmaCustomerPartnerRepo;
            _customerPartnerDirectoryAccessRepo = customerPartnerDirectoryAccessRepo;
        }

        
        [Obsolete]
        public async Task<IWrappedResponse> Delete(int id)
        {
            #region security

            // TODO add security

            #endregion

            var response = _olmaCustomerPartnerRepo.Delete(id);
            return response;
        }
        
        public async Task<IWrappedResponse> Create(CustomerPartnersCreateRequest request)
        {
            var cmd = ServiceCommand<CustomerPartner, MainRule>
                .Create(_serviceProvider)
                .When(new MainRule(request))
                .Then(CreateAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(MainRule rule)
        {
            var task= Task<IWrappedResponse>.Factory.StartNew(() =>
            {
                var request = rule.Context.Parent;

                var partnerDirectory = rule.Context.PartnerDirectory;
            
                var customerPartnerDirectoryAccess = new Olma.CustomerPartnerDirectoryAccess
                {
                    Partner = Mapper.Map<Olma.CustomerPartner>(request),
                    DirectoryId = partnerDirectory.Id,
                    Directory = partnerDirectory,
                };

                customerPartnerDirectoryAccess.Partner.Address = Mapper.Map<Olma.Address>(request.Address);
            
                _customerPartnerDirectoryAccessRepo.Create(customerPartnerDirectoryAccess);
                _customerPartnerDirectoryAccessRepo.Save();

                var responseData = Mapper.Map<Olma.CustomerPartner, CustomerPartner>(customerPartnerDirectoryAccess.Partner);
                return Created(responseData);
            });

            return await task;
        }
        
        [Obsolete]
        public async Task<IWrappedResponse<CustomerPartner>> Update(int id, CustomerPartnersUpdateRequest request)
        {
            #region security

            // TODO add security

            #endregion

            var response = _olmaCustomerPartnerRepo.Update<Olma.CustomerPartner, CustomerPartnersUpdateRequest, CustomerPartner>(id, request);
            return response;
        }
        
        [Obsolete]
        public async Task<IWrappedResponse> GetById(int id)
        {
            var response = _olmaCustomerPartnerRepo.GetById<Olma.CustomerPartner, CustomerPartner>(id);
            return response;
        }

        public async Task<IWrappedResponse> Search(CustomerPartnersSearchRequest request)
        {
            var cmd = ServiceCommand<Olma.Partner, Rules.CustomerPartner.Search.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.CustomerPartner.Search.MainRule(request))
                .Then(SearchAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> SearchAction(Rules.CustomerPartner.Search.MainRule rule)
        {
            var paginationResult = rule.Context.Rules.PaginationPartnerRule.Context.PaginationResult;
            
            return Ok(paginationResult);
        }
    }
}
