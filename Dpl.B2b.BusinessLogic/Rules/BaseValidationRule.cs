using System;

namespace Dpl.B2b.BusinessLogic.Rules
{
    /// <summary>
    /// Base class for using a general validation rule with a RuleState.
    /// </summary>
    public abstract class BaseValidationRule: BaseRule, IValidationRuleWithState
    {
        /// <summary>
        /// 
        /// Number of calls to the Validate method
        /// </summary>
        private int _validateCounter;

        /// <summary>
        /// Checks predicate or the last validity.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid()
        {
            // If no validate has been called yet, call validate automatically and return the result
            return _validateCounter == 0 
                ? Validate() 
                : RuleState.IsValid();
        }

        public virtual bool Validate()
        {
            _validateCounter++;

            Evaluate();

            return IsValid();
        }

        public virtual bool Validate(RuleStateDictionary ruleState)
        {
            _validateCounter++;
            
            Evaluate();

            ruleState.Merge(RuleState);

            return IsValid();
        }
    }
}