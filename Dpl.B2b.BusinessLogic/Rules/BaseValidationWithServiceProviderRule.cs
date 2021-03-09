using System;
using Dpl.B2b.BusinessLogic.Rules.BalanceTransfer;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts.Localizable;
using Microsoft.Extensions.DependencyInjection;

namespace Dpl.B2b.BusinessLogic.Rules
{
    /// <summary>
    /// Base class for using a general validation rule with a RuleState.
    /// </summary>
    public abstract class BaseValidationWithServiceProviderRule<TRule, TContext> : BaseValidationRule, IRuleWithServiceProvider<TRule>
        where TRule : class, IRule where TContext: class
    {
        protected abstract ILocalizableMessage Message { get; }
        
        public TRule AssignServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            
            return this as TRule;
        }
        
        IRule IRuleWithServiceProvider.AssignServiceProvider(IServiceProvider serviceProvider)
        {
            return AssignServiceProvider(serviceProvider);
        }
        
        public TRule AssignParentRule(IRule parentRule)
        {
            ParentRule = parentRule;
            
            return this as TRule;
        }

        private IServiceProvider _serviceProvider;
        public IServiceProvider ServiceProvider
        {
            get => ParentRule is IRuleWithServiceProvider parentRule && _serviceProvider == null
                ? parentRule.ServiceProvider
                : _serviceProvider;
            private set => _serviceProvider = value;
        }

        public TContext Context { get; protected set; }
        
        public override void Evaluate()
        {
            if (ServiceProvider==null)
            {
                throw new Exception("First assign a service scope");
            }
            
            BeforeEvaluate?.Invoke(this, new EvaluateInternalEventArgs(RuleState){});
            
            EvaluateInternal();

            AfterEvaluate?.Invoke(this, new EvaluateInternalEventArgs(RuleState));
            
            if(RuleState.IsValid())
                ValidEvaluate?.Invoke(this, new EvaluateInternalEventArgs(RuleState));
        }
        public event AfterEvaluateInternalHandler AfterEvaluate;
        public event BeforeEvaluateInternalHandler BeforeEvaluate;
        public event ValidInternalHandler ValidEvaluate;

        public delegate void ValidInternalHandler(object sender, EvaluateInternalEventArgs e);
        public delegate void AfterEvaluateInternalHandler(object sender, EvaluateInternalEventArgs e);
        public delegate void BeforeEvaluateInternalHandler(object sender, EvaluateInternalEventArgs e);
        
        /// <summary>
        /// Internal Method for Evaluate
        /// </summary>
        protected abstract void EvaluateInternal();

        /// <summary>
        /// Uplink to the consuming Rule
        /// </summary>
        public IRule ParentRule { get; protected set; }

        
        /// <summary>
        /// If the condition applies, the message is added to the RuleState under the given key
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        protected void AddMessage(bool condition, string key, ILocalizableMessage message)
        {
            RuleState.AddMessage(condition, key, message);
        }
        
        /// <summary>
        /// If the IRuleResult is of type RuleWithStateResult then the RuleState is united into local RuleState
        /// </summary>
        /// <param name="ruleResult"></param>
        protected void MergeFromResult(IRuleResult ruleResult)
        {
            // Merge result if Rule has a State Result
            if (ruleResult is RuleWithStateResult stateResult)
            {
                RuleState.Merge(stateResult.State);
            }
        }
    }
    
    public class EvaluateInternalEventArgs: EventArgs
    {
        public EvaluateInternalEventArgs(RuleStateDictionary ruleState)
        {
            RuleState = ruleState;
        }

        public RuleStateDictionary RuleState { get; }
    }
}

