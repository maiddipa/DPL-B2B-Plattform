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
    [Route("[controller]")]
    //[Authorize]
    public class OrderMatchesController : ControllerBase
    {
        private readonly IOrderMatchesService _orderMatchesService;

        public OrderMatchesController(IOrderMatchesService orderMatchesService)
        {
            _orderMatchesService = orderMatchesService;
        }

        [HttpGet]
        [System.Obsolete]
        public Task<ActionResult<IPaginationResult<OrderMatch>>> Get([FromQuery] OrderMatchesSearchRequest request)
        {
            return this._orderMatchesService.Search(request).Convert(this);
        }

        [HttpGet("{id}")]
        [System.Obsolete]
        public Task<ActionResult<OrderMatch>> GetById(int id)
        {
            return this._orderMatchesService.GetById(id).Convert(this);
        }

        [HttpPost]
        public Task<ActionResult<OrderMatch>> Post([FromBody] OrderMatchesCreateRequest request)
        {
            return this._orderMatchesService.Create(request).Convert<OrderMatch>(this);
        }

        [HttpPatch("{id}")]
        [System.Obsolete]
        public Task<ActionResult<OrderMatch>> Patch(int id, [FromBody] OrderMatchesUpdateRequest request)
        {
            return this._orderMatchesService.Update(id, request).Convert(this);
        }

        [HttpPatch("{id}/cancel")]
        [System.Obsolete]
        public Task<ActionResult<OrderMatch>> PatchOrderMatchCancelled(int id)
        {
            return this._orderMatchesService.Cancel(id).Convert(this);
        }
    }
}