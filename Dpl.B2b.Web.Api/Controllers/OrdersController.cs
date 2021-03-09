using System.Collections.Generic;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Authorization;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    //[Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IAuthorizationDataService _authData;

        public OrdersController(IOrdersService ordersService, IAuthorizationDataService authData)
        {
            _ordersService = ordersService;
            _authData = authData;
        }

        [HttpGet]
        [HasPermission(typeof(AttributeRequirement<CanReadOrderRequirement>))]
        public Task<ActionResult<IPaginationResult<Order>>> Search([FromQuery] OrderSearchRequest request)
        {
            return this._ordersService.Search(request).Convert<IPaginationResult<Order>>(this);
        }

        [HttpGet("{id}")]
        [HasPermission(typeof(AttributeRequirement<CanReadOrderRequirement>))]
        [System.Obsolete]
        public Task<ActionResult<Order>> GetById(int id)
        {
            return this._ordersService.GetById(id).Convert(this);
        }

        // TODO CHeck if we actually need this
        [HttpPatch("{id}")]
        [HasPermission(typeof(AttributeRequirement<CanUpdateOrderRequirement>))]
        [System.Obsolete]
        public Task<ActionResult<Order>> Patch(int id, [FromBody] OrderUpdateRequest request)
        {
            if (request != null && request.DplNote != null && _authData.GetUserRole() == UserRole.DplEmployee)
            {
                request.DplNote.UserId = _authData.GetUserId();
            }
            return this._ordersService.Update(id, request).Convert(this);
        }

        [HttpPatch("{id}/cancel")]
        [HasPermission(typeof(AttributeRequirement<CanCancelOrderRequirement>))]
        public Task<ActionResult<Order>> Cancel(int id, OrderCancelRequest request)
        {
            if (request?.DplNote != null && _authData.GetUserRole() == UserRole.DplEmployee)
            {
                request.DplNote.UserId = _authData.GetUserId();
            }
            
            return this._ordersService.Cancel(id, request).Convert<Order>(this);
        }
    }
}