using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Authorization;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using User = Dpl.B2b.Contracts.Models.User;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    [Authorize(Roles = "Manager")]
    public class UserAdministrationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAuthorizationDataService _authData;

        public UserAdministrationController(IAuthorizationService authorizationService, IUserService userService, IAuthorizationDataService authData)
        {
            _authorizationService = authorizationService;
            _userService = userService;
            _authData = authData;
        }
        [HttpPost]
        //[HasPermission(typeof(AttributeRequirement<CanAdminUserRequirement>))]
        public Task<ActionResult<UserListItem>> Post([FromForm] string values)
        {
            var request = new UserCreateRequest();
            JsonConvert.PopulateObject(values, request);
            return this._userService.Create(request).Convert<UserListItem>(this);
        }
        
        // Laut Kevin ein Put und kein Patch sein damit das mit DevExpress funktioniert, standard und kein manueller Aufruf
        [HttpPut]
        public Task<ActionResult<UserListItem>> Put([FromForm] int key, [FromForm] string values)
        {
            var request = new UserUpdateRequest() {Id = key, Values = values};
            
            return this._userService.Update(request).Convert<UserListItem>(this);
        }
        
        [HttpGet]
        //[HasPermission(typeof(CanReadVoucherRequirement))] TODO User management Permissions
        public object Get([FromQuery] UsersSearchRequest request, DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_userService.Search(request),loadOptions);
        }

        [HttpGet("[action]")]
        //[HasPermission(typeof(CanReadVoucherRequirement))] TODO User management Permissions
        public Task<ActionResult<IEnumerable<UserListItem>>> All([FromQuery] UsersSearchRequest request)
        {
            return this._userService.All(request).Convert<IEnumerable<UserListItem>>(this);
        }

        [HttpGet("[action]")]
        //[HasPermission(typeof(CanReadVoucherRequirement))] TODO User management Permissions
        public Task<ActionResult<IEnumerable<UserListItem>>> AllByOrganization([FromQuery] UsersByOrganizationRequest request)
        {
            return this._userService.AllByOrganization(request).Convert<IEnumerable<UserListItem>>(this);
        }

        [HttpPatch("{id}/patchlocked")]
        //[HasPermission(typeof(CanReadVoucherRequirement))] TODO User management Permissions
        public async Task<ActionResult<User>> PatchLocked([FromRoute]int id, [FromBody] bool locked)
        {
            var request = new UsersLockRequest() {Id = id, Locked = locked};
            
            return await this._userService.UpdateLocked(request).Convert<User>(this);
        }

        [HttpPatch("{id}/addtocustomer")]
        //[HasPermission(typeof(CanUpdateCustomerUserRequirement))] TODO User management Permissions
        public Task<ActionResult<User>> AddToCustomer([FromRoute] int id, [FromBody] int customerId)
        {
            var request = new UserAddToCustomerRequest() {UserId = id, CustomerId = customerId};
            return this._userService.AddToCustomer(request).Convert<User>(this);
        }
        
        [HttpPatch("{id}/removefromcustomer")]
        //[HasPermission(typeof(CanUpdateCustomerUserRequirement))] TODO User management Permissions
        public Task<ActionResult<User>> removeFromCustomer([FromRoute] int id, [FromBody] int customerId)
        {
            var request = new RemoveFromCustomerRequest() {UserId = id, CustomerId = customerId};
            return this._userService.RemoveFromCustomer(request).Convert<User>(this);
        }

        [HttpPost("{id}/reset-password")]
        public async Task<ActionResult<UserResetPasswordResult>> PostResetPassword([FromRoute] int id)
        {
            var request = new UserResetPasswordRequest() {UserId = id};
            
            return await this._userService.ResetPassword(request).Convert<UserResetPasswordResult>(this);
        }
    }
}
