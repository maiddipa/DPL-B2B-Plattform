using System;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;

namespace Dpl.B2b.BusinessLogic.Rules.Common.Operator
{
    /// <summary>
    /// Operator to check on a context
    /// </summary>
    /// <typeparam name="TContext">Context type</typeparam>
    public class ValidOperatorRule<TContext>: BaseValidationRule, IWithLocalizableMessage, IParentChildRule
    {
        private readonly TContext _context;
        private readonly Func<TContext, bool> _func;
        private readonly Func<TContext, bool> _condition;
        
        public ValidOperatorRule(Func<TContext, bool> condition, TContext context, Func<TContext, bool> func, ILocalizableMessage message = null, IRule parentRule = null, string ruleName=null)
        {
            _condition = condition;
            _context = context;
            _func = func;
            Message = message ?? new NotAllowedByRule();
            ParentRule = parentRule;
            RuleName = ruleName;
        }
        
        /// <summary>
        /// Operator to check on a context
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="context">Context for Operator</param>
        /// <param name="func">Function for the context</param>
        /// <param name="message">Is currently not supported</param>
        /// <param name="parentRule">Rule that contains this rule</param>
        /// <param name="ruleName">Name of this rule, if null then Namespace Name is taken</param>
        public ValidOperatorRule(bool condition, TContext context, Func<TContext, bool> func, ILocalizableMessage message = null, IRule parentRule = null, string ruleName=null): this((c) => condition, context,  func, message , parentRule , ruleName)
        {
            
        }

        /// <summary>
        /// Operator to check on a context
        /// </summary>
        /// <param name="context">Context for Operator</param>
        /// <param name="func">Function for the context</param>
        /// <param name="message">Is currently not supported</param>
        /// <param name="parentRule"></param>
        public ValidOperatorRule(TContext context, Func<TContext, bool> func, ILocalizableMessage message = null, IRule parentRule = null, string ruleName=null): this((c) => true, context,  func, message , parentRule , ruleName)
        {
            
        }

        public ILocalizableMessage Message { get; }
        
        public override void Evaluate()
        {
            var valid = _func(_context);
            RuleState.AddMessage(!valid, ResourceName, Message);
        }

        public override bool IsMatch()
        {
            return _condition(_context);
        }

        public IRule ParentRule { get; }
    }
}