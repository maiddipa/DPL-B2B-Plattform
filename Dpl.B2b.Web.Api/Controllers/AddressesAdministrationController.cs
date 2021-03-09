using System.Collections.Generic;
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
    [Route("[controller]")]
    [Authorize(Roles = "Manager")]
    public class AddressesAdministrationController : ControllerBase
    {
        private readonly IAddressesService _addressesService;

        public AddressesAdministrationController(IAddressesService addressesService)
        {
            _addressesService = addressesService;
        }
        
        [HttpGet("countries")]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            return await _addressesService.GetCountries().Convert<IEnumerable<Country>>(this);
        }
        
        [HttpGet("{id}/states")]
        public async Task<ActionResult<IEnumerable<CountryState>>> GetStatesByCountryId(int id)
        {
            return await _addressesService.GetStatesByCountryId(id).Convert<IEnumerable<CountryState>>(this);
        }

        [HttpGet]
        public async Task<LoadResult> Get(DataSourceLoadOptions loadOptions, [FromQuery] AddressesSearchRequest request)
        {
            var response = await _addressesService.Search(request) as WrappedResponse<IQueryable<AddressAdministration>>;
            return await DataSourceLoader.LoadAsync(response?.Data, loadOptions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddressAdministration>> GetById(int id)
        {
            var request = new AddressesSearchRequest {Id = id};
            return await _addressesService.Search(request).Convert<AddressAdministration>(this);
        }

        [HttpPost]
        public async Task<ActionResult<AddressAdministration>> Post([FromForm] string values)
        {
            var request = new AddressesCreateRequest();
            JsonConvert.PopulateObject(values, request);
            return await _addressesService.Create(request).Convert<AddressAdministration>(this);
        }

        [HttpPut]
        public async Task<ActionResult<AddressAdministration>> Put([FromForm] int key, [FromForm] string values)
        {
            var request = new ValuesUpdateRequest
            {
                Key = key,
                Values = values
            };

            return await _addressesService.Update(request).Convert<AddressAdministration>(this);
        }

        [HttpDelete]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<AddressAdministration>> Delete([FromForm] int key)
        {
            var request = new AddressesDeleteRequest
            {
                Id = key
            };
            return await _addressesService.Delete(request).Convert<AddressAdministration>(this);
        }
    }
}