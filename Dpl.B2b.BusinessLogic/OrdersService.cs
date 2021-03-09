using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class OrdersService : BaseService, IOrdersService
    {
        private readonly IRepository<Olma.Order> _olmaOrderRepo;
        private readonly ISynchronizationsService _synchronizationsService;
        private readonly IServiceProvider _serviceProvider;

        public OrdersService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IRepository<Olma.Order> olmaOrderRepo,
            ISynchronizationsService synchronizationsService,
            IServiceProvider serviceProvider
            ) : base(authData, mapper)
        {
            _olmaOrderRepo = olmaOrderRepo;
            _synchronizationsService = synchronizationsService;
            _serviceProvider = serviceProvider;
        }
        [Obsolete]
        public async Task<IWrappedResponse<Order>> GetById(int id)
        {
            var response = _olmaOrderRepo.GetById<Olma.Order, Order>(id);
            return response;
        }
        [Obsolete]
        public async Task<IWrappedResponse<Order>> Update(int id, OrderUpdateRequest request)
        {
            #region security

            // TODO add security

            #endregion
            //ToDo: Nach dem Matching dürfen die Aufträge nicht bearbeitet werden.


            var order = _olmaOrderRepo.FindByCondition(i => i.Id == id)
                .Include(i => i.DplNotes)
                .SingleOrDefault();

            if (order == null)
            {
                return NotFound<Order>(id);
            }

            // apply value from update
            Mapper.Map(request, order);

            if (IsDplEmployee)
            {
                var dplNote = Mapper.Map<Olma.EmployeeNote>(request.DplNote);
                order.DplNotes.Add(dplNote);
            }

            var response = _olmaOrderRepo.Update<Olma.Order, OrderUpdateRequest, Order>(id, request);
            return response;
        }

        public async Task<IWrappedResponse> Cancel(int id, OrderCancelRequest request)
        {
            var cmd = ServiceCommand<Order, Rules.Order.Cancel.MainRule>
                    .Create(_serviceProvider)
                    .When(new Rules.Order.Cancel.MainRule(id, request))
                    .Then(CancelAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> CancelAction(Rules.Order.Cancel.MainRule rule)
        {
            var order = rule.Context.Rules.OrderResourceRule.Context.Resource;
            var request = rule.Context.Request;

            if (IsDplEmployee)
            {
                var dplNote = Mapper.Map<Olma.EmployeeNote>(request.DplNote);
                order.DplNotes.Add(dplNote);

                order.Status = OrderStatus.Cancelled;
            }
            else
            {
                order.Status = OrderStatus.CancellationRequested;
            }

            _olmaOrderRepo.Save();

            var orderCancelSyncRequest = Mapper.Map<Olma.Order, OrderCancelSyncRequest>(order);
            orderCancelSyncRequest.IsApproved = IsDplEmployee;
            // TODO generate string from all emplyoeenote fields
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
                order.SyncDate = DateTime.UtcNow;
            }

            _olmaOrderRepo.Save();

            var responseOrder = Mapper.Map<Order>(order);
            return Updated(responseOrder);
        }

        public async Task<IWrappedResponse> Summary(OrderSearchRequest request)
        {
            var cmd = ServiceCommand<Order, Rules.Order.Summary.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Order.Summary.MainRule(request))
                .Then(SummaryAction);

            return await cmd.Execute();
        }

        private async Task<IWrappedResponse> SummaryAction(Rules.Order.Summary.MainRule rule)
        {
            var result = rule.Context.Rules.OrderSummaryValidRule.Context.OrderSummaries;
            return Ok(result);
        }

        public async Task<IWrappedResponse> Search(OrderSearchRequest request)
        {
            var cmd = ServiceCommand<Order, Rules.Order.Search.MainRule>
                .Create(_serviceProvider)
                .When(new Rules.Order.Search.MainRule(request))
                .Then(SearchAction);

            return await cmd.Execute();
        }

        public async Task<IWrappedResponse> SearchAction(Rules.Order.Search.MainRule rule)
        {
            var result = rule.Context.Rules.OrderValidRule.Context.Orders;
            return Ok(result);
        }
    }
}
