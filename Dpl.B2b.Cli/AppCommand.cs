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
using Microsoft.Extensions.DependencyInjection;
using DevExpress.DataProcessing;
using RestSharp.Extensions;
using Dpl.B2b.Common.Enumerations;
using Newtonsoft.Json;

namespace Dpl.B2b.Cli
{
    public static class AppCommand
    {
        public static void AddAppCommand(this CommandLineApplication app, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            app.Command("app", appCmd =>
            {
                appCmd.OnExecute(() =>
                {
                    Console.WriteLine("Specify a subcommand");
                    appCmd.ShowHelp();
                    return 1;
                });

                appCmd.Command("calculate-balances", calculateBalanceCmd =>
                {
                    calculateBalanceCmd.Description = "Will generate and save calculated balances for all posting accounts";

                    calculateBalanceCmd.OnExecuteAsync(async (token) =>
                    {
                        var postingAccountService = serviceProvider.GetService<IPostingAccountsService>();

                        Console.WriteLine($"Start - Generate Calculcated Balances");
                        await postingAccountService.GenerateCalculatedBalances();
                        Console.WriteLine($"Completed - Generate Calculcated Balances");
                    });
                });

                appCmd.Command("users", usersCmd =>
                {
                    usersCmd.Description = "Execute actions for users";

                    usersCmd.Command("add", addCmd =>
                    {
                        addCmd.Description = "Will add user";

                        var customerIdArg = addCmd.Option<int>("-c|--customerId", $"Id of the customer", CommandOptionType.SingleValue)
                            .IsRequired();
                        var emailArg = addCmd.Option<string>("-e|--email", $"User email", CommandOptionType.SingleValue)
                            .IsRequired();
                        var roleArg = addCmd.Option<UserRole>("-r|--role", $"Role of the user. Valid options are: {String.Join(", ", Enum.GetNames(typeof(UserRole)).Select(i => i.ToLower()))}", CommandOptionType.SingleValue)
                            .Accepts(config =>
                            {
                                config.Enum<UserRole>(true);
                            })
                            .IsRequired();

                        //customerId.ParsedValue

                        addCmd.OnExecuteAsync(async (token) =>
                        {
                            var context = serviceProvider.GetService<OlmaDbContext>();

                            var userExists = context.Users.Any(i => i.Upn == emailArg.ParsedValue);
                            if (userExists)
                            {
                                Console.WriteLine($"User with given email already exists!");
                                return;
                            }

                            var orgData = context.Customers.Where(i => i.Id == customerIdArg.ParsedValue)
                                .Select(i => new
                                {
                                    OrganizationId = i.OrganizationId,
                                    CustomerId = i.Id,
                                    DivisionIds = i.Divisions.Select(i => i.Id).ToList(),
                                    PostingAccountIds = i.Divisions.Select(i => i.PostingAccountId).ToList(),
                                })
                                .Single();

                            var userGroups = context.UserGroups.OfType<OrganizationUserGroup>().Where(i => i.OrganizationId == orgData.OrganizationId)
                                .Cast<UserGroup>()
                                .ToList()
                                .Concat(context.UserGroups.OfType<CustomerUserGroup>().Where(i => i.CustomerId == orgData.CustomerId).ToList())
                                .Concat(context.UserGroups.OfType<CustomerDivisionUserGroup>().Where(i => orgData.DivisionIds.Contains(i.CustomerDivisionId)).ToList())
                                .ToList();
                            
                            var user = new User()
                            {
                                Upn = emailArg.ParsedValue,
                                Role = roleArg.ParsedValue,
                                Person = new Person()
                                {
                                    Salutation = "",
                                    FirstName = "",
                                    LastName = "",
                                    Email = emailArg.ParsedValue,
                                    PhoneNumber = "",
                                    MobileNumber = "",
                                    Gender = PersonGender.Male,
                                },
                                UserGroups = userGroups.Select(group => new UserUserGroupRelation() { UserGroupId = group.Id }).ToList()
                            };
                            user.Customers = new List<UserCustomerRelation>(){new UserCustomerRelation(){User = user,CustomerId = customerIdArg.ParsedValue } };

                            context.Users.Add(user);
                            context.SaveChanges();

                            var response = new {
                                id = user.Id,
                                email = user.Upn,
                                groups = user.UserGroups.Select(i => i.UserGroupId),
                                organizationId = orgData.OrganizationId,
                            };

                            Console.WriteLine(JsonConvert.SerializeObject(response));
                        });
                    });
                });
            });
        }
    }
}
