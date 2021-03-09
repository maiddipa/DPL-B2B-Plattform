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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public Task<ActionResult<User>> Get(int? customerId)
        {
            if (customerId.HasValue)
            {
                // only dpl employees should be allowed to use this call
                return this._userService.GetByCustomerId(customerId.Value).Convert<User>(this);
            }

            return this._userService.Get().Convert<User>(this);
        }

        [HttpPut]
        public Task<ActionResult> UpdateSettings(Newtonsoft.Json.Linq.JObject settings)
        {
            return this._userService.UpdateSettings(settings).Convert(this);
        }
    }
}
