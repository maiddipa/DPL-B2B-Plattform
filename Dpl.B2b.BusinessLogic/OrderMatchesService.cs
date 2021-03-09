using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Localizable.Rules.Messages.Errors.OrderMatches;
using Dpl.B2b.Contracts.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class OrderMatchesService : BaseService, IOrderMatchesService
    {
        private readonly IRepository<Olma.LoadCarrier> _olmaLoadCarrierRepo;
        private readonly IRepository<Olma.OrderMatch> _olmaOrderMatchRepo;
        private readonly IRepository<Olma.Order> _olmaOrderRepo;
        private readonly INumberSequencesService _numberSequencesService;

        private readonly IMapsService _maps;
        private readonly IRepository<Olma.LmsOrder> _lmsOrderRepo;

        public OrderMatchesService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.LoadCarrier> olmaLoadCarrierRepo,
            IRepository<Olma.OrderMatch> olmaOrderMatchRepo,
            IRepository<Olma.Order> olmaOrderRepo,
            INumberSequencesService numberSequencesService,
            IMapsService maps,
            IRepository<Olma.LmsOrder> lmsOrderGroupRepo
        ) : base(authData, mapper)
        {
            _olmaLoadCarrierRepo = olmaLoadCarrierRepo;
            _olmaOrderMatchRepo = olmaOrderMatchRepo;
            _olmaOrderRepo = olmaOrderRepo;
            _numberSequencesService = numberSequencesService;
            _maps = maps;
            _lmsOrderRepo = lmsOrderGroupRepo;
        }
        [Obsolete]
        public async Task<IWrappedResponse<Contracts.Models.OrderMatch>> GetById(int id)
        {
            #region security

            // TODO add security

            #endregion
            var response = _olmaOrderMatchRepo.GetById<Olma.OrderMatch, Contracts.Models.OrderMatch>(id);
            return response;
        }

        public async Task<IWrappedResponse> Create(OrderMatchesCreateRequest request)
        {
            #region security

            // TODO add security

            #endregion

            if (request == null || request.DemandOrderRowGuid == Guid.Empty || request.SupplyOrderRowGuid == Guid.Empty)
            {
                return BadRequest<OrderMatch>("DemandOrderRowGuid or SupplyOrderRowGuid required");
            }

            Olma.OrderMatch orderMatch;
            
            var loadCarrierIds = new[] { request.LoadCarrierId, request.BaseLoadCarrierId }.Distinct()
                .Where(i => i != null).ToArray();
            var loadCarrierQuantityPerEurDict = _olmaLoadCarrierRepo.FindAll()
                .Where(i => loadCarrierIds.Contains(i.Id))
                .Include(i => i.Type).FromCache()
                .ToDictionary(i => i.Id, i => i.Type.QuantityPerEur);
            
            var virtualLmsOrders = _olmaOrderRepo
                .FindAll()
                .Where(i => i.RefLmsOrderRowGuid == request.DemandOrderRowGuid || i.RefLmsOrderRowGuid == request.SupplyOrderRowGuid)
                .SelectMany(i => i.Group.Orders)
                .Where(i => (i.Status == OrderStatus.Pending || i.Status == OrderStatus.Confirmed) && !i.IsDeleted)
                .Include(i => i.LoadingLocation).ThenInclude(i => i.Address)
                .Select(i => new Olma.LmsOrder()
                {
                    RowGuid = i.RefLmsOrderRowGuid,
                    Type = i.Type,
                    // HACK centralize cacluation of load to load carrier quantity
                    AvailableQuantity = i.QuantityType == OrderQuantityType.Load 
                        ? 1 * 33 * i.StackHeightMax * loadCarrierQuantityPerEurDict[i.LoadCarrierId]
                        : i.LoadCarrierQuantity.HasValue ? i.LoadCarrierQuantity.Value : 0,
                    StackHeightMin = i.StackHeightMin,
                    StackHeightMax = i.StackHeightMax,
                    FromDate = i.EarliestFulfillmentDateTime,
                    UntilDate = i.LatestFulfillmentDateTime,
                    // TODO rethink if loading location id should be nullable also when coming from lms
                    LoadingLocationId = i.LoadingLocationId.HasValue ? i.LoadingLocationId.Value : 0,
                    LoadingLocation = i.LoadingLocation,
                    OlmaOrderId = i.Id,
                    OlmaOrder = i,
                    LoadCarrierId = i.LoadCarrierId,
                    BaseLoadCarrierId = i.BaseLoadCarrierId,
                    Geblockt = false,
                    SupportsRearLoading = i.SupportsRearLoading,
                    SupportsSideLoading = i.SupportsSideLoading,
                    SupportsJumboVehicles = i.SupportsJumboVehicles,
                }).ToList();
            
            var orders = _lmsOrderRepo
                .FindAll()
                .IgnoreQueryFilters()
                .Where(i => i.RowGuid == request.DemandOrderRowGuid || i.RowGuid == request.SupplyOrderRowGuid)
                .Include(i => i.OlmaOrder).ThenInclude(i => i.Group).ThenInclude(i => i.Orders)
                .Include(i => i.LoadingLocation).ThenInclude(i => i.Address)
                // the status here MUST NOT include open state as when triggered as we should only match against confirmed orders
                //.IncludeFilter(og => og.OlmaOrder.Group.Orders.Where(o => (o.Status == OrderStatus.Confirmed || o.Status == OrderStatus.PartiallyMatched) && !o.IsDeleted))
                //.IncludeFilter(i => i.LoadingLocation.Address)
                .ToList();

            orders = orders
                .Concat(virtualLmsOrders)
                .ToList();
            
            

            var lmsDemandOrder = orders.FirstOrDefault(o => o.Type == OrderType.Demand);
            var lmsSupplyOrder = orders.FirstOrDefault(o => o.Type == OrderType.Supply);
            
            if (!request.SkipValidation)
            {
                Func<Olma.LmsOrder, OrderType, DateTime, bool> validate = (order, orderType, fulfillmentDate) =>
                {
                    return order.Type == orderType
                           && order.LoadCarrierId == request.LoadCarrierId
                           && ((order.StackHeightMin <= request.LoadCarrierStackHeight && request.LoadCarrierStackHeight <= order.StackHeightMax)
                               // HACK Partial Matching Remove comments when SUpportsPartialMatching is properly set + search is load based
                               //|| (order.SupportsPartialMatching && (
                               || ((
                                   request.LoadCarrierStackHeight <= order.StackHeightMax
                                   || (order.StackHeightMin <= request.LoadCarrierStackHeight && request.LoadCarrierStackHeight <= order.StackHeightMax)))
                           )
                           // no base load carrier specified or matching base load carriers
                           && (!request.BaseLoadCarrierId.HasValue || order.BaseLoadCarrierId == request.BaseLoadCarrierId.Value)
                           && (!request.SupportsRearLoading || order.SupportsRearLoading == request.SupportsRearLoading)
                           && (!request.SupportsSideLoading || order.SupportsSideLoading == request.SupportsSideLoading)
                           && (!request.SupportsJumboVehicles || order.SupportsJumboVehicles == request.SupportsJumboVehicles)
                           && order.FromDate.Value.Date <= fulfillmentDate && fulfillmentDate <= order.UntilDate.Value.Date;
                };
                
                if (!validate(lmsSupplyOrder, OrderType.Supply, request.SupplyFulfillmentDateTime.Date)
                    || !validate(lmsDemandOrder, OrderType.Demand, request.DemandFulfillmentDateTime.Date))
                {
                    return BadRequest<OrderMatch>(
                        ErrorHandler
                            .Create()
                            .AddMessage(new OrdersNotMatchError())
                            .GetServiceState());
                }

                if(request.LoadCarrierQuantity > lmsSupplyOrder.AvailableQuantity)
                {
                    return BadRequest<OrderMatch>(
                        ErrorHandler
                            .Create()
                            .AddMessage(new QuantityExceededSupplyError())
                            .GetServiceState());
                   
                }

                if (request.LoadCarrierQuantity > lmsDemandOrder.AvailableQuantity)
                { 
                    return BadRequest<OrderMatch>(
                        ErrorHandler
                            .Create()
                            .AddMessage(new QuantityExceededDemandError())
                            .GetServiceState());
                }

                var unavailableOrder = orders.Where(order => order.Id > 0 && !order.IsAvailable).ToList();
                
                if (unavailableOrder.Any())
                {
                    // Anmerkung: error werden momentan nur serverseitig gespeichert, nicht jedoch im http response. 
                    var error = unavailableOrder.Select(o => o.RowGuid.ToString()).ToArray();
                    
                    return BadRequest<OrderMatch>(
                        ErrorHandler
                            .Create()
                            .AddMessage(new AvailableError())
                            .GetServiceState()
                        , error);
                }
            }

            orderMatch = Mapper.Map<Olma.OrderMatch>(request);

            if (request.TransportType == OrderTransportType.ProvidedByOthers)
            {
                // TODO add additional status to transport 'Approved' or similar
                // which signals that this tarsnport has been approved for listing in transport broker in gerneral
                // on top of that we will show specific approved listings only to cetrain shippers
                orderMatch.Transport = new Olma.Transport()
                {
                    ReferenceNumber = await _numberSequencesService.GetTransportNumber(),
                    Status = Olma.TransportStatus.Requested,
                    Type = Olma.TransportType.OrderMatch,
                    // HACK Hardcoded 10 days as planned latest by
                    PlannedLatestBy = DateTime.UtcNow.Date.AddDays(10),
                    ConfirmedLatestBy = DateTime.UtcNow.Date.AddDays(14),
                    RoutedDistance = await _maps.GetRoutedDistance(lmsSupplyOrder.LoadingLocation.Address.GeoLocation, lmsDemandOrder.LoadingLocation.Address.GeoLocation)
                };

                orderMatch.Status = OrderMatchStatus.Pending;
            }
            else if (request.TransportType == OrderTransportType.Self)
            {
                orderMatch.Status = OrderMatchStatus.TransportScheduled;
            }

            // This block deals with permanent availabilities / deliveries
            // If this is a permananet one we need to geenrate a new Guid 
            // and match against that vs the guid of the permanent
            if (lmsSupplyOrder.IsPermanent)
            {
                orderMatch.RefLmsPermanentAvailabilityRowGuid = orderMatch.RefLmsAvailabilityRowGuid;
                orderMatch.RefLmsAvailabilityRowGuid = Guid.NewGuid();
            }
            else if (lmsDemandOrder.IsPermanent)
            {
                orderMatch.RefLmsPermanentDeliveryRowGuid = orderMatch.RefLmsDeliveryRowGuid;
                orderMatch.RefLmsDeliveryRowGuid = Guid.NewGuid();
            }

            orderMatch.DigitalCode = await _numberSequencesService.GetDigitalCode();

            var orderLoadStatus = orderMatch.Status == OrderMatchStatus.TransportScheduled ? OrderLoadStatus.TransportPlanned : OrderLoadStatus.Pending;
            var supplyLoadDetails = new Olma.OrderLoadDetail()
            {
                PlannedFulfillmentDateTime = request.SupplyFulfillmentDateTime,
                Status = orderLoadStatus,
                Address = lmsSupplyOrder.LoadingLocation != null ? lmsSupplyOrder.LoadingLocation.Address : null
            };

            var demandLoadDetails = new Olma.OrderLoadDetail()
            {
                PlannedFulfillmentDateTime = request.DemandFulfillmentDateTime,
                Status = orderLoadStatus,
                Address = lmsDemandOrder.LoadingLocation != null ? lmsDemandOrder.LoadingLocation.Address : null
            };

            
            if (lmsSupplyOrder.OlmaOrder != null)
            {
                var supplyOrder = lmsSupplyOrder.OlmaOrder.Group.Orders.FirstOrDefault(o => (o.Status == OrderStatus.Pending || o.Status == OrderStatus.Confirmed || o.Status == OrderStatus.PartiallyMatched) && !o.IsDeleted);
                if(supplyOrder == null)
                {
                    return BadRequest<OrderMatch>($"Supply order group out of Sync with LmsOrder, RowGuid: {lmsSupplyOrder.RowGuid}");

                }

                orderMatch.Supply = new Olma.OrderLoad()
                {
                    Order = supplyOrder,
                    Detail = request.TransportType == OrderTransportType.Self && request.SelfTransportSide == OrderType.Supply
                        ? demandLoadDetails
                        : supplyLoadDetails
                };

                supplyOrder.Status = request.LoadCarrierQuantity >= lmsSupplyOrder.AvailableQuantity ? OrderStatus.Matched : OrderStatus.PartiallyMatched;
            }

            if (lmsDemandOrder.OlmaOrder != null)
            {
                var demandOrder = lmsDemandOrder.OlmaOrder.Group.Orders.FirstOrDefault(o => (o.Status == OrderStatus.Pending || o.Status == OrderStatus.Confirmed || o.Status == OrderStatus.PartiallyMatched) && !o.IsDeleted);
                if (demandOrder == null)
                {
                    return BadRequest<OrderMatch>($"Demand order group out of Sync with LmsOrder, RowGuid: {lmsSupplyOrder.RowGuid}");
                }

                orderMatch.Demand = new Olma.OrderLoad()
                {
                    Order = demandOrder,
                    Detail = request.TransportType == OrderTransportType.Self && request.SelfTransportSide == OrderType.Demand
                        ? supplyLoadDetails
                        : demandLoadDetails
                };

                demandOrder.Status = request.LoadCarrierQuantity >= lmsDemandOrder.AvailableQuantity ? OrderStatus.Matched : OrderStatus.PartiallyMatched;
            }

            _olmaOrderMatchRepo.Create(orderMatch);
            var numberOfSavedEntities = 0;
            while (numberOfSavedEntities == 0)
            {
                try
                {
                    numberOfSavedEntities = _olmaOrderMatchRepo.Save();
                }
                catch (DbUpdateException ex)
                {
                    // check if the digital code already existed
                    if (ex.InnerException is SqlException innerException && innerException.Number == 2601)
                    {
                        orderMatch.DigitalCode = await _numberSequencesService.GetDigitalCode();
                    }
                }
            }

            var result = Mapper.Map<Olma.OrderMatch, Contracts.Models.OrderMatch>(orderMatch);

            return Created(result);
        }

        [Obsolete]
        public async Task<IWrappedResponse<Contracts.Models.OrderMatch>> Update(int id, OrderMatchesUpdateRequest request)
        {
            #region security

            // TODO add security

            #endregion

            //ToDo: OrderMatch Status überprüfen vor dem Updaten
            var response = _olmaOrderMatchRepo.Update<Olma.OrderMatch, OrderMatchesUpdateRequest, Contracts.Models.OrderMatch>(id, request);
            return response;
        }

        [Obsolete]
        public async Task<IWrappedResponse<Contracts.Models.OrderMatch>> Cancel(int id)
        {
            var orderMatch = _olmaOrderMatchRepo.FindByCondition(om => om.Id == id)
                .Include(i => i.Supply)
                .Include(i => i.Demand)
                .FirstOrDefault();

            if (orderMatch != null)
            {
                return NotFound<Contracts.Models.OrderMatch>(id);
            }

            if (orderMatch.Status != OrderMatchStatus.Pending)
            {
                return BadRequest<OrderMatch>("OrderMatch needs to be pending");
            }

            // ReSharper disable once PossibleNullReferenceException -- orderMatch will never be null
            orderMatch.Status = OrderMatchStatus.Cancelled;

            // TODO OrderLoad Think about how and when to update order status as well, and if we should not just soft delete the OrderLoad instead
            orderMatch.Demand.Detail.Status = OrderLoadStatus.Canceled;
            orderMatch.Supply.Detail.Status = OrderLoadStatus.Canceled;

            _olmaOrderMatchRepo.Update(orderMatch);

            var savedEntitiesCount = _olmaOrderMatchRepo.Save();
            if (savedEntitiesCount == 0)
            {
                return new WrappedResponse<Contracts.Models.OrderMatch>()
                {
                    ResultType = ResultType.Failed,
                    Data = null,
                    Errors = new[] { "" }
                };
            }

            var result = Mapper.Map<Olma.OrderMatch, Contracts.Models.OrderMatch>(orderMatch);

            return new WrappedResponse<Contracts.Models.OrderMatch>
            {
                ResultType = ResultType.Updated,
                Data = result
            };
        }
        [Obsolete]
        public async Task<IWrappedResponse<IPaginationResult<Contracts.Models.OrderMatch>>> Search(OrderMatchesSearchRequest request)
        {
            var olmaOrderMatchQuery = _olmaOrderMatchRepo
                .FindAll()
                .AsNoTracking();

            #region security

            // TODO add security

            #endregion

            #region convert search request

            if (request.Code != null)
            {
                olmaOrderMatchQuery = olmaOrderMatchQuery.Where(om => om.DigitalCode == request.Code);
            }

            if (request.PostingAccountId?.Count > 0)
            {
                olmaOrderMatchQuery = olmaOrderMatchQuery.Where(om =>
                    request.PostingAccountId.Contains(om.Demand.Order.PostingAccountId) ||
                    request.PostingAccountId.Contains(om.Supply.Order.PostingAccountId));
            }

            if (request.Statuses?.Count > 0)
            {
                olmaOrderMatchQuery = olmaOrderMatchQuery.Where(om => request.Statuses.Contains(om.Status));
            }

            #endregion

            #region ordering

            olmaOrderMatchQuery = olmaOrderMatchQuery.OrderByDescending(o => o.Id);

            #endregion

            var projectedQuery = olmaOrderMatchQuery.ProjectTo<Contracts.Models.OrderMatch>(Mapper.ConfigurationProvider);

            var mappedResult = projectedQuery.ToPaginationResult(request);

            return Ok(mappedResult);
        }
    }
}
