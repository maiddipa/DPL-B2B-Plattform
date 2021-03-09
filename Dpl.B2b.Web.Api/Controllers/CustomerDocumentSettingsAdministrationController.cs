using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("customer-document-settings")]
    [Authorize(Roles = "Manager")]
    public class CustomerDocumentSettingsAdministrationController : ControllerBase
    {
        private readonly ICustomerDocumentSettingsService _customerDocumentSettingsService;

        public CustomerDocumentSettingsAdministrationController(ICustomerDocumentSettingsService documentSettingsService)
        {
            _customerDocumentSettingsService = documentSettingsService;
        }

        [HttpGet]
        public async Task<LoadResult> Get(DataSourceLoadOptions loadOptions, [FromQuery] CustomerDocumentSettingSearchRequest request)
        {
            var response = await _customerDocumentSettingsService
                    .Search(request) as WrappedResponse<IQueryable<CustomerDocumentSettings>>;
            return await DataSourceLoader.LoadAsync(response?.Data, loadOptions);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDocumentSettings>> Post([FromForm] string values)
        {
            var request = new CustomerDocumentSettingCreateRequest();
            JsonConvert.PopulateObject(values, request);
            return await _customerDocumentSettingsService.Create(request).Convert<CustomerDocumentSettings>(this);
        }
        
        [HttpPut]
        public async Task<ActionResult<CustomerDocumentSettings>> Put([FromForm] int key, [FromForm] string values)
        {
            var request = new ValuesUpdateRequest
            {
                Key = key,
                Values = values
            };
            
            return await _customerDocumentSettingsService.Update(request).Convert<CustomerDocumentSettings>(this);
        }
        
        [HttpDelete]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<CustomerDocumentSettings>> Delete([FromForm] int key)
        {
            var request = new CustomerDocumentSettingDeleteRequest {Id = key };
            return await _customerDocumentSettingsService.Delete(request).Convert<CustomerDocumentSettings>(this);
        }
    }
}