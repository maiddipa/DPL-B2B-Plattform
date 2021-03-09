using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Rules;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    /// <summary>
    /// Helper for when-then pattern coded execution of a service command
    /// </summary>
    /// <typeparam name="TRuleOfWhen">The ChildRule Type</typeparam>
    /// <typeparam name="TResultOfThen">The Wrapped Result Type of the Action</typeparam>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    public class ServiceCommand<TResultOfThen, TRuleOfWhen>
        where TRuleOfWhen : IValidationRule
    {
        protected ServiceCommand(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public readonly IServiceProvider ServiceProvider;
        
        protected ServiceCommandConfig Config { get; } = new ServiceCommandConfig();

        /// <summary>
        /// Initialized new service command
        /// </summary>
        /// <returns></returns>
        public static ServiceCommand<TResultOfThen, TRuleOfWhen> Create(IServiceProvider serviceProvider=null)
        {
            return new ServiceCommand<TResultOfThen, TRuleOfWhen>(serviceProvider);
        }

        /// <summary>
        /// The rule for use in the in the action part of the command
        /// </summary>
        protected TRuleOfWhen Rule { get; set; }

        /// <summary>
        /// The action on valid when part of the command
        /// </summary>
        protected Func<TRuleOfWhen, Task<IWrappedResponse>> Action { get; set; }

        /// <summary>
        /// Precondition Rule, must be Valid 
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public ServiceCommand<TResultOfThen, TRuleOfWhen> When(TRuleOfWhen rule)
        {
            if (rule is IRuleWithServiceProvider ruleWithServiceProvider)
                ruleWithServiceProvider.AssignServiceProvider(ServiceProvider);
            
            Rule = rule;
            return this;
        }

        /// <summary>
        /// Define die Action of the Command
        /// </summary>
        /// <param name="then"></param>
        /// <returns></returns>
        public ServiceCommand<TResultOfThen, TRuleOfWhen> Then(Func<TRuleOfWhen, Task<IWrappedResponse>> then)
        {
            Action = then;
            return this;
        }

        /// <summary>
        /// First Validate IRule. If the rule is success the execute the action of the command 
        /// </summary>
        /// <returns></returns>
        public async Task<IWrappedResponse> Execute()
        {
            // Evaluate the Rule when
            var result = RulesEvaluator.Create().Eval(Rule).Evaluate();
         
            #region Security via Interface -> IValidationWithAuthorizationAbility 
           
            if (Rule is IRuleWithAuthorizationSkill ruleWithAuthorizationSkill)
            {                
                var rRule = ruleWithAuthorizationSkill.Authorization.ResourceRule;
                if (!rRule.IsValid())
                {
                    return await Task.Factory.StartNew(() => new WrappedResponse(){
                        ResultType = ResultType.NotFound,
                        Errors = new[] { string.Empty }
                    });
                }
                
                var aRule = ruleWithAuthorizationSkill.Authorization.AuthorizationRule;
                if (!aRule.IsValid())
                {
                    return await Task.Factory.StartNew(() => new WrappedResponse(){
                        ResultType = ResultType.Forbidden,
                        Errors = new[] { string.Empty }
                    });
                }
            }
            
            if (Rule is IRuleWithRessourceSkill ruleWithRessourceSkill)
            {                
                var rRule = ruleWithRessourceSkill.ResourceRule;
                if (!rRule.IsValid())
                {
                    return await Task.Factory.StartNew(() => new WrappedResponse(){
                        ResultType = ResultType.NotFound,
                        Errors = new[] { string.Empty }
                    });
                }
            }
           
            #endregion
            
            // if when valid -> return result of then or return the wrapped RuleState of when            
            return result.IsSuccess 
                ? await Action(Rule) 
                : result.WrappedResponse<TResultOfThen>();
        }

        /// <summary>
        /// Config Class
        /// </summary>
        public class ServiceCommandConfig
        {
            public bool ReservedForFuture { get; } = default;
        }
    }

    /// <summary>
    /// Helper for when-then pattern coded execution of a service command
    /// </summary>
    /// <typeparam name="TResult">The Wrapped Result Type of the Action</typeparam>
    /// <typeparam name="TRule">The ChildRule Type</typeparam>
    /// <typeparam name="TAuthorizationResource"></typeparam>
    /// <typeparam name="TRequirement"></typeparam>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class ServiceCommand<TResult, TRule, TAuthorizationResource, TRequirement> : ServiceCommand<TResult, TRule>
        where TRule : IValidationRule
        where TAuthorizationResource : OrganizationPermissionResource
        where TRequirement : ActionRequirement, IAuthorizationRequirement, new()
    {
        protected ServiceCommand(IServiceProvider serviceProvider = null) :base(serviceProvider){}
        
        /// <summary>
        /// Initialized new service command
        /// </summary>
        /// <returns></returns>
        public new static ServiceCommand<TResult, TRule, TAuthorizationResource, TRequirement> Create(IServiceProvider serviceProvider=null)
        {
            return  new ServiceCommand<TResult, TRule, TAuthorizationResource, TRequirement>(serviceProvider)
            {
                ActionRequirement = Activator.CreateInstance<TRequirement>()
            };
        }

        protected new ServiceCommandConfig Config { get; } = new ServiceCommandConfig();

        protected bool AuthorizedResourceLoaded { get; set; }

        /// <summary>
        /// Just a simple IRule
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public new ServiceCommand<TResult, TRule, TAuthorizationResource, TRequirement> When(TRule rule)
        {
            if (rule is IRuleWithServiceProvider ruleWithServiceProvider)
                ruleWithServiceProvider.AssignServiceProvider(ServiceProvider);
            return (ServiceCommand<TResult, TRule, TAuthorizationResource, TRequirement>) base.When(rule);
        }

        /// <summary>
        /// Define die Action of the Command
        /// </summary>
        /// <param name="then"></param>
        /// <returns></returns>
        public new ServiceCommand<TResult, TRule, TAuthorizationResource, TRequirement> Then(Func<TRule, Task<IWrappedResponse>> then)
        {
            return (ServiceCommand<TResult, TRule, TAuthorizationResource, TRequirement>) base.Then(then);
        }

        /// <summary>
        /// Check IRule first. If the rule is successful, perform the command`s action
        /// </summary>
        /// <returns></returns>
        public new async Task<IWrappedResponse> Execute()
        {
            var (response, valid) = await CheckAuthorized();

            return valid 
                ? await base.Execute() 
                : response;
        }

        /// <summary>
        /// Prüft die Autorisierung
        /// </summary>
        /// <returns>Wenn die Autorisierung erfolgreich ist wird als Response ein null zurückgeliefert ansonsten ein IWrappedResponse zurückgegeben</returns>
        private async Task<(IWrappedResponse Response, bool Valid)> CheckAuthorized()
        {
            #region Authorized Resource

            if (AuthorizedResourceLoaded && AuthorizedResource == null)
            {
                return (new WrappedResponse<TResult>()
                {
                    ResultType = ResultType.NotFound,
                    Errors = new[] { string.Empty }
                }, false);
            }

            // Check Authorized Resource
            if (AuthorizedResource == null)
            {
                // Konfigurierbar, per default exception
                if (Config.ThrowErrorOnNoneAuthorization)
                    throw new Exception(
                        "Authorization without Authorized Resource. First call SetAuthorizedResource before execute");

                // No Authorized Resource -> No security
                return (null, true);
            }

            // Authorized Resource not found -> ResultType.NotFound  
            if (AuthorizedResource.ResultType == ResultType.NotFound)
            {
                return (new WrappedResponse<TResult>()
                {
                    ResultType = ResultType.NotFound
                }, false);
            }

            #endregion

            #region Security

            // Authorized security
            var authorizationResult = await AuthorizedSecurity(AuthorizedResource.Data);

            // Accept
            if (authorizationResult.Succeeded) return (null, true);

            // Deny
            return (new WrappedResponse<TResult>()
            {
                ResultType = ResultType.Forbidden,
                Errors = GetErrorsFrom(authorizationResult).ToArray()
            }, false);

            #endregion
        }
        
        
        /// <summary>
        /// Use Service to authorize resource by a requirement 
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private async Task<AuthorizationResult> AuthorizedSecurity([JetBrains.Annotations.NotNull] TAuthorizationResource resource)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            
            var authData = ServiceProvider.GetService<IAuthorizationDataService>();
            var authService = ServiceProvider.GetService<IAuthorizationService>();

            var authorizationResult = await authService
                .AuthorizeAsync<TAuthorizationResource, TRequirement>(authData, resource);
                        
            return authorizationResult;
        }
        
        private IEnumerable<string> GetErrorsFrom(AuthorizationResult authorizationResult)
        {
            return authorizationResult
                ?.Failure
                ?.FailedRequirements
                ?.Select(r => "Requirement failed: " + r);
        }
        
        public IWrappedResponse<TAuthorizationResource> AuthorizedResource { get; protected internal set; }
        
        private TRequirement ActionRequirement { get; set; }

        public ServiceCommand<TResult, TRule, TAuthorizationResource, TRequirement> SetAuthorizedResource(IWrappedResponse<TAuthorizationResource> authorizedResource)
        {
            AuthorizedResource = authorizedResource;

            return this;
        }

        public ServiceCommand<TResult, TRule, TAuthorizationResource, TRequirement> SetAuthorizedResourceById<T>(int? id) where T : class
        {
            AuthorizedResourceLoaded = true;
            
            if (!id.HasValue) return this;
            
            var repository = ServiceProvider.GetService<IRepository<T>>();
            var resourceResponse = repository.GetById<T, TAuthorizationResource>(id.Value);
            
            return SetAuthorizedResource(resourceResponse);
        }

        public ServiceCommand<TResult, TRule, TAuthorizationResource, TRequirement> SetAuthorizedResourceBy<T, TEntity>(
            Expression<Func<TEntity, bool>> predicate, bool ignoreQueryFilters = false)
            where TEntity : class
        {
            var repo = ServiceProvider.GetService<IRepository<TEntity>>();
            var resourceResponse = repo.GetBy<TEntity, TAuthorizationResource>(predicate, ignoreQueryFilters);

            AuthorizedResourceLoaded = true;
            return SetAuthorizedResource(resourceResponse);
        }

        /// <summary>
        /// If Authorized Resource is not set than only rule will handle the action
        /// </summary>
        /// <returns></returns>
        public ServiceCommand<TResult, TRule, TAuthorizationResource, TRequirement> PassByErrorOnMissingAuthorizedResource()
        {
            Config.ThrowErrorOnNoneAuthorization = false;
            return this;
        }

        /// <summary>
        /// If Authorized Resource is not set than a Exception raised on execute. This is default action
        /// </summary>
        /// <returns></returns>
        public ServiceCommand<TResult, TRule, TAuthorizationResource, TRequirement> ThrowErrorOnNoneAuthorization()
        {
            Config.ThrowErrorOnNoneAuthorization = true;
            return this;
        }

        /// <summary>
        /// Config Class
        /// </summary>
        public new class ServiceCommandConfig : ServiceCommand<TResult, TRule>.ServiceCommandConfig
        {
            public bool ThrowErrorOnNoneAuthorization { get; set; } = true;
        }
    }
}