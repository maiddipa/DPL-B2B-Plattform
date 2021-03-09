using System;

namespace Dpl.B2b.Contracts.Localizable
{
    public abstract class LocalizableMessageBase : ILocalizableMessage
    {
        public abstract LocalizableMessageType Type { get; }

        public virtual string Id
        {
            get
            {
                var namespaceName = GetNamespaceName();
                var className = GetClassName();

                var messageType = Enum.GetName(typeof(LocalizableMessageType), Type);
                return $"{messageType}|{namespaceName}|{className}";
            }
        }

        public abstract string Meaning { get; }
        public abstract string Description { get; }

        private string GetNamespaceName()
        {
            var type = GetType();
            var index = type.Namespace.LastIndexOf(".") + 1;
            var length = type.Namespace.Length - index;
            var name = type.Namespace.Substring(index, length);
            return name;
        }

        private string GetClassName()
        {
            var type = GetType();
            var name = type.Name;
            return name;
        }
    }
}