using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Contracts;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    //[Authorize]
    [System.Obsolete]
    public class OrdersSummariesController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersSummariesController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<OrderSummary>>> Get([FromQuery] OrderSearchRequest request)
        {
            return this._ordersService.Summary(request).Convert<IEnumerable<OrderSummary>>(this);
        }
    }
}
