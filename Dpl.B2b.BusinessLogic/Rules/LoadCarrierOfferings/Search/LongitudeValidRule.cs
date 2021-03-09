using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.LoadCarrierOfferings;

namespace Dpl.B2b.BusinessLogic.Rules.LoadCarrierOfferings.Search
{
    public class LongitudeValidRule : BaseValidationRule, IParentChildRule
    {
        public LongitudeValidRule(double longitude, IRule parentRule)
        {
            Longitude = longitude;
            ParentRule = parentRule;
        }

        public readonly double Longitude;

        public override void Evaluate()
        {
            RuleState.AddMessage(Longitude < -180 || Longitude >180, ResourceName, new InvalidLongitude());
        }

        public IRule ParentRule { get; }
    }
}