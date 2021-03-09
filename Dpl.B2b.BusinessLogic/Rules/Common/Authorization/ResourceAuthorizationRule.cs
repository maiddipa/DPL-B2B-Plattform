using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Microsoft.AspNetCore.Authorization;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Common.Authorization
{
    public class ResourceAuthorizationRule<TRessource, TRequirement, TPermissionResource> : BaseValidationWithServiceProviderRule<
            ResourceAuthorizationRule<TRessource, TRequirement, TPermissionResource>, ResourceAuthorizationRule<TRessource, TRequirement, TPermissionResource>.ContextModel>,
        IRuleWithAuthorizationSkill
        where TRessource : class
        where TRequirement : IAuthorizationRequirement, new()
        where TPermissionResource : OrganizationPermissionResource
    {
        private List<Expression<Func<TRessource, object>>> _include= new List<Expression<Func<TRessource, object>>>();

        public ResourceAuthorizationRule(List<int> resourceIds, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(resourceIds, this);
            ParentRule = parentRule;
        }

        public ResourceAuthorizationRule(int resourceId, IRule parentRule)
        {
            var resourceIds = new List<int> {resourceId};

            // Create Context
            Context = new ContextModel(resourceIds, this);
            ParentRule = parentRule;
        }

        /// <summary>
        ///     Message for RuleState if Rule is invalid
        /// </summary>
        protected override ILocalizableMessage Message => new NotAllowedByRule();

        public (IValidationRule AuthorizationRule, IValidationRule ResourceRule) Authorization => (
            Context.Rules.AuthorizationRules[0],
            Context.Rules.ResourceRules[0]
        );

        public ResourceAuthorizationRule<TRessource, TRequirement, TPermissionResource> Include(Expression<Func<TRessource, object>> include)
        {
            _include.Add(include);
            return this;
        }
        
        public ResourceAuthorizationRule<TRessource, TRequirement, TPermissionResource> IncludeAll(IEnumerable<Expression<Func<TRessource, object>>> include)
        {
            _include.AddRange(include);
            return this;
        }

        /// <summary>
        ///     Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            rulesEvaluator
                .EvalMany(Context.Rules.ResourceRules)
                .EvalMany(Context.Rules.AuthorizationRules);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        ///     Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<List<int>>
        {
            public ContextModel(List<int> parent, IRule rule) : base(parent, rule)
            {
            }

            public List<int> ResourceIds => Parent;

            public RulesBundle Rules { get; protected internal set; }

            public TRessource this[int i] => Rules.ResourceRules[i].Context.Resource;
        }

        /// <summary>
        ///     Bundles of rules
        /// </summary>
        public class RulesBundle
        {
            public readonly ImmutableList<AuthorizationRule<TRessource, ContextModel, TPermissionResource, TRequirement>> AuthorizationRules;

            public readonly ImmutableList<ResourceRule<TRessource>> ResourceRules;

            public RulesBundle(ContextModel context, ResourceAuthorizationRule<TRessource, TRequirement, TPermissionResource> rule)
            {
                var resourceRulesBuilder = ImmutableList.CreateBuilder<ResourceRule<TRessource>>(); // returns ImmutableList.Builder
                var authorizationRulesBuilder =
                    ImmutableList.CreateBuilder<AuthorizationRule<TRessource, ContextModel, TPermissionResource, TRequirement>>(); // returns ImmutableList.Builder

                for (var i = 0; i < context.ResourceIds.Count; i++)
                {
                    var index = i;
                    var postingAccountId = context.ResourceIds[index];
                    var resourceRule = new ResourceRule<TRessource>(postingAccountId, rule);
                    
                    if (rule != null)
                    {
                        resourceRule.IncludeAll(rule._include);
                    }

                    resourceRulesBuilder.Add(resourceRule);

                    var getOperatorRule =
                        new GetOperatorRule<ContextModel, TRessource>(
                            context,
                            c => c.Rules.ResourceRules[index].Context.Resource);

                    var authorizationRule = new AuthorizationRule<
                        TRessource,
                        ContextModel,
                        TPermissionResource,
                        TRequirement>(getOperatorRule, rule);

                    authorizationRulesBuilder.Add(authorizationRule);
                }

                ResourceRules = resourceRulesBuilder.ToImmutable();
                AuthorizationRules = authorizationRulesBuilder.ToImmutable();
            }
        }

        #endregion
    }
}