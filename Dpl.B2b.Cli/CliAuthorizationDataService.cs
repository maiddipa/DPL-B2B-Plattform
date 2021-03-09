using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;

namespace Dpl.B2b.Cli
{
    // TODO implement authorization service for CLI app
    public class CliAuthorizationDataService : IAuthorizationDataService
    {
        public ClaimsPrincipal GetPrincipal()
        {
            throw new NotImplementedException();
        }

        public int GetOrganizationId()
        {
            return 0;
        }

        public int GetUserId()
        {
            return 0;
        }

        public string GetUserUpn()
        {
            throw new NotImplementedException();
        }

        public UserRole GetUserRole()
        {
            return UserRole.DplEmployee;
        }

        public int GetRootCustomerId()
        {
            return 0;
        }

        public IEnumerable<int> GetCustomerIds()
        {
            return new int[] { };
        }

        public IEnumerable<int> GetDivisionIds()
        {
            return new int[] { };
        }

        public IEnumerable<int> GetPostingAccountIds()
        {
            return new int[] { };
        }

        public Task<string> GetAccessToken()
        {
            throw new NotImplementedException();
        }

        public int GetTimeZoneOffset()
        {
            return 0;
        }
    }
}
