using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Dpl.B2b.BusinessLogic.State;
using Newtonsoft.Json;

namespace Dpl.B2b.BusinessLogic.Rules
{
    [Serializable]
    public class RuleState:IEnumerable<StateItem>
    {
        private dynamic Context
        {
            get;
        }
        public RuleState(){}

        public RuleState(dynamic context)
        {
            Context = context;
        }

        public StateItemCollection RuleStateItems { get; } = new StateItemCollection();
        public IEnumerator<StateItem> GetEnumerator()
        {
            return RuleStateItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}