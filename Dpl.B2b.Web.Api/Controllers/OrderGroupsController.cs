using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.Authorization;
using System.Collections.Generic;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Authorization;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    [Authorize]
    public class OrderGroupsController : ControllerBase
    {
        private readonly IOrderGroupsService _orderGroupsService;
        private readonly IAuthorizationDataService _authData;

        public OrderGroupsController(IOrderGroupsService orderGroupsService, IAuthorizationDataService authData)
        {
            _orderGroupsService = orderGroupsService;
            _authData = authData;
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<OrderGroup>>> Search([FromQuery] OrderGroupsSearchRequest request)
        {
            return this._orderGroupsService.Search(request).Convert<IEnumerable<OrderGroup>>(this);
        }

        [HttpGet("{id}")]
        [System.Obsolete]
        public Task<ActionResult<OrderGroup>> GetById(int id)
        {
            return this._orderGroupsService.GetById(id).Convert(this);
        }

        [HttpPost]
        [HasPermission(typeof(AttributeRequirement<CanCreateOrderRequirement>))]
        public Task<ActionResult<OrderGroup>> Post([FromBody] OrderGroupsCreateRequest request)
        {
            if (request?.DplNote != null && _authData.GetUserRole() == UserRole.DplEmployee)
            {
                request.DplNote.UserId = _authData.GetUserId();
            }
            return this._orderGroupsService.Create(request).Convert<OrderGroup>(this);
        }

        // TODO CHeck if we actually need this
        [HttpPatch("{id}")]
        [System.Obsolete]
        public Task<ActionResult<OrderGroup>> Patch(int id, [FromBody] OrderGroupsUpdateRequest request)
        {
            if (request != null && request.DplNote != null && _authData.GetUserRole() == UserRole.DplEmployee)
            {
                request.DplNote.UserId = _authData.GetUserId();
            }
            return this._orderGroupsService.Update(id, request).Convert(this);
        }

        [HttpPatch("{id}/cancel")]
        [System.Obsolete]
        public Task<ActionResult<OrderGroup>> PatchOrderGroupCancelled(int id, [FromBody] OrderGroupCancelRequest request)
        {
            if (request != null && request.DplNote != null && _authData.GetUserRole() == UserRole.DplEmployee)
            {
                request.DplNote.UserId = _authData.GetUserId();
            }
            return this._orderGroupsService.Cancel(id, request).Convert(this);
        }
    }
}