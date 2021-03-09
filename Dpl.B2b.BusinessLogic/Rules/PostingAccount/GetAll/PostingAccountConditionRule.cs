using System;
using System.Collections.Generic;
using System.Linq;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal.Ltms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.PostingAccount.GetAll
{
    public class PostingAccountConditionRule : BaseValidationWithServiceProviderRule<PostingAccountConditionRule,
        PostingAccountConditionRule.ContextModel>
    {
        public PostingAccountConditionRule(MainRule.ContextModel context, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(context, this);
            ParentRule = parentRule;
        }

        /// <summary>
        ///     Message for RuleState if Rule is invalid
        /// </summary>
        protected override ILocalizableMessage Message => new NotAllowedByRule();

        /// <summary>
        ///     Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);
            Context.ConditionsRepository = ServiceProvider.GetService<IRepository<Conditions>>();
            Context.LoadCarrierRepository = ServiceProvider.GetService<IRepository<Olma.LoadCarrier>>();

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            // Not Things to validate

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        ///     Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<MainRule.ContextModel>
        {
            public ContextModel(MainRule.ContextModel parent, IRule rule) : base(parent, rule)
            {
            }
            public RulesBundle Rules { get; protected internal set; }

            public IRepository<Olma.LoadCarrier> LoadCarrierRepository { get; protected internal set; }
            public IRepository<Conditions> ConditionsRepository { get; protected internal set; }
            public IList<PostingAccountCondition> PostingAccountsConditions => GetPostingAccountsConditions();

            private IList<PostingAccountCondition> GetPostingAccountsConditions()
            {
                var allLoadCarriers = LoadCarrierRepository.FindAll().Include(lc => lc.Quality)
                    .Include(lc => lc.Type).AsNoTracking()
                    .ToList();

                var postingAccountConditions = new List<PostingAccountCondition>();

                foreach (var postingAccount in Parent.PostingAccounts)
                {
                    var conditions = ConditionsRepository.FindByCondition(c =>
                            c.AccountId == postingAccount.RefLtmsAccountId && c.ValidFrom.Date <= DateTime.Today &&
                            (!c.ValidUntil.HasValue || c.ValidUntil.Value.Date >= DateTime.Today))
                        .Include(t => t.Term)
                        .Include(d => d.BookingDependent).AsNoTracking().ToList();

                    foreach (var condition in conditions)
                    {
                        var loadCarriers =
                            allLoadCarriers.Where(lc =>
                                condition.BookingDependent != null &&
                                lc.Type.RefLtmsArticleId == condition.BookingDependent.ArticleId);

                        if (condition.BookingDependent?.QualityId != null)
                            loadCarriers = loadCarriers.Where(flc =>
                                flc.Quality.RefLtmsQualityId == condition.BookingDependent.QualityId);

                        postingAccountConditions.AddRange(loadCarriers.Select(loadCarrier => new PostingAccountCondition
                        {
                            Type = condition.TermId.GetConditionType(),
                            Amount = condition.BookingDependent?.Amount,
                            MaxQuantity = condition.Term.UpperLimit,
                            MinQuantity = condition.Term.LowerLimit,
                            LoadCarrierId = loadCarrier.Id,
                            LoadCarrierTypeOrder = loadCarrier.Type.Order,
                            RefLtmsAccountId = postingAccount.RefLtmsAccountId
                        }));
                    }
                }

                return postingAccountConditions.OrderBy(c => c.LoadCarrierTypeOrder).ToList();
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