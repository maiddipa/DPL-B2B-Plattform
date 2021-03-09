using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.BalanceTransfer;

namespace Dpl.B2b.BusinessLogic.Rules.BalanceTransfer.Create
{
    public class RequiredDestinationAccountIdRule : BaseValidationWithServiceProviderRule<RequiredDestinationAccountIdRule, RequiredDestinationAccountIdRule.ContextModel>
    {
        public RequiredDestinationAccountIdRule(int? context, IRule parentRule)
        {
            Context = new ContextModel(context, this);
            ParentRule = parentRule;
        }

        protected override ILocalizableMessage Message => new RequiredDestinationAccountId();

        protected override void EvaluateInternal()
        {
            AddMessage(Context.DestinationAccountId == null, ResourceName, Message);
        }
        
        public class ContextModel : ContextModelBase<int?>
        {
            public ContextModel(int? context, IRule rule) :
                base(context, rule)
            {
                
            }

            public int? DestinationAccountId => Parent;
        }
    }
}