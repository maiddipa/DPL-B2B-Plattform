using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
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
    public class PostingRequestsController : ControllerBase
    {
        private readonly IPostingRequestsService _postingRequestsService;

        public PostingRequestsController(IPostingRequestsService postingRequestsService)
        {
            _postingRequestsService = postingRequestsService;
        }

        [HttpGet]
        public async Task<LoadResult> Get(DataSourceLoadOptions loadOptions,[FromQuery] PostingRequestsSearchRequest request)
        {
            var response =
                await _postingRequestsService.Search(request) as WrappedResponse<IQueryable<AccountingRecord>>;
            return await DataSourceLoader.LoadAsync(response?.Data, loadOptions);
        }

        [HttpGet("{id}")]
        [System.Obsolete]
        public Task<ActionResult<PostingRequest>> GetById(int id)
        {
            return this._postingRequestsService.GetById(id).Convert(this);
        }

        [HttpPost]
        public Task<ActionResult<IEnumerable<PostingRequest>>> Post([FromBody] PostingRequestsCreateRequest request)
        {
            return this._postingRequestsService.Create(request).Convert<IEnumerable<PostingRequest>>(this);
        }

        [HttpPatch("{id}")]
        public Task<ActionResult<PostingRequest>> Patch(int id, [FromBody] PostingRequestsUpdateRequest request)
        {
            return this._postingRequestsService.Update(id, request).Convert<PostingRequest>(this);
        }

        [HttpPatch("{id}/cancel")]
        public Task<ActionResult<PostingRequest>> PatchPostingRequestCancelled(int id)
        {
            return this._postingRequestsService.Cancel(id).Convert<PostingRequest>(this);
        }
    }
}
