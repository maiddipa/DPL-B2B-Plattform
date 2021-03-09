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
    public class VouchersSummariesController : ControllerBase
    {
        private readonly IVouchersService _vouchersService;

        public VouchersSummariesController(IVouchersService vouchersService)
        {
            _vouchersService = vouchersService;
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<VoucherSummary>>> Get([FromQuery] VouchersSearchRequest request)
        {
            return this._vouchersService.Summary(request).Convert<IEnumerable<VoucherSummary>>(this);
        }
    }
}
