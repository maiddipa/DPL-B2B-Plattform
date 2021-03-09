using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class CustomerDivisionsService : BaseService, ICustomerDivisionsService
    {
        private readonly IRepository<Olma.CustomerDivision> _olmaCustomerDivisionRepo;
        private readonly IServiceProvider _serviceProvider;

        public CustomerDivisionsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.CustomerDivision> olmaCustomerDivisionRepo,
            IServiceProvider serviceProvider
        ) : base(authData, mapper)
        {
            _olmaCustomerDivisionRepo = olmaCustomerDivisionRepo;
            _serviceProvider = serviceProvider;
        }
        public async Task<IWrappedResponse> GetAll()
        {
            var query = _olmaCustomerDivisionRepo.FindAll()
                .Include(c => c.DocumentSettings)
                .Where(i => !i.IsDeleted)
                .AsNoTracking();

            #region ordering

            query = query.OrderBy(c => c.Name);

            #endregion

            var projectedQuery = query.ProjectTo<CustomerDivision>(Mapper.ConfigurationProvider);

            var mappedResult = projectedQuery
                .ToList()
                .AsEnumerable();

            return Ok(mappedResult);
        }

        public async Task<IWrappedResponse> GetById(int id)
        {
            #region security

            // TODO add security

            #endregion

            return _olmaCustomerDivisionRepo.GetById<Olma.CustomerDivision, CustomerDivision>(id, true);
        }

        public async Task<IWrappedResponse> Create(CustomerDivisionCreateRequest request)
        {
            var cmd = ServiceCommand<CustomerDivision, Rules.CustomerDivision.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.CustomerDivision.Create.MainRule(request))
                .Then(CreateAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.CustomerDivision.Create.MainRule mainRule)
        {
            var request = mainRule.Context.Request;
            var customerDivisionDocumentSettings = mainRule.Context.CustomerDivisionDocumentSettings;

            var olmaCustomerDivision = Mapper.Map<Olma.CustomerDivision>(request);
            olmaCustomerDivision.DocumentSettings = customerDivisionDocumentSettings;
            _olmaCustomerDivisionRepo.Create(olmaCustomerDivision);
            _olmaCustomerDivisionRepo.Save();
            
            return Created(Mapper.Map<CustomerDivision>(olmaCustomerDivision));
        }
        
        public async Task<IWrappedResponse> Update(CustomerDivisionUpdateRequest request)
        {
            var cmd = ServiceCommand<CustomerDivision, Rules.CustomerDivision.Update.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.CustomerDivision.Update.MainRule(request))
                .Then(UpdateAction);
            
            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> UpdateAction(Rules.CustomerDivision.Update.MainRule mainRule)
        {
            var request = mainRule.Context.Request;
            var olmaCustomerDivision = mainRule.Context.CustomerDivision;
            
            Mapper.Map(request, olmaCustomerDivision);

            _olmaCustomerDivisionRepo.Save();
            
            var customerDivision =
                Mapper.Map<CustomerDivision>(olmaCustomerDivision);
            
            return Updated(customerDivision);
        }
        
        public async Task<IWrappedResponse> Delete(CustomerDivisionDeleteRequest request)
        {
            var cmd = ServiceCommand<CustomerDivision, Rules.CustomerDivision.Delete.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.CustomerDivision.Delete.MainRule(request))
                .Then(DeleteAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> DeleteAction(Rules.CustomerDivision.Delete.MainRule mainRule)
        {
            var customerDivision = mainRule.Context.CustomerDivision;
            _olmaCustomerDivisionRepo.Delete(customerDivision);
            _olmaCustomerDivisionRepo.Save();
            return Deleted<CustomerDivision>(null);
        }
    }
}
