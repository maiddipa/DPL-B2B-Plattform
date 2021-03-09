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

namespace Dpl.B2b.BusinessLogic.Rules.User.Create
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(UserCreateRequest request, IRule parentRule = null)
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
                .Eval(Context.Rules.UserUpnRequired)
                .Eval(Context.Rules.UpnValid)
                .Eval(Context.Rules.ValidEmail)
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
        public class ContextModel : ContextModelBase<UserCreateRequest>
        {
            public ContextModel(UserCreateRequest request, MainRule rule) : base(request, rule)
            {

            }

            public string Upn => ValidEmailRule<string>.IsEmail(Parent.Upn) ? Parent.Upn : $"{Parent.Upn}@dpl-ltms.com";

            public RulesBundle Rules { get; protected internal set; }

            public string DisplayName
            {
                get
                {
                    var dn= $"{Parent.FirstName} {Parent.LastName}".Trim();

                    return string.IsNullOrEmpty(dn) ? EmailNickname : dn;
                }
            }

            public string EmailNickname => CreateEmailNickname(Upn);

            public string FirstLoginPassword => Rules.Password.NewPassword;

            private static string CreateEmailNickname(string userUpn)
            {
                if (string.IsNullOrEmpty(userUpn)) 
                    return userUpn;
                
                var idx = userUpn.IndexOf("@", StringComparison.Ordinal);
                return idx >= 0 ? userUpn.Substring(0, idx) : userUpn;
            }

           
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, MainRule rule)
            {
                UpnValid =  new ValidOperatorRule<ContextModel>(
                    context,
                    ctx =>
                    {
                        var serviceProvider = rule.ServiceProvider;
                        var dbContext= serviceProvider.GetService<OlmaDbContext>();
                        var query = dbContext.Users
                            .Where(u => u.Upn == ctx.Upn)
                            .IgnoreQueryFilters();
                        
                        return !query.Any();

                    },
                    parentRule: rule,
                    ruleName: nameof(UpnValid),
                    message: new NotAllowedByRule());

                UserUpnRequired = new ValidOperatorRule<ContextModel>(
                    context,
                    ctx => !string.IsNullOrEmpty(ctx.Upn),
                    parentRule: rule,
                    ruleName: nameof(UserUpnRequired),
                    message: new NotAllowedByRule());

                ValidEmail = new ValidEmailRule<ContextModel>(context, ctx => ctx.Parent.Email);

                Password = new RandomPasswordRule();
            }

            public ValidEmailRule<ContextModel> ValidEmail { get; }

            public ValidOperatorRule<ContextModel> UserUpnRequired { get; protected internal set; }

            public  ValidOperatorRule<ContextModel> UpnValid { get; protected internal set; }
            
            public  RandomPasswordRule Password { get; protected internal set; }
        }
        
        #endregion
    }
}