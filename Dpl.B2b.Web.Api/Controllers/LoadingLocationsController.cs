using System.Threading.Tasks;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("loading-locations")]
    //[Authorize]
    public class LoadingLocationsController : ControllerBase
    {
        private readonly ILoadingLocationsService _loadingLocationsService;

        public LoadingLocationsController(ILoadingLocationsService loadingLocationsService)
        {
            _loadingLocationsService = loadingLocationsService;
        }

        [HttpGet]
        public Task<ActionResult<IPaginationResult<LoadingLocation>>> Get([FromBody] LoadingLocationSearchRequest request)
        {
            return this._loadingLocationsService.Search(request).Convert<IPaginationResult<LoadingLocation>>(this);
        }

        [HttpGet("{id}")]
        public Task<ActionResult<LoadingLocation>> GetById(int id)
        {
            return this._loadingLocationsService.GetById(id).Convert<LoadingLocation>(this);
        }

        [HttpPatch("{id}")]
        public Task<ActionResult<LoadingLocation>> Patch(int id, [FromBody] UpdateLoadingLocationRequest request)
        {
            return this._loadingLocationsService.Patch(id, request).Convert(this);
        }
    }
}