using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Dpl.B2b.Web.Api.Extensions;
using AutoMapper;
using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using DevExpress.Web.Internal;
using Dpl.B2b.BusinessLogic.Extensions;
using Dpl.B2b.Contracts;
using Dpl.B2b.Web.Api.Authorization;
using Dpl.B2b.Web.Api.Authorization.Requirements;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema.Generation;
using DevExpress.XtraReports.Security;
using Dpl.B2b.BusinessLogic;
using Dpl.B2b.Dal;
using Dpl.B2b.Dal.Models;
using Dpl.B2b.BusinessLogic.Authorization;
using Dpl.B2b.Contracts.Options;
using Microsoft.AspNetCore.Authorization.Infrastructure;

// this ensures that the approriate standard response codes per REST operation are shown in SwaggerUI
// [assembly: ApiConventionType(typeof(Dpl.B2b.Web.Api.DplApiConventions))]
namespace Dpl.B2b.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationInsightsOptions>(options => Configuration.GetSection("ApplicationInsights").Bind(options));
            services.Configure<CacheOptions>(options => Configuration.GetSection("Cache").Bind(options));
            services.Configure<CacheManager.AzureRedisConfig>(options => Configuration.GetSection("AzureRedis").Bind(options));

            services.AddApplicationInsightsTelemetry(Configuration);

            services.ConfigureCors();
            services.AddAuthentication(AzureADDefaults.JwtBearerAuthenticationScheme)
                .AddAzureADBearer(options => { Configuration.Bind("AzureAd", options); });

            if (Environment.IsDevelopment() || Environment.IsEnvironment("CustomDev"))
            {
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

                #region MiniProfiler

                services.AddMemoryCache();

                // Profile is available via
                // https://localhost:5001/profiler/results-index
                services.AddMiniProfiler(options => {
                    options.RouteBasePath = "/profiler";
                    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();
                }).AddEntityFramework();

                #endregion
            }

            #region Authorization

            services.AddAuthorization(options =>
            {
                // Sample how to create policies with more than one requirement
                // leave this here as a sample, but comment out later
                //options.AddPolicy(Policy.SamplePolicyWIthMoreThanOneRequirement, policy =>
                //{
                //    policy.AddRequirements(new MyRequirement1());
                //    policy.AddRequirements(new MyRequirement2());
                //});
            });

            services.AddAuthorizationHandlers(Configuration, Environment);

            services.AddSingleton<IAuthorizationDataService, ClaimsService>();

            // register the dynamic policy provider & permission authorization handler
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

            #endregion

            services.AddHttpContextAccessor();

            services.AddWebApiServices(Configuration, Environment);

            // Start - NSwag
            services.AddOpenApiDocument();
            // ENd - NSwag

            services.AddRouting(options => options.LowercaseUrls = true);

            // devexpress reporting
            services.AddDevExpressControls();
            // DO NOT MOVE, when moved to any other place we recieve an access violation exception
            services.AddScoped<DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension, BusinessLogic.Reporting.ReportingStorageService>();

            services.AddControllers()
                //.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null) // Added by DevExtreme project Setup Wizard and
                                                                                                       // disabled because: not sure if it will have sideeffects 
                .ConfigureApiBehaviorOptions(options =>
                {
                    //options.InvalidModelStateResponseFactory = context =>
                    //{
                    //    var problemDetails = new DplProblemDetails(context.ModelState)
                    //    {
                    //        Instance = context.HttpContext.Request.Path,
                    //        Status = Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest,
                    //        Type = $"https://httpstatuses.com/400",
                    //        Detail = ""//ApiConstants.Messages.ModelStateValidation
                    //    };

                    //    return new BadRequestObjectResult(problemDetails)
                    //    {
                    //        ContentTypes =
                    //        {
                    //            ApiConstants.ContentTypes.ProblemJson,
                    //            ApiConstants.ContentTypes.ProblemXml
                    //        }
                    //    };
                    //};
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

                    // this seems like a more advanced way to handle null / default values
                    // with seperation between serialization / deserialization
                    // but could not get it to work
                    // options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore; // Ignore causes errors in the client model
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

                })
                // devexpress reporting
                .AddDefaultReportingControllers();

            // devexpress reporting
            services.ConfigureReportingServices(configurator =>
            {
                configurator.ConfigureReportDesigner(designerConfigurator =>
                {
                    designerConfigurator.RegisterDataSourceWizardConfigFileConnectionStringsProvider();
                });
                configurator.ConfigureWebDocumentViewer(viewerConfigurator =>
                {
                    viewerConfigurator.UseCachedReportSourceBuilder();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension reportStorage)
        {
            if (Environment.IsDevelopment() || Environment.IsEnvironment("CustomDev"))
            {
                app.UseDeveloperExceptionPage();

                // Start - NSwag
                app.UseOpenApi();
                app.UseSwaggerUi3();
                // ENd - NSwag
            }
            else if (Environment.IsStaging())
            {
                // enable developer exception page in non development non production environments
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseMiniProfiler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            // this adds all claims necessary for security to function
            app.UsePopulateSecurityClaims();

            app.UseAuthorization();

            app.UseCors("CorsPolicy");


            DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(reportStorage);
            //DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode = DevExpress.XtraReports.UI.DataBindingMode.Expressions; from sample but expression is newer than the old reports
            DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode = DevExpress.XtraReports.UI.DataBindingMode.Bindings;
            DevExpress.XtraReports.Web.ClientControls.LoggerService.Initialize(ProcessDevexpressException);
            app.UseDevExpressControls();

            ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Unrestricted);

            // this needs to happen before custom authorization middleware is run
            // so that middleware has access to route data
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // reporting


        }

        void ProcessDevexpressException(Exception ex, string message)
        {
            // Log exceptions here. For instance:
            System.Diagnostics.Debug.WriteLine("[{0}]: Exception occured. Message: '{1}'. Exception Details:\r\n{2}",
                DateTime.Now, message, ex);
        }
    }
}
