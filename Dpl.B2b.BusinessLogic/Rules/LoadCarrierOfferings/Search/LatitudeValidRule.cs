using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.LoadCarrierOfferings;
using Dpl.B2b.Contracts.Models;
using PostSharp.Extensibility;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.LoadCarrierOfferings.Search
{
    public class LatitudeValidRule : BaseValidationRule, IParentChildRule
    {
        public LatitudeValidRule(double latitude, IRule parentRule)
        {
            Latitude = latitude;
            ParentRule = parentRule;
        }

        public readonly double Latitude;

        public override void Evaluate()
        {
            RuleState.AddMessage(Latitude < -90 || Latitude > 90, ResourceName, new InvalidLatitude());
        }

        public IRule ParentRule { get; }
    }
}