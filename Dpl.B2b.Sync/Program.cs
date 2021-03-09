using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Sync.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dpl.B2b.Sync
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .AddServiceLocator()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureAppConfiguration((host, builder) =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile("appsettings.json", optional: true);
                    builder.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", optional: true);
                    builder.AddEnvironmentVariables();
                    builder.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<Settings>(hostContext.Configuration);
                    services.AddHostedService<Worker>();
                    //services.AddHostedService<OrderSyncManagerService>();
                    //services.AddHostedService<PostingRequestSyncManagerService>();
                    services.AddHostedService<OrderDlqManagerService>();
                    services.AddHostedService<PostingRequestDlqManagerService>();
                    services.AddSingleton<IAuthorizationDataService, SyncAuthorizationDataService>();
                    services.AddWebApiServices(hostContext.Configuration);

                });
    }
}