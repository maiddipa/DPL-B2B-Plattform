using System;
using System.IO;
using System.Threading.Tasks;
using Dpl.B2b.BusinessLogic;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using McMaster.Extensions.Hosting.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Reflection;
using Microsoft.Extensions.Hosting.Internal;

namespace Dpl.B2b.Cli
{
    class Program
    {
        async static Task<int> Main(string[] args)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddEnvironmentVariables()
                .Build();

            #region how to use host builder + DI, but only for attribute based console apps, builder apps coming

            // see here for details:
            //https://natemcmaster.github.io/CommandLineUtils/docs/advanced/generic-host.html
            // https://github.com/natemcmaster/CommandLineUtils/issues/271 <= builder apps

            //var builder = new HostBuilder()
            //    .ConfigureServices((hostContext, services) =>
            //    {
            //        //services.AddLogging(config =>
            //        //{
            //        //    config.ClearProviders();
            //        //    config.AddProvider(new SerilogLoggerProvider(Log.Logger));
            //        //});
            //    });

            //await builder.RunCommandLineApplicationAsync<MyAttributeBasedConsoleApp>(args);

            #endregion

            // use delegated authentication to always execute commands in the context of a specific user
            // var result = await Authentication.AuthenticateDelegated(new[] { "profile" });

            // setup manual DI until host builder based DI is available for command line builder apps
            var services = new ServiceCollection();
            services.AddWebApiServices(configuration,new HostingEnvironment()); //TODO Check if default HostingEnviroment is ok at this point, ERRO: value parameter cannot be NULL
            services.AddSingleton<IAuthorizationDataService, CliAuthorizationDataService>();

            var serviceProvider = services
                .BuildServiceProvider();

            ServiceLocator.SetLocatorProvider(serviceProvider);

            var localizationService = serviceProvider.GetService<ILocalizationUpdateService>();

            var app = new CommandLineApplication
            {
                Name = "dpl",
                Description = "Eval management and operational tasks for the Dpl.B2b platform.",
            };

            app.HelpOption(inherited: true);
            app.Command("localization", localizationCmd =>
            {
                localizationCmd.OnExecute(() =>
                {
                    Console.WriteLine("Specify a subcommand");
                    localizationCmd.ShowHelp();
                    return 1;
                });

                localizationCmd.Command("generate", generateCmd =>
                {
                    generateCmd.OnExecuteAsync(async(token) => { await localizationService.GenerateLocalizableEntries(); });
                });

                localizationCmd.Command("retrieve", retrieveCmd =>
                {
                    retrieveCmd.OnExecuteAsync(async (token) => { await localizationService.UpdateLocalizationTexts(); });
                });

                localizationCmd.Command("export", exportCmd =>
                {
                    var path = exportCmd.Argument<string>("path", "Path the localization files will be exported to.")
                        .Accepts(configure =>
                        {
                            configure.ExistingDirectory();
                        })
                        .IsRequired();
                    exportCmd.OnExecuteAsync(async (token) => { await localizationService.ExportLocalizationsToPath(path.ParsedValue); });
                });

                localizationCmd.Command("upload", uploadCmd =>
                {
                    var filePath = uploadCmd.Argument<string>("path", "Path of the messages file (xliff format) that will be uploaded.")
                        .Accepts(configure =>
                        {
                            configure.ExistingFile();
                        })
                        .IsRequired();
                    uploadCmd.OnExecuteAsync(async (token) => { await localizationService.UploadLocalizationsFromFile(filePath.ParsedValue); });
                });

                localizationCmd.Command("clean", cleanCmd =>
                {
                    cleanCmd.OnExecuteAsync(async (token) => { await localizationService.DeleteFrontendTermsWithoutContext(); });
                });
            });

            app.AddAppCommand(configuration, serviceProvider);
            app.AddDbCommand(configuration, localizationService, serviceProvider);
            app.AddSimulateCommand(configuration, serviceProvider);
            app.AddImportCommand(configuration, serviceProvider);

            app.OnExecute(() =>
            {
                Console.WriteLine("Specify a subcommand");
                app.ShowHelp();
                return 1;
            });

            return app.Execute(args);
        }
    }
}
