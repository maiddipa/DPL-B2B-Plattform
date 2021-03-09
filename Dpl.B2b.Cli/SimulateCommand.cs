using Dpl.B2b.Contracts;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Common.Enumerations;
using Olma = Dpl.B2b.Dal.Models;
using Dpl.B2b.Contracts.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

namespace Dpl.B2b.Cli
{
    public static class SimulateCommand
    {
        const string ServiceBusConnectionString = "Endpoint=sb://dpllivesynclocal.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=6yZtbvE22nv76qUkhqOAInpxoGdG6Xz0IlUWZXdxdX8=";
        const string OrderQueueName = "ordersyncrequestladham";
        const string PostingRequestQueueName = "postingrequestssyncrequestladham";
        static IQueueClient _orderQueueClient;
        static IQueueClient _postingRequestQueueClient;

        public static void AddSimulateCommand(this CommandLineApplication app, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            app.Command("sim", cmd =>
            {
                cmd.OnExecute(() =>
                {
                    Console.WriteLine("Specify a subcommand");
                    cmd.ShowHelp();
                    return 1;
                });

                cmd.Command("posting", postingCmd =>
                {
                    postingCmd.Description = "Will convert all pending Olma PostRequests to Ltms Bookings.";

                    //var all = postingCmd.Option("a", "Convert all Posting Requests", CommandOptionType.NoValue, config => config.LongName = "all");
                    //var id = postingCmd.Argument("id", "Id of the posting request to convert");
                    //var database = postingCmd.Argument<Databases>("database-name", "Name of the database. Valid options are: olma, ltms")
                    //.Accepts(configure =>
                    //{
                    //    configure.Enum<Databases>(true);
                    //})
                    //.IsRequired();

                    postingCmd.OnExecuteAsync(async (token) =>
                    {
                        var mapper = serviceProvider.GetService<IMapper>();
                        var syncService = serviceProvider.GetService<ISyncService>();
                        var olmaRepo = serviceProvider.GetService<IRepository<Olma.PostingRequest>>();

                        var pendingRequests = olmaRepo.FindByCondition(i => i.Status == PostingRequestStatus.Pending)
                            .IgnoreQueryFilters().Where(a => !a.IsDeleted)
                            .ProjectTo<PostingRequestsCreateRequest>(mapper.ConfigurationProvider).ToList();

                        if (pendingRequests.Count > 0)
                        {
                            var result = await syncService.CreateLtmsBookings(pendingRequests);
                            if (result)
                            {
                                foreach (var request in pendingRequests)
                                {
                                    var record = olmaRepo
                                        .FindByCondition(r => r.RefLtmsTransactionId == request.RefLtmsTransactionId)
                                        .IgnoreQueryFilters().SingleOrDefault();
                                    if (record != null)
                                    {
                                        record.Status = PostingRequestStatus.Confirmed;
                                        olmaRepo.Update(record);
                                    }
                                }

                                olmaRepo.Save();
                                Console.WriteLine(
                                    $"Converterd all Olma PostingRequests to Ltms Bookings successfully.");
                            }
                            else
                            {
                                Console.WriteLine(
                                    $"Something has gone wrong, no Olma PostingRequests have been converted to Ltms Bookings.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Nothing to do");
                        }
                    });
                });

                cmd.Command("transport", transportCmd =>
                {
                    transportCmd.Description =
                        "Will accept a bid for all trasports with status 'Requested' and that have active bids.";

                    //var all = postingCmd.Option("a", "Convert all Posting Requests", CommandOptionType.NoValue, config => config.LongName = "all");
                    //var id = postingCmd.Argument("id", "Id of the posting request to convert");
                    //var database = postingCmd.Argument<Databases>("database-name", "Name of the database. Valid options are: olma, ltms")
                    //.Accepts(configure =>
                    //{
                    //    configure.Enum<Databases>(true);
                    //})
                    //.IsRequired();

                    transportCmd.OnExecuteAsync(async (token) =>
                    {
                        var olmaTransportRepo = serviceProvider.GetService<IRepository<Dal.Models.Transport>>();
                        var transportService = serviceProvider.GetService<ITransportOfferingsService>();

                        var transportsToAccept = olmaTransportRepo.FindAll()
                            .IgnoreQueryFilters()
                            .Include(i => i.Bids)
                            .Where(i => i.Status == Dal.Models.TransportStatus.Requested && i.Bids.Any(i => i.Status == Common.Enumerations.TransportBidStatus.Active))
                            .ToList();

                        foreach (var transport in transportsToAccept)
                        {
                            var bidToAccept = transport.Bids.First(i =>
                                i.Status == Common.Enumerations.TransportBidStatus.Active);
                            await transportService.AcceptBid(transport.Id,
                                new Contracts.Models.TransportOfferingBidAcceptRequest() {BidId = bidToAccept.Id});
                        }

                        Console.WriteLine(
                            $"Accepted bids for all transfers with status 'Requested' and that had active bids.");
                    });
                });

                cmd.Command("ordermatch", orderMatchCmd =>
                {
                    orderMatchCmd.Description = "Will create a orderMatch of to given OrderIds.";

                    var optionSupply = orderMatchCmd.Option<int>("-s|--supply <OrderId>",
                            "Required. The Id of the supply order."
                            , CommandOptionType.SingleValue)
                        .Accepts(o => o.Range(1, 99999))
                        .IsRequired();
                    var optionDemand = orderMatchCmd.Option<int>("-d|--demand <OrderId>",
                            "Required. The Id of the demand order."
                            , CommandOptionType.SingleValue)
                        .Accepts(o => o.Range(1, 99999))
                        .IsRequired();



                    orderMatchCmd.OnExecuteAsync(async (token) =>
                    {
                        int supplyId = optionSupply.ParsedValue;
                        int demandId = optionDemand.ParsedValue;
                        Console.WriteLine(
                            $"Create Order Match for OrderId:{supplyId} (Supply) and OrderId:{demandId} (Demand)");

                        var orderRepo = serviceProvider.GetService<IRepository<Olma.Order>>();
                        var supplyOrder = orderRepo.FindByCondition(o => o.Id == supplyId).IgnoreQueryFilters()
                            .AsNoTracking().FirstOrDefault();
                        var demandOrder = orderRepo.FindByCondition(o => o.Id == demandId).IgnoreQueryFilters()
                            .AsNoTracking().FirstOrDefault();
                        var supplyOrderGroupId = supplyOrder.GroupId;
                        var demandOrderGroupId = demandOrder.GroupId;

                        if (supplyOrderGroupId == null || supplyOrderGroupId == 0 || demandOrderGroupId == null ||
                            demandOrderGroupId == 0)
                        {
                            Console.WriteLine($"Couldn't find OrderGroups");
                            return;
                        }

                        DateTime demandFulfillmentDate;
                        DateTime supplyFulfillmentDate;
                        try
                        {
                            GetFulfillmentDates(supplyOrder, demandOrder, out supplyFulfillmentDate,
                                out demandFulfillmentDate);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(
                                $"Error searching for matching fulfillment Dates on weekday: {e.Message}");
                            return;
                        }



                        var orderMatchService = serviceProvider.GetService<IOrderMatchesService>();


                        var createRequest = new OrderMatchesCreateRequest()
                        {
                            // TODO Update to use RowGuids instead
                            //SupplyOrderGroupId = (int) supplyOrderGroupId,
                            //DemandOrderGroupId = (int) demandOrderGroupId,
                            TransportType = OrderTransportType.Self,
                            SelfTransportSide = OrderType.Demand,
                            DemandFulfillmentDateTime = demandFulfillmentDate,
                            SupplyFulfillmentDateTime = supplyFulfillmentDate
                        };
                        var response = (IWrappedResponse<OrderMatch>) orderMatchService.Create(createRequest).Result;
                        var orderMatchId = response.Data?.Id;
                        if (orderMatchId == null)
                        {
                            Console.WriteLine($"Error while creating OrderMatch");
                            return;
                        }

                        Console.WriteLine($"OrderMatch Id: {orderMatchId} created");

                        //Update OrderMatch Status

                        var orderMatchRepo = serviceProvider.GetService<IRepository<Olma.OrderMatch>>();
                        var orderMatch = orderMatchRepo.GetByKey((int) orderMatchId);
                        orderMatch.Status = OrderMatchStatus.TransportScheduled;
                        Console.WriteLine($"Set OrderMatch.Status to :{nameof(OrderMatchStatus.TransportScheduled)}");
                        orderMatchRepo.Update(orderMatch);
                        orderMatchRepo.Save();
                    });
                });

                cmd.Command("dlqorder", dlqCmd =>
                {
                    dlqCmd.Description = "send message to DLQ";

                    dlqCmd.OnExecuteAsync(async (token) => await TransferMessageToDlqAsync(OrderQueueName));
                });

                cmd.Command("dlqpostingrequest", dlqCmd =>
                {
                    dlqCmd.Description = "send message to DLQ";

                    dlqCmd.OnExecuteAsync(async (token) => await TransferMessageToDlqAsync(PostingRequestQueueName));
                });

            });
        }

        private static async Task TransferMessageToDlqAsync(string queueName)
        {
            var receiver = new MessageReceiver(ServiceBusConnectionString, queueName);

            var message = await receiver.ReceiveAsync();
            Console.WriteLine("msg received");
            string msgBody = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine(msgBody);
            await receiver.DeadLetterAsync(message.SystemProperties.LockToken);

            Console.WriteLine("msg dlq");
        }

        private static void GetFulfillmentDates(Olma.Order supplyOrder, Olma.Order demandOrder, out DateTime supplyFulfillmentDate, out DateTime demandFulfillmentDate,bool canBeOnSameDay = false)
        {
            var supplyRange = new Tuple<DateTime, DateTime>(supplyOrder.EarliestFulfillmentDateTime.Date,supplyOrder.LatestFulfillmentDateTime.Date);
            var demandRange = new Tuple<DateTime, DateTime>(demandOrder.EarliestFulfillmentDateTime.Date,demandOrder.LatestFulfillmentDateTime.Date);

            if (!supplyRange.Item1.Intersects(supplyRange.Item2,demandRange.Item1,demandRange.Item2))
                throw new ArgumentException("Date Ranges to not inersect");
            var demandDate = demandRange.Item1;
            var supplyDate = demandDate.SubstractWorkDay().IsInRange(supplyRange.Item1, supplyRange.Item2) && !canBeOnSameDay
                ? demandDate.SubstractWorkDay()
                : demandDate;
            while (!(
                (demandDate != supplyDate || canBeOnSameDay)  &&
                !supplyDate.IsWeekend() &&
                !demandDate.IsWeekend() &&
                demandDate.IsInRange(demandRange.Item1,demandRange.Item2) &&
                supplyDate.IsInRange(supplyRange.Item1,supplyRange.Item2)
                ))
            {
                demandDate = demandDate.AddWorkDay();
                if (supplyDate.DayOfWeek != DayOfWeek.Friday || demandDate.DayOfWeek == DayOfWeek.Tuesday)
                    supplyDate = supplyDate.AddWorkDay();
                if (!canBeOnSameDay && supplyDate == demandDate)
                    demandDate = demandDate.AddWorkDay(); //Add another day if supply and demand are on same day
                if (demandDate > demandRange.Item2)
                    throw new Exception("No Fulfillment Date found");
            }

            demandFulfillmentDate = demandDate;
            supplyFulfillmentDate = supplyDate;
        }
    }
}
