using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Contracts.Sync.Errors;
using Dpl.B2b.Contracts.Sync.Infos;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Olma = Dpl.B2b.Dal.Models;


namespace Dpl.B2b.Sync.Tasks
{
    public class OrderDlqManagerService : BackgroundService
    {
        private readonly ILogger<OrderDlqManagerService> _logger;
        static IQueueClient _deadLetterQueueClient;
        static IQueueClient _queueClient;
        private readonly Settings _settings;


        public OrderDlqManagerService(IOptions<Settings> settings,
            IServiceProvider services,
            ILogger<OrderDlqManagerService> logger)
        {
            Services = services;
            _settings = settings.Value;
            _logger = logger;
        }

        public static IServiceProvider Services { get; set; }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("OrderDlqManagerService is starting.");

            _queueClient = new QueueClient(_settings.SyncServiceBusConnectionString,
                $"{_settings.OrderSyncRequestQueueName}");
            _deadLetterQueueClient = new QueueClient(_settings.SyncServiceBusConnectionString,
                $"{_settings.OrderSyncRequestQueueName}/$DeadLetterQueue");
            RegisterOnMessageHandlerAndReceiveMessages();
            return Task.CompletedTask;
        }

        private void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _deadLetterQueueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            _logger.LogDebug("ProcessMessages start");

            Guid orderRowGuid = Guid.Empty;
            switch (message.ContentType)
            {
                case nameof(OrderCreateSyncRequest):
                    var orderCreateSyncRequest = message.As<OrderCreateSyncRequest>();
                    orderRowGuid = orderCreateSyncRequest.OrderRowGuid;
                    break;

                case nameof(OrderCancelSyncRequest):
                    var orderCancelSyncRequest = message.As<OrderCancelSyncRequest>();
                    orderRowGuid = orderCancelSyncRequest.OrderRowGuid;
                    break;
            }

            using var scope = Services.CreateScope();
            var olmaOrderSyncErrorRepo =
                scope.ServiceProvider.GetRequiredService<IRepository<Olma.OrderSyncError>>();

            var olmaOrderSyncError = olmaOrderSyncErrorRepo.FindByCondition(ose => ose.MessageId == message.MessageId)
                .Include(o => o.Order).FirstOrDefault();
            var deadLetterReason = message.UserProperties.ContainsKey("DeadLetterReason")
                ? message.UserProperties["DeadLetterReason"].ToString()
                : string.Empty;

            bool automatedProcessingPossible = false;

            if (olmaOrderSyncError == null && deadLetterReason != null)
            {
                if (deadLetterReason.Equals(nameof(TtlExpiredException), StringComparison.OrdinalIgnoreCase)
                    || deadLetterReason.Equals(nameof(DatabaseTemporarilyUnavailable),
                        StringComparison.OrdinalIgnoreCase))
                {
                    automatedProcessingPossible = true;
                }
            }

            if (olmaOrderSyncError == null)
            {
                var olmaOrderRepo =
                    scope.ServiceProvider.GetRequiredService<IRepository<Olma.Order>>();

                var order = olmaOrderRepo.FindByCondition(o => o.RefLmsOrderRowGuid == orderRowGuid).IgnoreQueryFilters()
                    .SingleOrDefault();
                if (order != null)
                {
                    Olma.OrderSyncError orderSyncError = new Olma.OrderSyncError
                    {
                        EnqueuedDateTime = message.SystemProperties.EnqueuedTimeUtc,
                        ContentType = message.ContentType,
                        CreateDateTime = DateTime.UtcNow,
                        MessageDeliveryCount = message.SystemProperties.DeliveryCount,
                        MessageId = message.MessageId,
                        SessionId = message.SessionId,
                        DeadLetterErrorDescription = message.UserProperties.ContainsKey("DeadLetterErrorDescription")
                            ? message.UserProperties["DeadLetterErrorDescription"].ToString()
                            : string.Empty,
                        DeadLetterReason = deadLetterReason,
                        SyncErrorStatus = automatedProcessingPossible
                            ? SyncErrorStatus.AutomatedProcessing
                            : SyncErrorStatus.ManuallyProcessing,
                        OrderId = order.Id
                    };

                    if (!automatedProcessingPossible)
                    {
                        order.SyncNote = nameof(UserActionRequired);
                        olmaOrderRepo.Update(order);
                        olmaOrderRepo.Save();
                    }

                    olmaOrderSyncErrorRepo.Create(orderSyncError);
                    olmaOrderSyncErrorRepo.Save();
                }
                else
                {
                    //log error
                }
            }
            else
            {
                olmaOrderSyncError.SyncErrorStatus = SyncErrorStatus.ManuallyProcessing;
                olmaOrderSyncError.UpdateDateTime = DateTime.UtcNow;
                if (olmaOrderSyncError.Order != null)
                {
                    olmaOrderSyncError.Order.SyncNote = nameof(UserActionRequired);
                }

                olmaOrderSyncErrorRepo.Update(olmaOrderSyncError);
                olmaOrderSyncErrorRepo.Save();
            }

            if (automatedProcessingPossible)
            {
                var messageClone = message.Clone();
                await _queueClient.SendAsync(messageClone);
            }

            await _deadLetterQueueClient.CompleteAsync(message.SystemProperties.LockToken);

            _logger.LogDebug("ProcessMessages end.");
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
