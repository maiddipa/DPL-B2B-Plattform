using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.Common;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Ltms = Dpl.B2b.Dal.Ltms;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class OrderLoadsService : BaseService, IOrderLoadsService
    {
        private readonly IRepository<Olma.OrderLoad> _olmaOrderLoadRepo;
        private readonly ISynchronizationsService _synchronizationsService;
        private readonly IServiceProvider _serviceProvider;

        public OrderLoadsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.OrderLoad> olmaOrderLoadRepo,
            ISynchronizationsService synchronizationsService,
            IServiceProvider serviceProvider
            ) : base(authData, mapper)
        {
            _olmaOrderLoadRepo = olmaOrderLoadRepo;
            _synchronizationsService = synchronizationsService;
            _serviceProvider = serviceProvider;
        }

        public async Task<IWrappedResponse> Cancel(int id, OrderLoadCancelRequest request)
        {
            #region security

            // TODO add security

            #endregion

            var orderLoad = _olmaOrderLoadRepo.FindByCondition(o => o.Id == id)
                .Include(i => i.Detail)
                .Include(i => i.DplNotes)
                .Include(o => o.Order)
                .Include(o => o.CreatedBy)
                .FirstOrDefault();

            if (orderLoad == null)
            {
                return NotFound<OrderLoad>(id);
            }

            var cancellableStates = new[] {OrderLoadStatus.Pending, OrderLoadStatus.TransportPlanned};
            if (IsDplEmployee)
            {
                cancellableStates = cancellableStates.Append(OrderLoadStatus.CancellationRequested).ToArray();
            }

            if (!cancellableStates.Contains(orderLoad.Detail.Status))
            {
                return BadRequest<OrderLoad>("OrderLoad is in a state that cannot be cancelled");
            }

            var status = IsDplEmployee ? OrderLoadStatus.Canceled : OrderLoadStatus.CancellationRequested;
            orderLoad.Detail.Status = status;
            if (IsDplEmployee)
            {
                var dplNote = Mapper.Map<Olma.EmployeeNote>(request.DplNote);
                orderLoad.DplNotes.Add(dplNote);
            }

            _olmaOrderLoadRepo.Save();

            var orderCancelSyncRequest = Mapper.Map<Olma.OrderLoad, OrderCancelSyncRequest>(orderLoad);
            orderCancelSyncRequest.Note = IsDplEmployee ? request.DplNote.Text : request.Note;
            var ordersSyncRequest = new OrdersSyncRequest
            {
                OrderCancelSyncRequests = new List<OrderCancelSyncRequest>
                {
                    orderCancelSyncRequest
                }
            };
            var syncResult = await _synchronizationsService.SendOrdersAsync(ordersSyncRequest);

            if (syncResult.ResultType == ResultType.Ok)
            {
                orderLoad.Order.SyncDate = DateTime.UtcNow;
            }
            _olmaOrderLoadRepo.Save();

            var responseOrderLoad = Mapper.Map<OrderLoad>(orderLoad);
            return Updated(responseOrderLoad);
        }
        
        
        public async Task<IWrappedResponse> Search(OrderLoadSearchRequest request)
        {
            var cmd = ServiceCommand<Order, Rules.OrderLoad.Search.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.OrderLoad.Search.MainRule(request))
                .Then(SearchAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> SearchAction(Rules.OrderLoad.Search.MainRule rule)
        {
            var request = rule.Context.OrderLoadSearchRequest;
            var query = BuildOrderLoadSearchQuery(request);

            var projectedQuery = query.ProjectTo<OrderLoad>(Mapper.ConfigurationProvider);
            var result = projectedQuery.ToPaginationResult(request);

            return Ok(result);
        }

        private IQueryable<Olma.OrderLoad> BuildOrderLoadSearchQuery(OrderLoadSearchRequest request)
        {
            var query = _olmaOrderLoadRepo.FindAll()
                .Where(ol => ol.Detail.Status != OrderLoadStatus.Pending)
                .AsNoTracking();

            if (request.DivisionId != null)
            {
                query = query.Where(i => request.DivisionId.Contains(i.Order.DivisionId));
            }

            if (request.DigitalCode != null)
            {
                query = query.Where(i => i.Order.Type == OrderType.Supply
                     ? i.SupplyOrderMatch.DigitalCode.StartsWith(request.DigitalCode)
                     : i.DemandOrderMatch.DigitalCode.StartsWith(request.DigitalCode)
                     );
            }

            if (request.Type?.Count > 0)
            {
                query = query.Where(i => request.Type.Contains(i.Order.Type));
            }

            if (request.TransportType?.Count > 0)
            {
                query = query.Where(i => request.TransportType.Contains(i.Order.TransportType));
            }

            if (request.Status?.Count > 0)
            {
                query = query.Where(i => request.Status.Contains(i.Detail.Status));
            }

            if (request.PostingAccountId?.Count > 0)
            {
                query = query.Where(i => request.PostingAccountId.Contains(i.Order.PostingAccountId));
            }

            if (request.LoadCarrierId?.Count > 0)
            {
                query = query.Where(i => i.Order.Type == OrderType.Supply
                    ? request.LoadCarrierId.Contains(i.SupplyOrderMatch.LoadCarrierId)
                    : request.LoadCarrierId.Contains(i.DemandOrderMatch.LoadCarrierId));
            }

            if (request.BaseLoadCarrierId?.Count > 0)
            {
                query = query.Where(i => i.Order.Type == OrderType.Supply
                    ? i.SupplyOrderMatch.BaseLoadCarrierId.HasValue && request.BaseLoadCarrierId.Contains(i.SupplyOrderMatch.BaseLoadCarrierId.Value)
                    : i.DemandOrderMatch.BaseLoadCarrierId.HasValue && request.BaseLoadCarrierId.Contains(i.DemandOrderMatch.BaseLoadCarrierId.Value));
            }

            if (request.LoadingLocationId?.Count > 0)
            {
                query = query.Where(i => i.Order.LoadingLocationId.HasValue && request.LoadingLocationId.Contains(i.Order.LoadingLocationId.Value));
            }

            if (request.LoadCarrierQuantityFrom != null)
            {
                query = query.Where(i => i.Order.Type == OrderType.Supply
                    ? i.SupplyOrderMatch.LoadCarrierQuantity >= request.LoadCarrierQuantityFrom
                    : i.DemandOrderMatch.LoadCarrierQuantity >= request.LoadCarrierQuantityFrom);
            }

            if (request.LoadCarrierQuantityTo != null)
            {
                query = query.Where(i => i.Order.Type == OrderType.Supply
                    ? i.SupplyOrderMatch.LoadCarrierQuantity <= request.LoadCarrierQuantityTo
                    : i.DemandOrderMatch.LoadCarrierQuantity <= request.LoadCarrierQuantityTo);
            }

            if (request.PlannedFulfilmentDateFrom != null)
            {
                query = query.Where(i => request.PlannedFulfilmentDateFrom <= i.Detail.PlannedFulfillmentDateTime);
            }

            if (request.PlannedFulfilmentDateTo != null)
            {
                query = query.Where(i => request.PlannedFulfilmentDateTo >= i.Detail.PlannedFulfillmentDateTime);
            }

            if (request.ActualFulfillmentDateFrom != null)
            {
                query = query.Where(i => request.PlannedFulfilmentDateFrom <= i.Detail.ActualFulfillmentDateTime);
            }

            if (request.ActualFulfillmentDateTo != null)
            {
                query = query.Where(i => request.PlannedFulfilmentDateTo >= i.Detail.ActualFulfillmentDateTime);
            }

            if (request.HasDplNote.HasValue)
            {
                // TODO OrderLoad add DplNote for order loads
                //query = query.Where(i => request.PlannedFulfilmentDateTo >= i.Order.ActualFulfillmentDateTime);
            }

            #region ordering

            switch (request.SortBy)
            {
                case OrderLoadSearchRequestSortOptions.Id:
                    query = query.OrderBy(i => i.Id, request.SortDirection);
                    break;
                case OrderLoadSearchRequestSortOptions.CreatedAt:
                    // HACK we should have non auditable field to query as well for order createdAt like orderdate
                    query = query.OrderBy(i => i.CreatedAt, request.SortDirection);
                    break;
                case OrderLoadSearchRequestSortOptions.PostalCode:
                    query = query.OrderBy(i => i.Detail.Address.PostalCode, request.SortDirection);
                    break;
                case OrderLoadSearchRequestSortOptions.LoadCarrierName:
                    query = query.OrderBy(i => i.Order.Type == OrderType.Supply ? i.SupplyOrderMatch.LoadCarrier.Name : i.DemandOrderMatch.LoadCarrier.Name, request.SortDirection);
                    break;
                case OrderLoadSearchRequestSortOptions.LoadCarrierQuantity:
                    query = query.OrderBy(i => i.Order.Type == OrderType.Supply ? i.SupplyOrderMatch.LoadCarrierQuantity : i.DemandOrderMatch.LoadCarrierQuantity, request.SortDirection);
                    break;
                case OrderLoadSearchRequestSortOptions.NumberOfStacks:
                    query = query.OrderBy(i => i.Order.Type == OrderType.Supply ? i.SupplyOrderMatch.NumberOfStacks : i.DemandOrderMatch.NumberOfStacks, request.SortDirection);
                    break;
                case OrderLoadSearchRequestSortOptions.StackHeight:
                    query = query.OrderBy(i => i.Order.Type == OrderType.Supply ? i.SupplyOrderMatch.LoadCarrierStackHeight : i.DemandOrderMatch.LoadCarrierStackHeight, request.SortDirection);
                    break;
                case OrderLoadSearchRequestSortOptions.BaseLoadCarrierName:
                    query = query.OrderBy(i => i.Order.Type == OrderType.Supply ? i.SupplyOrderMatch.BaseLoadCarrier.Name : i.DemandOrderMatch.BaseLoadCarrier.Name, request.SortDirection);
                    break;
                case OrderLoadSearchRequestSortOptions.BaseLoadCarrierQuantity:
                    query = query.OrderBy(i => i.Order.Type == OrderType.Supply ? i.SupplyOrderMatch.BaseLoadCarrierQuantity : i.DemandOrderMatch.BaseLoadCarrierQuantity, request.SortDirection);
                    break;
                case OrderLoadSearchRequestSortOptions.Status:
                    query = query.OrderBy(i => i.Detail.Status, request.SortDirection);
                    break;
                case OrderLoadSearchRequestSortOptions.PlannedFulfillmentDateTime:
                    query = query.OrderBy(i => i.Detail.PlannedFulfillmentDateTime, request.SortDirection);
                    break;
                case OrderLoadSearchRequestSortOptions.ActualFulfillmentDateTime:
                    query = query.OrderBy(i => i.Detail.ActualFulfillmentDateTime.HasValue ? i.Detail.ActualFulfillmentDateTime.Value : DateTime.MaxValue, request.SortDirection);
                    break;
                default:
                    query = query.OrderBy(i => i.CreatedAt, System.ComponentModel.ListSortDirection.Descending);
                    break;
            }

            #endregion

            return query;
        }

    }
}
