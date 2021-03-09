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
    public class DocumentSettingsService : BaseService, IDocumentSettingsService
    {
        private readonly IRepository<Olma.Organization> _olmaCustomerRepo;
        private readonly IRepository<Olma.CustomerDocumentSetting> _olmaCustomerDocumentSettingRepo;
        private readonly IRepository<Olma.CustomerDivisionDocumentSetting> _olmaCustomerDivisionDocumentSettingRepo;

        public DocumentSettingsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.Organization> olmaCustomerRepo,
            IRepository<Olma.CustomerDocumentSetting> olmaCustomerDocumentSettingRepo,
            IRepository<Olma.CustomerDivisionDocumentSetting> olmaCustomerDivisionDocumentSettingRepo
        )
            : base(authData, mapper)
        {
            _olmaCustomerRepo = olmaCustomerRepo;
            _olmaCustomerDocumentSettingRepo = olmaCustomerDocumentSettingRepo;
            _olmaCustomerDivisionDocumentSettingRepo = olmaCustomerDivisionDocumentSettingRepo;
        }

        public async Task<IWrappedResponse> GetAll()
        {
            var query = _olmaCustomerRepo.FindAll()
                .AsNoTracking();

            var projectedQuery = query.ProjectTo<DocumentSettings>(Mapper.ConfigurationProvider);
            var result = projectedQuery
                // there needs to be exactly one entry otherwise something went wrong
                .Single();

            return Ok(result);
        }

        public async Task<IWrappedResponse> UpdateCustomerDocumentSettings(int id,
            CustomerDocumentSettingsUpdateRequest request)
        {
            #region security

            // TODO implement security for updating customer document settings via calling authorize manually

            #endregion

            var result = _olmaCustomerDocumentSettingRepo
                .Update<Olma.CustomerDocumentSetting, CustomerDocumentSettingsUpdateRequest, CustomerDocumentSettings>(id, request);

            return result;
        }

        public async Task<IWrappedResponse> UpdateDivisionCustomerDocumentSettings(int id,
            CustomerDivisionDocumentSettingsUpdateRequest request)
        {
            #region security

            // TODO implement security for updating customer division document settings via calling authorize manually

            #endregion

            var result = _olmaCustomerDivisionDocumentSettingRepo
                .Update<Olma.CustomerDivisionDocumentSetting, CustomerDivisionDocumentSettingsUpdateRequest, CustomerDivisionDocumentSettings>(id, request);

            return result;
        }
    }
}
