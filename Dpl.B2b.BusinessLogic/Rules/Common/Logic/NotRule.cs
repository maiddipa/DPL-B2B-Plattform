using System;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using JetBrains.Annotations;

namespace Dpl.B2b.BusinessLogic.Rules.Common.Logic
{
    public class NotRule: BaseValidationRule {
        private readonly IValidationRule _rule;

        public NotRule([NotNull] IValidationRule rule) {
            _rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }
        
        public override void Evaluate()
        {
            var valid = !_rule.Validate();
            
            RuleState.AddMessage(!valid, ResourceName, new NotAllowedByRule());
        }
    }
}