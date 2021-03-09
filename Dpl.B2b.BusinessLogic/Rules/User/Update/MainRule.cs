using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
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
using Dpl.B2b.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.User.Update
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(UserUpdateRequest request, IRule parentRule = null)
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
                .Eval(Context.Rules.UserResourceRule)
                .Eval(Context.Rules.ValidEmail);
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }
        
        /// <summary>
        /// Aktualisiert den User mit den Daten aus dem Context
        /// </summary>
        public void PatchUser()
        {
            //JsonConvert.PopulateObject(source:string, target:object);

            var dbContext = ServiceProvider.GetService<OlmaDbContext>();
            var userEntry = dbContext.Entry(Context.User);
            var personNav = userEntry.Navigation("Person");
            if (!personNav.IsLoaded)
            {
                personNav.Load();
            }
                
            Context.User.Person ??= new Olma.Person();

            if (Context.ChangedRequest(nameof(Context.User.Role)))
            {
                Context.User.Role = Context.Role;                    
            }
                
            if (Context.ChangedRequest(nameof(Olma.Person.Salutation)))
            {
                Context.User.Person.Salutation = Context.Salutation;                    
            }
                
            if (Context.ChangedRequest(nameof(Olma.Person.FirstName)))
            {
                Context.User.Person.FirstName = Context.FirstName;                    
            }
                
            if (Context.ChangedRequest(nameof(Olma.Person.LastName)))
            {
                Context.User.Person.LastName = Context.LastName;                    
            }
                
            if (Context.ChangedRequest(nameof(Olma.Person.MobileNumber)))
            {
                Context.User.Person.MobileNumber = Context.MobileNumber;                    
            }
                
            if (Context.ChangedRequest(nameof(Olma.Person.PhoneNumber)))
            {
                Context.User.Person.PhoneNumber = Context.PhoneNumber;                    
            }
                
            if (Context.ChangedRequest(nameof(Olma.Person.Email)))
            {
                Context.User.Person.Email = Context.Email;                    
            }
                
            if (Context.ChangedRequest(nameof(Olma.Person.Gender)))
            {
                Context.User.Person.Gender = Context.Gender;                    
            }
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<UserUpdateRequest>
        {
            public ContextModel(UserUpdateRequest request, MainRule rule) : base(request, rule)
            {
                var json = (JObject) JsonConvert.DeserializeObject(request.Values);

                if (json==null)
                    return;
                
                var dictionary = json.ToObject<Dictionary<string, object>>() ?? new Dictionary<string, object>();
                ValuesDic = new Dictionary<string, object>(dictionary , StringComparer.CurrentCultureIgnoreCase);
                
                if (ValuesDic == null) 
                    return;

                Salutation = GetValue<string>(nameof(Person.Salutation));
                FirstName = GetValue<string>(nameof(Person.FirstName));
                LastName = GetValue<string>(nameof(Person.LastName));
                MobileNumber = GetValue<string>(nameof(Person.MobileNumber));
                PhoneNumber = GetValue<string>(nameof(Person.PhoneNumber));
                Email = GetValue<string>(nameof(Person.Email));
                
                Enum.TryParse(GetValue<string>(nameof(Person.Gender)), ignoreCase: true, out Gender);
                Enum.TryParse(GetValue<string>(nameof(Olma.User.Role)), ignoreCase: true, out Role);
            }
            
            public Dictionary<string, object> ValuesDic { get; protected internal set; }

            public T GetValue<T>(string key, T defaultValue=default(T))
            {
                if (ValuesDic.ContainsKey(key))
                {
                    return (T) ValuesDic[key];
                }
                return defaultValue;
            }
            
            public RulesBundle Rules { get; protected internal set; }

            public bool ChangedRequest(string key)
            {
                return ValuesDic.ContainsKey(key);
            }

            public Olma.User User => Rules.UserResourceRule.Context.Resource;

            public string Salutation { get; protected internal set; }
            public string FirstName { get; protected internal set; }
            public string LastName { get; protected internal set; }
            public string MobileNumber { get; protected internal set; }
            public string PhoneNumber { get; protected internal set; }
            public string Email { get; protected internal set; }
            
            public readonly PersonGender Gender;
           
            public readonly UserRole Role;
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
                         .Include(p=>p.Customers).ThenInclude(c => c.Customer)
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

                ValidEmail = new ValidEmailRule<ContextModel>(context, ctx => ctx.Email);
            }
            
            public ResourceRule<Olma.User> UserResourceRule { get; }
            public ValidOperatorRule<ContextModel> UserIdRequired { get; set; }
            
            public ValidEmailRule<ContextModel> ValidEmail { get; set; }
        }
        
        #endregion
    }
}