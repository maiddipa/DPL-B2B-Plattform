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
    public class LoadingLocationsAdministrationController : ControllerBase
    {
        private readonly ILoadingLocationsService _loadingLocationsService;

        public LoadingLocationsAdministrationController(ILoadingLocationsService loadingLocationsService)
        {
            _loadingLocationsService = loadingLocationsService;
        }
        
        [HttpGet("division/{id}")]
        public async Task<ActionResult<IEnumerable<LoadingLocationAdministration>>> GetByCustomerDivisionId(int id)
        {
            var request = new LoadingLocationsSearchRequest
            {
                CustomerDivisionId = id
            };
            return await _loadingLocationsService.Search(request).Convert<IEnumerable<LoadingLocationAdministration>>(this);
        }
        
        
        [HttpGet("{id}")]
        public async Task<ActionResult<LoadingLocationAdministration>> GetById(int id)
        {
            var request = new LoadingLocationsSearchRequest
            {
                Id = id
            };
            return await _loadingLocationsService.Search(request).Convert<LoadingLocationAdministration>(this);
        }

        [HttpPost]
        public async Task<ActionResult<LoadingLocationAdministration>> Post([FromBody] LoadingLocationsCreateRequest request)
        {
            return await _loadingLocationsService.Create(request).Convert<LoadingLocationAdministration>(this);
        }

        [HttpPatch]
        public async Task<ActionResult<LoadingLocationAdministration>> Patch([FromBody] LoadingLocationsUpdateRequest request)
        {
            return await _loadingLocationsService.Update(request).Convert<LoadingLocationAdministration>(this);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<LoadingLocationAdministration>> Delete(int id)
        {
            var request = new LoadingLocationsDeleteRequest
            {
                Id = id
            };
            return await _loadingLocationsService.Delete(request).Convert<LoadingLocationAdministration>(this);
        }
    }
}