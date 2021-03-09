using System;
using Dpl.B2b.BusinessLogic;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace Dpl.B2b.Web.Api.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Type type) : base(type.AssemblyQualifiedName)
        {
        }


        public HasPermissionAttribute(Policy policy) : base(Enum.GetName(typeof(Policy), policy))
        {
        }
    }

    public enum Policy
    {
        SamplePolicyWIthMoreThanOneRequirement
    }
}