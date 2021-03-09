using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Dpl.B2b.BusinessLogic.Rules
{
    [DataContract]
    public class RuleStatesResult
    {
        [DataMember(Name="Type")]
        public Type Discriminator => Type.BusinessLogik;
        
        [DataMember(Name = "States")]
        public RuleStateDictionary States { get; set; }

        public enum Type
        {
            BusinessLogik
        }
    }
}