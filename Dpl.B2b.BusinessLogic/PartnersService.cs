using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class PartnersService : BaseService, IPartnersService
    {
        private readonly IRepository<Olma.Partner> _olmaPartnerRepo;
        private readonly IServiceProvider _serviceProvider;

        public PartnersService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.Partner> olmaPartnerRepo,
            IServiceProvider serviceProvider
        ) : base(authData, mapper)
        {
            _olmaPartnerRepo = olmaPartnerRepo;
            _serviceProvider = serviceProvider;
        }
        
        //Create wird zu einem späteren Zeitpunkt implemetiert
        public async Task<IWrappedResponse> Create(PartnersCreateRequest request)
        {
            var cmd = ServiceCommand<Partner, Rules.Partner.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Partner.Create.MainRule(request))
                .Then(CreateAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.Partner.Create.MainRule mainRule)
        {
            var request = mainRule.Context.Request;

            var olmaPartner = Mapper.Map<Olma.Partner>(request);
            _olmaPartnerRepo.Create(olmaPartner);
            _olmaPartnerRepo.Save();

            var response = _olmaPartnerRepo
                .GetById<Olma.Partner, Partner>(olmaPartner.Id, true);

            return response;
        }

        public async Task<IWrappedResponse> Update(ValuesUpdateRequest request)
        {
            var cmd = ServiceCommand<Partner, Rules.Partner.Update.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Partner.Update.MainRule(request))
                .Then(UpdateAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> UpdateAction(Rules.Partner.Update.MainRule mainRule)
        {
            var request = mainRule.Context.Request;
            var olmaPartner = mainRule.Context.Partner;

            JsonConvert.PopulateObject(request.Values, olmaPartner);

            _olmaPartnerRepo.Save();

            var partner = Mapper.Map<Partner>(olmaPartner);

            return Updated(partner);
        }

        public async Task<IWrappedResponse> Delete(PartnersDeleteRequest request)
        {
            var cmd = ServiceCommand<Partner, Rules.Partner.Delete.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Partner.Delete.MainRule(request))
                .Then(DeleteAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> DeleteAction(Rules.Partner.Delete.MainRule mainRule)
        {
            var partner = mainRule.Context.Partner;
            _olmaPartnerRepo.Delete(partner);
            _olmaPartnerRepo.Save();
            return Deleted<Partner>(null);
        }

        public async Task<IWrappedResponse> Search(PartnersSearchRequest request)
        {
            var query = _olmaPartnerRepo.FindAll()
                .IgnoreQueryFilters()
                .AsNoTracking().Where(d => !d.IsDeleted);

            if (request.Id.HasValue)
            {
                var olmaPartner = await query.Where(d => d.Id == request.Id).SingleOrDefaultAsync();
                return Ok(Mapper.Map<Partner>(olmaPartner));
            }

            if (!string.IsNullOrEmpty(request.CompanyName))
            {
                var olmaPartner = await query.Where(d => d.CompanyName.Contains(request.CompanyName)).ToListAsync();
                return Ok(Mapper.Map<IEnumerable<Partner>>(olmaPartner.AsEnumerable()));
            }

            return Ok(query.ProjectTo<Partner>(Mapper.ConfigurationProvider));
        }
        
    }
}
