namespace Dpl.B2b.BusinessLogic.Rules.Common.Logic
{
    public class NullRule : BaseValidationRule
    {

        public NullRule() : base()
        {

        }

        public override void Evaluate()
        {
            RuleState.Clear();
        }
    }
}