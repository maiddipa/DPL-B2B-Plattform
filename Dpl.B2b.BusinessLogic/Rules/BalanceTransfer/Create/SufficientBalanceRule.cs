using System.Collections.Generic;
using System.Linq;
using Dpl.B2b.BusinessLogic.Rules.Common;
using Dpl.B2b.BusinessLogic.Rules.Common.Operator;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.BalanceTransfer;
using Dpl.B2b.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dpl.B2b.BusinessLogic.Rules.BalanceTransfer.Create
{
    public class SufficientBalanceRule : BaseValidationWithServiceProviderRule<SufficientBalanceRule, SufficientBalanceRule.ContextModel>
    {
        public SufficientBalanceRule(MainRule.ContextModel context, IRule parentRule)
        {
            Context = new ContextModel(context, this);
            ParentRule = parentRule;
        }

        protected override ILocalizableMessage Message => new SufficientBalance();

        protected override void EvaluateInternal()
        {
            // Check Resource
            var resourceExistsRule = new ResourceValidRule(this);

            resourceExistsRule.Context.SourceAccountId = Context.SourceAccountId;
            resourceExistsRule.Context.LoadCarrierId = Context.LoadCarrierId;
            resourceExistsRule.Context.LoadCarrierQuantity = Context.LoadCarrierQuantity;
            resourceExistsRule.Validate(RuleState);

            // resource issues 
            if (!resourceExistsRule.IsValid()) return;

            // Safe properties
            var postingAccountId = resourceExistsRule.Context.SourceAccountId;
            var loadCarrierId = resourceExistsRule.Context.LoadCarrierId;
            var quantity = resourceExistsRule.Context.LoadCarrierQuantity;

            // Service
            var postingAccountsService = ServiceProvider.GetService<IPostingAccountsService>();

            // Current State
            var balance = (IWrappedResponse<IEnumerable<Balance>>) postingAccountsService.GetBalances(postingAccountId).Result;

            // Extract State
            var balanceDic = balance.Data.ToDictionary((b) => b.LoadCarrierId, (b) => b);
            var value = balanceDic[loadCarrierId];

            // BL
            var maxBalance = value.CoordinatedBalance - value.ProvisionalCharge - value.PostingRequestBalanceCharge - value.UncoordinatedCharge;

            // TODO Anmerkung von DoS TK-10/07 . Vorzeichen ist anders herum zu bewerten.  WorkItem #3397
            // Validate
            AddMessage(maxBalance < quantity, ResourceName, Message);
        }

        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel context, IRule rule) :
                base(context, rule)
            {
            }

            public int? LoadCarrierId => Parent.LoadCarrierId;
            public int? LoadCarrierQuantity => Parent.LoadCarrierQuantity;
            public int? SourceAccountId => Parent.SourceAccount?.Id;
        }
    }
}