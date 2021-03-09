using System;
using System.Runtime.Serialization;
using Dpl.B2b.BusinessLogic.Rules;
using Dpl.B2b.Contracts.Localizable;

namespace Dpl.B2b.BusinessLogic.State
{
    [DataContract]
    public class StateItem: IEquatable<StateItem>
    {
        public StateItem()
        {

        }

        public StateItem(ILocalizableMessage localizableMessage)
        {
            switch (localizableMessage.Type)
            {
                case LocalizableMessageType.Error:
                {
                    Type = StateItemType.Error;
                    break;
                }
                case LocalizableMessageType.Warning:
                {
                    Type = StateItemType.Warning;
                    break;
                }
                
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException(nameof(localizableMessage.Type), (int)localizableMessage.Type, localizableMessage.GetType());
            }

            RuleMessage = localizableMessage;
        }

        [DataMember]
        public StateItemType Type { get; }

        private ILocalizableMessage RuleMessage { get; }

        [DataMember]
        public string MessageId => $"{RuleMessage.Id}";

        public bool Equals(StateItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return MessageId == other.MessageId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StateItem) obj);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = MessageId.GetHashCode();
                hashCode = (hashCode*397) ^ (Type != null ? Type.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}