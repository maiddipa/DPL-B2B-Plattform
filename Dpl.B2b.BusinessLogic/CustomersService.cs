using System;
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
    public class CustomersService : BaseService, ICustomersService
    {
        private readonly IRepository<Olma.Customer> _olmaCustomerRepo;
        private readonly IServiceProvider _serviceProvider;

        public CustomersService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.Customer> olmaCustomerRepo,
            IServiceProvider serviceProvider
        ) : base(authData, mapper)
        {
            _olmaCustomerRepo = olmaCustomerRepo;
            _serviceProvider = serviceProvider;
        }
        public async Task<IWrappedResponse> GetAll()
        {
            var query = _olmaCustomerRepo.FindAll()
                .AsNoTracking();

            #region ordering

            query = query.OrderBy(c => c.Name);

            #endregion

            var projectedQuery = query.ProjectTo<Customer>(Mapper.ConfigurationProvider);

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

            return _olmaCustomerRepo.GetById<Olma.Customer, Customer>(id, true);
        }

        public async Task<IWrappedResponse> Create(CustomerCreateRequest request)
        {
            var cmd = ServiceCommand<Customer, Rules.Customer.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Customer.Create.MainRule(request))
                .Then(CreateAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.Customer.Create.MainRule mainRule)
        {
            var request = mainRule.Context.Request;

            var olmaCustomer = Mapper.Map<Olma.Customer>(request);
            _olmaCustomerRepo.Create(olmaCustomer);
            _olmaCustomerRepo.Save();
            return Created(Mapper.Map<Customer>(olmaCustomer));
        }
        
        public async Task<IWrappedResponse> Update(CustomerUpdateRequest request)
        {
            var cmd = ServiceCommand<Customer, Rules.Customer.Update.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Customer.Update.MainRule(request))
                .Then(UpdateAction);
            
            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> UpdateAction(Rules.Customer.Update.MainRule mainRule)
        {
            var request = mainRule.Context.Request;
            var olmaCustomer = mainRule.Context.Customer;
            
            Mapper.Map(request, olmaCustomer);

            _olmaCustomerRepo.Save();
            
            var customer =
                Mapper.Map<Customer>(olmaCustomer);
            
            return Updated(customer);
        }
        
        public async Task<IWrappedResponse> Delete(CustomerDeleteRequest request)
        {
            var cmd = ServiceCommand<Customer, Rules.Customer.Delete.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Customer.Delete.MainRule(request))
                .Then(DeleteAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> DeleteAction(Rules.Customer.Delete.MainRule mainRule)
        {
            var customer = mainRule.Context.Customer;
            _olmaCustomerRepo.Delete(customer);
            _olmaCustomerRepo.Save();
            return Deleted<Customer>(null);
        }
    }
}
