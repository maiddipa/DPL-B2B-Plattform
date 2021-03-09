using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

namespace Live2LMSService
   {
   class Config
      {
      public const string LiveServiceUser = "LiveOnlineService";

      // Instanz Name
      public string ServiceInstanceName = "Default";
      // Datenbank
      // LMS
      public string LMSServerName = "";
      public string LMSUserName = "";
      public string LMSPassword = "";
      public bool LMSUseIntegratedSecurity = false;
      public string LMSDatabaseName = "";
      public int LMSDatabaseTimeout = 15;

      public string LMSSchemaName = "dbo";
      private string OverrideDefaultSchemaString = "LMS";
      public bool DevelopmentEnvironment = false;

      // OrderManagement
      public string LiveQueueName = "";  // ordersyncrequest
      public string LiveQueueConnectionString = ""; // Endpoint=sb://dpllivesyncdvit.servicebus.windows.net/;SharedAccessKeyName=Manager;SharedAccessKey=BcqZ0kNRCUJx2I0BqeXZwVpare7XjHb38/fQMACPujs=;

      public int WaitSeconds = 10;

      public Config()
         {
         }

      public void Load()
         {
         var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

         var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", true, false)
             .AddJsonFile($"appsettings.{environmentName}.json", true, false);

         var configuration = builder.Build();
         string configurationSection = "Settings";
         var section = configuration.GetSection(configurationSection);

         // Instanz Name
         this.ServiceInstanceName = section.GetValue<string>("ServiceInstanceName"); ;

         // LMS
         this.LMSServerName = section.GetValue<string>("LMSServerName");
         this.LMSUserName = section.GetValue<string>("LMSUserName");
         this.LMSPassword = section.GetValue<string>("LMSPassword");
         this.LMSUseIntegratedSecurity = section.GetValue<bool>("LMSUseIntegratedSecurity");
         this.LMSDatabaseName = section.GetValue<string>("LMSDatabaseName");
         this.LMSDatabaseTimeout = section.GetValue<int>("LMSDatabaseTimeout");

         this.OverrideDefaultSchemaString = section.GetValue<string>("OverrideDefaultSchemaString");
         this.DevelopmentEnvironment = section.GetValue<bool>("DevelopmentEnvironment");
         if (this.DevelopmentEnvironment)
            {
            this.LMSSchemaName = this.OverrideDefaultSchemaString;
            }

         // Live Queue
         this.LiveQueueName = section.GetValue<string>("LiveQueueName");
         this.LiveQueueConnectionString = section.GetValue<string>("LiveQueueConnectionString");

         // Diverses
         this.WaitSeconds = section.GetValue<int>("WaitSeconds");
         }
      }
   }
