using System;
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
    public class PostingAccountBalancesController : ControllerBase
    {
        private readonly IPostingAccountBalancesService _postingAccountBalancesService;

        public PostingAccountBalancesController(IPostingAccountBalancesService postingAccountBalancesService)
        {
            _postingAccountBalancesService = postingAccountBalancesService;
        }

        [HttpGet]
        public async Task<ActionResult<BalancesSummary>> Get( [FromQuery] BalancesSearchRequest request)
        {
            return await this._postingAccountBalancesService.Search(request).Convert<BalancesSummary>(this);
        }
    }
}