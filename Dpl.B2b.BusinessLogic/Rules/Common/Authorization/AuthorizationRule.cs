using System;
using System.Threading.Tasks;
using AutoMapper;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Dpl.B2b.BusinessLogic.Rules.Common.Authorization
{
	/// <summary>
    /// Regel zum überprüfen der Autorisierung zum lesen einer Resource 
    /// </summary>
    /// <typeparam name="TResource">Resource Type</typeparam>
    /// <typeparam name="TContext">Context Type</typeparam>
    /// <typeparam name="TPermissionResource">PermissionResource Type</typeparam>
    /// <typeparam name="TRequirement">Requirement Type</typeparam>
    public class AuthorizationRule<TResource, TContext, TPermissionResource, TRequirement> :
        BaseValidationWithServiceProviderRule<
            AuthorizationRule<TResource, TContext, TPermissionResource, TRequirement>,
            AuthorizationRule<TResource, TContext, TPermissionResource, TRequirement>.ContextModel>
        where TResource : class
        where TRequirement : IAuthorizationRequirement, new()
        where TPermissionResource : OrganizationPermissionResource
    {
        public AuthorizationRule(GetOperatorRule<TContext, TResource> getOp, IRule rule)
        {
            Context = new ContextModel(getOp, this);
            ParentRule = rule;
        }

        /// <summary>
        ///     Message for RuleState if Rule is invalid
        /// </summary>
        protected override ILocalizableMessage Message => new NotAuthorized();

        /// <summary>
        ///     Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);

            var mapper = ServiceProvider.GetService<IMapper>();
            
            var permissionResource = mapper.Map<TPermissionResource>(Context.Resource);

            if (permissionResource == null)
            {
                RuleState.AddMessage(ResourceName, Message);
                return;
            }

            try
            {
                var result = Check(permissionResource);
                AddMessage(!result.Result.Succeeded, ResourceName, Message);
            }
            catch (Exception exception)
            {
                RuleState.AddMessage(ResourceName, Message);
            }
        }
        
        private async Task<AuthorizationResult> Check(TPermissionResource resource)
        {
            var authData = ServiceProvider.GetService<IAuthorizationDataService>();
            var authService = ServiceProvider.GetService<IAuthorizationService>();

            var authorizationResult =
                await authService.AuthorizeAsync<TPermissionResource, TRequirement>(authData, resource);
            return authorizationResult;
        }
        
        #region Internal

        public class ContextModel : ContextModelBase<GetOperatorRule<TContext, TResource>>
        {
            public ContextModel(GetOperatorRule<TContext, TResource> parent, IRule rule) : base(parent, rule)
            {
            }

            public RulesBundle Rules { get; protected internal set; }

            public TResource Resource =>  Parent.Evaluate();
        }

        /// <summary>
        ///     Bundles of rules
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
            }
        }

        #endregion
    }
}