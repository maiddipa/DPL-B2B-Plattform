using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Contracts.Sync.Errors;
using Dpl.B2b.Contracts.Sync.Infos;
using Dpl.B2b.Dal.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Olma = Dpl.B2b.Dal.Models;


namespace Dpl.B2b.Sync.Tasks
{
    public class PostingRequestDlqManagerService : BackgroundService
    {
        private readonly ILogger<OrderDlqManagerService> _logger;
        static IQueueClient _deadLetterQueueClient;
        static IQueueClient _queueClient;
        private readonly Settings _settings;

        public PostingRequestDlqManagerService(IOptions<Settings> settings,
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
            _logger.LogDebug("PostingRequestDlqManagerService is starting.");

            _queueClient = new QueueClient(_settings.SyncServiceBusConnectionString,
                $"{_settings.PostingRequestSyncRequestQueueName}");
            _deadLetterQueueClient = new QueueClient(_settings.SyncServiceBusConnectionString,
                $"{_settings.PostingRequestSyncRequestQueueName}/$DeadLetterQueue");
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

            var postingRequestsSyncRequest = message.As<PostingRequestsSyncRequest>();
            Guid refLtmsTransactionId = Guid.Empty;

            if (postingRequestsSyncRequest.PostingRequestCreateSyncRequests?.Any() == true)
            {
                refLtmsTransactionId =
                    postingRequestsSyncRequest.PostingRequestCreateSyncRequests.Select(pr => pr.RefLtmsTransactionId).FirstOrDefault();
            }else if (postingRequestsSyncRequest.PostingRequestRollbackSyncRequests?.Any() == true)
            {
                refLtmsTransactionId =
                    postingRequestsSyncRequest.PostingRequestRollbackSyncRequests.Select(pr => pr.RefLtmsTransactionId).FirstOrDefault();
            }

            using var scope = Services.CreateScope();
            var olmaPostingRequestSyncErrorRepo =
                scope.ServiceProvider.GetRequiredService<IRepository<PostingRequestSyncError>>();

            var olmaPostingRequestSyncError =
                olmaPostingRequestSyncErrorRepo.FindByCondition(ose => ose.MessageId == message.MessageId).FirstOrDefault();
            var deadLetterReason = message.UserProperties.ContainsKey("DeadLetterReason")
                ? message.UserProperties["DeadLetterReason"].ToString()
                : string.Empty;

            bool automatedProcessingPossible = false;

            if (olmaPostingRequestSyncError == null && deadLetterReason != null)
            {
                if (deadLetterReason.Equals(nameof(TtlExpiredException), StringComparison.OrdinalIgnoreCase)
                    || deadLetterReason.Equals(nameof(DatabaseTemporarilyUnavailable), StringComparison.OrdinalIgnoreCase))
                {
                    automatedProcessingPossible = true;
                }
            }

            if (olmaPostingRequestSyncError == null)
            {
                PostingRequestSyncError postingRequestSyncError = new PostingRequestSyncError
                {
                    EnqueuedDateTime = message.SystemProperties.EnqueuedTimeUtc,
                    ContentType = message.ContentType,
                    CreateDateTime = DateTime.UtcNow,
                    MessageDeliveryCount = message.SystemProperties.DeliveryCount,
                    MessageId = message.MessageId,
                    SessionId = message.SessionId,
                    DeadLetterErrorDescription = message.UserProperties.ContainsKey("DeadLetterErrorDescription") ? message.UserProperties["DeadLetterErrorDescription"].ToString() : string.Empty,
                    DeadLetterReason = deadLetterReason,
                    SyncErrorStatus = automatedProcessingPossible ? SyncErrorStatus.AutomatedProcessing : SyncErrorStatus.ManuallyProcessing,
                    RefLtmsTransactionId = refLtmsTransactionId
                };

                olmaPostingRequestSyncErrorRepo.Create(postingRequestSyncError);
                olmaPostingRequestSyncErrorRepo.Save();
            }
            else
            {
                olmaPostingRequestSyncError.SyncErrorStatus = SyncErrorStatus.ManuallyProcessing;
                olmaPostingRequestSyncError.UpdateDateTime = DateTime.UtcNow;

                var olmaPostingRequestRepo =
                    scope.ServiceProvider.GetRequiredService<IRepository<Olma.PostingRequest>>();

                var postingRequests = olmaPostingRequestRepo
                    .FindByCondition(pr => pr.RefLtmsTransactionId == refLtmsTransactionId);

                foreach (var postingRequest in postingRequests)
                {
                    postingRequest.SyncNote = nameof(UserActionRequired);
                    olmaPostingRequestRepo.Update(postingRequest);
                }

                olmaPostingRequestRepo.Save();

                olmaPostingRequestSyncErrorRepo.Update(olmaPostingRequestSyncError);
                olmaPostingRequestSyncErrorRepo.Save();
            }

            if (automatedProcessingPossible)
            {
                var messageClone = message.Clone();
                await _queueClient.SendAsync(messageClone);
            }

            await _deadLetterQueueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
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
