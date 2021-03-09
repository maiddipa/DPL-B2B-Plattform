using System.Collections.Generic;
using System.Threading.Tasks;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    [Authorize]
    public class PostingAccountsController : ControllerBase
    {
        private readonly IPostingAccountsService _postingAccountsService;

        public PostingAccountsController(IPostingAccountsService postingAccountsService)
        {
            _postingAccountsService = postingAccountsService;
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<PostingAccount>>> Get()
        {
            return this._postingAccountsService.GetAll().Convert<IEnumerable<PostingAccount>>(this);
        }

        [HttpGet("{id}/balances")]
        public Task<ActionResult<IEnumerable<Balance>>> GetBalances(int id)
        {
            return this._postingAccountsService.GetBalances(id).Convert<IEnumerable<Balance>>(this);
        }

        //ToDo add PostingAccount ID
        [HttpGet("allowedDestinationAccounts")]
        public Task<ActionResult<IEnumerable<AllowedPostingAccount>>> GetAllowedDestinationAccounts()
        {
            return this._postingAccountsService.GetAllowedDestinationAccounts().Convert<IEnumerable<AllowedPostingAccount>>(this); 
        }
    }
}