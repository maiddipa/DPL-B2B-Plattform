using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Web.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dpl.B2b.Web.Api.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DplApiConventions))]
    [Route("[controller]")]
    [Authorize(Roles = "Manager")]
    public class PermissionsAdministrationController : ControllerBase
    {
        private readonly IPermissionsAdminService _permissionsService;

        public PermissionsAdministrationController(IPermissionsAdminService permissionsService)
        {
            _permissionsService = permissionsService;
        }

        //[HttpGet]
        ////[HasPermission(typeof(CanReadVoucherRequirement))] TODO User management Permissions
        //public object Get([FromQuery] UsersSearchRequest request, DataSourceLoadOptions loadOptions)
        //{
        //    return DataSourceLoader.Load(_permissionsService.Search(request),loadOptions);
        //}

        [HttpGet("[action]")]
        //[HasPermission(typeof(CanReadVoucherRequirement))] TODO User management Permissions
        public Task<ActionResult<IEnumerable<GroupPermission>>> All([FromQuery] UsersSearchRequest request)
        {
            return this._permissionsService.All(request).Convert<IEnumerable<GroupPermission>>(this);
        }
    }
}
