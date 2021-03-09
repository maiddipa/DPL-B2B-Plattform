using System.ComponentModel;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Order;
using Dpl.B2b.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Rules.Order.Search
{
    public class OrderValidRule : BaseValidationWithServiceProviderRule<OrderValidRule, OrderValidRule.ContextModel>
    {
        public OrderValidRule(MainRule.ContextModel context, IRule parentRule)
        {
            // Create Context
            Context = new ContextModel(context, this);
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
            Context.Mapper = ServiceProvider.GetService<IMapper>();

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
            public OrderSearchRequest Request => Parent.OrderSearchRequest;
            public IMapper Mapper { get; protected internal set; }
            public IPaginationResult<Contracts.Models.Order> Orders => GetOrders();

            private IPaginationResult<Contracts.Models.Order> GetOrders()
            {
                var query = Parent.Rules.OrderSearchQueryRule.Context.OrderSearchQuery;

                #region Ordering

                switch (Request.SortBy)
                {
                    case OrderSearchRequestSortOptions.Id:
                        query = query.OrderBy(i => i.Id, Request.SortDirection);
                        break;
                    case OrderSearchRequestSortOptions.CreatedAt:
                        query = query.OrderBy(i => i.CreatedAt, Request.SortDirection);
                        break;
                    case OrderSearchRequestSortOptions.PostalCode:
                        query = query.OrderBy(i => i.LoadingLocation.Address.PostalCode, Request.SortDirection);
                        break;
                    case OrderSearchRequestSortOptions.LoadCarrierName:
                        query = query.OrderBy(i => i.LoadCarrier.Name, Request.SortDirection);
                        break;
                    case OrderSearchRequestSortOptions.LoadCarrierQuantity:
                        query = query.OrderBy(i => i.LoadCarrierQuantity, Request.SortDirection);
                        break;
                    case OrderSearchRequestSortOptions.NumberOfStacks:
                        query = query.OrderBy(i => i.NumberOfStacks, Request.SortDirection);
                        break;
                    case OrderSearchRequestSortOptions.StackHeightMax:
                        query = query.OrderBy(i => i.StackHeightMax, Request.SortDirection);
                        break;
                    case OrderSearchRequestSortOptions.BaseLoadCarrierName:
                        query = query.OrderBy(i => i.BaseLoadCarrier.Name, Request.SortDirection);
                        break;
                    case OrderSearchRequestSortOptions.BaseLoadCarrierQuantity:
                        query = query.OrderBy(i => i.BaseLoadCarrierId != null ? i.NumberOfStacks : 0,
                            Request.SortDirection);
                        break;
                    case OrderSearchRequestSortOptions.Status:
                        query = query.OrderBy(i => i.Status, Request.SortDirection);
                        break;
                    case OrderSearchRequestSortOptions.HasDplNote:
                        query = query.OrderBy(
                            i => i.DplNotes.Any() || i.Loads.Any(i => i.Detail.LoadCarrierReceipt.DplNotes.Any()),
                            Request.SortDirection);
                        break;
                    case OrderSearchRequestSortOptions.EarliestFulfillmentDateTime:
                        query = query.OrderBy(i => i.EarliestFulfillmentDateTime, Request.SortDirection);
                        break;
                    case OrderSearchRequestSortOptions.LatestFulfillmentDateTime:
                        query = query.OrderBy(i => i.LatestFulfillmentDateTime, Request.SortDirection);
                        break;
                    default:
                        query = query.OrderBy(i => i.CreatedAt, ListSortDirection.Descending);
                        break;
                }

                #endregion

                var projectedQuery = query.ProjectTo<Contracts.Models.Order>(Mapper.ConfigurationProvider);
                var result = projectedQuery.ToPaginationResult(Request);
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