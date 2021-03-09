using System.Linq;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;

namespace Dpl.B2b.BusinessLogic.Rules.Common.Logic
{
    /// <summary>
    /// Wrapper für eine Validation Rule
    /// Achtung: Der Einsatz dieser Regel durchbricht die vereinigung des RuleState
    /// Das bedeutet die verwendung dieser Regel reicht den internen RuleState nicht zum Aufrufer
    /// </summary>
    public class OrRule: BaseValidationRule {
        private readonly IValidationRule[] _rules;

        public OrRule(params IValidationRule[] rules) {
            _rules = rules;
        }
        
        public override void Evaluate()
        {
            if (!_rules.Any()) return;
            
            var valid = _rules.Any(r => r.IsValid());
            RuleState.AddMessage(!valid, RuleName, new NotAllowedByRule());
        }
    }
}