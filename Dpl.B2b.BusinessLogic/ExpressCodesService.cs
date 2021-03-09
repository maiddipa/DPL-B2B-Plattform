using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExpress.DataProcessing;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.Rules.ExpressCode.Search;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class ExpressCodesService : BaseService, IExpressCodesService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IRepository<Olma.ExpressCode> _olmaExpressCodeRepo;
        private readonly IRepository<Olma.PartnerPreset> _olmaPartnerPresetRepo;
        private readonly IRepository<Olma.CustomerPartner> _olmaCustomerPartnerRepo;
        private readonly IRepository<Olma.PartnerDirectory> _olmaPartnerDirectoryRepo;

        public ExpressCodesService(
            IAuthorizationDataService authData,
            IMapper mapper, 
            IRepository<Olma.ExpressCode> olmaExpressCodeRepo,
            IRepository<Olma.PartnerPreset> olmaPartnerPresetRepo,
            IRepository<Olma.CustomerPartner> olmaCustomerPartnerRepository, 
            IRepository<Olma.PartnerDirectory> olmaPartnerDirectoryRepo, 
            IServiceProvider serviceProvider) : base(authData, mapper)
        {
            _olmaExpressCodeRepo = olmaExpressCodeRepo;
            _olmaPartnerPresetRepo = olmaPartnerPresetRepo;
            _olmaCustomerPartnerRepo = olmaCustomerPartnerRepository;
            _olmaPartnerDirectoryRepo = olmaPartnerDirectoryRepo;
            _serviceProvider = serviceProvider;
        }

        // nachsehen
        public async Task<IWrappedResponse> GetAll()
        {
            #region Security
            //TODO Check Security Implementation for GetAll
            var customerIds = AuthData.GetCustomerIds();
            var query = _olmaExpressCodeRepo.FindByCondition(e => customerIds.Contains(e.IssuingCustomer.Id)).AsNoTracking();

            #endregion
            
            #region ordering

            query.OrderBy(e => e.DigitalCode);

            #endregion

            #region Projektion Mapping

            var projectedQuery = query.ProjectTo<ExpressCode>(Mapper.ConfigurationProvider);
            var result = projectedQuery.ToList().AsEnumerable();

            #endregion

            return Ok(result);
        }
        
        public async Task<IWrappedResponse> Cancel(int id)
        {

            var cmd = ServiceCommand<IWrappedResponse, Rules.ExpressCode.Cancel.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.ExpressCode.Cancel.MainRule(id))
                .Then(CancelAction);

            return await cmd.Execute();

        }

        private async Task<IWrappedResponse> CancelAction(Rules.ExpressCode.Cancel.MainRule rule)
        {
            var expressCode = rule.Context.ExpressCode;
            var id = expressCode.Id;

            expressCode.IsCanceled = true;
            var response =
                _olmaExpressCodeRepo.Update<Olma.ExpressCode, Olma.ExpressCode, ExpressCode>(id, expressCode);
            return response;
        }

        public async Task<IWrappedResponse> GetByCode(ExpressCodesSearchRequest request)
        {
            var cmd = ServiceCommand<IWrappedResponse, Rules.ExpressCode.GetByCode.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.ExpressCode.GetByCode.MainRule(request))
                .Then(GetByCodeAction);
            return await cmd.Execute();

        }

        private async Task<IWrappedResponse> GetByCodeAction(Rules.ExpressCode.GetByCode.MainRule rule)
        {
            return Ok(rule.Context.DigitalCode);
        }

        // noch nicht
        public async Task<IWrappedResponse> Create(ExpressCodeCreateRequest request)
        {
            #region Security 

            //TODO Implement security Create 

            #endregion

            var result = 
                _olmaExpressCodeRepo.Create<Olma.ExpressCode, ExpressCodeCreateRequest, ExpressCode>(request);
            return result;
        }

        // noch nicht
        public async Task<IWrappedResponse> Update(int id, ExpressCodeUpdateRequest request)
        {
            #region Security

            //TODO Implement security Update

            #endregion

            var result =
                _olmaExpressCodeRepo.Update<Olma.ExpressCode, ExpressCodeUpdateRequest, ExpressCode>(id,request);
            return result;
        }
       
        public async Task<IWrappedResponse> Search(ExpressCodesSearchRequest request)
        {
            var cmd = ServiceCommand<IWrappedResponse, Rules.ExpressCode.Search.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.ExpressCode.Search.MainRule(request))
                .Then(SearchAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> SearchAction(Rules.ExpressCode.Search.MainRule rule)
        {
            var projectedQuery = rule.Context.Query.ProjectTo<ExpressCode>(Mapper.ConfigurationProvider);
            var mappedResult = projectedQuery.ToPaginationResult(rule.Context.ExpressCodesSearchRequest);

            foreach (var expressCode in mappedResult.Data)
            {
                expressCode.VoucherPresets = GetVoucherExpressCodeDetails(expressCode.Id);
            }

            return Ok(mappedResult);
        }

        #region Helper


        private VoucherExpressCodeDetails GetVoucherExpressCodeDetails(int expressCodeId)
        {
            var partnerPresets = _olmaPartnerPresetRepo.FindByCondition(p => p.ExpressCode.Id == expressCodeId)
                .Include(nameof(Olma.Partner))
                .Include(p => p.Partner.DefaultAddress);
            if (!partnerPresets.Any()) return null;

            var voucherPreset = new VoucherExpressCodeDetails();
            foreach (var preset in partnerPresets)
            {
                switch (preset.Type)
                {
                    case Olma.ExpressCodePresetType.Recipient:
                        voucherPreset.Recipient = Mapper.Map<CustomerPartner>(preset.Partner); //TODO Make shure, none of them are allready set (Double Entries)
                        break;
                    case Olma.ExpressCodePresetType.Supplier:
                        voucherPreset.Supplier = Mapper.Map<CustomerPartner>(preset.Partner);
                        break;
                    case Olma.ExpressCodePresetType.Shipper:
                        voucherPreset.Shipper = Mapper.Map<CustomerPartner>(preset.Partner);
                        break;
                    case Olma.ExpressCodePresetType.SubShipper:
                        voucherPreset.SubShipper = Mapper.Map<CustomerPartner>(preset.Partner);
                        break;
                }
            }
            return voucherPreset;
        }
        #endregion
    }

    //TODO check necessity of alternative class defintions
    #region alternative and further class definitions
    //public class ExpressCodesService : IExpressCodesService
    //{
    //    private readonly IMapper _mapper;
    //    private readonly ILamaExpressCodeDataService _lama;

    //    public ExpressCodesService(
    //        IMapper mapper,
    //        ILamaExpressCodeDataService lama
    //    )
    //    {
    //        _mapper = mapper;
    //        _lama = lama;
    //    }
    //}






    //public class ExpressCodesManipulateQueryService : IManipulateQuery<Lama.ExpressCode, ExpressCodeSearchRequest>
    //{
    //    IQueryable<Lama.ExpressCode> IManipulateQuery<Lama.ExpressCode, ExpressCodeSearchRequest>.ManipulateQuery(IQueryable<Lama.ExpressCode> query, ExpressCodeSearchRequest request)
    //    {
    //        return query;
    //    }
    //}
    #endregion
}
