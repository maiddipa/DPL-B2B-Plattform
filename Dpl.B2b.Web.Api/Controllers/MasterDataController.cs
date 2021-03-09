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
    [Route("master-data")]
    [Authorize]
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterDataService _masterDataService;
        private readonly IPermissionsService _permissionsService;

        public MasterDataController(IMasterDataService masterDataService, IPermissionsService permissionsService)
        {
            _masterDataService = masterDataService;
            _permissionsService = permissionsService;
        }

        [HttpGet]
        public Task<ActionResult<MasterData>> Get()
        {
            return this._masterDataService.Get().Convert<MasterData>(this);
        }
        
        [HttpPatch("updateCache")]
        [Authorize(Roles = "Manager")]
        public void UpdateCache()
        {
            this._permissionsService.UpdateCache();
        }
    }
}