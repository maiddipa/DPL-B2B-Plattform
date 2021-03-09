using System;
using System.Linq.Dynamic.Core;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.User.UpdateLock
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(UsersLockRequest request, IRule parentRule = null)
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

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            rulesEvaluator
                .Eval(Context.Rules.UserIdRequired)
                .Eval(Context.Rules.UserLockedRequired)
                .Eval(Context.Rules.UserResourceRule);
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<UsersLockRequest>
        {
            public ContextModel(UsersLockRequest request, MainRule rule) : base(request, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }

            public Olma.User User => this.Rules.UserResourceRule.Context.Resource;
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                var userId = context.Parent.Id.GetValueOrDefault();
                UserResourceRule = new ResourceRule<Olma.User>(userId, rule)
                     .IgnoreQueryFilter()
                     .UseEagleLoad(t => t
                         .Include(p => p.Person)
                         .Include(p=>p.Customers).ThenInclude(r => r.Customer)
                         .Include(p=>p.UserSettings)
                         .Include(p=>p.Permissions)
                         .Include(p=>p.Submissions)
                         .Include(p=>p.UserGroups)
                     );
                
                UserIdRequired = new ValidOperatorRule<ContextModel>(
                    context,
                    ctx => ctx.Parent.Id.HasValue,
                    parentRule: rule,
                    ruleName: nameof(UserIdRequired),
                    message: new NotAllowedByRule());

                UserLockedRequired = new ValidOperatorRule<ContextModel>(
                    context,
                    ctx => ctx.Parent.Locked.HasValue,
                    parentRule: rule,
                    ruleName: nameof(UserLockedRequired),
                    message: new NotAllowedByRule());
            }
            
            public ResourceRule<Olma.User> UserResourceRule { get; }
            public ValidOperatorRule<ContextModel> UserIdRequired { get; set; }
            public ValidOperatorRule<ContextModel> UserLockedRequired { get; set; }

        }

        #endregion
    }
}