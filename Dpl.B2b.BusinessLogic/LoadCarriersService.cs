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
using Z.EntityFramework.Plus;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class LoadCarriersService : BaseService, ILoadCarriersService
    {
        private readonly IRepository<Olma.LoadCarrier> _olmaLoadCarrierRepo;

        public LoadCarriersService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.LoadCarrier> olmaLoadCarrierRepo
            ) : base(authData, mapper)
        {
            _olmaLoadCarrierRepo = olmaLoadCarrierRepo;
        }

        public async Task<IWrappedResponse> GetAll()
        {
            var query = _olmaLoadCarrierRepo.FindAll()
                .AsNoTracking();

            var loadCarriers = query
                .OrderBy(l => l.Type.Order).ThenBy(i => i.Quality.Order)
                .ProjectTo<LoadCarrier>(Mapper.ConfigurationProvider)
                .FromCache();

            //var loadCarriers = Mapper.Map<IEnumerable<LoadCarrier>>(query);
            return Ok(loadCarriers);
        }
    }

    //TODO check necessity
    #region alternative and further class definitions
    //public class LoadCarriersService : ILoadCarriersService
    //{
    //    private readonly IMapper _mapper;
    //    private readonly ILamaBookingLoadCarrierDataService _lama;

    //    public LoadCarriersService(
    //        IMapper mapper,
    //        ILamaBookingLoadCarrierDataService lama
    //    )
    //    {
    //        _mapper = mapper;
    //        _lama = lama;
    //    }

    //    public async Task<IPaginationResult<LoadCarrier>> Search(LoadCarrierSearchRequest request)
    //    {
    //        var result = await _lama.Find(request);
    //        var mappedResult = _mapper.Map<PaginationResult<LoadCarrier>>(result);
    //        return mappedResult;
    //    }

    //    public async Task<LoadCarrier> GetById(int id)
    //    {
    //        var result = await _lama.GetById(id);
    //        return _mapper.Map<LoadCarrier>(result);
    //    }
    //}








    //public class LoadCarriersManipulateQueryService : IManipulateQuery<Lama.LoadCarrier, LoadCarrierSearchRequest>
    //{
    //    IQueryable<Lama.LoadCarrier> IManipulateQuery<Lama.LoadCarrier, LoadCarrierSearchRequest>.ManipulateQuery(IQueryable<Lama.LoadCarrier> query, LoadCarrierSearchRequest request)
    //    {
    //        return query;
    //    }
    //}
    #endregion
}
