using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.BalanceTransfer;
using Microsoft.Extensions.DependencyInjection; //using JetBrains.Annotations;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.BalanceTransfer.Create
{
    public class ValidSourceAccountRule : BaseValidationWithServiceProviderRule<ValidSourceAccountRule,
        ValidSourceAccountRule.ContextModel>, IParentChildRule
    {
        public ValidSourceAccountRule(MainRule.ContextModel request, IRule parentRule)
        {
            Context = new ContextModel(request, this);
            ParentRule = parentRule;
        }

        protected override ILocalizableMessage Message => new ValidSourceAccount();

        protected override void EvaluateInternal()
        {
            Context.Rules = new RulesBundle(Context, this);
            
            var ruleResult = RulesEvaluator.Create()
                .Eval(Context.Rules.RequiredSourceAccountIdRule)
                .Evaluate();
            
            MergeFromResult(ruleResult);
            
            if (!ruleResult.IsSuccess) 
                return;

            var postingAccountRepository = ServiceProvider.GetService<IRepository<Olma.PostingAccount>>();
            Context.SourceAccount = postingAccountRepository.GetByKey(Context.SourceAccountId);
            
            AddMessage(Context.SourceAccount == null, ResourceName, Message);
        }
        
        #region Internal
        
        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel([NotNull] MainRule.ContextModel request, [NotNull] ValidSourceAccountRule rule) :
                base(request, rule)
            {
                
            }

            public RulesBundle Rules { get; protected internal set; } 
            
            public int? SourceAccountId => Parent.Parent.SourceAccountId;

            public Olma.PostingAccount SourceAccount { get; set; }
        }
        
        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public class RulesBundle
        {
            public RequiredSourceAccountIdRule RequiredSourceAccountIdRule { get; }

            public RulesBundle(ContextModel context, IRule rule)
            {
                RequiredSourceAccountIdRule = new RequiredSourceAccountIdRule(context.SourceAccountId, rule);
            }
        }
        
        #endregion
    }
}