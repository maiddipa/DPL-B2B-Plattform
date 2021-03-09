using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Dpl.B2b.Web.Api.Authorization
{
    public class ClaimsService : IAuthorizationDataService
    {
        const string TIMEZONE_OFFSET = "x-timezone-offset";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal GetPrincipal()
        {
            return _httpContextAccessor.HttpContext.User;
        }

        public int GetOrganizationId()
        {
            return _httpContextAccessor.HttpContext.User
                .FindAll(DynamicClaimTypes.OrganizationId)
                .Select(c => int.Parse(c.Value))
                .First();
        }

        public int GetTimeZoneOffset()
        {
            if (!_httpContextAccessor.HttpContext.Request.Headers.ContainsKey(TIMEZONE_OFFSET))
            {
                return 0;
            }

            var timeZoneOffsetInMinutes = int.Parse(_httpContextAccessor.HttpContext.Request.Headers[TIMEZONE_OFFSET]);
            return timeZoneOffsetInMinutes;
        }

        public int GetUserId()
        {
            return _httpContextAccessor.HttpContext.User
                .FindAll(DynamicClaimTypes.UserId)
                .Select(c => int.Parse(c.Value))
                .First();
        }
        public string GetUserUpn()
        {
            var upnClaim = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.Upn);

            if (upnClaim == null)
            {
                return null;
            }

            return upnClaim.Value;
        }

        public Dpl.B2b.Common.Enumerations.UserRole GetUserRole()
        {
            //// HACK Remove this line and uncomment lines below to enable dpl employee access
            //return Dpl.B2b.Common.Enumerations.UserRole.Retailer;

            var upnClaim = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.Role);


            return upnClaim != null && upnClaim.Value == "Manager"
                ? Common.Enumerations.UserRole.DplEmployee
                : Dpl.B2b.Common.Enumerations.UserRole.Retailer;
        }

        [Obsolete]
        /// <summary>
        /// Use OrganizationId instead
        /// </summary>
        public int GetRootCustomerId()
        {
            return _httpContextAccessor.HttpContext.User
                .FindAll(DynamicClaimTypes.RootCustomerId)
                .Select(c => int.Parse(c.Value))
                .First();
        }

        public IEnumerable<int> GetCustomerIds()
        {
            return _httpContextAccessor.HttpContext.User
                .FindAll(DynamicClaimTypes.CustomerIds)
                .Select(c => int.Parse(c.Value));
        }

        public IEnumerable<int> GetDivisionIds()
        {
            return _httpContextAccessor.HttpContext.User
                .FindAll(DynamicClaimTypes.DivisionIds)
                .Select(c => int.Parse(c.Value));
        }

        public IEnumerable<int> GetPostingAccountIds()
        {
            return _httpContextAccessor.HttpContext.User
                .FindAll(DynamicClaimTypes.PostingAccountIds)
                .Select(c => int.Parse(c.Value));
        }

        public async Task<string> GetAccessToken()
        {
            return await _httpContextAccessor.HttpContext.Request.HttpContext.GetTokenAsync("access_token");
        }
    }

    public static class PermissionClaimType
    {
        public const string ClaimTypeName = "Permission";

        public const string DivisionCan = "division.can";
    }
}
