using System.Dynamic;
using System.Linq;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.BalanceTransfer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.BalanceTransfer.Create
{
    public class ValidDestinationAccountRule : BaseValidationWithServiceProviderRule<ValidDestinationAccountRule, ValidDestinationAccountRule.ContextModel>
    {
        public ValidDestinationAccountRule(MainRule.ContextModel request, IRule parentRule)
        {
            Context = new ContextModel(request, this);
            ParentRule = parentRule;
        }

        protected override ILocalizableMessage Message => new ValidDestinationAccount();

        protected override void EvaluateInternal()
        {
            Context.Rules = new RulesBundle(Context, this);

            var ruleResult = RulesEvaluator.Create()
                .Eval(Context.Rules.RequiredDestinationAccountIdRule)
                .Evaluate();
            
            MergeFromResult(ruleResult);
            
            if (!ruleResult.IsSuccess) 
                return;
            
            var postingAccountRepository = ServiceProvider.GetService<IRepository<Olma.PostingAccount>>();

            Context.DestinationAccount = postingAccountRepository
                .FindByCondition(a => a.Id == Context.DestinationAccountId && !a.IsDeleted)
                .IgnoreQueryFilters().SingleOrDefault(); //HACK to get RefLtmsAccountId of Destination Acccount

            AddMessage(Context.DestinationAccount == null, ResourceName, Message);
        }

        # region Internal
        
        /// <summary>
        /// Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel request, ValidDestinationAccountRule rule) :
                base(request, rule)
            {
            }

            public RulesBundle Rules { get; protected internal set; }
            
            public Olma.PostingAccount DestinationAccount { get; set; }

            public Olma.ExpressCode ExpressCode => Parent.ExpressCode;

            public int? DestinationAccountId => ExpressCode?.PostingAccountPreset?.DestinationAccountId ??
                                                Parent.Parent.DestinationAccountId;
        }

        /// <summary>
        /// Bundles of rules 
        /// </summary>
        public  class RulesBundle
        {
            public readonly RequiredDestinationAccountIdRule RequiredDestinationAccountIdRule;

            public RulesBundle(ContextModel context, IRule rule)
            {
                RequiredDestinationAccountIdRule = new RequiredDestinationAccountIdRule(context.DestinationAccountId, rule);
            }
        }
        
        # endregion
    }
}
