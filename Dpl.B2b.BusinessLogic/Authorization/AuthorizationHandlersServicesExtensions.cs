using Dpl.B2b.Common.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dpl.B2b.Contracts.Authorization.Model;
using Dpl.B2b.Contracts;

namespace Dpl.B2b.BusinessLogic.Authorization
{
    public static class AuthorizationHandlersServicesExtensions
    {
        public static void AddAuthorizationHandlers(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env = null)
        {
            // Note calling add handlers will always register resource based handler + attribute handler (without resource / referenceId)

            // admin - only dpl employees should be able todo any of the below things
            // create / delete ORG missing
            // create / update / delete posting account missing

            // organization
            // update read / update ORG account missing
            // create CUSTOMER missing

            // customer
            // update / read / delete CUSTOMER account missing
            // create DIVISION account missing

            // division
            // create / update read / delete DIVISION account missing

            services.AddActionRequirementHandlers<CanCreateVoucherRequirement>(PermissionResourceType.Division);
            services.AddActionRequirementHandlers<CanReadVoucherRequirement>(PermissionResourceType.Division);
            services.AddActionRequirementHandlers<CanCancelVoucherRequirement>(PermissionResourceType.Division);

            services.AddActionRequirementHandlers<CanCreateLoadCarrierReceiptRequirement>(PermissionResourceType.Division);
            services.AddActionRequirementHandlers<CanReadLoadCarrierReceiptRequirement>(PermissionResourceType.Division);
            services.AddActionRequirementHandlers<CanCancelLoadCarrierReceiptRequirement>(PermissionResourceType.Division);

            services.AddActionRequirementHandlers<CanCreateOrderRequirement>(PermissionResourceType.Division);
            services.AddActionRequirementHandlers<CanReadOrderRequirement>(PermissionResourceType.Division);
            services.AddActionRequirementHandlers<CanUpdateOrderRequirement>(PermissionResourceType.Division);
            services.AddActionRequirementHandlers<CanCancelOrderRequirement>(PermissionResourceType.Division);

            services.AddActionRequirementHandlers<OrderLoadRequirements.CanRead>(PermissionResourceType.Division);
            services.AddActionRequirementHandlers<OrderLoadRequirements.CanCancel>(PermissionResourceType.Division);

            services.AddActionRequirementHandlers<AccountingRecordsRequirements.CanRead>(PermissionResourceType.Division);

            services.AddActionRequirementHandlers<LivePoolingRequirements.CanCreateSearch>(PermissionResourceType.Division);
            services.AddActionRequirementHandlers<LivePoolingRequirements.CanReadOrders>(PermissionResourceType.Division);

            services.AddActionRequirementHandlers<SortingRequirements.CanCreate>(PermissionResourceType.Division);

            // create / read / cancel express requirements code missing

            // posting account
            // read posting account missing

            services.AddActionRequirementHandlers<CanCreateBalanceTransferRequirement>(PermissionResourceType.PostingAccount);
            services.AddActionRequirementHandlers<CanReadBalanceTransferRequirement>(PermissionResourceType.PostingAccount);
            services.AddActionRequirementHandlers<CanCancelBalanceTransferRequirement>(PermissionResourceType.PostingAccount);
        }

        internal static void AddActionRequirementHandlers<TRequirement>(this IServiceCollection services, PermissionResourceType resourceType)
            where TRequirement : ActionRequirement, IAuthorizationRequirement, new()
        {
            switch (resourceType)
            {
                case PermissionResourceType.Organization:
                    services.AddSingleton<IAuthorizationHandler, OrganizationActionRequirementAuthorizationHandler<TRequirement, OrganizationPermissionResource>>();                    
                    break;
                case PermissionResourceType.Customer:
                    services.AddSingleton<IAuthorizationHandler, CustomerActionRequirementAuthorizationHandler<TRequirement, CustomerPermissionResource>>();
                    break;
                case PermissionResourceType.Division:
                    services.AddSingleton<IAuthorizationHandler, DivisionActionRequirementAuthorizationHandler<TRequirement, DivisionPermissionResource>>();
                    break;
                case PermissionResourceType.PostingAccount:
                    services.AddSingleton<IAuthorizationHandler, PostingAccountActionRequirementAuthorizationHandler<TRequirement, PostingAccountPermissionResource>>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();

            }

            services.AddSingleton<IAuthorizationHandler, AttributeActionRequirementAuthorizationHandler<AttributeRequirement<TRequirement>>>();
        }
    }
}
