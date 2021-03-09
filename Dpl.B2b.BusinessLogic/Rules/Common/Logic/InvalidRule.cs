using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;

namespace Dpl.B2b.BusinessLogic.Rules.Common.Logic
{
    public class InvalidRule : BaseValidationRule
    {

        public InvalidRule() : base()
        {

        }

        public override void Evaluate()
        {
            RuleState.Clear();
            RuleState.AddMessage(ResourceName, new NotAllowedByRule());
        }
    }
}