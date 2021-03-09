using System;
using System.Dynamic;

namespace Dpl.B2b.BusinessLogic.Rules
{
    public class ContextModelBase<T>
    {
        public ContextModelBase(T parentContext, IRule rule)
        {
            Parent = parentContext;
            Rule = rule;
        }

        public IRule Rule { get; }
        
        public T Parent { get; }

        private dynamic _state;
        public dynamic State
        {
            get => _state ??= new ExpandoObject();
            set => _state = value;
        }
    }
}