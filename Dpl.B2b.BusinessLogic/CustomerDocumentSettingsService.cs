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
    public class CustomerDocumentSettingsService : BaseService, ICustomerDocumentSettingsService
    {
        private readonly IRepository<Olma.CustomerDocumentSetting> _customerDocumentSettingRepo;
        private readonly IServiceProvider _serviceProvider;

        public CustomerDocumentSettingsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.CustomerDocumentSetting> customerDocumentSettingRepo,
            IServiceProvider serviceProvider
        ): base(authData, mapper)
        {
            _customerDocumentSettingRepo = customerDocumentSettingRepo;
            _serviceProvider = serviceProvider;
        }

        public async Task<IWrappedResponse> Search(CustomerDocumentSettingSearchRequest request)
        {
            var query = _customerDocumentSettingRepo.FindAll()
                .IgnoreQueryFilters()
                .AsNoTracking().Where(d => !d.IsDeleted);
            
            if (request.CustomerId.HasValue)
            {
               query = query.Where(d => d.CustomerId == request.CustomerId);
               return Ok(query.ProjectTo<CustomerDocumentSettings>(Mapper.ConfigurationProvider));
            }
            
            return new WrappedResponse
            {
                ResultType = ResultType.BadRequest,
                State = ErrorHandler.Create().AddMessage(new GeneralError()).GetServiceState()
            };
        }

        public async Task<IWrappedResponse> Create(CustomerDocumentSettingCreateRequest request)
        {
            var cmd = ServiceCommand<CustomerDocumentSettings, Rules.CustomerDocumentSetting.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.CustomerDocumentSetting.Create.MainRule(request))
                .Then(CreateAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.CustomerDocumentSetting.Create.MainRule mainRule)
        {
            var request = mainRule.Context.Request;

            var olmaCustomerDocumentSetting = Mapper.Map<Olma.CustomerDocumentSetting>(request);
            _customerDocumentSettingRepo.Create(olmaCustomerDocumentSetting);
            _customerDocumentSettingRepo.Save();
            var response = _customerDocumentSettingRepo
                .GetById<Olma.CustomerDocumentSetting, CustomerDocumentSettings>(olmaCustomerDocumentSetting.Id);
            return response;
        }
        
        public async Task<IWrappedResponse> Update(ValuesUpdateRequest request)
        {
            var cmd = ServiceCommand<CustomerDocumentSettings, Rules.CustomerDocumentSetting.Update.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.CustomerDocumentSetting.Update.MainRule(request))
                .Then(UpdateAction);
            
            return await cmd.Execute();
        }
        
        private async Task<IWrappedResponse> UpdateAction(Rules.CustomerDocumentSetting.Update.MainRule mainRule)
        {
            var request = mainRule.Context.Request;
            var olmaCustomerDocumentSetting = mainRule.Context.CustomerDocumentSetting;
            
            JsonConvert.PopulateObject(request.Values, olmaCustomerDocumentSetting);

            _customerDocumentSettingRepo.Save();
            
            var customerCustomerDocumentSetting =
                Mapper.Map<CustomerDocumentSettings>(olmaCustomerDocumentSetting);
            
            return Updated(customerCustomerDocumentSetting);
        }
        
        public async Task<IWrappedResponse> Delete(CustomerDocumentSettingDeleteRequest request)
        {
            var cmd = ServiceCommand<CustomerDocumentSettings, Rules.CustomerDocumentSetting.Delete.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.CustomerDocumentSetting.Delete.MainRule(request))
                .Then(DeleteAction);
            
            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> DeleteAction(Rules.CustomerDocumentSetting.Delete.MainRule mainRule)
        {
            var customerDocumentSetting = mainRule.Context.CustomerDocumentSetting;
            _customerDocumentSettingRepo.Delete(customerDocumentSetting);
            _customerDocumentSettingRepo.Save();
            return Deleted<CustomerDocumentSettings>(null);
        }
    }
}