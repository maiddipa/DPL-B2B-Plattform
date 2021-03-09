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
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Warnings.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.TransportOfferings.Search
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(TransportOfferingsSearchRequest request, IRule parentRule = null)
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

            Context.AuthDivisionIds = ServiceProvider.GetService<IAuthorizationDataService>()
                .GetDivisionIds()
                .ToArray();
            
            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            rulesEvaluator.Eval(Context.Rules.QueryTransportOfferings);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<TransportOfferingsSearchRequest>
        {
            public ContextModel(TransportOfferingsSearchRequest parent, MainRule rule) : base(parent, rule)
            {

            }

            public RulesBundle Rules { get; protected internal set; }
            
            public IPaginationResult<TransportOffering> TransportOfferingsPaginationResult => Rules.QueryTransportOfferings.Context.TransportOfferingsPaginationResult;

            public int[] DivisionIds => new[] {Parent.DivisionId};
            
            public int[] AuthDivisionIds { get; protected internal set; }
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
                QueryTransportOfferings = new QueryTransportOfferingsRule(context, rule);
            }

            public QueryTransportOfferingsRule QueryTransportOfferings { get; protected internal set; }
        }

        #endregion
    }
}