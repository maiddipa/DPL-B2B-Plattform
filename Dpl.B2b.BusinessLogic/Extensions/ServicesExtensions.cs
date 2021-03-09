using System.IO;
using AutoMapper;
using Azure.Core.Extensions;
using Azure.Storage.Blobs;
using Dpl.B2b.BusinessLogic.Graph;
using Dpl.B2b.BusinessLogic.Mappings;
using Dpl.B2b.BusinessLogic.Rules;
using Dpl.B2b.Contracts;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Olma = Dpl.B2b.Dal.Models;
using Ltms = Dpl.B2b.Dal.Ltms;
using Lms = Dpl.B2b.Dal.Models.Lms;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Dpl.B2b.BusinessLogic.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddWebApiServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env = null)
        {
            services.AddDbContext<OlmaDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Olma"), x =>
                {
                    x.UseNetTopologySuite();
                    x.MigrationsAssembly(typeof(OlmaDbContext).Assembly.FullName);
                    x.EnableRetryOnFailure();
                });

                if(env.IsDevelopment() || env.IsEnvironment("CustomDev"))
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            InitDistributedCache(services, configuration);

            if (env != null && (env.IsDevelopment() || env.IsEnvironment("CustomDev")))
            {
                services.AddSingleton<IStorageService, Storage.LocalDiskStorageService>();
            }
            else
            {
                // TODO find out how to properly use AddBlobServiceClient
                // generally there should be only one instance created
                // so far we create it ourselves in the storage service but there seems to be an extension method
                // but im not not sure how to use it
                //(services as IAzureClientFactoryBuilder).AddBlobServiceClient(configuration.GetConnectionString(Constants.Documents.ConnectionString));
                services.AddSingleton<IStorageService, Storage.AzureStorageService>();
            }

            #region LMS Availability/Delivery

            services.AddScoped<IRepository<Lms.LmsAvailability>, OlmaRepository<Lms.LmsAvailability>>();
            services.AddScoped<IRepository<Lms.LmsDelivery>, OlmaRepository<Lms.LmsDelivery>>();
            services.AddScoped<IRepository<Lms.LmsAvail2deli>, OlmaRepository<Lms.LmsAvail2deli>>();
            
            #endregion

            #region Address

            services.AddScoped<IRepository<Olma.Address>, OlmaRepository<Olma.Address>>();
            services.AddScoped<IAddressesService, AddressesService>();

            #endregion

            #region AccountingRecordsService

            services.AddScoped<IAccountingRecordsService, AccountingRecordsService>();
            services.AddScoped<IRepository<Olma.PostingRequest>, OlmaRepository<Olma.PostingRequest>>();
            services.AddScoped<IRepository<Olma.PostingRequestSyncError>, OlmaRepository<Olma.PostingRequestSyncError>>();
            services.AddScoped<IRepository<Ltms.Processes>, OlmaRepository<Ltms.Processes>>();

            #endregion

            #region BalanceTransfersService

            services.AddScoped<IBalanceTransfersService, BalanceTransfersService>();

            #endregion

            #region CountriesService

            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IRepository<Olma.Country>, OlmaRepository<Olma.Country>>();
            services.AddScoped<IRepository<Olma.CountryState>, OlmaRepository<Olma.CountryState>>();

            #endregion

            #region OrganizationsAdminService

            services.AddScoped<IOrganizationsService, OrganizationsService>();

            #endregion

            #region CustomersService

            services.AddScoped<ICustomersService, CustomersService>();
            services.AddScoped<IRepository<Olma.Customer>, OlmaRepository<Olma.Customer>>();

            #endregion

            #region CustomerDivisionsService

            services.AddScoped<ICustomerDivisionsService, CustomerDivisionsService>();

            #endregion

            #region DocumentsService

            services.AddScoped<IDocumentsService, DocumentsService>();
            services.AddScoped<IRepository<Olma.Document>, OlmaRepository<Olma.Document>>();

            #endregion

            #region DocumentSettingsService

            services.AddScoped<IDocumentSettingsService, DocumentSettingsService>();
            services.AddScoped<ICustomerDocumentSettingsService, CustomerDocumentSettingsService>();
            services.AddScoped<IDivisionDocumentSettingsService, DivisionDocumentSettingsService>();
            services.AddScoped<IRepository<Olma.Organization>, OlmaRepository<Olma.Organization>>();
            services.AddScoped<IRepository<Olma.CustomerDocumentSetting>, OlmaRepository<Olma.CustomerDocumentSetting>>();
            services.AddScoped<IRepository<Olma.CustomerDivisionDocumentSetting>, OlmaRepository<Olma.CustomerDivisionDocumentSetting>>();

            #endregion

            #region DocumentTypesService

            services.AddSingleton<IDocumentTypesService, DocumentTypesService>();
            services.AddScoped<IRepository<Olma.DocumentType>, OlmaRepository<Olma.DocumentType>>();

            #endregion

            #region DplEmployeeService

            services.AddScoped<IDplEmployeeService, DplEmployeeService>();

            #endregion

            #region ExpressCodesService

            services.AddScoped<IExpressCodesService, ExpressCodesService>();
            services.AddScoped<IRepository<Olma.ExpressCode>, OlmaRepository<Olma.ExpressCode>>();
            services.AddScoped<IRepository<Olma.ExpressCodeUsageCondition>, OlmaRepository<Olma.ExpressCodeUsageCondition>>();

            #endregion

            #region ImportService

            services.AddScoped<IImportService, ImportService>();

            #endregion

            #region LoadCarriersService

            services.AddScoped<ILoadCarriersService, LoadCarriersService>();
            services.AddScoped<IRepository<Olma.LoadCarrier>, OlmaRepository<Olma.LoadCarrier>>();

            #endregion

            #region LoadCarrierOfferingsService

            services.AddScoped<ILoadCarrierOfferingsService, LoadCarrierOfferingsService>();
            services.AddScoped<IRepository<Olma.LmsOrder>, OlmaRepository<Olma.LmsOrder>>();

            #endregion

            #region LoadCarrierReceiptsService

            services.AddScoped<ILoadCarrierReceiptsService, LoadCarrierReceiptsService>();
            services.AddScoped<IRepository<Olma.LoadCarrierReceipt>, OlmaRepository<Olma.LoadCarrierReceipt>>();

            #endregion

            #region LoadCarrierSortingService

            services.AddScoped<ILoadCarrierSortingService, LoadCarrierSortingService>();
            services.AddScoped<IRepository<Olma.LoadCarrierSorting>, OlmaRepository<Olma.LoadCarrierSorting>>();

            #endregion

            #region Localization + Localization Update Service

            services.AddSingleton<ILocalizationService, LocalizationService>();
            services.AddScoped<ILocalizationUpdateService, LocalizationUpdateService>();
            services.AddScoped<IRepository<Olma.LocalizationLanguage>, OlmaRepository<Olma.LocalizationLanguage>>();
            services.AddScoped<IRepository<Olma.LocalizationItem>, OlmaRepository<Olma.LocalizationItem>>();
            services.AddScoped<IRepository<Olma.LocalizationText>, OlmaRepository<Olma.LocalizationText>>();
            services.AddScoped<IRepository<Olma.LocalizationLanguage>, OlmaRepository<Olma.LocalizationLanguage>>();

            #endregion

            #region LoadingLocationsService

            services.AddScoped<ILoadingLocationsService, LoadingLocationsService>();
            services.AddScoped<IRepository<Olma.LoadingLocation>, OlmaRepository<Olma.LoadingLocation>>();
            services.AddScoped<IRepository<Olma.LoadingLocationLoadCarrierDimensions>, OlmaRepository<Olma.LoadingLocationLoadCarrierDimensions>>();

            #endregion

            #region MasterDataService

            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IRepository<Olma.LocalizationLanguage>, OlmaRepository<Olma.LocalizationLanguage>>();
            services.AddScoped<IRepository<Olma.DocumentState>, OlmaRepository<Olma.DocumentState>>();

            #endregion

            #region MapsService

            services.AddSingleton<IMapsService, ThirdParty.MapsService>();

            #endregion

            #region NumberSequencesService

            services.AddScoped<INumberSequencesService, NumberSequencesService>();
            services.AddScoped<IRepository<Olma.DocumentNumberSequence>, OlmaRepository<Olma.DocumentNumberSequence>>();

            #endregion

            #region Order / OrderMatch

            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IOrderGroupsService, OrderGroupsService>();
            services.AddScoped<IRepository<Olma.Order>, OlmaRepository<Olma.Order>>();
            services.AddScoped<IRepository<Olma.OrderGroup>, OlmaRepository<Olma.OrderGroup>>();
            services.AddScoped<IRepository<Olma.OrderSyncError>, OlmaRepository<Olma.OrderSyncError>>();

            services.AddScoped<IOrderMatchesService, OrderMatchesService>();
            services.AddScoped<IRepository<Olma.OrderMatch>, OlmaRepository<Olma.OrderMatch>>();

            #endregion

            #region OrderLoads

            services.AddScoped<IOrderLoadsService, OrderLoadsService>();
            services.AddScoped<IRepository<Olma.OrderLoad>, OlmaRepository<Olma.OrderLoad>>();

            #endregion

            #region PartnersService / PartnerDirectoriesService
            services.AddScoped<IPartnersService, PartnersService>();
            services.AddScoped<ICustomerPartnersService, CustomerPartnersService>();
            services.AddScoped<IPartnerDirectoriesService, PartnerDirectoriesService>();
            services.AddScoped<IRepository<Olma.PartnerDirectory>, OlmaRepository<Olma.PartnerDirectory>>();
            services.AddScoped<IRepository<Olma.PartnerPreset>, OlmaRepository<Olma.PartnerPreset>>();
            services.AddScoped<IRepository<Olma.Partner>, OlmaRepository<Olma.Partner>>();
            services.AddScoped<IRepository<Olma.CustomerPartner>, OlmaRepository<Olma.CustomerPartner>>();
            services.AddScoped<IRepository<Olma.CustomerPartnerDirectoryAccess>, OlmaRepository<Olma.CustomerPartnerDirectoryAccess>>();


            #endregion

            #region PermissionService

            services.AddSingleton<IPermissionsService, PermissionsService>();
            services.AddScoped<IPermissionsAdminService, PermissionsAdminService>();

            #endregion

            #region PostingAccountsService

            services.AddScoped<IPostingAccountsService, PostingAccountsService>();
            services.AddScoped<IPostingAccountBalancesService, PostingAccountBalancesService>();
            services.AddScoped<IRepository<Olma.PostingAccount>, OlmaRepository<Olma.PostingAccount>>();
            services.AddScoped<IRepository<Olma.CalculatedBalance>, OlmaRepository<Olma.CalculatedBalance>>();
            services.AddScoped<IRepository<Olma.CalculatedBalancePosition>, OlmaRepository<Olma.CalculatedBalancePosition>>();
            services.AddScoped<IRepository<Ltms.Bookings>, OlmaRepository<Ltms.Bookings>>();

            #endregion
            
            		
            #region PostingAccountConditionsService
            services.AddScoped<IPostingAccountConditionsService, PostingAccountConditionsService>();
            services.AddScoped<IRepository<Ltms.Conditions>, OlmaRepository<Ltms.Conditions>>();
            services.AddScoped<IRepository<Olma.LoadCarrier>, OlmaRepository<Olma.LoadCarrier>>();
            #endregion

            #region PostingRequestsService

            services.AddScoped<IPostingRequestsService, PostingRequestsService>();

            #endregion

            #region Reference Services (Ltms / Lms)

            services.AddScoped<ILtmsReferenceLookupService, LtmsReferenceLookupService>();

            #endregion

            #region ReportingService / ReportGeneratorService

            services.AddScoped<Reporting.IReportGeneratorService, Reporting.ReportGeneratorService>();
            //services.AddScoped<DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension, Reporting.ReportingStorageService>();
            services.AddScoped<IRepository<Olma.DocumentTemplate>, OlmaRepository<Olma.DocumentTemplate>>();
            services.AddScoped<IRepository<Olma.DocumentTemplateLabel>, OlmaRepository<Olma.DocumentTemplateLabel>>();
            services.AddScoped<IRepository<Olma.CustomDocumentLabel>, OlmaRepository<Olma.CustomDocumentLabel>>();
            services.AddScoped<IRepository<Olma.DocumentType>, OlmaRepository<Olma.DocumentType>>();
            services.AddScoped<IRepository<Olma.AdditionalField>, OlmaRepository<Olma.AdditionalField>>();

            #endregion

            #region Sync

            services.AddScoped<ISyncService, Sync.SyncService>();
            services.AddScoped<IRepository<Ltms.Transactions>, OlmaRepository<Ltms.Transactions>>();
            services.AddScoped<IRepository<Ltms.Accounts>, OlmaRepository<Ltms.Accounts>>();
            services.AddScoped<IRepository<Ltms.Pallet>, OlmaRepository<Ltms.Pallet>>();

            #endregion

            #region TransportOfferingsService

            services.AddScoped<ITransportOfferingsService, TransportOfferingsService>();
            services.AddScoped<IRepository<Olma.Transport>, OlmaRepository<Olma.Transport>>();
            services.AddScoped<IRepository<Olma.TransportBid>, OlmaRepository<Olma.TransportBid>>();

            #endregion

            #region VoucherService / VoucherReasonType

            services.AddScoped<IVouchersService, VouchersService>();
            services.AddScoped<IRepository<Olma.Voucher>, OlmaRepository<Olma.Voucher>>();
            services.AddScoped<IRepository<Olma.CustomerDivision>, OlmaRepository<Olma.CustomerDivision>>();
            services.AddScoped<IRepository<Olma.VoucherReasonType>, OlmaRepository<Olma.VoucherReasonType>>();

            #endregion

            #region UploadService

            services.AddScoped<IUploadsService, UploadsService>();

            #endregion

            #region UserService

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRepository<Olma.User>, OlmaRepository<Olma.User>>();
            services.AddScoped<IRepository<Olma.Permission>, OlmaRepository<Olma.Permission>>();
            services.AddScoped<IRepository<Olma.UserCustomerRelation>, OlmaRepository<Olma.UserCustomerRelation>>();

            #endregion
            
            #region UserGroupService

            services.AddScoped<IUserGroupService, UserGroupService>();
            services.AddScoped<IRepository<Olma.UserGroup>, OlmaRepository<Olma.UserGroup>>();
            services.AddScoped<IRepository<Olma.OrganizationUserGroup>, OlmaRepository<Olma.OrganizationUserGroup>>();
            services.AddScoped<IRepository<Olma.CustomerDivisionUserGroup>, OlmaRepository<Olma.CustomerDivisionUserGroup>>();
            services.AddScoped<IRepository<Olma.CustomerUserGroup>, OlmaRepository<Olma.CustomerUserGroup>>();
            services.AddScoped<IRepository<Olma.UserUserGroupRelation>, OlmaRepository<Olma.UserUserGroupRelation>>();

            #endregion

            #region SynchronizationsService
            services.AddScoped<ISynchronizationsService, SynchronizationsService>();
            #endregion

            #region AutoMapper

            // Auto Mapper Configurations
            var config = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });


            config.AssertConfigurationIsValid();

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            #endregion
            
            #region BlacklistFactory
            
            Blacklist.Init(
                rootPath: env?.ContentRootPath??Directory.GetCurrentDirectory(), 
                blacklistJsonFilename: "blacklist.json");    
            
            services.AddSingleton<IBlacklistFactory, BlacklistFactory>();

            
            #endregion
            
            #region GraphFactory
            
            services.AddSingleton<IGraphFactory, GraphFactory>();
            
            #endregion
        }

        /// <summary>
        /// Falls Redis Database hinterlegt wurde wird dieser als DistributedCache verwendet ansonsten wird ein simpler
        /// MemoryCache dafür eingerichtet.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void InitDistributedCache(IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString= configuration["AzureRedis:ConnectionString2"];
            if (redisConnectionString == null)
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddStackExchangeRedisCache(options => { options.Configuration = configuration["AzureRedis:ConnectionString"]; });
                InitCacheManager(services, configuration);
            } 
        }

        private static void InitCacheManager(IServiceCollection services, IConfiguration configuration)
        {
            // CacheManager
            services.AddSingleton<ICacheManager, CacheManager>((x) =>
            {
                var cfg= x.GetService<IOptions<CacheManager.AzureRedisConfig>>();
                var x2= new CacheManager(cfg); 
                x2.SetUpCacheManager();
                return x2;
            });
        }
    }
}
