using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    [Route("division-document-settings")]
    [Authorize(Roles = "Manager")]
    public class DivisionDocumentSettingsAdministrationController : ControllerBase
    {
        private readonly IDivisionDocumentSettingsService _divisionDocumentSettingsService;

        public DivisionDocumentSettingsAdministrationController(IDivisionDocumentSettingsService documentSettingsService)
        {
            _divisionDocumentSettingsService = documentSettingsService;
        }
        
        [HttpGet("document-types")]
        public async Task<ActionResult<IEnumerable<DocumentType>>> GetDocumentTypes()
        {
            return await _divisionDocumentSettingsService.GetDocumentTypes().Convert<IEnumerable<DocumentType>>(this);
        }
        

        [HttpGet]
        public async Task<LoadResult> Get(DataSourceLoadOptions loadOptions, [FromQuery] CustomerDivisionDocumentSettingSearchRequest request)
        {
            var response = await _divisionDocumentSettingsService
                    .Search(request) as WrappedResponse<IQueryable<CustomerDivisionDocumentSetting>>;
            return await DataSourceLoader.LoadAsync(response?.Data, loadOptions);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDivisionDocumentSetting>> Post([FromForm] string values)
        {
            var request = new CustomerDivisionDocumentSettingCreateRequest();
            JsonConvert.PopulateObject(values, request);
            return await _divisionDocumentSettingsService.Create(request).Convert<CustomerDivisionDocumentSetting>(this);
        }
        
        [HttpPut]
        public async Task<ActionResult<CustomerDivisionDocumentSetting>> Put([FromForm] int key, [FromForm] string values)
        {
            var request = new ValuesUpdateRequest
            {
                Key = key,
                Values = values
            };
            
            return await _divisionDocumentSettingsService.Update(request).Convert<CustomerDivisionDocumentSetting>(this);
        }
        
        [HttpDelete]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<CustomerDivisionDocumentSetting>> Delete([FromForm] int key)
        {
            var request = new CustomerDivisionDocumentSettingDeleteRequest {Id = key };
            return await _divisionDocumentSettingsService.Delete(request).Convert<CustomerDivisionDocumentSetting>(this);
        }
    }
}