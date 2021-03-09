using System;
using DevExpress.CodeParser;
using Dpl.B2b.BusinessLogic.Rules;

namespace Dpl.B2b.BusinessLogic.Rules
{
    public abstract class BaseRule : IRuleWithState
    {
        public RuleStateDictionary RuleState { get; protected set; } = new RuleStateDictionary();

        public abstract void Evaluate();
        
        public virtual bool IsMatch()
        {
            return true;
        }

        private string _ruleName;
        
        public string RuleName
        {
            get => _ruleName ?? this.NamespaceName();
            protected set => _ruleName = value;
        }

        //TODO RF => RuleName Alias 
        public string ResourceName
        {
            get => RuleName;
            set => RuleName = value;
        }

       
    }
}