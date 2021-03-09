using System.Collections.Generic;
using System.Threading.Tasks;
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
    //[Authorize]
    public class ChatUsersController : ControllerBase
    {
        private readonly IChatUsersService _chatUsersService;

        public ChatUsersController(IChatUsersService chatUsersService)
        {
            _chatUsersService = chatUsersService;
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<ChatUser>>> Get([FromQuery] ChatUsersSearchRequest request)
        {
            return this._chatUsersService.Search(request).Convert(this);
        }

        //[HttpGet("{id}")]
        //public Task<ActionResult<AccountingRecordProcess>> GetById(int id)
        //{
        //    return this._accountingRecordsService.GetById(id).Convert(this);
        //}
    }
}