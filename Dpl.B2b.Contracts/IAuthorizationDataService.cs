using Dpl.B2b.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dpl.B2b.Contracts
{
    public interface IAuthorizationDataService
    {
        ClaimsPrincipal GetPrincipal();
        int GetOrganizationId();

        int GetUserId();

        string GetUserUpn();

        UserRole GetUserRole();

        int GetRootCustomerId();

        IEnumerable<int> GetCustomerIds();

        IEnumerable<int> GetDivisionIds();

        IEnumerable<int> GetPostingAccountIds();
        
        Task<string> GetAccessToken();

        int GetTimeZoneOffset();
    }
}
