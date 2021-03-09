using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    //[Authorize]
    public class PartnersController : ControllerBase
    {
        private readonly ICustomerPartnersService _customerPartnersService;

        public PartnersController(ICustomerPartnersService customerPartnersService)
        {
            _customerPartnersService = customerPartnersService;
        }

        [HttpGet]
        public Task<ActionResult<IPaginationResult<CustomerPartner>>> Get([FromQuery] CustomerPartnersSearchRequest request)
        {
            return this._customerPartnersService.Search(request).Convert<IPaginationResult<CustomerPartner>>(this);
        }

        [HttpGet("{id}")]
        public Task<ActionResult<CustomerPartner>> GetById(int id)
        {
            return this._customerPartnersService.GetById(id).Convert<CustomerPartner>(this);
        }

        [HttpPost]
        public Task<ActionResult<CustomerPartner>> Post([FromBody] CustomerPartnersCreateRequest request)
        {
            return this._customerPartnersService.Create(request).Convert<CustomerPartner>(this);
        }

        [HttpPatch("{id}")]
        [System.Obsolete]
        public Task<ActionResult<CustomerPartner>> Patch(int id, [FromBody] CustomerPartnersUpdateRequest request)
        {
            return this._customerPartnersService.Update(id, request).Convert(this);
        }

        [HttpDelete("{id}")]
        [System.Obsolete]
        public Task<ActionResult> Delete(int id)
        {
            return this._customerPartnersService.Delete(id).Convert(this);
        }
    }
}
