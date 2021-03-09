using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Common
{
    public class LocalizationLanguageValidRule : BaseValidationWithServiceProviderRule<LocalizationLanguageValidRule, LocalizationLanguageValidRule.ContextModel>
    {
        public LocalizationLanguageValidRule(int localizationLanguageId, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(localizationLanguageId, this);
            ParentRule = parentRule;
        }

        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => new ResourceNotFound();

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
            // No need to validate deeper

            var dbContext = ServiceProvider.GetService<OlmaDbContext>();
            
            var exists = dbContext.LocalizationLanguages.Count(l => l.Id == Context.LocalizationId) > 0;
            AddMessage(!exists, ResourceName, Message);

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
            public ContextModel(int parent, LocalizationLanguageValidRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }

            public int LocalizationId => Parent;
        }

        /// <summary>
        /// Bundles of rules 
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