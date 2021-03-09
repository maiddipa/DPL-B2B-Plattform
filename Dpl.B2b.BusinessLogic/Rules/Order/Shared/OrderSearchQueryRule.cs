using System.Collections.Generic;
using System.Linq;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Order.Shared
{
    public class OrderSearchQueryRule : BaseValidationWithServiceProviderRule<OrderSearchQueryRule,
        OrderSearchQueryRule.ContextModel>
    {
        public OrderSearchQueryRule(OrderSearchRequest request, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(request, this);
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
            Context.OrderRepository = ServiceProvider.GetService<IRepository<Olma.Order>>();

            // Initialized Evaluator
            var rulesEvaluator = RulesEvaluator.Create().StopEvaluateOnFirstInvalidRule();

            // Assign rules to the Evaluator
            // Not things to validate

            // Evaluate 
            var ruleResult = rulesEvaluator.Evaluate();

            AddMessage(!ruleResult.IsSuccess, ResourceName, Message);
            MergeFromResult(ruleResult);
        }

        #region Internal

        /// <summary>
        ///     Context for this Rule
        /// </summary>
        public class ContextModel : ContextModelBase<OrderSearchRequest>
        {
            public ContextModel(OrderSearchRequest parent, IRule rule) : base(parent, rule)
            {
            }

            public RulesBundle Rules { get; protected internal set; }
            public IRepository<Olma.Order> OrderRepository { get; protected internal set; }

            public IQueryable<Olma.Order> OrderSearchQuery => BuildOrderSearchQuery();

            public OrderSearchRequest Request => Parent;
            
            private IQueryable<Olma.Order> BuildOrderSearchQuery()
            {
                var query = OrderRepository.FindAll()
                    .Include(ol => ol.Loads)
                    .ThenInclude(d => d.Detail)
                    .AsNoTracking();

                if (Request.Type?.Count > 0) 
                    query = query.Where(i => Request.Type.Contains(i.Type));

                if (Request.TransportType?.Count > 0)
                    query = query.Where(i => Request.TransportType.Contains(i.TransportType));

                if (Request.Status?.Count > 0)
                {
                    // if status contains pending, add value "Confirmed"
                    if (Request.Status.Contains(OrderStatus.Pending)) 
                        Request.Status.Add(OrderStatus.Confirmed);
                    
                    var orderLoadStatus = new List<OrderLoadStatus>();
                    foreach (var orderStatus in Request.Status)
                    {
                        switch (orderStatus)
                        {
                            case OrderStatus.Cancelled:
                                orderLoadStatus.Add(OrderLoadStatus.Canceled);
                                break;
                            case OrderStatus.Pending:
                            case OrderStatus.Confirmed:
                                orderLoadStatus.Add(OrderLoadStatus.Pending);
                                break;
                            case OrderStatus.Fulfilled:
                                orderLoadStatus.Add(OrderLoadStatus.Fulfilled);
                                break;
                            case OrderStatus.CancellationRequested:
                                orderLoadStatus.Add(OrderLoadStatus.CancellationRequested);
                                break;
                        
                        }
                    }

                    query = query.Where(i => ((i.Loads == null || i.Loads.Count == 0) && Request.Status.Contains(i.Status)) 
                                             || (i.Loads.Any() && orderLoadStatus.Contains(i.Loads.First().Detail.Status)));
                }

                if (Request.DivisionId?.Count > 0) query = query.Where(i => Request.DivisionId.Contains(i.DivisionId));

                if (Request.PostingAccountId?.Count > 0)
                    query = query.Where(i => Request.PostingAccountId.Contains(i.PostingAccountId));

                if (Request.LoadCarrierId?.Count > 0)
                    query = query.Where(i => Request.LoadCarrierId.Contains(i.LoadCarrierId));

                if (Request.BaseLoadCarrierId?.Count > 0)
                    query = query.Where(i =>
                        i.BaseLoadCarrierId != null && Request.BaseLoadCarrierId.Contains(i.BaseLoadCarrierId.Value));

                if (Request.LoadingLocationId?.Count > 0)
                    query = query.Where(i =>
                        i.LoadingLocationId.HasValue && Request.LoadingLocationId.Contains(i.LoadingLocationId.Value));

                if (Request.QuantityType?.Count > 0)
                    query = query.Where(i => Request.QuantityType.Contains(i.QuantityType));

                if (Request.LoadCarrierQuantityFrom != null)
                    query = query.Where(i => i.LoadCarrierQuantity >= Request.LoadCarrierQuantityFrom);

                if (Request.LoadCarrierQuantityTo != null)
                    query = query.Where(i => i.LoadCarrierQuantity <= Request.LoadCarrierQuantityTo);

                if (Request.LoadCarrierQuantityFrom != null)
                    query = query.Where(i => i.Loads.Sum(l => i.Type == OrderType.Supply
                                                 ? l.SupplyOrderMatch.LoadCarrierQuantity
                                                 : l.DemandOrderMatch.LoadCarrierQuantity) >=
                                             Request.CurrentLoadCarrierQuantityFrom);

                if (Request.LoadCarrierQuantityTo != null)
                    query = query.Where(i => i.Loads.Sum(l => i.Type == OrderType.Supply
                                                 ? l.SupplyOrderMatch.LoadCarrierQuantity
                                                 : l.DemandOrderMatch.LoadCarrierQuantity) >=
                                             Request.CurrentLoadCarrierQuantityTo);

                if (Request.OrderDateFrom != null) query = query.Where(i => Request.OrderDateFrom <= i.CreatedAt);

                if (Request.OrderDateTo != null) query = query.Where(i => i.CreatedAt <= Request.OrderDateTo);

                if (Request.FulfilmentDateFrom != null)
                    query = query.Where(i => Request.FulfilmentDateFrom <= i.EarliestFulfillmentDateTime
                                             || Request.FulfilmentDateFrom <= i.LatestFulfillmentDateTime);

                if (Request.FulfilmentDateTo != null)
                    query = query.Where(i => Request.FulfilmentDateTo >= i.EarliestFulfillmentDateTime
                                             || Request.FulfilmentDateTo >= i.LatestFulfillmentDateTime);

                if (Request.HasDplNote.HasValue)
                    query = query.Where(i =>
                        (i.DplNotes.Any() || i.Loads.Any(i => i.Detail.LoadCarrierReceipt.DplNotes.Any())) ==
                        Request.HasDplNote.Value);

                return query;
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