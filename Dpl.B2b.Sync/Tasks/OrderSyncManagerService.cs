using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.Sync.Tasks
{
    public class OrderSyncManagerService : BackgroundService
    {
        private readonly ILogger<OrderSyncManagerService> _logger;


        public OrderSyncManagerService(
            IServiceProvider services,
            ILogger<OrderSyncManagerService> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("OrdersSyncRequestManagerService is starting.");

            stoppingToken.Register(() => _logger.LogDebug("OrdersSyncRequestManagerService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("OrdersSyncRequestManagerService is working.");

                await CheckOrdersSyncDate(stoppingToken);
            }

            _logger.LogDebug("OrdersSyncRequestManagerService work stopping.");
        }

        private async Task CheckOrdersSyncDate(CancellationToken stoppingToken)
        {
            using var scope = Services.CreateScope();

            var olmaOrderRepo =
                scope.ServiceProvider.GetRequiredService<IRepository<Olma.Order>>();

            var orders = olmaOrderRepo.FindByCondition(o =>
                    //Order created but not synchronized
                    (!o.IsDeleted && !o.SyncDate.HasValue)
                    //Order canceled but not synchronized
                    || (!o.IsDeleted && o.SyncDate.HasValue && o.ChangedAt.HasValue && o.ChangedAt > o.SyncDate &&
                    o.Status == OrderStatus.CancellationRequested))
                .Include(og => og.LoadingLocation).ThenInclude(loc => loc.Address)
                .Include(og => og.CreatedBy).ThenInclude(u => u.Person)
                .Include(og => og.ChangedBy).ThenInclude(u => u.Person)
                .Include(og => og.DeletedBy).ThenInclude(u => u.Person)
                .Include(og => og.LoadCarrier).Include(og => og.BaseLoadCarrier)
                .Include(og => og.PostingAccount).ThenInclude(pa => pa.CustomerDivisions).ThenInclude(cd => cd.Customer)
                .IgnoreQueryFilters().ToList();

            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var synchronizationsService =
                scope.ServiceProvider.GetRequiredService<ISynchronizationsService>();

            foreach (var order in orders)
            {
                var ordersSyncRequest = new OrdersSyncRequest
                {
                    OrderCreateSyncRequests = new List<OrderCreateSyncRequest>(),
                    OrderCancelSyncRequests = new List<OrderCancelSyncRequest>()
                };

                if (!order.IsDeleted && !order.SyncDate.HasValue)
                {
                    ordersSyncRequest.OrderCreateSyncRequests.Add(
                        mapper.Map<OrderCreateSyncRequest>(order));
                }

                if (!order.IsDeleted && order.SyncDate.HasValue && order.ChangedAt.HasValue &&
                    order.ChangedAt > order.SyncDate && order.Status == OrderStatus.CancellationRequested)
                {
                    ordersSyncRequest.OrderCreateSyncRequests.Add(
                        mapper.Map<Olma.Order, OrderCreateSyncRequest>(order));
                }

                var syncResult = await synchronizationsService.SendOrdersAsync(ordersSyncRequest);
                if (syncResult.ResultType == ResultType.Ok)
                {
                    order.SyncDate = DateTime.UtcNow;
                    olmaOrderRepo.Update(order);
                    olmaOrderRepo.Save();
                }
            }

            await Task.Delay(30000, stoppingToken);
        }
    }
}
