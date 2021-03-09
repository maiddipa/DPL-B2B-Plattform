using System;

namespace Dpl.B2b.BusinessLogic.Rules.Common.Operator
{
    /// <summary>
    /// Operator to get value dynamically from a context
    /// </summary>
    /// <typeparam name="TContext">Context type</typeparam>
    /// <typeparam name="TResult">Operator result</typeparam>
    public class GetOperatorRule<TContext, TResult> : IRule<TResult>
    {
        private readonly TContext _context;
        private readonly Func<TContext, TResult> _func;

        public GetOperatorRule(TContext context, Func<TContext, TResult> func, string ruleName=null)
        {
            if (string.IsNullOrEmpty(ruleName))
            {
                RuleName = this.NamespaceName();
            }
            
            _context = context;
            _func = func;
            RuleName = ruleName;
        }
        
        public TResult Result { get; private set; }

        public TResult Evaluate()
        {
            Result=_func(_context);
                
            return Result;
        }
            
        void IRule.Evaluate()
        {
            Result=_func(_context);

            return;
        }

        public bool IsMatch()
        {
            return true;
        }

        public string RuleName { get; private set; }
    }
}