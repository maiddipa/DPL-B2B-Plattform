using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.LoadCarrierOfferings;

namespace Dpl.B2b.BusinessLogic.Rules.LoadCarrierOfferings.Search
{
    public class StackHighValidRule : BaseValidationRule, IParentChildRule
    {
        public StackHighValidRule(int stackHigh, IRule parentRule)
        {
            StackHigh = stackHigh;
            ParentRule = parentRule;
        }

        public readonly int StackHigh;

        public override void Evaluate()
        {
            RuleState.AddMessage(StackHigh<0, ResourceName, new InvalidStackHigh());
        }

        public IRule ParentRule { get; }
    }
}