using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.BalanceTransfer;

namespace Dpl.B2b.BusinessLogic.Rules.BalanceTransfer.Create
{
    public class RequiredLoadCarrierIdRule :BaseValidationWithServiceProviderRule<RequiredLoadCarrierIdRule, RequiredLoadCarrierIdRule.ContextModel>
    {
        public RequiredLoadCarrierIdRule(MainRule.ContextModel context, IRule parentRule)
        {
            Context = new ContextModel(context, this);
            ParentRule = parentRule;
        }

        protected override ILocalizableMessage Message => new RequiredLoadCarrierQuantity();

        protected override void EvaluateInternal()
        {
            AddMessage(!Context.Parent.LoadCarrierQuantity.HasValue, ResourceName, Message);
        }
        
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel context, IRule rule) :
                base(context, rule)
            {
                
            }
        }
    }
}