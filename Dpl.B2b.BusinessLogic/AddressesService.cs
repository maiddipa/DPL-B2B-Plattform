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
using Newtonsoft.Json;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class AddressesService : BaseService, IAddressesService
    {
        private readonly IRepository<Olma.Address> _olmaAddressRepo;
        private readonly IRepository<Olma.Country> _olmaCountryRepo;
        private readonly IRepository<Olma.CountryState> _olmaCountryStateRepo;
        private readonly IServiceProvider _serviceProvider;

        public AddressesService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.Address> olmaAddressRepo,
            IRepository<Olma.Country> olmaCountryRepo,
            IRepository<Olma.CountryState> olmaCountryStateRepo,
            IServiceProvider serviceProvider
        ) : base(authData, mapper)
        {
            _olmaAddressRepo = olmaAddressRepo;
            _olmaCountryRepo = olmaCountryRepo;
            _olmaCountryStateRepo = olmaCountryStateRepo;
            _serviceProvider = serviceProvider;
        }

        public async Task<IWrappedResponse> GetCountries()
        {
            var countries = await _olmaCountryRepo.FindAll().Include(c => c.States)
                .AsNoTracking().ToListAsync();
            
            return Ok(Mapper.Map<IEnumerable<Country>>(countries.AsEnumerable()));
        }

        public async Task<IWrappedResponse> GetStatesByCountryId(int id)
        {
            var states = await _olmaCountryStateRepo.FindAll()
                .AsNoTracking().Where(s => s.CountryId == id).ToListAsync();
            
            return Ok(Mapper.Map<IEnumerable<CountryState>>(states.AsEnumerable()));
        }

        public async Task<IWrappedResponse> Search(AddressesSearchRequest request)
        {
            var query = _olmaAddressRepo.FindAll()
                .IgnoreQueryFilters()
                .AsNoTracking().Where(d => !d.IsDeleted);

            if (request.Id.HasValue)
            {
                var olmaAddressRepo = await query.Where(d => d.Id == request.Id).SingleOrDefaultAsync();
                return Ok(Mapper.Map<AddressAdministration>(olmaAddressRepo));
            }

            return Ok(query.ProjectTo<AddressAdministration>(Mapper.ConfigurationProvider));
        }

        public async Task<IWrappedResponse> Create(AddressesCreateRequest request)
        {
            var cmd = ServiceCommand<AddressAdministration, Rules.Address.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Address.Create.MainRule(request))
                .Then(CreateAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.Address.Create.MainRule mainRule)
        {
            var request = mainRule.Context.Request;

            var olmaAddress = Mapper.Map<Olma.Address>(request);
            _olmaAddressRepo.Create(olmaAddress);
            _olmaAddressRepo.Save();
            
            var response = _olmaAddressRepo
                .GetById<Olma.Address, AddressAdministration>(olmaAddress.Id, true);
            
            return response;
        }
        
        public async Task<IWrappedResponse> Update(ValuesUpdateRequest request)
        {
            var cmd = ServiceCommand<AddressAdministration, Rules.Address.Update.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Address.Update.MainRule(request))
                .Then(UpdateAction);
            
            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> UpdateAction(Rules.Address.Update.MainRule mainRule)
        {
            var request = mainRule.Context.Request;
            var olmaAddress = mainRule.Context.Address;
            
            JsonConvert.PopulateObject(request.Values, olmaAddress);

            _olmaAddressRepo.Save();
            
            var customerDivision =
                Mapper.Map<AddressAdministration>(olmaAddress);
            
            return Updated(customerDivision);
        }
        
        public async Task<IWrappedResponse> Delete(AddressesDeleteRequest request)
        {
            var cmd = ServiceCommand<AddressAdministration, Rules.Address.Delete.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Address.Delete.MainRule(request))
                .Then(DeleteAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> DeleteAction(Rules.Address.Delete.MainRule mainRule)
        {
            var customerDivision = mainRule.Context.Address;
            _olmaAddressRepo.Delete(customerDivision);
            _olmaAddressRepo.Save();
            return Deleted<AddressAdministration>(null);
        }
    }
}
