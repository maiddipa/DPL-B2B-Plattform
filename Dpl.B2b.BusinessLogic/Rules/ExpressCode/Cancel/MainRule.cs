using System;
using System.Linq;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.ExpressCode;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.ExpressCode.Cancel
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(int request, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;
        }

        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => new NotAllowedByRule();

        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);
            Context.AuthData = ServiceProvider.GetService<IAuthorizationDataService>();

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            rulesEvaluator
                .Eval(Context.Rules.ExpressCodeAuthorizationRule);
            
            //Check issuer
            var customerIds = Context.AuthData.GetCustomerIds().ToList();
            var divisionIds = Context.AuthData.GetDivisionIds().ToList();

            if (customerIds.Count >0 && Context.ExpressCode.IssuingCustomerId != null && !customerIds.Contains((int)Context.ExpressCode.IssuingCustomerId))
                RuleState.Add(ResourceName, new RuleState(Context.ExpressCode) { RuleStateItems = { new DigitalCodeIssuingCustomerDoesNotMatch() } });

            if (divisionIds.Count > 0 && Context.ExpressCode.IssuingDivisionId != null && !divisionIds.Contains((int) Context.ExpressCode.IssuingDivisionId))
                RuleState.Add(ResourceName,new RuleState(Context.ExpressCode){RuleStateItems = { new DigitalCodeIssuingDivisionDoesNotMatch() }});

            if (Context.ExpressCode.ValidTo.HasValue && Context.ExpressCode.ValidTo < DateTime.Today)
                RuleState.Add(ResourceName, new RuleState(Context.ExpressCode) { RuleStateItems = { new DigitalCodeExpired() } });
            //TODO: implement generic ExpiredRule

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<int>
        {
            public ContextModel(int parent, IRule rule) : base(parent, rule)
            {
                Id = parent;
            }
            public Olma.ExpressCode ExpressCode => this.Rules.ExpressCodeAuthorizationRule.Context[0];

            public RulesBundle Rules { get; protected internal set; }

            public int Id { get; protected internal set; }
            public IAuthorizationDataService AuthData { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                ExpressCodeAuthorizationRule = new ResourceAuthorizationRule<Olma.ExpressCode,CanCancelExpressCodeRequirement,DivisionPermissionResource>(context.Parent,rule);
            }

            public ResourceAuthorizationRule<Olma.ExpressCode, CanCancelExpressCodeRequirement, DivisionPermissionResource> ExpressCodeAuthorizationRule { get; protected internal set; }
        }

        #endregion
    }
}