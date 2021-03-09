using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class PermissionsAdminService : BaseService, IPermissionsAdminService
    {
        private readonly IRepository<Olma.Permission> _permissionRepo;

        public PermissionsAdminService(
            IAuthorizationDataService authData,
            IMapper mapper, IRepository<Olma.Permission> permissionRepo) : base(authData, mapper)
        {
            _permissionRepo = permissionRepo;
        }

        public async Task<IWrappedResponse> All(UsersSearchRequest request)
        {
            var actions =  new List<ResourceAction>();
            var resourceType = PermissionResourceType.Organization;
            var referenceId = 0;

        
            if (request.OrganizationId != null)
            {
                referenceId = (int) request.OrganizationId;
                resourceType = PermissionResourceType.Organization;
                actions =  Common.Enumerations.ResourceActions.Organization.Admin
                    .Union(Common.Enumerations.ResourceActions.Organization.Standard).Distinct().ToList();
            }
            else if (request.CustomerId != null)
            {
                referenceId = (int) request.CustomerId;
                resourceType = PermissionResourceType.Customer;
                actions = Common.Enumerations.ResourceActions.Customer.Admin
                    .Union(Common.Enumerations.ResourceActions.Customer.Standard).Distinct().ToList();
            }
            else if (request.DivisionId != null)
            {
                referenceId = (int) request.DivisionId;
                resourceType = PermissionResourceType.Division;
                actions = Common.Enumerations.ResourceActions.Customer.Admin
                    .Union(Common.Enumerations.ResourceActions.Customer.Standard).Distinct().ToList();
            }
            else
            {
                return Failed<IEnumerable<GroupPermission>>();
            }    

            var permissions = actions.Select(action => new GroupPermission()
            {
                Action = action.ToString(), 
                Resource = resourceType, 
                ReferenceId = referenceId
            }).AsEnumerable();

            return Ok(permissions);
        }
    }
}
