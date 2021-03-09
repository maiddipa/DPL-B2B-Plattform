using System;
using System.Collections.Generic;
using Microsoft.Azure.ServiceBus;
using System.Text.Json;
namespace Dpl.B2b.BusinessLogic.ErrorHandlers.Exceptions
{
    [Serializable]
    public class SynchronizationException : Exception
    {
        private string _messagesJson;
        public SynchronizationException()
        {
        }

        public SynchronizationException(string exceptionMessage)
            : base(exceptionMessage)
        {
        }

        public SynchronizationException(string exceptionMessage, Exception inner)
            : base(exceptionMessage, inner)
        {
        }
    }
}