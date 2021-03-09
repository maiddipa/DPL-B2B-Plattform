using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    [Authorize(Roles = "Manager")]
    public class UserGroupAdministrationController : ControllerBase
    {
        private readonly IUserGroupService _userGroupService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAuthorizationDataService _authData;

        public UserGroupAdministrationController(IAuthorizationService authorizationService, IUserGroupService userGroupService, IAuthorizationDataService authData)
        {
            _authorizationService = authorizationService;
            _userGroupService = userGroupService;
            _authData = authData;
        }

        [HttpPost]
        //[HasPermission(typeof(AttributeRequirement<CanAdminUserRequirement>))]
        public Task<ActionResult<UserGroupListItem>> Post([FromForm] string values)
        {
            var request = new UserGroupCreateRequest();
            JsonConvert.PopulateObject(values, request);
            return this._userGroupService.Create(request).Convert<UserGroupListItem>(this);
        }

        [HttpGet]
        //[HasPermission(typeof(CanReadVoucherRequirement))] TODO User management Permissions
        public object Get(DataSourceLoadOptions loadOptions, [FromQuery] UserGroupsSearchRequest request)
        {
            return DataSourceLoader.Load(this._userGroupService.Search(request), loadOptions);
        }

        
        [HttpGet("[action]")]
        //[HasPermission(typeof(CanReadVoucherRequirement))] TODO User management Permissions
        public object Members (DataSourceLoadOptions loadOptions, [FromQuery] UserGroupsSearchRequest request)
        {
            return DataSourceLoader.Load(this._userGroupService.GetMembers(request), loadOptions);
        }

        [HttpPut("updatePermissions")]
        //[HasPermission(typeof(AttributeRequirement<CanAdminUsergroupsRequirement>))]
        public Task<ActionResult<UserGroupListItem>> PutPermissions([FromForm] int key, [FromForm] string values)
        {
            var request = new UserGroupUpdatePermissionsRequest();
            JsonConvert.PopulateObject(values,request);
            return this._userGroupService.UpdatePermissions(key, request).Convert<UserGroupListItem>(this);
        }

        [HttpPut("updateMembers")]
        [Authorize]
        //[HasPermission(typeof(AttributeRequirement<CanAdminUsergroupsRequirement>))]
        public Task<ActionResult<UserGroupListItem>> PatchMembers([FromForm] int key,[FromForm] string values)
        {
            var request = new UserGroupUpdateMembershipRequest();
            JsonConvert.PopulateObject(values,request);
            return this._userGroupService.UpdateMembers(key, request).Convert<UserGroupListItem>(this);
        }

        //[HttpPatch("{id}/adduser")]
        //[Obsolete]
        ////[HasPermission(typeof(AttributeRequirement<CanAdminUsergroupsRequirement>))]
        //public Task<ActionResult<UserGroupListItem>> PatchAddUser(int id, [FromQuery] int userId)
        //{
        //    return this._userGroupService.AddUser(id, userId).Convert<UserGroupListItem>(this);
        //}

        //[HttpPatch("{id}/removeuser")]
        //[Obsolete] 
        ////[HasPermission(typeof(AttributeRequirement<CanAdminUsergroupsRequirement>))]
        //public Task<ActionResult<UserGroupListItem>> PatchRemoveUser(int id, [FromQuery] int userId)
        //{
        //    return this._userGroupService.RemoveUser(id, userId).Convert<UserGroupListItem>(this);
        //}
    }
}
