using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.LoadCarrierOfferings;

namespace Dpl.B2b.BusinessLogic.Rules.LoadCarrierOfferings.Search
{
    public class RadiusValidRule : BaseValidationRule, IParentChildRule
    {
        public RadiusValidRule(int radius, IRule parentRule)
        {
            Radius = radius;
            ParentRule = parentRule;
        }

        public readonly int Radius;

        public override void Evaluate()
        {
            RuleState.AddMessage(Radius<0, ResourceName, new InvalidRadius());
        }

        public IRule ParentRule { get; }
    }
}