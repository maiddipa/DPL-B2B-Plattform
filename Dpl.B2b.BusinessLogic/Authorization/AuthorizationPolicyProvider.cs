using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Dpl.B2b.BusinessLogic.Authorization
{
    //thanks to https://www.jerriepelser.com/blog/creating-dynamic-authorization-policies-aspnet-core/
    //And to GholamReza Rabbal see https://github.com/JonPSmith/PermissionAccessControl/issues/3
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;

        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
            _options = options.Value;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // if the policy is wither already generated or is a manually created policy
            var policy = await base.GetPolicyAsync(policyName);
            if(policy != null)
            {
                return policy;
            }

            // if policy doesnt exist yet generate it assuming the policy name is the fully qualified type name
            var requirement = (IAuthorizationRequirement) Activator.CreateInstance(Type.GetType(policyName));
            var authorizationPolicy = new AuthorizationPolicyBuilder()
                                          .AddRequirements(requirement)
                                          .Build();
            return authorizationPolicy;
        }
    }
}
