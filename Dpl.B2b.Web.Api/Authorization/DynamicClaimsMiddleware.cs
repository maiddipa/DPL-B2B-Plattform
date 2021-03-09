using Dpl.B2b.Contracts.Models;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using System.Net;

namespace Dpl.B2b.Web.Api.Authorization
{
    public class DynamicClaimsMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IPermissionsService _permissionsService;
        private readonly IDistributedCache _cache;

        public DynamicClaimsMiddleware(RequestDelegate next, IOptions<CustomClaimsOptions> options, IPermissionsService permissionsService, IDistributedCache cache)
        {
            _next = next;
            _permissionsService = permissionsService;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // authentication has already happened at this point hence the upn is available
            var upnClaim = context.User.FindFirst(ClaimTypes.Upn);

            // TODO Check if we can prevent claims middleware from running at all when authentication has not happened instead for checking if there is a claim
            if (upnClaim == null)
            {
                await _next(context);
                return;
            }

            var upn = upnClaim.Value;

            var user = await _permissionsService.GetUser(upn);
            if (user == null)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            var claims = new List<Claim>();

            claims.AddRange(GetUserClaims(user));

            // get READ claims from permissions for resources
            claims.AddRange(GetClaimsForRessourceTypeAndActions(user, PermissionResourceType.Organization, new[] { ResourceAction.Read }, DynamicClaimTypes.OrganizationId));
            claims.AddRange(GetClaimsForRessourceTypeAndActions(user, PermissionResourceType.Customer, new[] { ResourceAction.Read }, DynamicClaimTypes.CustomerIds));
            claims.AddRange(GetClaimsForRessourceTypeAndActions(user, PermissionResourceType.Division, new[] { ResourceAction.Read }, DynamicClaimTypes.DivisionIds));

            // generate posting account ids from cached user
            claims.AddRange(user.PostingAccountIds.Select(postingAccuontId => new Claim(DynamicClaimTypes.PostingAccountIds, postingAccuontId.ToString())));



            // TODO check if if we need to provide the authentication scheme, found it in a sample not sure what it does or if its needed
            var appIdentity = new ClaimsIdentity(claims, AzureADDefaults.JwtBearerAuthenticationScheme);
            context.User.AddIdentity(appIdentity);

            await _next(context);
        }

        //private IEnumerable<Claim> GetClaimsForUserPermissions(CachedUser user)
        //{
        //    //The following ResourceTypes are added via GetClaimsForResourceTypeAndActions
        //    var resourceTypesToExclude = new List<PermissionResourceType>()
        //    {
        //        PermissionResourceType.Organization,
        //        PermissionResourceType.Customer,
        //        PermissionResourceType.Division,
        //        PermissionResourceType.PostingAccount
        //    }; //TODO Get all applicable  Resources

        //    var permissionsList = user.Permissions;
        //    return permissionsList.Select(p => new Claim(DynamicClaimTypes.Permissions, p.Resource + p.Action));
        //}

        private IEnumerable<Claim> GetClaimsForRessourceTypeAndActions(CachedUser user, PermissionResourceType resourceType, ResourceAction[] actions, string claimType)            
        {
            var ids = user.Permissions
                .Where(p => p.Resource == resourceType && actions.Contains(p.Action))
                .Select(p => p.ReferenceId.ToString())
                .ToArray();

            return ids
                .Select(id => new Claim(claimType, id))
                .ToList();
        }


        private IEnumerable<Claim> GetUserClaims(CachedUser user)
        {
            return new[] { new Claim("UserId", user.Id.ToString()) };
        }
    }

    public class CustomClaimsOptions
    {
    }
}
