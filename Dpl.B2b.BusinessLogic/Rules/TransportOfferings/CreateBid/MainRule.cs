using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.BalanceTransfer;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using JetBrains.Annotations;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.TransportOfferings.CreateBid
{
    /// <summary>
    /// If DigitalCode ist given, PostingAccount, LoadCarrier and Quantity are taken from PostingAccountPreset,
    /// else they are take from the request
    /// </summary>
    public class MainRule : BaseValidationWithServiceProviderRule<MainRule, MainRule.ContextModel>
    {
        public MainRule(TransportOfferingBidCreateRequest transportOfferingBidCreateRequest)
        {
            // Create Context
            Context = new ContextModel(transportOfferingBidCreateRequest, this);
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
            var rulesEvaluator= RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();
            
            // Assign rules to the Evaluator
            rulesEvaluator.Eval(Context.Rules.TransportValid);

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();
            
            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal
        
        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<TransportOfferingBidCreateRequest>
        {
            public ContextModel(TransportOfferingBidCreateRequest parent, IRule rule) : base(parent, rule)
            {

            }
            
            [NotNull] public Olma.Transport Transport => Rules.TransportValid.Context.Transport;
            
            public RulesBundle Rules { get; protected internal set; }

        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public readonly TransportValidRule TransportValid;
            
            public RulesBundle(ContextModel context, IRule rule)
            {
                TransportValid = new TransportValidRule(context, rule);
            }
        }
        #endregion
    }
}