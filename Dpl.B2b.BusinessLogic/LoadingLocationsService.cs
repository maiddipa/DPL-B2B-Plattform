using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;
using Ltms = Dpl.B2b.Dal.Ltms;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class LoadingLocationsService : BaseService, ILoadingLocationsService
    {
        private readonly IRepository<Olma.LoadingLocation> _olmaLoadingLocationRepo;
        private readonly IRepository<Olma.Country> _olmaCountryRepo;
        private readonly IRepository<Olma.CountryState> _olmaCountryStateRepo;
        private readonly IServiceProvider _serviceProvider;

        public LoadingLocationsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.LoadingLocation> olmaLoadingLocationRepo,
            IRepository<Olma.Country> olmaCountryRepo,
            IRepository<Olma.CountryState> olmaCountryStateRepo,
            IServiceProvider serviceProvider
            ) : base(authData, mapper)
        {
            _olmaLoadingLocationRepo = olmaLoadingLocationRepo;
            _olmaCountryRepo = olmaCountryRepo;
            _olmaCountryStateRepo = olmaCountryStateRepo;
            _serviceProvider = serviceProvider;
        }
        
        public async Task<IWrappedResponse> Create(LoadingLocationsCreateRequest request)
        {
            var cmd = ServiceCommand<LoadingLocationAdministration, Rules.LoadingLocation.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.LoadingLocation.Create.MainRule(request))
                .Then(CreateAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.LoadingLocation.Create.MainRule mainRule)
        {
            var request = mainRule.Context.Request;

            var olmaLoadingLocation = Mapper.Map<Olma.LoadingLocation>(request);
            olmaLoadingLocation.BusinessHours = Mapper.Map<ICollection<Olma.BusinessHour>>(request.BusinessHours);
            olmaLoadingLocation.BusinessHourExceptions = Mapper.Map<ICollection<Olma.BusinessHourException>>(request.BusinessHourExceptions);
            _olmaLoadingLocationRepo.Create(olmaLoadingLocation);
            _olmaLoadingLocationRepo.Save();

            var response = _olmaLoadingLocationRepo
                .GetById<Olma.LoadingLocation, LoadingLocationAdministration>(olmaLoadingLocation.Id, true);

            return Created(response.Data);
        }

        public async Task<IWrappedResponse> Update(LoadingLocationsUpdateRequest request)
        {
            var cmd = ServiceCommand<LoadingLocationAdministration, Rules.LoadingLocation.Update.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.LoadingLocation.Update.MainRule(request))
                .Then(UpdateAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> UpdateAction(Rules.LoadingLocation.Update.MainRule mainRule)
        {
            var request = mainRule.Context.Request;
            var olmaLoadingLocation = mainRule.Context.LoadingLocation;

            Mapper.Map(request, olmaLoadingLocation);
            _olmaLoadingLocationRepo.Save();

            var loadingLocation =
                Mapper.Map<LoadingLocationAdministration>(olmaLoadingLocation);

            return Updated(loadingLocation);
        }

        public async Task<IWrappedResponse> Delete(LoadingLocationsDeleteRequest request)
        {
            var cmd = ServiceCommand<LoadingLocationAdministration, Rules.LoadingLocation.Delete.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.LoadingLocation.Delete.MainRule(request))
                .Then(DeleteAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> DeleteAction(Rules.LoadingLocation.Delete.MainRule mainRule)
        {
            var loadingLocation = mainRule.Context.LoadingLocation;
            _olmaLoadingLocationRepo.Delete(loadingLocation);
            _olmaLoadingLocationRepo.Save();
            return Deleted<LoadingLocationAdministration>(null);
        }

        public async Task<IWrappedResponse> Search(LoadingLocationsSearchRequest request)
        {
            var query = _olmaLoadingLocationRepo.FindAll()
                .IgnoreQueryFilters().Where(i => !i.IsDeleted)
                .AsNoTracking().Where(d => !d.IsDeleted);

            if (request.Id.HasValue)
            {
                query = query.Where(d => d.Id == request.Id);
                return Ok( await query.ProjectTo<LoadingLocationAdministration>(Mapper.ConfigurationProvider).FirstOrDefaultAsync());
            }

            if (request.CustomerDivisionId.HasValue)
            {
                var olmaLoadingLocations = await query.Where(d => d.CustomerDivisionId  == request.CustomerDivisionId)
                    .Include(a => a.Address).ToListAsync();
                return Ok(Mapper.Map<IEnumerable<LoadingLocationAdministration>>(olmaLoadingLocations.AsEnumerable()));
            }

            return new WrappedResponse
            {
                ResultType = ResultType.BadRequest,
                State = ErrorHandler.Create().AddMessage(new GeneralError()).GetServiceState()
            };
        }
        
        [Obsolete]
        public async Task<IWrappedResponse> GetById(int id)
        {
            #region security

            // TODO add security for GetById

            #endregion

            var response = _olmaLoadingLocationRepo.GetById<Olma.LoadingLocation,LoadingLocation>(id);

            #region Add PublicHolidays

            if (response.ResultType == ResultType.NotFound)
                return response;

            var countryStateId = response.Data.Address.State;
            var countryId = response.Data.Address.Country;
            var stateHolidays = _olmaCountryStateRepo.FindByCondition(c => c.Id == countryStateId)
                .Include(c => c.PublicHolidays)
                .SelectMany(c => c.PublicHolidays).ToList();
            var countryHolidays = _olmaCountryRepo.FindByCondition(c => c.Id == countryId)
                .Include(c => c.PublicHolidays)
                .SelectMany(c => c.PublicHolidays).ToList();

                var publicHolidays = new List<PublicHoliday>();

                if (stateHolidays.Count > 0)
                {
                    publicHolidays.AddRange(Mapper.Map<List<Olma.PublicHoliday>, List<PublicHoliday>>(stateHolidays));
                }

                if (countryHolidays.Count > 0)
                {
                    publicHolidays.AddRange(Mapper.Map<List<Olma.PublicHoliday>, List<PublicHoliday>>(countryHolidays));
                }
                response.Data.PublicHolidays = publicHolidays;

            #endregion

            return response;
        }
        [Obsolete]
        public async Task<IWrappedResponse<LoadingLocation>> Patch(int id, UpdateLoadingLocationRequest request)
        {
            #region security

            // TODO add security

            #endregion

            var response = _olmaLoadingLocationRepo.Update<Olma.LoadingLocation, UpdateLoadingLocationRequest,LoadingLocation> (id, request);

            return response;
        }
        
        [Obsolete]
        public async Task<IWrappedResponse> Search(LoadingLocationSearchRequest request)
        {
            var query = _olmaLoadingLocationRepo.FindAll()
                .AsNoTracking();

            #region security

            // TODO add security

            #endregion


            #region convert search request

            if (request.Divisions?.Count > 0)
            {
                var divisionIds = request.Divisions;
                query = query.Where(l => l.CustomerDivisionId != null && divisionIds.Contains((int)l.CustomerDivisionId));
            }

            if (request.Partners?.Count > 0)
            {
                var partnerIds = request.Partners;
                query = query.Where(l => l.CustomerPartnerId != null && partnerIds.Contains((int) l.CustomerPartnerId));
            }

            #endregion

            #region ordering

            query = query.OrderBy(l => l.Id);

            #endregion

            var projectedQuery = query.ProjectTo<LoadingLocation>(Mapper.ConfigurationProvider);

            #region Add PublicHolidays

            var statesDict = _olmaCountryStateRepo.FindAll().Include(c => c.PublicHolidays).ToDictionary(s => s.Id, s =>  s.PublicHolidays);
            var countriesDict = _olmaCountryRepo.FindAll().Include(c => c.PublicHolidays).ToDictionary(c => c.Id, c =>  c.PublicHolidays);

            var mappedResult = projectedQuery.ToPaginationResult(request);

            foreach (var loadingLocation in mappedResult.Data)
            {
                var publicHolidays = new List<PublicHoliday>();
                var stateId = loadingLocation.Address.State;
                var countryId = loadingLocation.Address.Country;
                if (stateId != null && statesDict.ContainsKey((int)stateId))
                {
                     publicHolidays.AddRange(Mapper.Map<ICollection<Olma.PublicHoliday>, ICollection<PublicHoliday>>(statesDict[(int)stateId]));
                }

                if (countryId != null && countriesDict.ContainsKey((int)countryId))
                {
                    publicHolidays.AddRange(Mapper.Map<ICollection<Olma.PublicHoliday>,ICollection<PublicHoliday>>(countriesDict[(int)countryId]));
                }
                loadingLocation.PublicHolidays = publicHolidays.AsEnumerable();
            }

            #endregion

            return Ok(mappedResult);
        }
    }
}
