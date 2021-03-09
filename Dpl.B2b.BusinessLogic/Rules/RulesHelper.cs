using System;
using Dpl.B2b.BusinessLogic.Rules.Common.Logic;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;

namespace Dpl.B2b.BusinessLogic.Rules
{
    // Helpers for AndRule / OrRule
    public static class RulesHelper
    {
        /// <summary>
        /// Factory für eine UND Regel
        /// Achtung: Der Einsatz dieser Regel durchbricht die vereinigung des RuleState
        /// Das bedeutet die verwendung dieser Regel reicht den internen RuleState nicht zum Aufrufer
        /// </summary>
        public static IRule And(params IValidationRule[] rules)
        {
            return new AndRule(rules);
        }

        /// <summary>
        /// Factory für eine ODER Regel
        /// Achtung: Der Einsatz dieser Regel durchbricht die vereinigung des RuleState
        /// Das bedeutet die verwendung dieser Regel reicht den internen RuleState nicht zum Aufrufer
        /// </summary>
        public static IRule Or(params IValidationRule[] rules)
        {
            return new OrRule(rules);
        }

        /// <summary>
        /// Factory für eine NICHT Regel
        /// Achtung: Der Einsatz dieser Regel durchbricht die vereinigung des RuleState
        /// Das bedeutet die verwendung dieser Regel reicht den internen RuleState nicht zum Aufrufer
        /// </summary>
        public static IRule Not(IValidationRule rules)
        {
            return new NotRule(rules);
        }

        public static string NamespaceName([NotNull] this IRule rule) {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            
            var type = rule.GetType();
            if (type.Namespace == null) return string.Empty;
                
            //var index = type.Namespace.LastIndexOf(".", StringComparison.Ordinal) + 1;
            var index = type.Namespace.LastIndexOf("Rules", StringComparison.Ordinal);
                
            var length = type.Namespace.Length - index;
            var name = type.Namespace.Substring(index, length);
            return name;
        }
    }
}