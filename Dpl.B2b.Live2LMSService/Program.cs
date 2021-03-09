using Live2LMSService;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using Topshelf;

namespace Dpl.B2b.Live2LMSService
   {
   class Program
      {
      private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

      static void Main(string[] args)
         {
            {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string configFile = exePath + @"\" + "Log4Net.config";

            // https://stackoverflow.com/questions/1535736/how-can-i-change-the-file-location-programmatically
            log4net.GlobalContext.Properties["LogFileFolder"] = exePath; //log file folder

            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo(configFile));
            
            AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;

            HostFactory.Run(serviceConfig =>
            {
               serviceConfig.Service<ServiceMain>(serviceInstance =>
               {
                  serviceInstance.ConstructUsing(() => new ServiceMain());
                  serviceInstance.WhenStarted(execute => execute.OnStart());
                  serviceInstance.WhenPaused(execute => execute.OnPause());
                  serviceInstance.WhenContinued(execute => execute.OnContinue());
                  serviceInstance.WhenStopped(execute => execute.OnStop());

               });
               serviceConfig.SetServiceName("Live2LMSService");
               serviceConfig.SetDisplayName("Live2LMS Service");
               serviceConfig.SetDescription("Service für die Übertragung der Daten vom DPL :Live zur LMS Datenbank");

               serviceConfig.StartAutomaticallyDelayed();
               serviceConfig.EnableServiceRecovery(recoveryOption => recoveryOption.RestartService(1));
               serviceConfig.EnablePauseAndContinue();
               serviceConfig.DependsOnEventLog();
            });

            }

         }
      private static void LogUnhandledException(object sender, UnhandledExceptionEventArgs e)
         {
         Log.Error(String.Format("Eine {0}fatale unbehandelte Ausnahme ist aufgetreten", e.IsTerminating ? String.Empty : "nicht-"), e.ExceptionObject as Exception);
         }
      }
   }
