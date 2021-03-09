using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dpl.B2b.BusinessLogic.ErrorHandlers;
using Dpl.B2b.BusinessLogic.ErrorHandlers.Exceptions;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Dpl.B2b.BusinessLogic
{
    [HandleServiceExceptions]
    public class SynchronizationsService : BaseService, ISynchronizationsService
    {
        private static IQueueClient _queueClient;
        private readonly string _orderSyncRequestQueueName;
        private readonly string _postingRequestSyncRequestQueueName;
        private readonly string _syncServiceBusConnectionString;
        private readonly string _voucherSyncRequestQueueName;

        public SynchronizationsService(
            IAuthorizationDataService authData,
            IMapper mapper,
            IConfiguration configuration) : base(authData, mapper)
        {
            var syncQueuesSection = configuration.GetSection("SyncQueues");
            _syncServiceBusConnectionString = syncQueuesSection["SyncServiceBusConnectionString"];
            _orderSyncRequestQueueName = syncQueuesSection["OrderSyncRequestQueueName"];
            _postingRequestSyncRequestQueueName = syncQueuesSection["PostingRequestSyncRequestQueueName"];
            _voucherSyncRequestQueueName = syncQueuesSection["VoucherSyncRequestQueueName"];
        }

        public async Task<IWrappedResponse> SendOrdersAsync(OrdersSyncRequest request)
        {
            try
            {
                _queueClient = new QueueClient(_syncServiceBusConnectionString, _orderSyncRequestQueueName,
                    retryPolicy: RetryPolicy.Default);

                var messages = new List<Message>();

                messages.AddRange(request.OrderCreateSyncRequests.AsMessages());
                messages.AddRange(request.OrderCancelSyncRequests.AsMessages());
                messages.AddRange(request.OrderFulfillSyncRequests.AsMessages());
                messages.AddRange(request.OrderRollbackFulfillmentSyncRequests.AsMessages());


                await _queueClient.SendAsync(messages);
                return new WrappedResponse
                {
                    ResultType = ResultType.Ok
                };
            }
            catch (Exception exception)
            {
                ErrorHandler.Create()
                    .LogException(new SynchronizationException("Send orders failed.", exception), request);
                return new WrappedResponse
                {
                    ResultType = ResultType.Failed
                };
            }
        }

        public async Task<IWrappedResponse> SendPostingRequestsAsync(PostingRequestsSyncRequest request)
        {
            try
            {
                _queueClient = new QueueClient(_syncServiceBusConnectionString,
                    _postingRequestSyncRequestQueueName, retryPolicy: RetryPolicy.Default);

                var messageBody = JsonConvert.SerializeObject(request);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody)) {ContentType = @"application/json"};


                await _queueClient.SendAsync(message);
                return new WrappedResponse
                {
                    ResultType = ResultType.Ok
                };
            }
            catch (Exception exception)
            {
                ErrorHandler.Create()
                    .LogException(new SynchronizationException("Send posting request failed.", exception), request);
                return new WrappedResponse
                {
                    ResultType = ResultType.Failed
                };
            }
        }

        public async Task<IWrappedResponse> SendVoucherRequestsAsync(VoucherSyncRequest request)
        {
            try
            {
                _queueClient = new QueueClient(_syncServiceBusConnectionString,
                    _voucherSyncRequestQueueName, retryPolicy: RetryPolicy.Default);

                var messageBody = JsonConvert.SerializeObject(request);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody)) {ContentType = @"application/json"};


                await _queueClient.SendAsync(message);
                return new WrappedResponse
                {
                    ResultType = ResultType.Ok
                };
            }
            catch (Exception exception)
            {
                ErrorHandler.Create()
                    .LogException(new SynchronizationException("Send posting request failed.", exception), request);
                return new WrappedResponse
                {
                    ResultType = ResultType.Failed
                };
            }
        }

        public async Task<IWrappedResponse<long>> SchedulePostingRequestsAsync(PostingRequestsSyncRequest request,
            DateTimeOffset dateTimeOffset)
        {
            _queueClient = new QueueClient(_syncServiceBusConnectionString,
                _postingRequestSyncRequestQueueName, retryPolicy: RetryPolicy.Default);

            var messageBody = JsonConvert.SerializeObject(request);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            var numberSequence = await _queueClient.ScheduleMessageAsync(message, dateTimeOffset);

            return new WrappedResponse<long>
            {
                ResultType = ResultType.Ok,
                Data = numberSequence
            };
        }

        public async Task<IWrappedResponse> CancelScheduledPostingRequestsAsync(long sequenceNumber)
        {
            _queueClient = new QueueClient(_syncServiceBusConnectionString,
                _postingRequestSyncRequestQueueName, retryPolicy: RetryPolicy.Default);

            await _queueClient.CancelScheduledMessageAsync(sequenceNumber);

            return new WrappedResponse
            {
                ResultType = ResultType.Ok
            };
        }
    }
}