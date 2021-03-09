using Dpl.B2b.Contracts;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Dpl.B2b.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;
using Dpl.B2b.Dal;

namespace Dpl.B2b.Cli
{
    public static class ImportCommand
    {
        public static void AddImportCommand(this CommandLineApplication app, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            app.Command("import", cmd =>
            {
                cmd.OnExecute(() =>
                {
                    Console.WriteLine("Specify a subcommand");
                    cmd.ShowHelp();
                    return 1;
                });

                cmd.Command("partners", cmd =>
                {
                    cmd.Description = "Will drop / create a database and afterwards seed with dev data";

                    var directoryId = cmd.Argument<int>("directory-id", $"CustomerPartnerDirectory to add these partners to")
                        .IsRequired();

                    var filePath = cmd.Argument<string>("file-path", $"Path of csv file")
                    .Accepts(configure =>
                    {
                        configure.ExistingFile();
                    })
                    .IsRequired();

                    var delimiter = cmd.Argument<string>("delimiter", $"Delimiter used. Defaults to ';'");

                    cmd.OnExecuteAsync(async (token) =>
                    {
                        var rows = Helper.ReadCsv<CustomerPartnerImportRecord, CustomerPartnerImportRecordCsvMap>(filePath.ParsedValue, false, Encoding.UTF8, delimiter: delimiter.ParsedValue ?? ";");

                        using (serviceProvider.CreateScope())
                        {
                            var importService = serviceProvider.GetService<IImportService>();
                            var result = await importService.ImportCustomerPartners(directoryId.ParsedValue, rows);
                        }

                        Console.WriteLine($"Import of partners from '{filePath.ParsedValue}' completed.");
                    });
                });
            });
        }

    }

    internal class CustomerPartnerImportRecordCsvMap : CsvHelper.Configuration.ClassMap<CustomerPartnerImportRecord>
    {
        public CustomerPartnerImportRecordCsvMap()
        {            
            Map(m => m.Type);
            Map(m => m.CompanyName);
            Map(m => m.Reference);
            Map(m => m.ReferenceOld).Default(null);
            Map(m => m.Street1);
            Map(m => m.Street2);
            Map(m => m.PostalCode);
            Map(m => m.City);
            Map(m => m.CountryIso3Code).Name("CountryISO3Code");
        }
    }
}
