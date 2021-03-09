using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DevExpress.CodeParser;
using DevExpress.Data.Linq.Helpers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.LoadCarrierOfferings;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite.Geometries;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.LoadCarrierOfferings.Search
{
    public partial class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(LoadCarrierOfferingsSearchRequest request)
        {
            Context = new ContextModel(request, this);
        }

        /// <summary>
        /// Message for RuleState if Rule is invalid 
        /// </summary>
        protected override ILocalizableMessage Message => new LoadCarrierOfferingsSearch();

        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);
            Context.TakeCount = 100;
            
            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create();

            // Assign rules to the Evaluator
            rulesEvaluator.Eval(Context.Rules.LoadCarrierOfferingValidRule);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();
            
            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<LoadCarrierOfferingsSearchRequest>
        {
            public ContextModel(LoadCarrierOfferingsSearchRequest parent, IRule rule) : base(parent, rule)
            {
               
            }

            public int TakeCount { get; protected internal set; } = 100;
            
            public RulesBundle Rules { get; protected internal set; }
            
            public IEnumerable<LoadCarrierOffering> LoadCarrierOfferings => Rules.LoadCarrierOfferingValidRule.Context.LoadCarrierOfferings;
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule parentRule)
            {
                LoadCarrierOfferingValidRule = new LoadCarrierOfferingValidRule(context, parentRule);
            }

            
            
            public LoadCarrierOfferingValidRule LoadCarrierOfferingValidRule { get; }
        }

        #endregion
    }
}
