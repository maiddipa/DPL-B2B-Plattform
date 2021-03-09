using Dpl.B2b.Contracts;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dpl.B2b.Dal;
using Dpl.B2b.Dal.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using NetTopologySuite.Geometries;
using System.Text;

namespace Dpl.B2b.Cli
{
    public static class DbCommand
    {
        public static void AddDbCommand(this CommandLineApplication app, IConfiguration configuration, ILocalizationUpdateService localizationService, IServiceProvider serviceProvider)
        {
            app.Command("db", cmd =>
            {
                cmd.OnExecute(() =>
                {
                    Console.WriteLine("Specify a subcommand");
                    cmd.ShowHelp();
                    return 1;
                });

                cmd.Command("reset", resetCmd =>
                {
                    resetCmd.Description = "Will drop / create a database and afterwards seed with dev data";

                    var validOptions = String.Join(", ", Enum.GetNames(typeof(Databases)).Select(i => i.ToLower()));
                    var database = resetCmd.Argument<Databases>("database-name", $"Name of the database. Valid options are: {validOptions}")
                    .Accepts(configure =>
                    {
                        configure.Enum<Databases>(true);
                    })
                    .IsRequired();

                    resetCmd.OnExecuteAsync(async (token) =>
                    {
                        switch (database.ParsedValue)
                        {
                            case Databases.Olma:
                                {
                                    Console.WriteLine("Configure db context");
                                    var options = new DbContextOptionsBuilder<Dal.OlmaDbContext>()
                                        .EnableSensitiveDataLogging(true)
                                        .UseSqlServer(configuration.GetConnectionString("Olma"),
                                            x => x.UseNetTopologySuite()).Options;
                                    Console.WriteLine("Run dev seed");
                                    Dal.OlmaDbContextDevSeed.Seed(options, serviceProvider);
                                    Console.WriteLine("Update localization texts");
                                    await localizationService.UpdateLocalizationTexts();
                                    break;
                                }
                            // TODO decide if this can be removed
                            //case Databases.Ltms:
                            //{
                            //    Console.WriteLine("Configure db context");
                            //    var ltmsOptions = new DbContextOptionsBuilder<Dal.LTMS.LTMSContext>().UseSqlServer(configuration.GetConnectionString("LTMS"),x => x.UseNetTopologySuite()).Options;
                            //    Console.WriteLine("Run dev seed");
                            //    Dal.LTMS.LTMSDbContextDevSeed.Seed(ltmsOptions);
                            //}
                            //    break;
                            //case Databases.Ltms_Demo:
                            //{
                            //    Console.WriteLine("Configure db context");
                            //    var ltmsOptions = new DbContextOptionsBuilder<Dal.LTMS.LTMSContext>().UseSqlServer(configuration.GetConnectionString("LTMS"),x => x.UseNetTopologySuite()).Options;
                            //    Console.WriteLine("Run dev seed for demo");
                            //    Dal.LTMS.LTMSDbContextDevSeed.Seed(ltmsOptions, true);
                            //}
                            //    break;
                            case Databases.Dates:
                                {
                                    Console.WriteLine("Configure db context");
                                    var options = new DbContextOptionsBuilder<Dal.OlmaDbContext>()
                                        .EnableSensitiveDataLogging(true)
                                        .UseSqlServer(configuration.GetConnectionString("Olma"),
                                            x => x.UseNetTopologySuite()).Options;
                                    ResetDates(options, serviceProvider);
#if DEBUG
                                    Console.WriteLine("Please press any key");
                                    Console.ReadLine();
#endif
                                }
                                break;
                            default:
                                break;
                        };

                        Console.WriteLine($"Reset of {database.ParsedValue} db completed.");
                    });
                });
            });
        }


