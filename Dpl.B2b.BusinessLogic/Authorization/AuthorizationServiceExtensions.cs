using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dpl.B2b.BusinessLogic.Authorization
{
    public static class AuthorizationServiceExtensions
    {
        async public static Task<AuthorizationResult> AuthorizeAsync<TResourceData, TR1>(this IAuthorizationService authService, IAuthorizationDataService authData, TResourceData resource)
            where TResourceData : OrganizationPermissionResource
            where TR1 : IAuthorizationRequirement, new()
        {
            ClaimsPrincipal user = authData.GetPrincipal();
            var authResult = await authService.AuthorizeAsync(user, resource, Requirements.Get<TR1>());
            return authResult;
        }
    }

    public static class Requirements
    {
        public static IAuthorizationRequirement[] Get<TR1>()
            where TR1 : IAuthorizationRequirement, new()
        {
            return new[] {
                new TR1() as IAuthorizationRequirement
            };
        }

        public static IAuthorizationRequirement[] Get<TR1, TR2>()
            where TR1 : IAuthorizationRequirement, new()
            where TR2 : IAuthorizationRequirement, new()
        {
            return new[] {
                new TR1() as IAuthorizationRequirement,
                new TR2() as IAuthorizationRequirement
            };
        }

        public static IAuthorizationRequirement[] Get<TR1, TR2, TR3>()
            where TR1 : IAuthorizationRequirement, new()
            where TR2 : IAuthorizationRequirement, new()
            where TR3 : IAuthorizationRequirement, new()
        {
            return new[] {
                new TR1() as IAuthorizationRequirement,
                new TR2() as IAuthorizationRequirement,
                new TR3() as IAuthorizationRequirement
            };
        }
    }
}
