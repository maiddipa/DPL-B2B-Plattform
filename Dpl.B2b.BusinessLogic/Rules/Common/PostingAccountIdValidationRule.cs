using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Dpl.B2b.BusinessLogic.Rules.OrderGroup;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Common
{
    public class PostingAccountIdValidationRule : BaseValidationWithServiceProviderRule<PostingAccountIdValidationRule,
        PostingAccountIdValidationRule.ContextModel>
    {
        public PostingAccountIdValidationRule(int postingAccountId, IRule parentRule)
        {
            ResourceName = "PostingAccount";
            Context = new ContextModel(postingAccountId, this);
            ParentRule = parentRule;
        }

        protected override ILocalizableMessage Message => new PostingAccountNotFound();

        protected override void EvaluateInternal()
        {
            if (Context.PostingAccountId == 0)
            {
                RuleState.Add(ResourceName, new RuleState {RuleStateItems = {new PostingAccountRequired()}});
                return;
            }

            var postingAccountRepository = ServiceProvider.GetService<IRepository<Olma.PostingAccount>>();
            var postingAccountExists = postingAccountRepository.FindAll()
                .Any(pa => pa.Id == Context.PostingAccountId);

            AddMessage(!postingAccountExists, ResourceName, Message);
        }

        public class ContextModel : ContextModelBase<int>
        {
            public ContextModel(int postingAccountId,
                [NotNull] PostingAccountIdValidationRule rule) :
                base(postingAccountId, rule)
            {
            }

            public int PostingAccountId => Parent;
        }
    }
}