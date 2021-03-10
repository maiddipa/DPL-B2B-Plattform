using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Authorization;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Dpl.B2b.Common.Enumerations;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    [ApiConventionType(typeof(DplApiConventions))]
    [Authorize]
    public class VouchersController : ControllerBase
    {
        private readonly IVouchersService _vouchersService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAuthorizationDataService _authData;

        public VouchersController(IVouchersService vouchersService, IAuthorizationService authorizationService, IAuthorizationDataService authData)
        {
            _vouchersService = vouchersService;
            _authorizationService = authorizationService;
            _authData = authData;
        }

        [HttpGet]
        [HasPermission(typeof(AttributeRequirement<CanReadVoucherRequirement>))]
        public Task<ActionResult<IPaginationResult<Voucher>>> Get([FromQuery] VouchersSearchRequest request)
        {
            return this._vouchersService.Search(request).Convert<IPaginationResult<Voucher>>(this);
        }

        [HttpGet]
        [HasPermission(typeof(AttributeRequirement<CanReadVoucherRequirement>))]
        public Task<ActionResult<IPaginationResult<Voucher>>> GetExport([FromQuery] VouchersSearchRequest request)
        {
            return this._vouchersService.Filter(request).Convert<IPaginationResult<Voucher>>(this);
        }

        [HttpGet("{id}")]
        [HasPermission(typeof(AttributeRequirement<CanReadVoucherRequirement>))]
        [System.Obsolete]
        public  Task<ActionResult<Voucher>> GetById(int id)
        {
            return  this._vouchersService.GetById(id).Convert<Voucher>(this);
        }

        [HttpPost]
        [HasPermission(typeof(AttributeRequirement<CanCreateVoucherRequirement>))]
        public Task<ActionResult<Voucher>> Post([FromBody] VouchersCreateRequest request)
        {
            if (request != null && request.DplNote != null && _authData.GetUserRole() == UserRole.DplEmployee)
            {
                request.DplNote.UserId = _authData.GetUserId();
            }
            return this._vouchersService.Create(request).Convert<Voucher>(this);
        }

        [HttpPatch("{id}/submit")]
        public Task<ActionResult<Voucher>> Patch(int id,[FromBody] VouchersAddToSubmissionRequest request)
        {
            return this._vouchersService.AddToSubmission(id,request).Convert(this);
        }

        [HttpPatch("{id}/remove")]
        public Task<ActionResult<Voucher>> Patch(int id,[FromBody] VouchersRemoveFromSubmissionRequest request)
        {
            return this._vouchersService.RemoveFromSubmission(id,request).Convert(this);
        }

        [HttpPatch("{id}/cancel")]
        [HasPermission(typeof(AttributeRequirement<CanCancelVoucherRequirement>))]
        public Task<ActionResult<Voucher>> PatchVoucherCancel(int id,[FromBody] VouchersCancelRequest request)
        {
            if (request != null && request.DplNote != null && _authData.GetUserRole() == UserRole.DplEmployee)
            {
                request.DplNote.UserId = _authData.GetUserId();
            }
            return this._vouchersService.Cancel(id,request).Convert<Voucher>(this);
        }

    }
}
