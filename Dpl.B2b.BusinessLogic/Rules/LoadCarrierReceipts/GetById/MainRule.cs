using System;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.BusinessLogic.Extensions;
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

namespace Dpl.B2b.BusinessLogic.Rules.LoadCarrierReceipts.GetById
{
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(int request, IRule parentRule = null)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;

            ValidEvaluate += OnValidEvaluate;
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
            // No need to validate deeper
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();
            
            // Add all Message from ruleResult
            MergeFromResult(ruleResult);
            
            // Add Message if ruleResult is not success
            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
        }

        private void OnValidEvaluate(object sender, EvaluateInternalEventArgs evaluateInternalEventArgs)
        {
            var loadCarrierReceiptRepo = ServiceProvider.GetService<IRepository<Olma.LoadCarrierReceipt>>();
            
            Context.LoadCarrierReceipt =  loadCarrierReceiptRepo.GetById<Olma.LoadCarrierReceipt, Contracts.Models.LoadCarrierReceipt>(Context.LoadCarrierReceiptId);
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
            
            public IWrappedResponse LoadCarrierReceipt { get; protected internal set; }
            
            public int LoadCarrierReceiptId => Parent;
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