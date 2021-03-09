using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
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
using Newtonsoft.Json;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Common
{
    public class BlacklistRule : BaseValidationWithServiceProviderRule<BlacklistRule, BlacklistRule.ContextModel>
    {
        public BlacklistRule(IEnumerable<string> inputs, IEnumerable<Regex> blacklist, IRule parentRule = null)
        {
            var inputsImmutable = inputs.ToImmutableArray();
            var blacklistImmutable = blacklist.ToImmutableArray();
            
            // Create Context
            Context = new ContextModel((inputsImmutable, blacklistImmutable), this);
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
            rulesEvaluator.Eval(Context.Rules.NotOnBlacklist);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal
        
        
        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<(ImmutableArray<string> Inputs, ImmutableArray<Regex> Blacklist)>
        {
            protected internal ImmutableArray<Regex> Blacklist => Parent.Blacklist;

            public ImmutableArray<string> Inputs => Parent.Inputs;
            
            public ContextModel((ImmutableArray<string> Inputs, ImmutableArray<Regex> Blacklist) parent, BlacklistRule rule) : base(parent, rule)
            {
                
            }
            
            public RulesBundle Rules { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public readonly ValidOperatorRule<ContextModel> NotOnBlacklist;

            public RulesBundle(ContextModel context, IRule rule)
            {
                NotOnBlacklist = new ValidOperatorRule<ContextModel>(
                    context,
                    (ctx)=> 
                    {
                        var onBlacklist = ctx.Blacklist.Any(regex =>
                        {
                            var matchResult = ctx.Inputs.Where(x=>!string.IsNullOrEmpty(x)).Select(input => regex.Match(input).Success);
                            return matchResult.Any(i => i==true);
                        });
                        return !onBlacklist;
                    },
                    new BlacklistMatch(),
                    rule,
                    "NotOnBlacklist");
            }
        }

        #endregion
    }
}