using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class OrganizationsService : BaseService, IOrganizationsService
    {
        private readonly IRepository<Olma.Organization> _olmaOrganizationRepo;
        private readonly IServiceProvider _serviceProvider;

        public OrganizationsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.Organization> olmaOrganizationRepo,
            IServiceProvider serviceProvider
        ) : base(authData, mapper)
        {
            _olmaOrganizationRepo = olmaOrganizationRepo;
            _serviceProvider = serviceProvider;
        }

        public async Task<IWrappedResponse> GetById(int id)
        {
            var olmaOrganization = await _olmaOrganizationRepo.FindByCondition(o => o.Id == id)
                .Include(o => o.Customers).ThenInclude(c => c.Divisions).ThenInclude(d => d.LoadingLocations)
                .ThenInclude(ll => ll.Address).IgnoreQueryFilters().AsNoTracking().SingleOrDefaultAsync();

            var organizationScopedDataSets = new List<OrganizationScopedDataSet>
            {
                Mapper.Map<OrganizationScopedDataSet>(olmaOrganization)
            };

            organizationScopedDataSets.AddRange(
                Mapper.Map<IEnumerable<OrganizationScopedDataSet>>(olmaOrganization.Customers));

            organizationScopedDataSets.AddRange(
                Mapper.Map<IEnumerable<OrganizationScopedDataSet>>(
                    olmaOrganization.Customers.SelectMany(c => c.Divisions)));

            organizationScopedDataSets.AddRange(Mapper.Map<IEnumerable<OrganizationScopedDataSet>>(
                olmaOrganization.Customers.SelectMany(c => c.Divisions.SelectMany(d => d.LoadingLocations))));

            return Ok((IEnumerable<OrganizationScopedDataSet>) organizationScopedDataSets);
        }
        
        public async Task<IWrappedResponse> GetOrganizationById(int id)
        {
            #region security

            // TODO add security

            #endregion

            return _olmaOrganizationRepo.GetById<Olma.Organization, Organization>(id, true);
        }

        public async Task<IWrappedResponse> GetAll()
        {
            var olmaOrganizations = await _olmaOrganizationRepo.FindAll()
                .IgnoreQueryFilters().Where(i => !i.IsDeleted).AsNoTracking().ToListAsync();
            var organizations = Mapper.Map<IEnumerable<Organization>>(olmaOrganizations);
            return Ok(organizations);
        }
        
        public async Task<IWrappedResponse> Create(OrganizationCreateRequest request)
        {
            var cmd = ServiceCommand<Organization, Rules.Organization.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Organization.Create.MainRule(request))
                .Then(CreateAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.Organization.Create.MainRule mainRule)
        {
            var request = mainRule.Context.Request;

            var olmaOrganization = Mapper.Map<Olma.Organization>(request);
            _olmaOrganizationRepo.Create(olmaOrganization);
            _olmaOrganizationRepo.Save();
            
            var response = _olmaOrganizationRepo
                .GetById<Olma.Organization, Organization>(olmaOrganization.Id, true);
            
            return Created(response.Data);
        }
        
        public async Task<IWrappedResponse> Update(OrganizationUpdateRequest request)
        {
            var cmd = ServiceCommand<Organization, Rules.Organization.Update.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Organization.Update.MainRule(request))
                .Then(UpdateAction);
            
            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> UpdateAction(Rules.Organization.Update.MainRule mainRule)
        {
            var request = mainRule.Context.Request;
            var olmaOrganization = mainRule.Context.Organization;
            
            Mapper.Map(request, olmaOrganization);

            _olmaOrganizationRepo.Save();
            
            var organization =
                Mapper.Map<Organization>(olmaOrganization);
            
            return Updated(organization);
        }
        
        public async Task<IWrappedResponse> Delete(OrganizationDeleteRequest request)
        {
            var cmd = ServiceCommand<Organization, Rules.Organization.Delete.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Organization.Delete.MainRule(request))
                .Then(DeleteAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> DeleteAction(Rules.Organization.Delete.MainRule mainRule)
        {
            var organization = mainRule.Context.Organization;
            _olmaOrganizationRepo.Delete(organization);
            _olmaOrganizationRepo.Save();
            return Deleted<Organization>(null);
        }
    }
}