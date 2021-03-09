using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class DivisionDocumentSettingsService : BaseService, IDivisionDocumentSettingsService
    {
        private readonly IRepository<Olma.CustomerDivisionDocumentSetting> _customerDivisionDocumentSettingRepo;
        private readonly IRepository<Olma.DocumentType> _documentTypeRepo;
        private readonly IServiceProvider _serviceProvider;

        public DivisionDocumentSettingsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.CustomerDivisionDocumentSetting> customerDivisionDocumentSettingRepo,
            IRepository<Olma.DocumentType> documentTypeRepo,
            IServiceProvider serviceProvider
        ): base(authData, mapper)
        {
            _customerDivisionDocumentSettingRepo = customerDivisionDocumentSettingRepo;
            _documentTypeRepo = documentTypeRepo;
            _serviceProvider = serviceProvider;
        }

        public async Task<IWrappedResponse> GetDocumentTypes()
        {
            var documentTypes = await _documentTypeRepo.FindAll().AsNoTracking().ToListAsync();
            return Ok(Mapper.Map<IEnumerable<DocumentType>>(documentTypes.AsEnumerable()));
        }

        public async Task<IWrappedResponse> Search(CustomerDivisionDocumentSettingSearchRequest request)
        {
            var query = _customerDivisionDocumentSettingRepo.FindAll()
                .IgnoreQueryFilters()
                .AsNoTracking().Where(d => !d.IsDeleted);
            
            if (request.CustomerDivisionId.HasValue)
            {
               query = query.Where(d => d.DivisionId == request.CustomerDivisionId);
               return Ok(query.ProjectTo<CustomerDivisionDocumentSetting>(Mapper.ConfigurationProvider));
            }
            
            return new WrappedResponse
            {
                ResultType = ResultType.BadRequest,
                State = ErrorHandler.Create().AddMessage(new GeneralError()).GetServiceState()
            };
        }

        public async Task<IWrappedResponse> Create(CustomerDivisionDocumentSettingCreateRequest request)
        {
            var cmd = ServiceCommand<CustomerDivisionDocumentSetting, Rules.DivisionDocumentSetting.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.DivisionDocumentSetting.Create.MainRule(request))
                .Then(CreateAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.DivisionDocumentSetting.Create.MainRule mainRule)
        {
            var request = mainRule.Context.Request;

            var olmaCustomerDivisionDocumentSetting = Mapper.Map<Olma.CustomerDivisionDocumentSetting>(request);
            _customerDivisionDocumentSettingRepo.Create(olmaCustomerDivisionDocumentSetting);
            _customerDivisionDocumentSettingRepo.Save();
            var response = _customerDivisionDocumentSettingRepo
                .GetById<Olma.CustomerDivisionDocumentSetting, CustomerDivisionDocumentSetting>(olmaCustomerDivisionDocumentSetting.Id);
            return response;
        }
        
        public async Task<IWrappedResponse> Update(ValuesUpdateRequest request)
        {
            var cmd = ServiceCommand<CustomerDivisionDocumentSetting, Rules.DivisionDocumentSetting.Update.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.DivisionDocumentSetting.Update.MainRule(request))
                .Then(UpdateAction);
            
            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> UpdateAction(Rules.DivisionDocumentSetting.Update.MainRule mainRule)
        {
            var request = mainRule.Context.Request;
            var olmaCustomerDivisionDocumentSetting = mainRule.Context.CustomerDivisionDocumentSetting;
            
            JsonConvert.PopulateObject(request.Values, olmaCustomerDivisionDocumentSetting);

            _customerDivisionDocumentSettingRepo.Save();
            
            var customerDivisionDocumentSetting =
                Mapper.Map<CustomerDivisionDocumentSetting>(olmaCustomerDivisionDocumentSetting);
            
            return Updated(customerDivisionDocumentSetting);
        }
        
        public async Task<IWrappedResponse> Delete(CustomerDivisionDocumentSettingDeleteRequest request)
        {
            var cmd = ServiceCommand<CustomerDivisionDocumentSetting, Rules.DivisionDocumentSetting.Delete.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.DivisionDocumentSetting.Delete.MainRule(request))
                .Then(DeleteAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> DeleteAction(Rules.DivisionDocumentSetting.Delete.MainRule mainRule)
        {
            var customerDivisionDocumentSetting = mainRule.Context.CustomerDivisionDocumentSetting;
            _customerDivisionDocumentSettingRepo.Delete(customerDivisionDocumentSetting);
            _customerDivisionDocumentSettingRepo.Save();
            return Deleted<CustomerDivisionDocumentSetting>(null);
        }
    }
}