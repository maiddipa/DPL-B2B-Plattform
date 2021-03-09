using Dpl.B2b.Contracts;

namespace Dpl.B2b.BusinessLogic.Rules
{
    public static class RulesExtensions{
        public static WrappedResponse<T> WrappedResponse<T>(this IRuleResult rulesResult)
        {
            
            return new WrappedResponse<T>
            {
                ResultType = ResultType.BadRequest,
                State = ((RuleWithStateResult) rulesResult).State
            };
        }
    }
}