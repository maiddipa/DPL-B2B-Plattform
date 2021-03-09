using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.Authorization;
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
    public class BalanceTransfersController : ControllerBase
    {
        private readonly IBalanceTransfersService _balanceTransfersService;
        private readonly IAuthorizationDataService _authData;
        private readonly IAuthorizationService _authorizationService;

        public BalanceTransfersController(IBalanceTransfersService balanceTransfersService, IAuthorizationService authorizationService, IAuthorizationDataService authData)
        {
            _balanceTransfersService = balanceTransfersService;
            _authData = authData;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [HasPermission(typeof(AttributeRequirement<CanReadBalanceTransferRequirement>))]
        [System.Obsolete]
        public Task<ActionResult<IPaginationResult<BalanceTransfer>>> Get([FromQuery] BalanceTransferSearchRequest request)
        {
            return this._balanceTransfersService.Search(request).Convert(this);
        }

        [HttpGet("{id}")]
        [HasPermission(typeof(AttributeRequirement<CanReadBalanceTransferRequirement>))]
        [System.Obsolete]
        public Task<ActionResult<BalanceTransfer>> GetById(int id)
        {
            return this._balanceTransfersService.GetById(id).Convert(this);
        }

        [HttpPost]
        [HasPermission(typeof(AttributeRequirement<CanCreateBalanceTransferRequirement>))]
        public Task<ActionResult<PostingRequest>> Post([FromBody] BalanceTransferCreateRequest request)
        {
            if (request != null && request.DplNote != null && _authData.GetUserRole() == UserRole.DplEmployee)
            {
                request.DplNote.UserId = _authData.GetUserId();
            }
            return this._balanceTransfersService.Create(request).Convert<PostingRequest>(this);
        }

        [HttpPatch("{id}/cancel")]
        [HasPermission(typeof(AttributeRequirement<CanCreateBalanceTransferRequirement>))]
        [System.Obsolete]
        public Task<ActionResult<BalanceTransfer>> Post(int id,[FromBody] BalanceTransferCancelRequest request)
        {
            return this._balanceTransfersService.Cancel(id,request).Convert(this);
        }
    }
}