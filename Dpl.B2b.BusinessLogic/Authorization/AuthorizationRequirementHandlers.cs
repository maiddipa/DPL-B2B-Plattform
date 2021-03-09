using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Dpl.B2b.BusinessLogic.Authorization
{
    public abstract class OrganizationActionRequirementAuthorizationHandler<TRequirement, TResource> : AuthorizationHandler<TRequirement, TResource>
        where TRequirement : ActionRequirement, IAuthorizationRequirement
        where TResource : OrganizationPermissionResource
    {

        protected IPermissionsService PermissionsService { get; }

        public OrganizationActionRequirementAuthorizationHandler(IPermissionsService permissionsService)
        {
            PermissionsService = permissionsService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement, TResource resource)
        {
            var referenceId = resource.OrganizationId;
            var action = requirement.Action;

            var hasPermission = PermissionsService.HasPermission(PermissionResourceType.Organization, action, referenceId);
            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }

    public class CustomerActionRequirementAuthorizationHandler<TRequirement, TResource> : OrganizationActionRequirementAuthorizationHandler<TRequirement, TResource>
        where TRequirement : ActionRequirement, IAuthorizationRequirement
        where TResource : CustomerPermissionResource
    {
        public CustomerActionRequirementAuthorizationHandler(IPermissionsService permissionsService) : base(permissionsService)
        {
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement, TResource resource)
        {
            await base.HandleRequirementAsync(context, requirement, resource);

            if (context.HasSucceeded)
            {
                return;
            }

            var referenceId = resource.CustomerId;
            var action = requirement.Action;

            var hasPermission = PermissionsService.HasPermission(PermissionResourceType.Customer, action, referenceId);
            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }

    public class DivisionActionRequirementAuthorizationHandler<TRequirement, TResource> : CustomerActionRequirementAuthorizationHandler<TRequirement, TResource>
        where TRequirement : ActionRequirement, IAuthorizationRequirement
        where TResource : DivisionPermissionResource
    {
        public DivisionActionRequirementAuthorizationHandler(IPermissionsService permissionsService) : base(permissionsService)
        {
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement, TResource resource)
        {
            await base.HandleRequirementAsync(context, requirement, resource);
            if (context.HasSucceeded)
            {
                return;
            }

            var referenceId = resource.DivisionId;
            var action = requirement.Action;

            var hasPermission = PermissionsService.HasPermission(PermissionResourceType.Division, action, referenceId);
            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }

    public class PostingAccountActionRequirementAuthorizationHandler<TRequirement, TResource> : CustomerActionRequirementAuthorizationHandler<TRequirement, TResource>
        where TRequirement : ActionRequirement, IAuthorizationRequirement
        where TResource : PostingAccountPermissionResource
    {
        public PostingAccountActionRequirementAuthorizationHandler(IPermissionsService permissionsService) : base(permissionsService)
        {
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement, TResource resource)
        {
            await base.HandleRequirementAsync(context, requirement, resource);
            if (context.HasSucceeded)
            {
                return;
            }

            var referenceId = resource.PostingAccountId;
            var action = requirement.Action;

            var hasPermission = PermissionsService.HasPermission(PermissionResourceType.PostingAccount, action, referenceId);
            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }


    public class AttributeActionRequirementAuthorizationHandler<TRequirement> : AuthorizationHandler<TRequirement>
        where TRequirement : ActionRequirement, IAuthorizationRequirement
    {

        public AttributeActionRequirementAuthorizationHandler(IPermissionsService permissionsService)
        {
            PermissionsService = permissionsService;
        }

        protected IPermissionsService PermissionsService { get; }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            var action = requirement.Action;

            var hasPermission = PermissionsService.HasAttributePermission(action);
            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}
