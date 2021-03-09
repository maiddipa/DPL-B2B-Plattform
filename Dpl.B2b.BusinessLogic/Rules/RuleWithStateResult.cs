namespace Dpl.B2b.BusinessLogic.Rules
{
    public interface IRuleWithStateResult: IRuleResult{
        RuleStateDictionary State { get; }
    }

    public class RuleWithStateResult : IRuleWithStateResult
    {
        public RuleWithStateResult(RuleStateDictionary state=null)
        {
            state ??= new RuleStateDictionary();

            State = state;
        }

        public RuleStateDictionary State { get; }

        public bool IsSuccess => State.IsValid();
    }
}