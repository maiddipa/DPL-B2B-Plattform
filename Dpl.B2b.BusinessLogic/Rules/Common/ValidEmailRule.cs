using System;
using System.Text.RegularExpressions;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;

namespace Dpl.B2b.BusinessLogic.Rules.Common
{
    public class ValidEmailRule<TContext>: BaseValidationRule, IWithLocalizableMessage, IParentChildRule, IRule
    {
        private readonly TContext _context;
        private readonly Func<TContext, string> _func;
        private const string EmailPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        
        public ValidEmailRule(TContext context, Func<TContext, string> func, string ruleName=null, IRule parentRule=null)
        {
            if (string.IsNullOrEmpty(ruleName))
            {
                RuleName = this.NamespaceName();
            }
            
            _context = context;
            _func = func;
            Message = new InvalidEmail();
            ParentRule = parentRule;
        }
        
        private string Email=> _func(_context);

        public override void Evaluate()
        {
            var invalid = !IsEmail(Email);
            RuleState.AddMessage(invalid, ResourceName, Message);
        }

        public static bool IsEmail(string email)
        {
            //check first string
            return email == null || Regex.IsMatch(email, EmailPattern);
        }

        public ILocalizableMessage Message { get; }
        public IRule ParentRule { get; }
    }
}