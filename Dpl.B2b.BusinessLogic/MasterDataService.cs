using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class MasterDataService : BaseService, IMasterDataService
    {
        private readonly IRepository<Olma.Country> _olmaCountryRepo;
        private readonly IRepository<Olma.DocumentState> _olmaDocumentStateRepo;
        private readonly IRepository<Olma.LocalizationLanguage> _olmaLanguagesRepo;
        private readonly IRepository<Olma.LoadCarrier> _olmaLoadCarrierRepo;
        private readonly IRepository<Olma.VoucherReasonType> _olmaVoucherReasonTypeRepo;
        private readonly ILoadCarriersService _loadCarriersService;

        public MasterDataService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.Country> olmaCountryRepo,
            IRepository<Olma.DocumentState> olmaDocumentStateRepo,
            IRepository<Olma.LocalizationLanguage> olmaLanguagesRepo,
            IRepository<Olma.LoadCarrier> olmaLoadCarrierRepo,
            IRepository<Olma.VoucherReasonType> olmaVoucherReasonTypeRepo,
            ILoadCarriersService loadCarriersService
        ) : base(authData, mapper)
        {
            _olmaCountryRepo = olmaCountryRepo;
            _olmaDocumentStateRepo = olmaDocumentStateRepo;
            _olmaLanguagesRepo = olmaLanguagesRepo;
            _olmaLoadCarrierRepo = olmaLoadCarrierRepo;
            _olmaVoucherReasonTypeRepo = olmaVoucherReasonTypeRepo;
            _loadCarriersService = loadCarriersService;
        }

        public async Task<IWrappedResponse> Get()
        {
            var countries = _olmaCountryRepo.FindAll()
                .AsNoTracking()
                .ProjectTo<Country>(Mapper.ConfigurationProvider)
                .FromCache()
                .ToList();

            var documentStates = _olmaDocumentStateRepo.FindAll()
                .AsNoTracking()
                .ProjectTo<DocumentState>(Mapper.ConfigurationProvider)
                .FromCache()
                .ToList();

            var languages = _olmaLanguagesRepo.FindAll()
                .AsNoTracking()
                .ProjectTo<Language>(Mapper.ConfigurationProvider)
                .FromCache()
                .ToList();

            var loadCarriersServiceResponse = (IWrappedResponse<IEnumerable<LoadCarrier>>) _loadCarriersService.GetAll().Result;
            var loadCarriers =  loadCarriersServiceResponse.Data;

            var voucherReasonTypes = _olmaVoucherReasonTypeRepo.FindAll()
                .AsNoTracking()
                .OrderBy(v => v.Order)
                .ProjectTo<VoucherReasonType>(Mapper.ConfigurationProvider)
                .FromCache()
                .ToList();


            var masterData = new MasterData()
            {
                Countries = countries,
                DocumentStates = documentStates,
                Languages = languages,
                LoadCarriers = loadCarriers,
                VoucherReasonTypes = voucherReasonTypes
            };

            return Ok(masterData);
        }
    }
}
