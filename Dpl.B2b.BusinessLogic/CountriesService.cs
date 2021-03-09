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
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;
using Ltms = Dpl.B2b.Dal.Ltms;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class CountriesService : BaseService, ICountriesService
    {
        private readonly IRepository<Olma.Country> _olmaCountryRepo;

        public CountriesService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.Country> olmaCountryRepo
        ) : base(authData, mapper)
        {
            _olmaCountryRepo = olmaCountryRepo;
        }

        public async Task<IWrappedResponse> GetAll()
        {
            var query = _olmaCountryRepo.FindAll()
                .AsNoTracking();

            var projectedQuery = query.ProjectTo<Country>(Mapper.ConfigurationProvider);

            var mappedResult = projectedQuery
                .ToList()
                .AsEnumerable();

            return Ok(mappedResult);
        }
    }
}
