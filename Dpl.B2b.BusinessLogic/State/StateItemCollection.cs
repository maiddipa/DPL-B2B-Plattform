using System;
using System.Collections.ObjectModel;
using Dpl.B2b.Contracts.Localizable;

namespace Dpl.B2b.BusinessLogic.State
{
    [Serializable]
    public class StateItemCollection : Collection<StateItem>
    {
        public void Add(ILocalizableMessage message)
        {
            Add(new StateItem(message));
        }
    }
}