using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.PostingAccount;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.PostingAccount.Shared
{
    public class PostingAccountValidRule : BaseValidationWithServiceProviderRule<PostingAccountValidRule,
        PostingAccountValidRule.ContextModel>
    {
        public PostingAccountValidRule(PostingAccountsSearchRequest request, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(request, this);
            ParentRule = parentRule;
        }

        /// <summary>
        ///     Message for RuleState if Rule is invalid
        /// </summary>
        protected override ILocalizableMessage Message => new PostingAccountsNotFound();

        /// <summary>
        ///     Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);
            Context.PostingAccountRepository = ServiceProvider.GetService<IRepository<Olma.PostingAccount>>();
            Context.Mapper = ServiceProvider.GetService<IMapper>();

            AddMessage(!Context.PostingAccounts.Any(), ResourceName, Message);
        }

        #region Internal

        /// <summary>
        ///     Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<PostingAccountsSearchRequest>
        {
            public ContextModel(PostingAccountsSearchRequest parent, IRule rule) : base(parent, rule)
            {
            }

            private int? CustomerId => Parent.CustomerId;
            private int? PostingAccountId => Parent.PostingAccountId;

            public IRepository<Olma.PostingAccount> PostingAccountRepository { get; protected internal set; }
            public IMapper Mapper { get; protected internal set; }

            public IEnumerable<Contracts.Models.PostingAccount> PostingAccounts => GetPostingAccounts();

            public IEnumerable<Olma.PostingAccount> OlmaPostingAccounts { get; set; }

            public RulesBundle Rules { get; protected internal set; }

            private IEnumerable<Contracts.Models.PostingAccount> GetPostingAccounts()
            {
                var query = PostingAccountRepository.FindAll()
                    .Include(i => i.CalculatedBalance)
                    .Include(i => i.CalculatedBalance.Positions)
                    .AsNoTracking();

                if (CustomerId != null)
                    query = query.Where(i => i.CustomerDivisions.Any(d => d.CustomerId == CustomerId));
                
                if (PostingAccountId != null)
                    query = query.Where(i => i.Id == PostingAccountId);

                query = query.OrderBy(i => i.DisplayName);

                OlmaPostingAccounts = query.ToList();

                var postingAccountsList = Mapper.Map<IEnumerable<Contracts.Models.PostingAccount>>(OlmaPostingAccounts);

                return postingAccountsList;
            }
        }

        /// <summary>
        ///     Bundles of rules
        /// </summary>
        public class RulesBundle
        {
            public RulesBundle(ContextModel context, IRule rule)
            {
            }
        }

        #endregion
    }
}