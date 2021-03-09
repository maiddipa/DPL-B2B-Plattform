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
    public class UploadsController : ControllerBase
    {
        private readonly IUploadsService _uploadsService;

        public UploadsController(IUploadsService uploadsService)
        {
            _uploadsService = uploadsService;
        }

        [HttpGet]
        public Task<ActionResult<IPaginationResult<Upload>>> Get([FromQuery] UploadSearchRequest request)
        {
            return this._uploadsService.Search(request).Convert(this);
        }

        [HttpGet("{id}")]
        public Task<ActionResult<Upload>> GetById(int id)
        {
            return this._uploadsService.GetById(id).Convert(this);
        }

        [HttpPost]
        public Task<ActionResult<Upload>> Post([FromBody] CreateUploadRequest request)
        {
            return this._uploadsService.Post(request).Convert(this);
        }

        [HttpPatch("{id}")]
        public Task<ActionResult<Upload>> Patch(int id, [FromBody] UpdateUploadRequest request)
        {
            return this._uploadsService.Patch(id, request).Convert(this);
        }

        [HttpDelete("{id}")]
        public Task<ActionResult> Delete(int id)
        {
            return this._uploadsService.Delete(id).Convert(this);
        }

        [HttpGet("{id}/balances")]
        public Task<ActionResult<IEnumerable<string>>> GetBalances(int id)
        {
            return this._uploadsService.GetBalances(id).Convert(this);
        }
    }
}
