using System.Collections;
using System.Collections.Generic;
using Dpl.B2b.BusinessLogic.Rules;
using Dpl.B2b.BusinessLogic.State;

namespace Dpl.B2b.BusinessLogic.ErrorHandlers
{
    public class ServiceState:IEnumerable<StateItem>
    {
        private dynamic Context
        {
            get;
        }
        public ServiceState(){}

        public ServiceState(dynamic context)
        {
            Context = context;
        }

        public StateItemCollection StateItems { get; } = new StateItemCollection();
        public IEnumerator<StateItem> GetEnumerator()
        {
            return StateItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
