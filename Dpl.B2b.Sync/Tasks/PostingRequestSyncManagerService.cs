using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class PostingRequestSyncManagerService : BackgroundService
    {
        private readonly ILogger<PostingRequestSyncManagerService> _logger;

        public PostingRequestSyncManagerService(
            IServiceProvider services,
            ILogger<PostingRequestSyncManagerService> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("PostingRequestManagerService is starting.");

            stoppingToken.Register(() => _logger.LogDebug("PostingRequestManagerService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("PostingRequestManagerService is working.");

                await CheckPostingRequestsSyncDate(stoppingToken);
            }

            _logger.LogDebug("PostingRequestManagerService work stopping.");
        }

        private async Task CheckPostingRequestsSyncDate(CancellationToken stoppingToken)
        {
            using var scope = Services.CreateScope();
            var olmaPostingRequestRepo =
                scope.ServiceProvider.GetRequiredService<IRepository<Olma.PostingRequest>>();

            var postingRequests = olmaPostingRequestRepo.FindByCondition(pr =>
                    //PostingRequest created but not synchronized
                    !pr.IsDeleted && !pr.SyncDate.HasValue)
                .Include(lc => lc.LoadCarrier)
                .Include(pa => pa.PostingAccount)
                .IgnoreQueryFilters().ToList();

            if (postingRequests.Any())
            {
                var postingRequestsGroupedByTransactionId = postingRequests.GroupBy(pr => pr.RefLtmsTransactionId);

                foreach (var postingRequestGroup in postingRequestsGroupedByTransactionId)
                {
                    var postingRequestsSyncRequest = new PostingRequestsSyncRequest
                    {
                        PostingRequestCreateSyncRequests = new List<PostingRequestCreateSyncRequest>()
                    };

                    var postingRequestCreateSyncRequest = new PostingRequestCreateSyncRequest
                    {
                        IsSortingRequired = postingRequestGroup.Select(i => i.IsSortingRequired).FirstOrDefault(),
                        CreditRefLtmsAccountId =
                            postingRequestGroup.Select(pr => pr.DestinationRefLtmsAccountId).FirstOrDefault(),
                        DebitRefLtmsAccountId = postingRequestGroup.Select(pr => pr.SourceRefLtmsAccountId).FirstOrDefault(),
                        IssuedByRefLtmsAccountId =
                            postingRequestGroup.Select(pr => pr.PostingAccount.RefLtmsAccountId).FirstOrDefault(),
                        Note = postingRequestGroup.Select(pr => pr.Note).FirstOrDefault(),
                        ReferenceNumber = postingRequestGroup.Select(pr => pr.ReferenceNumber).FirstOrDefault(),
                        RefLtmsProcessId = postingRequestGroup.Select(pr => pr.RefLtmsProcessId).FirstOrDefault(),
                        RefLtmsProcessType =
                            (RefLtmsProcessType) postingRequestGroup.Select(pr => pr.RefLtmsProcessTypeId).FirstOrDefault(),
                        RefLtmsTransactionId = postingRequestGroup.Select(pr => pr.RefLtmsTransactionId).FirstOrDefault(),
                        Positions = postingRequestGroup.Select(pr => new PostingRequestPosition
                        {
                            RefLtmsPalletId = pr.LoadCarrier.RefLtmsPalletId,
                            LoadCarrierQuantity = pr.LoadCarrierQuantity,
                            RefLtmsBookingRowGuid = pr.RowGuid
                        })
                    };

                    postingRequestsSyncRequest.PostingRequestCreateSyncRequests.Add(postingRequestCreateSyncRequest);
                    var synchronizationsService =
                        scope.ServiceProvider.GetRequiredService<ISynchronizationsService>();

                    var syncResult = await synchronizationsService.SendPostingRequestsAsync(postingRequestsSyncRequest);
                    if (syncResult.ResultType == ResultType.Ok)
                    {
                        foreach (var olmaPostingRequest in postingRequestGroup)
                        {
                            olmaPostingRequest.SyncDate = DateTime.UtcNow;
                            olmaPostingRequestRepo.Update(olmaPostingRequest);
                        }
                        olmaPostingRequestRepo.Save();
                    }
                }
            }

            await Task.Delay(30000, stoppingToken);
        }
    }
}