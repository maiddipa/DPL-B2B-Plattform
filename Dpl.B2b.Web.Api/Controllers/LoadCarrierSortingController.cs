using System.Collections.Generic;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("load-carrier-sortings")]
    [Authorize]
    public class LoadCarrierSortingController : ControllerBase
    {
        private readonly ILoadCarrierSortingService _loadCarrierSortingService;

        public LoadCarrierSortingController(ILoadCarrierSortingService loadCarriersService)
        {
            _loadCarrierSortingService = loadCarriersService;
        }

        [HttpPost]
        [HasPermission(typeof(AttributeRequirement<CanCreateLoadCarrierReceiptRequirement>))] //TODO Create a separate Requirement for sorting
        public Task<ActionResult<LoadCarrierSorting>> Post(
            [FromBody] LoadCarrierSortingCreateRequest request)
        {
            return this._loadCarrierSortingService.Create(request).Convert<LoadCarrierSorting>(this);
        }

        [HttpGet]
        [HasPermission(typeof(AttributeRequirement<CanReadLoadCarrierReceiptRequirement>))] //TODO Create a separate Requirement for sorting
        public async Task<ActionResult<LoadCarrierSorting>> GetByLoadCarrierReceiptId(int loadCarrierReceiptId)
        {
            var response = _loadCarrierSortingService.GetByLoadCarrierReceiptId(loadCarrierReceiptId);
            return await response.Convert<LoadCarrierSorting>(this);
        }
    }
}