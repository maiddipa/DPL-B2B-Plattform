using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Order;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Order.Summary
{
    public class OrderSummaryValidRule : BaseValidationWithServiceProviderRule<OrderSummaryValidRule,
        OrderSummaryValidRule.ContextModel>
    {
        public OrderSummaryValidRule(MainRule.ContextModel context, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(context, this)
            {
                Mapper = ServiceProvider.GetService<IMapper>()
            };
            ParentRule = parentRule;
        }

        /// <summary>
        ///     Message for RuleState if Rule is invalid
        /// </summary>
        protected override ILocalizableMessage Message => new OrderValid();

        /// <summary>
        ///     Internal Method for Evaluate
        /// </summary>
        protected override void EvaluateInternal()
        {
            // Initialized all rules. Instancing only takes place during execution, since otherwise the service scope has not yet been assigned
            Context.Rules = new RulesBundle(Context, this);

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
            public IMapper Mapper { get; protected internal set; }
            public IEnumerable<OrderSummary> OrderSummaries => GetSummaries();

            private IEnumerable<OrderSummary> GetSummaries()
            {
                var query = Parent.Rules.OrderSearchQueryRule.Context.OrderSearchQuery
                    .Include(o => o.LoadCarrier)
                    .ThenInclude(l => l.Type)
                    .AsEnumerable();

                var summaryByLoadCarrierAndStatusLookup =
                    query.GroupBy(i => new {i.LoadCarrier.TypeId, i.Status})
                        .Select(g => new
                        {
                            g.Key, //TODO Implement calculation for other OrderQuantityTypes, at this time only OrderQuantityType.Load is supported
                            Sum = g.Sum(p =>
                                p.QuantityType == OrderQuantityType.Load
                                    ? p.StackHeightMax * 33 * p.LoadCarrier.Type.QuantityPerEur
                                    : 0), //HACK Hardcoded StackCount
                            Count = g.Count()
                        })
                        .ToLookup(i => i.Key.TypeId);

                var result = summaryByLoadCarrierAndStatusLookup.Select(g => new OrderSummary
                {
                    LoadCarrierId = g.Key,
                    Sum = new OrderAggregate
                    {
                        Open = g.Where(i => i.Key.Status == OrderStatus.Pending).Select(i => i.Sum).SingleOrDefault(),
                        Confirmed = g.Where(i => i.Key.Status == OrderStatus.Confirmed).Select(i => i.Sum)
                            .SingleOrDefault(),
                        PartiallyMatched = g.Where(i => i.Key.Status == OrderStatus.PartiallyMatched).Select(i => i.Sum)
                            .SingleOrDefault(),
                        Matched =
                            g.Where(i => i.Key.Status == OrderStatus.Matched).Select(i => i.Sum).SingleOrDefault(),
                        //Planned = g.Where(i => i.Key.Status == OrderStatus.TransportPlanned).Select(i => i.Sum).SingleOrDefault(),
                        //Fulfilled = g.Where(i => i.Key.Status == OrderStatus.Fulfilled).Select(i => i.Sum).SingleOrDefault(),
                        Cancelled = g.Where(i => i.Key.Status == OrderStatus.Cancelled).Select(i => i.Sum)
                            .SingleOrDefault(),
                        Expired = g.Where(i => i.Key.Status == OrderStatus.Expired).Select(i => i.Sum).SingleOrDefault()
                    },
                    Count = new OrderAggregate
                    {
                        Open = g.Where(i => i.Key.Status == OrderStatus.Pending).Select(i => i.Count).SingleOrDefault(),
                        Confirmed = g.Where(i => i.Key.Status == OrderStatus.Confirmed).Select(i => i.Count)
                            .SingleOrDefault(),
                        //Planned = g.Where(i => i.Key.Status == OrderStatus.TransportPlanned).Select(i => i.Count).SingleOrDefault(),
                        PartiallyMatched = g.Where(i => i.Key.Status == OrderStatus.PartiallyMatched)
                            .Select(i => i.Count).SingleOrDefault(),
                        Matched = g.Where(i => i.Key.Status == OrderStatus.Matched).Select(i => i.Count)
                            .SingleOrDefault(),
                        //Fulfilled = g.Where(i => i.Key.Status == OrderStatus.Fulfilled).Select(i => i.Count).SingleOrDefault(),
                        Cancelled = g.Where(i => i.Key.Status == OrderStatus.Cancelled).Select(i => i.Count)
                            .SingleOrDefault(),
                        Expired = g.Where(i => i.Key.Status == OrderStatus.Expired).Select(i => i.Count)
                            .SingleOrDefault()
                    }
                });

                return result;
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