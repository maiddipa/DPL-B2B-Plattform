using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;


namespace Dpl.B2b.BusinessLogic.Rules.PartnerDirectory.GetAll
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule()
        {
            // Create Context
            Context = new ContextModel(null, this);
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
            rulesEvaluator.Eval(Context.Rules.PartnerDirectorListRule);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<object>
        {
            public ContextModel(object context, IRule rule) : base(context, rule)
            {

            }
            
            public RulesBundle Rules { get; protected internal set; }

            public IEnumerable<Contracts.Models.PartnerDirectory> MappedResult => Rules.PartnerDirectorListRule.Context.MappedResult;
        }
        
        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                PartnerDirectorListRule = new PartnerDirectorListRule(context, rule);
            }

            public readonly PartnerDirectorListRule PartnerDirectorListRule;
            
            
        }

        #endregion
    }
}