        public static void ResetDates(DbContextOptions<OlmaDbContext> options, IServiceProvider serviceProvider)
        {
            using var context = new OlmaDbContext(options, serviceProvider);
            context.Database.EnsureCreated();
            var initialDate = context.Organizations
                .IgnoreQueryFilters().Min(a => a.CreatedAt);
            var timeSpanToAdd = (TimeSpan)(DateTime.UtcNow - initialDate);
            var weeksToAdd = timeSpanToAdd.Days / 7;
            if (weeksToAdd == 0)
            {
                Console.WriteLine($"Nothing to do. TimeSpan to short: {timeSpanToAdd.ToString()}");
                return;
            }

            Console.WriteLine($"Add {weeksToAdd} weeks to each DateTime.");


            var entityTypes = context.Model.GetEntityTypes();

            foreach (var entityType in entityTypes)
            {
                var properties = entityType.GetProperties();
                var datePropertiesToUpdate = (from
                        propertyInfo in properties
                                              let type = Nullable.GetUnderlyingType(propertyInfo.ClrType) ?? propertyInfo.ClrType
                                              where type == typeof(DateTime)
                                              select propertyInfo).ToList();

                //Skip Exceptions, DateTimes witch should not be reset
                RemoveDateTimePropertyExceptionsToReset(datePropertiesToUpdate, entityType);

                if (datePropertiesToUpdate.Count > 0)
                {
                    switch (entityType.Name)
                    {
                        case "Dpl.B2b.Dal.Models.Address":
                            UpdateAllDateProperties<Address>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.PostingAccount":
                            UpdateAllDateProperties<PostingAccount>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.PostingRequest":
                            UpdateAllDateProperties<PostingRequest>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.Customer":
                            UpdateAllDateProperties<Customer>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.CustomerDivision":
                            UpdateAllDateProperties<CustomerDivision>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.CustomerPartner":
                            UpdateAllDateProperties<CustomerPartner>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.ExpressCode":
                            UpdateAllDateProperties<ExpressCode>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.PostingAccountPreset":
                            UpdateAllDateProperties<PostingAccountPreset>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.LoadCarrier":
                            UpdateAllDateProperties<LoadCarrier>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.LoadingLocation":
                            UpdateAllDateProperties<LoadingLocation>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.Partner":
                            UpdateAllDateProperties<Partner>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.Permission":
                            UpdateAllDateProperties<Permission>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.Organization":
                            UpdateAllDateProperties<Organization>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        //case "Dpl.B2b.Dal.Models.User":
                        //    UpdateAllDateProperties<User>(datePropertiesToUpdate, context, timeSpanToAdd);
                        //    break;
                        case "Dpl.B2b.Dal.Models.AdditionalField":
                            UpdateAllDateProperties<AdditionalField>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.AdditionalFieldValue":
                            UpdateAllDateProperties<AdditionalFieldValue>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.BusinessHour":
                            UpdateAllDateProperties<BusinessHour>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.BusinessHourException":
                            UpdateAllDateProperties<BusinessHourException>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.CalculatedBalance":
                            UpdateAllDateProperties<CalculatedBalance>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.CalculatedBalancePosition":
                            UpdateAllDateProperties<CalculatedBalancePosition>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.CustomerDivisionDocumentSetting":
                            UpdateAllDateProperties<CustomerDivisionDocumentSetting>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.CustomerDocumentSetting":
                            UpdateAllDateProperties<CustomerDocumentSetting>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.OrganizationPartnerDirectory":
                            UpdateAllDateProperties<OrganizationPartnerDirectory>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.CustomerPartnerDirectoryAccess":
                            UpdateAllDateProperties<CustomerPartnerDirectoryAccess>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.PartnerPreset":
                            UpdateAllDateProperties<PartnerPreset>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.CustomerSortingWorker":
                            UpdateAllDateProperties<CustomerSortingWorker>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.DeliveryNote":
                            UpdateAllDateProperties<DeliveryNote>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.Document":
                            UpdateAllDateProperties<Document>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.DocumentNumberSequence":
                            UpdateAllDateProperties<DocumentNumberSequence>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.File":
                            UpdateAllDateProperties<File>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.LoadCarrierReceipt":
                            UpdateAllDateProperties<LoadCarrierReceipt>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.Order":
                            UpdateAllDateProperties<Order>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.OrderCondition":
                            UpdateAllDateProperties<OrderCondition>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.OrderMatch":
                            UpdateAllDateProperties<OrderMatch>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.OrderSerie":
                            UpdateAllDateProperties<OrderSerie>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.OrderGroup":
                            UpdateAllDateProperties<OrderGroup>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.PartnerDirectory":
                            UpdateAllDateProperties<PartnerDirectory>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.PartnerDirectoryAccess":
                            UpdateAllDateProperties<PartnerDirectoryAccess>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.Person":
                            UpdateAllDateProperties<Person>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.SortingInterruption":
                            UpdateAllDateProperties<SortingInterruption>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.SortingShiftLog":
                            UpdateAllDateProperties<SortingShiftLog>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.SortingShiftLogPosition":
                            UpdateAllDateProperties<SortingShiftLogPosition>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.SortingWorker":
                            UpdateAllDateProperties<SortingWorker>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.Submission":
                            UpdateAllDateProperties<Submission>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.SubmitterProfile":
                            UpdateAllDateProperties<SubmitterProfile>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.Transport":
                            UpdateAllDateProperties<Transport>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.TransportBid":
                            UpdateAllDateProperties<TransportBid>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                        case "Dpl.B2b.Dal.Models.Voucher":
                            UpdateAllDateProperties<Voucher>(datePropertiesToUpdate, context, weeksToAdd);
                            break;
                    }
                    //if (entityType.Name == typeof(PostingAccount).FullName)
                    //{
                    //    .GetMethod(nameof(UpdateAllDateProperties)).MakeGenericMethod(entityType.ClrType).Invoke(new object[]
                    //        {datePropertiesToUpdate,context,timeSpanToAdd});
                    //    //UpdateAllDateProperties<t>(datePropertiesToUpdate, context, timeSpanToAdd);
                    //}
                }


            }
        }

        private static void UpdateAllDateProperties<T>(IEnumerable<IProperty> propertiesToUpdate, DbContext context, int weeksToAdd) where T : class
        {
            Console.WriteLine("Update Entity " + typeof(T).Name);
            foreach (var fieldToUpdate in propertiesToUpdate)
            {
                Console.WriteLine("  " + fieldToUpdate.Name);
                var records = context.Set<T>().IgnoreQueryFilters();
                foreach (var record in records)
                {
                    var entry = context.Set<T>().Update(record);
                    var originalValue = (DateTime?)entry.Property(fieldToUpdate.Name).OriginalValue;
                    if (originalValue != null)
                        entry.Property(fieldToUpdate.Name).CurrentValue = originalValue.Value.AddDays(7 * weeksToAdd);
                }
            }
            context.SaveChanges();
        }

        private static void RemoveDateTimePropertyExceptionsToReset(List<IProperty> properties, IEntityType entityType)
        {
            //Add Properties here, witch should not be updated
            if (entityType.Name == typeof(BusinessHour).FullName)
                properties.RemoveAll(p => p.Name == "FromTime" || p.Name == "ToTime");
            if (entityType.Name == typeof(BusinessHourException).FullName)
                properties.RemoveAll(p => p.Name == "FromDateTime" || p.Name == "ToDateTime");
        }

    }

    public enum Databases
    {
        Olma = 0,
        Ltms = 1,
        Ltms_Demo = 3,
        Dates = 4
    }
}
