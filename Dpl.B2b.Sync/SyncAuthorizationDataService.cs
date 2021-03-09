using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;

namespace Dpl.B2b.Sync
{
    public class SyncAuthorizationDataService : IAuthorizationDataService
    {
        public ClaimsPrincipal GetPrincipal()
        {
            throw new NotImplementedException();
        }

        public int GetOrganizationId()
        {
            return 1;
        }

        public int GetUserId()
        {
            return 1;
        }

        public string GetUserUpn()
        {
            throw new NotImplementedException();
        }

        public UserRole GetUserRole()
        {
            throw new NotImplementedException();
        }

        public int GetRootCustomerId()
        {
            return 1;
        }

        public IEnumerable<int> GetCustomerIds()
        {
            return new[] { 1 };
        }

        public IEnumerable<int> GetDivisionIds()
        {
            return new[] { 1 };
        }

        public IEnumerable<int> GetPostingAccountIds()
        {
            return new[] { 1, 2 };
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