using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.BalanceTransfer;

namespace Dpl.B2b.BusinessLogic.Rules.BalanceTransfer.Create
{
    public class RequiredSourceAccountIdRule : BaseValidationWithServiceProviderRule<RequiredSourceAccountIdRule, RequiredSourceAccountIdRule.ContextModel>
    {
        public RequiredSourceAccountIdRule(int? context, IRule parentRule)
        {
            Context = new ContextModel(context, this);
            ParentRule = parentRule;
        }

        protected override ILocalizableMessage Message => new RequiredSourceAccountId();

        protected override void EvaluateInternal()
        {
            AddMessage(Context.SourceAccountId == null, ResourceName, Message);
        }
        
        public class ContextModel : ContextModelBase<int?>
        {
            public ContextModel(int? context, IRule rule) :
                base(context, rule)
            {
                
            }

            public int? SourceAccountId => Parent;
        }
    }
}