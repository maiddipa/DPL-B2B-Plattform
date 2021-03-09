using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.BusinessLogic.Rules.User.Shared;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.User.ResetPassword
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(UserResetPasswordRequest request, IRule parentRule = null)
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
                .Eval(Context.Rules.UserResourceRule)
                .Eval(Context.Rules.Password);
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }
        
        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<UserResetPasswordRequest>
        {
            public ContextModel(UserResetPasswordRequest request, MainRule rule) : base(request, rule)
            {
                
            }

            public RulesBundle Rules { get; protected internal set; }

            public int UserId => Parent.UserId;

            public string NewPassword => Rules.Password.NewPassword;

            public Olma.User User => Rules.UserResourceRule.Context.Resource;
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, MainRule rule)
            {
                
                UserResourceRule = new ResourceRule<Olma.User>(context.UserId, rule)
                    .IgnoreQueryFilter()
                    .UseEagleLoad(t => t
                        .Include(p => p.Person)
                        .Include(p=>p.UserSettings)
                        .Include(p=>p.Permissions)
                        .Include(p=>p.Submissions)
                        .Include(p=>p.UserGroups)
                    );

                Password = new RandomPasswordRule();
            }
            
            public ResourceRule<Olma.User> UserResourceRule { get; }
            
            public RandomPasswordRule Password { get; } 
        }
        
        #endregion
    }
}