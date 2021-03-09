using AutoMapper;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.BusinessLogic.Rules;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.Contracts.Authorization.Model;
using Olma = Dpl.B2b.Dal.Models;
using LinqKit;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    internal class OrderGroupsService : BaseService, IOrderGroupsService
    {
        private readonly Dal.OlmaDbContext _olmaDbContext;
        private readonly IRepository<Olma.OrderGroup> _olmaOrderGroupRepo;
        private readonly IOrderMatchesService _orderMatchesService;
        private readonly ISynchronizationsService _synchronizationsService;
        private readonly INumberSequencesService _numberSequencesService;
        private readonly ILoadCarriersService _loadCarriersService;
        private readonly IServiceProvider _serviceProvider;

        public OrderGroupsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            Dal.OlmaDbContext olmaDbContext,
            IRepository<Olma.OrderGroup> olmaOrderGroupRepo,
            IOrderMatchesService orderMatchesService,
            ISynchronizationsService synchronizationsService,
            INumberSequencesService numberSequencesService,
            ILoadCarriersService loadCarriersService,
            IServiceProvider serviceProvider
        ) : base(authData, mapper)
        {
            _olmaDbContext = olmaDbContext;
            _olmaOrderGroupRepo = olmaOrderGroupRepo;
            _orderMatchesService = orderMatchesService;
            _synchronizationsService = synchronizationsService;
            _numberSequencesService = numberSequencesService;
            _loadCarriersService = loadCarriersService;
            _serviceProvider = serviceProvider;
        }

        public async Task<IWrappedResponse> Search(OrderGroupsSearchRequest request)
        {
            #region security

            // TODO add security

            #endregion

            // TODO Any one of the fields on the request must be filled

            var query = _olmaOrderGroupRepo.FindAll()
                .AsNoTracking();

            var predicate = PredicateBuilder.New<Olma.OrderGroup>();

            if (request.Id?.Count > 0)
            {
                predicate = predicate.Or(group => request.Id.Contains(group.Id));
            }

            if (request.OrderId?.Count > 0)
            {
                predicate = predicate.Or(group => group.Orders.Any(order => request.OrderId.Contains(order.Id)));
            }

            if (request.OrderLoadId?.Count > 0)
            {

                predicate = predicate.Or(group => group.Orders.Any(order => order.Loads.Any(load => request.OrderLoadId.Contains(load.Id))));
            }
            
            if (request.LoadCarrierReceiptId?.Count > 0)
            {
                predicate = predicate.Or(group => group.Orders.Any(order => order.Loads.Any(load => request.LoadCarrierReceiptId.Cast<int?>().Contains(load.Detail.LoadCarrierReceiptId))));
            }

            query = query.Where(predicate);

            var orderGroups = query.ProjectTo<OrderGroup>(Mapper.ConfigurationProvider)
                .AsEnumerable();

            return Ok(orderGroups);
        }

        public async Task<IWrappedResponse> Create(OrderGroupsCreateRequest request)
        {
            var cmd = ServiceCommand<OrderGroup, Rules.OrderGroup.Create.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.OrderGroup.Create.MainRule(request))
                .Then(CreateAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CreateAction(Rules.OrderGroup.Create.MainRule rule)
        {
            Olma.OrderGroup orderGroup = new Olma.OrderGroup()
            {
                Orders = new List<Olma.Order>()
            };

            // create orders for order group
            switch (rule.Context.OrderGroupsCreateRequest.QuantityType)
            {
                case OrderQuantityType.Load:
                    {
                        for (var i = 0; i < rule.Context.OrderGroupsCreateRequest.NumberOfLoads; i++)
                        {
                            Olma.Order order = Mapper.Map<Olma.Order>(rule.Context.OrderGroupsCreateRequest);
                            order.Status = OrderStatus.Pending;
                            orderGroup.Orders.Add(order);
                        }

                        break;
                    }
                case OrderQuantityType.LoadCarrierQuantity:
                    {
                        Olma.Order order = Mapper.Map<Olma.Order>(rule.Context.OrderGroupsCreateRequest);
                        order.Status = OrderStatus.Pending;
                        orderGroup.Orders.Add(order);
                        break;
                    }
                case OrderQuantityType.Stacks:
                    {
                        Olma.Order order = Mapper.Map<Olma.Order>(rule.Context.OrderGroupsCreateRequest);
                        order.Status = OrderStatus.Pending;
                        orderGroup.Orders.Add(order);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException($"Unknown QuantityType: {rule.Context.OrderGroupsCreateRequest.QuantityType}");
            }

            bool transactionRolledBack = false;
            IWrappedResponse<OrderMatch> orderMatchServiceResponse = null;
            _olmaDbContext.Database.CreateExecutionStrategy().Execute(operation: () =>
            {
                using (_olmaDbContext.Database.BeginTransaction())
                {
                    _olmaOrderGroupRepo.Create(orderGroup);
                    _olmaOrderGroupRepo.Save();

                    // this needs to happen after order is created in db as otherwise no orderGroupid exists
                    if (rule.Context.OrderGroupsCreateRequest.MatchLmsOrderGroupRowGuid.HasValue)
                    {
                        var order = orderGroup.Orders.Single();
                        var orderMatchQuantity = this.CalculateOrderMatchQuantities(order, order.StackHeightMax);

                        var orderMatchesCreateRequest = Mapper.Map<OrderMatchesCreateRequest>(order);
                        orderMatchesCreateRequest = Mapper.Map(orderMatchQuantity, orderMatchesCreateRequest);

                        if (rule.Context.OrderGroupsCreateRequest.Type == OrderType.Demand)
                        {
                            orderMatchesCreateRequest.SupplyOrderRowGuid = rule.Context.OrderGroupsCreateRequest.MatchLmsOrderGroupRowGuid.Value;
                            orderMatchesCreateRequest.DemandOrderRowGuid = order.RefLmsOrderRowGuid;
                        }
                        else
                        {
                            orderMatchesCreateRequest.DemandOrderRowGuid = rule.Context.OrderGroupsCreateRequest.MatchLmsOrderGroupRowGuid.Value;
                            orderMatchesCreateRequest.SupplyOrderRowGuid = order.RefLmsOrderRowGuid;
                        }

                        orderMatchServiceResponse = (IWrappedResponse<OrderMatch>) _orderMatchesService.Create(orderMatchesCreateRequest).Result;

                        if (orderMatchServiceResponse.ResultType != ResultType.Created)
                        {
                            _olmaDbContext.Database.RollbackTransaction();
                            transactionRolledBack = true;
                        }
                        else
                        {
                            _olmaDbContext.Database.CommitTransaction();
                        }
                    }
                    else
                    {
                        _olmaDbContext.Database.CommitTransaction();
                    }
                }
            });

            if (transactionRolledBack)
            {
                return orderMatchServiceResponse;
            }

            var orders = _olmaDbContext.Orders.Where(i => i.GroupId == orderGroup.Id)
                .Include(og => og.LoadingLocation).ThenInclude(loc => loc.Address)
                .Include(og => og.LoadingLocation.BusinessHours)
                .Include(og => og.LoadingLocation.CustomerDivision).ThenInclude(cd => cd.Customer)
                .Include(og => og.CreatedBy).ThenInclude(u => u.Person)
                .Include(og => og.LoadCarrier)
                .Include(og => og.BaseLoadCarrier)
                .Include(og => og.PostingAccount).ThenInclude(pa => pa.CustomerDivisions)
                .ThenInclude(cd => cd.Customer);

            DateTime syncDate = DateTime.UtcNow;
            foreach (var order in orders)
            {
                order.OrderNumber = await _numberSequencesService.GetProcessNumber(ProcessType.Order, order.Id);
                var orderCreateSyncRequest = Mapper.Map<Olma.Order, OrderCreateSyncRequest>(order);
                if (orderMatchServiceResponse != null)
                {
                    switch (order.Type)
                    {
                        case OrderType.Demand:
                            orderCreateSyncRequest.RefLmsAvailabilityRowGuid = orderMatchServiceResponse.Data.RefLmsAvailabilityRowGuid;
                            orderCreateSyncRequest.RefLmsPermanentAvailabilityRowGuid = orderMatchServiceResponse.Data.RefLmsPermanentAvailabilityRowGuid;
                            break;
                        case OrderType.Supply:
                            orderCreateSyncRequest.RefLmsDeliveryRowGuid = orderMatchServiceResponse.Data.RefLmsDeliveryRowGuid;
                            orderCreateSyncRequest.RefLmsPermanentDeliveryRowGuid = orderMatchServiceResponse.Data.RefLmsPermanentDeliveryRowGuid;
                            break;
                    }

                    orderCreateSyncRequest.DigitalCode = orderMatchServiceResponse.Data.DigitalCode;

                    orderCreateSyncRequest.LoadCarrierQuantity = orderMatchServiceResponse.Data.LoadCarrierQuantity;
                    orderCreateSyncRequest.BaseLoadCarrierQuantity = orderMatchServiceResponse.Data.BaseLoadCarrierQuantity;
                }

                var ordersSyncRequest = new OrdersSyncRequest
                {
                    OrderCreateSyncRequests = new List<OrderCreateSyncRequest>
                    {
                        orderCreateSyncRequest
                    }
                };
                var syncResult = await _synchronizationsService.SendOrdersAsync(ordersSyncRequest);

                if (syncResult.ResultType == ResultType.Ok)
                {
                    order.SyncDate ??= syncDate;
                }
            }

            _olmaDbContext.SaveChanges();

            var result = Mapper.Map<Olma.OrderGroup, OrderGroup>(orderGroup);

            return new WrappedResponse<OrderGroup>
            {
                ResultType = ResultType.Created,
                Data = result
            };
        }

        [Obsolete]
        public async Task<IWrappedResponse<OrderGroup>> GetById(int id)
        {
            var response = _olmaOrderGroupRepo.GetById<Olma.OrderGroup, OrderGroup>(id);
            return response;
        }
        [Obsolete]
        public async Task<IWrappedResponse<OrderGroup>> Update(int id, OrderGroupsUpdateRequest request)
        {
            #region security

            // TODO add security

            #endregion

            var orderGroup = _olmaOrderGroupRepo.FindByCondition(og => og.Id == id)
                .Include(o => o.Orders).ThenInclude((i => i.DplNotes))
                .FirstOrDefault();

            if (orderGroup == null || orderGroup.Orders.Count == 0)
                return new WrappedResponse<OrderGroup>
                {
                    ResultType = ResultType.NotFound,
                    Data = null,
                    Errors = new[] { "Orders not found" }
                };

            var openedOrders = orderGroup.Orders
                .Where(o => o.Status == OrderStatus.Pending)
                .ToList();

            if (openedOrders.Count > 0
                && openedOrders.Count != orderGroup.Orders.Count
                && request.CheckUpdatedProperties(orderGroup.Orders.First()))
            {
                Olma.OrderGroup newOrderGroup = Mapper.Map<OrderGroupsUpdateRequest, Olma.OrderGroup>(request);
                _olmaOrderGroupRepo.Create(newOrderGroup);

                foreach (var order in openedOrders)
                {
                    Mapper.Map(request, order);
                    order.Group = newOrderGroup;
                }

                _olmaOrderGroupRepo.Save();

                var newOrderGroupData = Mapper.Map<Olma.OrderGroup, OrderGroup>(newOrderGroup);

                return new WrappedResponse<OrderGroup>
                {
                    ResultType = ResultType.Updated,
                    Data = newOrderGroupData
                };
            }

            var numberOfOrders = orderGroup.Orders.Count;
            bool updated = false;
            if (orderGroup.Orders.Count < request.NumberOfLoads)
            {
                for (int i = 0; i < request.NumberOfLoads - numberOfOrders; i++)
                {
                    orderGroup.Orders.Add(Mapper.Map<OrderGroupsUpdateRequest, Olma.Order>(request));
                }

                updated = true;
            }
            else
            {
                if (numberOfOrders > request.NumberOfLoads &&
                    numberOfOrders - request.NumberOfLoads <= openedOrders.Count)
                {
                    var ordersToDelete = orderGroup.Orders.Where(o => o.Status == OrderStatus.Pending)
                        .Take(orderGroup.Orders.Count - request.NumberOfLoads);
                    foreach (var order in ordersToDelete)
                    {
                        orderGroup.Orders.Remove(order);
                    }

                    updated = true;
                }
            }

            if (request.CheckUpdatedProperties(orderGroup.Orders.First()))
            {
                Mapper.Map(request, orderGroup);

                foreach (var order in orderGroup.Orders)
                {
                    Mapper.Map(request, order);
                }

                updated = true;
            }

            if (!updated)
            {
                return new WrappedResponse<OrderGroup>
                {
                    ResultType = ResultType.BadRequest,
                    Data = null,
                    Errors = new[] { string.Empty }
                };
            }

            var result = Mapper.Map<Olma.OrderGroup, OrderGroup>(orderGroup);

            return new WrappedResponse<OrderGroup>
            {
                ResultType = ResultType.Updated,
                Data = result
            };
        }
        [Obsolete]
        public async Task<IWrappedResponse<OrderGroup>> Cancel(int id, OrderGroupCancelRequest request)
        {
            #region security

            // TODO add security

            #endregion

            var orderGroup = _olmaOrderGroupRepo.FindByCondition(og => og.Id == id)
                .Include(o => o.Orders)
                .FirstOrDefault();

            if (orderGroup == null)
            {
                return new WrappedResponse<OrderGroup>
                {
                    ResultType = ResultType.NotFound,
                    Data = null,
                    Errors = new[] { string.Empty }
                };
            }

            var rulesResult = RulesEvaluator.Create()
                .StopEvaluateOnFirstInvalidRule()
                .Eval(new Rules.OrderGroup.Cancel.MainRule(orderGroup))
                .Evaluate();

            if (!rulesResult.IsSuccess)
            {
                // return Bad Request Error403
                // TODO Result erzeugen

                return new WrappedResponse<OrderGroup>()
                {
                    ResultType = ResultType.BadRequest,
                    State = ((RuleWithStateResult)rulesResult).State
                };
            }

            var isDplEmployee = AuthData.GetUserRole() != UserRole.DplEmployee;

            var cancellableStates = new[] { OrderStatus.Pending, OrderStatus.Confirmed, OrderStatus.PartiallyMatched, OrderStatus.Matched };
            if (isDplEmployee)
            {
                cancellableStates = cancellableStates.Append(OrderStatus.CancellationRequested).ToArray();
            }

            var cancellableOrders = orderGroup.Orders
                .Where(i => cancellableStates.Contains(i.Status));

            var status = isDplEmployee ? OrderStatus.CancellationRequested : OrderStatus.Cancelled;

            foreach (var orderGroupOrder in cancellableOrders)
            {
                orderGroupOrder.Status = status;
            }

            _olmaOrderGroupRepo.Save();

            // TODO Armine send service bus message
            // note When dpl employee has pefromed cancellation, LMS should just execute it with no check (state of order is already cancelled tehn)
            // When non dpl employee perfroms this check then status is cancellation requested and lms decides if this can be perfomed automatically
            // or if dpl employee intervention is required

            var result = Mapper.Map<Olma.OrderGroup, OrderGroup>(orderGroup);

            return new WrappedResponse<OrderGroup>
            {
                ResultType = ResultType.Updated,
                Data = result
            };
        }

        private OrderMatchQuantity CalculateOrderMatchQuantities(Olma.Order order, int stackHeight)
        {
            var quantities = new OrderMatchQuantity();

            var loadcarrierIds = new[] { order.LoadCarrierId, order.BaseLoadCarrierId }
                .Where(i => i.HasValue)
                .Select(i => i.Value)
                .Distinct()
                .ToArray();

            // TODO move call to load carriers into cached method
            var loadCarriersServiceResponse = (IWrappedResponse<IEnumerable<LoadCarrier>>) _loadCarriersService.GetAll().Result;
            var loadCarrierQuantityPerEurDict = loadCarriersServiceResponse.Data
                .Where(i => loadcarrierIds.Contains(i.Id))
                .ToDictionary(i => i.Id, i => i.Type.QuantityPerEur);

            // TODO expand live pooling to provide base load carrier info
            //var hasBaseLoadCarrier = selfTransportOrderGroup.BaseLoadCarrierId != null || request.BaseLoadCarrierId != null;
            var hasBaseLoadCarrier = order.BaseLoadCarrierId != null;

            switch (order.QuantityType)
            {
                case OrderQuantityType.Load:
                    {
                        var quantityPerEurLoadCarrier = loadCarrierQuantityPerEurDict[order.LoadCarrierId];
                        var quantityPerEurBaseLoadCarrier = order.BaseLoadCarrierId.HasValue
                            ? loadCarrierQuantityPerEurDict[order.BaseLoadCarrierId.Value]
                            : 0;

                        // TODO HACK, current assumes number of stacks = 33 both load/baseLoad carrier
                        // instead this should be dependent on their types
                        var numberOfStacks = 33;

                        return new OrderMatchQuantity()
                        {
                            LoadCarrierQuantity = numberOfStacks * quantityPerEurLoadCarrier * stackHeight,
                            BaseLoadCarrierQuantity = hasBaseLoadCarrier
                                ? 33 * quantityPerEurBaseLoadCarrier
                                : 0,
                            LoadCarrierStackHeight = stackHeight,
                            NumberOfStacks = numberOfStacks
                        };
                    }
                case OrderQuantityType.Stacks:
                    {
                        var numberOfStacks = order.NumberOfStacks.HasValue ? order.NumberOfStacks.Value : 0;
                        var quantityPerEurLoadCarrier = loadCarrierQuantityPerEurDict[order.LoadCarrierId];
                        var quantityPerEurBaseLoadCarrier = order.BaseLoadCarrierId.HasValue
                            ? loadCarrierQuantityPerEurDict[order.BaseLoadCarrierId.Value]
                            : 0;

                        return new OrderMatchQuantity
                        {
                            LoadCarrierQuantity = numberOfStacks * quantityPerEurLoadCarrier * stackHeight,
                            BaseLoadCarrierQuantity = hasBaseLoadCarrier
                                ? numberOfStacks * quantityPerEurBaseLoadCarrier
                                : 0,
                            LoadCarrierStackHeight = stackHeight,
                            NumberOfStacks = numberOfStacks
                        };
                    }
                case OrderQuantityType.LoadCarrierQuantity:
                    {
                        var loadCarrierQuantity = order.LoadCarrierQuantity.Value;
                        var quantityPerEurLoadCarrier = loadCarrierQuantityPerEurDict[order.LoadCarrierId];

                        int remainder;
                        var numberOfStacks =
                            Math.DivRem(loadCarrierQuantity, quantityPerEurLoadCarrier * stackHeight, out remainder) +
                            (remainder > 0 ? 1 : 0);

                        var quantityPerEurBaseLoadCarrier = order.BaseLoadCarrierId.HasValue
                            ? loadCarrierQuantityPerEurDict[order.BaseLoadCarrierId.Value]
                            : 0;

                        var baseLoadCarrierQuantity = hasBaseLoadCarrier
                            ? numberOfStacks * quantityPerEurBaseLoadCarrier
                            : 0;

                        return new OrderMatchQuantity()
                        {
                            LoadCarrierQuantity = loadCarrierQuantity,
                            BaseLoadCarrierQuantity = baseLoadCarrierQuantity,
                            LoadCarrierStackHeight = loadCarrierQuantity < stackHeight ? loadCarrierQuantity : stackHeight,
                            NumberOfStacks = numberOfStacks,
                        };
                    }
                default:
                    throw new ArgumentOutOfRangeException($"Unknown quantity type {order.QuantityType}");
            }
        }
    }

    internal class OrderMatchQuantity
    {
        public int LoadCarrierStackHeight { get; set; }
        public int LoadCarrierQuantity { get; set; }
        public int NumberOfStacks { get; set; }
        public int BaseLoadCarrierQuantity { get; set; }
    }
}