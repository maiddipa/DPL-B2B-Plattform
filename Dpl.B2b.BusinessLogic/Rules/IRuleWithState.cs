
namespace Dpl.B2b.BusinessLogic.Rules
{
    public interface IRuleWithState : IRule
    {
        RuleStateDictionary RuleState { get; }
    }
}