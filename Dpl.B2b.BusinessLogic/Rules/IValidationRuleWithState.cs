namespace Dpl.B2b.BusinessLogic.Rules
{
    public interface IValidationRuleWithState : IValidationRule, IRuleWithState
    {
        /// <summary>
        /// Checks the status for validity and inserts the internal status into the transferred status
        /// </summary>
        /// <param name="ruleState">Outer RuleState</param>
        /// <returns></returns>
        public bool Validate(RuleStateDictionary ruleState);
    }
}