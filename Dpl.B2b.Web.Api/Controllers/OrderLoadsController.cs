using System;
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
    [Route("order-loads")]
    //[Authorize]
    public class OrderLoadsController : ControllerBase
    {
        private readonly IOrderLoadsService _ordersLoadsService;
        private readonly IAuthorizationDataService _authData;

        public OrderLoadsController(IOrderLoadsService ordersLoadsService, IAuthorizationDataService authData)
        {
            _ordersLoadsService = ordersLoadsService;
            _authData = authData;
        }

        [HttpGet]
        [HasPermission(typeof(AttributeRequirement<BusinessLogic.Authorization.OrderLoadRequirements.CanRead>))]
        public Task<ActionResult<IPaginationResult<OrderLoad>>> Search([FromQuery] OrderLoadSearchRequest request)
        {
            return this._ordersLoadsService.Search(request).Convert<IPaginationResult<OrderLoad>>(this);
        }

        [HttpPatch("{id}/cancel")]
        [HasPermission(typeof(AttributeRequirement<BusinessLogic.Authorization.OrderLoadRequirements.CanCancel>))]
        public Task<ActionResult<OrderLoad>> Cancel(int id, OrderLoadCancelRequest request)
        {
            if (request != null && request.DplNote != null && _authData.GetUserRole() == UserRole.DplEmployee)
            {
                request.DplNote.UserId = _authData.GetUserId();
            }
            return this._ordersLoadsService.Cancel(id, request).Convert<OrderLoad>(this);
        }
    }
}