using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.BalanceTransfer;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.TransportOfferings;

namespace Dpl.B2b.BusinessLogic.Rules.TransportOfferings.CreateBid
{
    public class TransportIdRequiredRule :BaseValidationWithServiceProviderRule<TransportIdRequiredRule, TransportIdRequiredRule.ContextModel>
    {
        public TransportIdRequiredRule(TransportValidRule.ContextModel context, IRule parentRule)
        {
            Context = new ContextModel(context, this);
            ParentRule = parentRule;
        }

        protected override ILocalizableMessage Message => new TransportIdRequired();

        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
           // No need to validate
            
            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            var hasNoTransportId = Context.TransportId == null;
            AddMessage(hasNoTransportId, ResourceName, Message);
            
            MergeFromResult(ruleResult);
        }
        
        public class ContextModel : ContextModelBase<TransportOfferings.CreateBid.TransportValidRule.ContextModel>
        {
            public ContextModel(TransportOfferings.CreateBid.TransportValidRule.ContextModel context, IRule rule) :
                base(context, rule)
            {
                
            }
            
            public RulesBundle Rules { get; protected internal set; }

            public int? TransportId => Parent.TransportId;
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
    }
}