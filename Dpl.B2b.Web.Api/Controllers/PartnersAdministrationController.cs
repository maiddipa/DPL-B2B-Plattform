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

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    [Authorize(Roles = "Manager")]
    public class PartnersAdministrationController : ControllerBase
    {
        private readonly IPartnersService _partnersService;

        public PartnersAdministrationController(IPartnersService partnersService)
        {
            _partnersService = partnersService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Partner>> GetById(int id)
        {
            var request = new PartnersSearchRequest
            {
                Id = id
            };
            return await _partnersService.Search(request).Convert<Partner>(this);
        }
        
        [HttpGet("company/{name}")]
        public async Task<ActionResult<IEnumerable<Partner>>> GetByName(string name)
        {
            var request = new PartnersSearchRequest
            {
                CompanyName = name
            };
            return await _partnersService.Search(request).Convert<IEnumerable<Partner>>(this);
        }

        [HttpGet]
        public async Task<LoadResult> Get(DataSourceLoadOptions loadOptions)
        {
            var response = (WrappedResponse<IQueryable<Partner>>) await _partnersService.Search(new PartnersSearchRequest());
            return await DataSourceLoader.LoadAsync(response.Data, loadOptions);
        }
        
        [HttpPost]
        public async Task<ActionResult<Partner>> Post([FromBody] PartnersCreateRequest request)
        {
            return await _partnersService.Create(request).Convert<Partner>(this);
        }

        [HttpPut]
        public async Task<ActionResult<Partner>> Put([FromForm] int key, [FromForm] string values)
        {
            var request = new ValuesUpdateRequest
            {
                Key = key,
                Values = values
            };
            
            return await _partnersService.Update(request).Convert<Partner>(this);
        }

        [HttpDelete]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<Partner>> Delete([FromForm] int key)
        {
            var request = new PartnersDeleteRequest {Id = key};
            return await _partnersService.Delete(request).Convert<Partner>(this);
        }
    }
}