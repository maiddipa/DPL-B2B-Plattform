using System.Collections.Generic;
using System.Threading.Tasks;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("load-carriers")]
    //[Authorize]
    public class LoadCarriersController : ControllerBase
    {
        private readonly ILoadCarriersService _loadCarriersService;

        public LoadCarriersController(ILoadCarriersService loadCarriersService)
        {
            _loadCarriersService = loadCarriersService;
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<LoadCarrier>>> Get()
        {
            return _loadCarriersService.GetAll().Convert<IEnumerable<LoadCarrier>>(this);
        }
    }
}