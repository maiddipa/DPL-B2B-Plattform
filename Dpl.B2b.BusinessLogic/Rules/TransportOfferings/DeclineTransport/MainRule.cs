using System;
using System.Collections.Generic;
using System.Linq;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Authorization;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.TransportOfferings.DeclineTransport
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>, IRuleWithRessourceSkill
    {
        public MainRule(int transportId, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(transportId, this);
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
                .Eval(Context.Rules.QueryTransport)
                .Eval(Context.Rules.TransportHasWinningBid);
        
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
            public ContextModel(int parent, MainRule rule) : base(parent, rule)
            {
                
            }
            
            public RulesBundle Rules { get; protected internal set; }

            public int TransportId => Parent;
            
            public Olma.Transport Transport => Rules.QueryTransport.Context.Transport;
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            { 
                QueryTransport=new QueryTransportRule(context, rule);
                
                TransportHasWinningBid = new ValidOperatorRule<ContextModel>(
                    context,
                    c => c.Transport.WinningBid != null);
            }
            
            public QueryTransportRule QueryTransport { get; protected internal set; }
            public ValidOperatorRule<ContextModel> TransportHasWinningBid { get; set; }
        }

        #endregion

        public IValidationRule ResourceRule => Context.Rules.QueryTransport.Context.Rules.Required;
    }
}