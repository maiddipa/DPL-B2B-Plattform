using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class OrganizationsAdministrationController : ControllerBase
    {
        private readonly IOrganizationsService _organizationsService;

        public OrganizationsAdministrationController(IOrganizationsService organizationsService)
        {
            _organizationsService = organizationsService;
        }
        
        [HttpGet("{id}")]
        public Task<ActionResult<IEnumerable<OrganizationScopedDataSet>>> GetById(int id)
        {
            return _organizationsService.GetById(id).Convert<IEnumerable<OrganizationScopedDataSet>>(this);
        }

        [HttpGet("organization/{id}")]
        public Task<ActionResult<Organization>> GetOrganizationById(int id)
        {
            return _organizationsService.GetOrganizationById(id).Convert<Organization>(this);
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<Organization>>> Get()
        {
            return _organizationsService.GetAll().Convert<IEnumerable<Organization>>(this);
        }
        
        [HttpPost]
        public async Task<ActionResult<Organization>> Post([FromBody] OrganizationCreateRequest request)
        {
            return await _organizationsService.Create(request).Convert<Organization>(this);
        }

        [HttpPatch]
        public async Task<ActionResult<Organization>> Patch([FromBody] OrganizationUpdateRequest request)
        {
            return await _organizationsService.Update(request).Convert<Organization>(this);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Organization>> Delete(int id)
        {
            var request = new OrganizationDeleteRequest
            {
                Id = id
            };
            return await _organizationsService.Delete(request).Convert<Organization>(this);
        }
    }
}