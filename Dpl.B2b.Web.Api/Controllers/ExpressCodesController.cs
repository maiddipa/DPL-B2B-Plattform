using System.Threading.Tasks;
using Dpl.B2b.Common.Enumerations;
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
    [Authorize]
    public class ExpressCodesController : ControllerBase
    {
        private readonly IExpressCodesService _expressCodesService;

        public ExpressCodesController(IExpressCodesService expressCodesService)
        {
            _expressCodesService = expressCodesService;
        }

        [HttpGet]
        public Task<ActionResult<IPaginationResult<ExpressCode>>> Get([FromQuery] ExpressCodesSearchRequest request)
        {
            return this._expressCodesService.Search(request).Convert<IPaginationResult<ExpressCode>>(this);
        }

        // TODO Discuss if GetById OR GetByCode
        //[HttpGet("{id}")]
        //public Task<ActionResult<ExpressCode>> GetById(int id)
        //{
        //    return this._expressCodesService.GetById(id).Convert(this);
        //}

        [HttpGet("/code-type")]
        public Task<ActionResult<ExpressCode>> GetByCode([FromQuery] ExpressCodesSearchRequest request)
        {
            return this._expressCodesService.GetByCode(request).Convert<ExpressCode>(this);
        }

        [HttpPost]
        public Task<ActionResult<ExpressCode>> Post([FromBody] ExpressCodeCreateRequest request)
        {
            return this._expressCodesService.Create(request).Convert<ExpressCode>(this);
        }

        [HttpPatch("{id}")]
        public Task<ActionResult<ExpressCode>> Patch(int id,[FromBody] ExpressCodeUpdateRequest request)
        {
            return this._expressCodesService.Update(id, request).Convert<ExpressCode>(this);
        }

        [HttpPatch("{id}/cancel")]
        public Task<ActionResult<ExpressCode>> PatchExpressCodeCancelled(int id)
        {
            return this._expressCodesService.Cancel(id).Convert<ExpressCode>(this);
        }
    }
}