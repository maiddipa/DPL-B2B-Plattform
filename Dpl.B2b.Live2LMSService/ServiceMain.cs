//#define TEST_ONLY

using System;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using System.Reflection;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Common.Enumerations;
using Microsoft.Azure.ServiceBus;
using Dpl.B2b.Live2LMSService;
using Newtonsoft.Json;
using log4net;

namespace Live2LMSService
   {
   public partial class ServiceMain
      {
      public static string ErrorReasonMessageType = "MessageType";
      public static string ErrorDescriptionMessageType = "Unbekannter MessageType: ";
      public static string ErrorReasonCustomerNumber = "CustomerNumber";
      public static string ErrorDescriptionCustomerNumber = "Unbekannte CustomerNumber: ";
      public static string ErrorReasonCustomerNumberLoadingLocation = "CustomerNumberLoadingLocation";
      public static string ErrorDescriptionCustomerNumberLoadingLocation = "Unbekannte CustomerNumberLoadingLocation: ";
      public static string ErrorReasonRefLmsLoadCarrierId = "RefLmsLoadCarrierId";
      public static string ErrorDescriptionRefLmsLoadCarrierId = "Unbekannte RefLmsLoadCarrierId: ";
      public static string ErrorReasonException = "Exception";
      public static string ErrorDescriptionException = "Exception: ";
      public static string ErrorReasonRowNotFound = "RowNotFound";
      public static string ErrorDescriptionRowNotFound = "Datensatz nicht gefunden (RowID): ";
      public static string ErrorReasonConditionColumnnValue = "ConditionColumnnValue";
      public static string ErrorDescriptionConditionColumnnValue = "Spalte erfüllt nicht Bedingung: ";
      public static string ErrorReasonPermanentAvailabilityAlreadyExists = "PermanentAvailabilityAlreadyExists";
      public static string ErrorDescriptionPermanentAvailabilityAlreadyExists = "Permanente Verfügbarkeit existiert bereits: ";
      public static string ErrorReasonPermanentAvailabilityNotExists = "PermanentAvailabilityNotExists";
      public static string ErrorDescriptionPermanentAvailabilityNotExists = "Permanente Verfügbarkeit existiert nicht: ";
      public static string ErrorReasonNotImplemented = "NotImplemented";
      public static string ErrorDescriptionNotImplemented = "Aktion für Message nicht implementiert";
      public static string ErrorReasonNoLoadingLocationForCustomer = "NoLoadingLocationForCustomer";
      public static string ErrorDescriptionNoLoadingLocationForCustomer = "Keine LoadingLocation für CustomerNumber: ";
      public static string ErrorReasonTooManyUpdateHits = "TooManyUpdateHits ";
      public static string ErrorDescriptionTooManyUpdateHits = "Zu viele Treffer beim Update: ";

      private bool ServiceExitFlag = false;
      private bool ServiceErrorFlag = false;
      private ManualResetEvent RunWorkerThread = new ManualResetEvent(true);

      private Thread workerThread = null;

      // Event Log
      //private string EventLogName = "Live2LMSService";
      //private string EventLogSource = "L2L";

      // Log4Net
      private ILog Log4NetLog = LogManager.GetLogger(typeof(Program));

      // Konfiguration
      private Config MyConfig = new Config();

      // Datenbankverbindung
      // LMS
      private SqlConnection LMSSqlConnection;

      // Service Queue
      private LiveQueueTools LQTools;

      public ServiceMain()
         {
         try
            {
            InitializeComponent();

            // Konfiguration laden
            this.MyConfig.Load();

            // EventLog
            //this.EventLogName = MyConfig.ServiceInstanceName + "_" + this.EventLogName;
            //this.EventLogSource = this.EventLogSource + "_" + MyConfig.ServiceInstanceName;
            //if (!System.Diagnostics.EventLog.SourceExists(this.EventLogSource))
            //   System.Diagnostics.EventLog.CreateEventSource(this.EventLogSource, this.EventLogName);
            // Log4Net


            this.LogInformation("Service wurde geladen, Live2LMSService V" + Assembly.GetExecutingAssembly().GetName().Version.ToString());

            }
         catch (System.Threading.ThreadAbortException)
            {
            }
         catch (Exception Ex)
            {
            this.LogException("ServiceMain", Ex);
            }
         }

      // Service starten
      public void OnStart()
         {
         try
            {
            // Konfiguration laden
            this.MyConfig.Load();

            this.ServiceExitFlag = false;

            // Datenbankobjekte anlegen
            this.InitDBs();

            // Queue Object anlegen
            this.LQTools = new LiveQueueTools();

            // Thread starten, der die eigentliche Arbeit erledigt
            if ((workerThread == null) ||
                ((workerThread.ThreadState &
                  (System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Stopped)) != 0))
               {
               this.LogInformation("OnStart - Starte Worker Thread...");

               this.workerThread = new Thread(new ThreadStart(ServiceWorkerMethod));
               this.workerThread.Start();
               this.RunWorkerThread.Set();
               }
            if (workerThread != null)
               {
               this.LogInformation("OnStart - Worker Thread gestartet, Status = " +
                                   workerThread.ThreadState.ToString());
               }
            }
         catch (Exception Ex)
            {
            this.LogException("OnStart", Ex);
            }
         }

      // Stop this service.
      public void OnStop()
         {
         try
            {
            // Signal the worker thread to exit.
            if ((workerThread != null) && (workerThread.IsAlive))
               {
               this.LogInformation("OnStop - Stoppe Worker Thread");
               if (!this.ServiceExitFlag)
                  {
                  this.ServiceExitFlag = true;
                  this.RunWorkerThread.Reset();
                  Thread.Sleep(3000);
                  if (workerThread.IsAlive)
                     this.workerThread.Interrupt();
                  }
               }
            }
         catch (System.Threading.ThreadAbortException Ex)
            {
            this.LogException("OnStop1", Ex);
            }
         catch (Exception Ex)
            {
            this.LogException("OnStop", Ex);
            }
         if(this.ServiceErrorFlag)
            {
            System.Environment.Exit(0);
            }
         }

      // Pause the service.
      public void OnPause()
         {
         try
            {
            // Pause the worker thread.
            if ((workerThread != null) &&
                (workerThread.IsAlive) &&
                ((workerThread.ThreadState &
                  (System.Threading.ThreadState.Suspended | System.Threading.ThreadState.SuspendRequested)) == 0))
               {
               this.LogInformation("OnPause - Pausiere Worker Thread...");

               this.RunWorkerThread.Reset();
               Thread.Sleep(5000);
               }

            if (workerThread != null)
               {
               this.LogInformation("OnPause - Worker Thread pausiert, Status = " +
                                   workerThread.ThreadState.ToString());
               }
            }
         catch (Exception Ex)
            {
            this.LogException("OnPause", Ex);
            }
         }

      // Continue a paused service.
      public void OnContinue()
         {
         try
            {
            // Signal the worker thread to continue.
            if ((workerThread != null) &&
                ((workerThread.ThreadState &
                  (System.Threading.ThreadState.Suspended | System.Threading.ThreadState.SuspendRequested |
                   System.Threading.ThreadState.WaitSleepJoin)) != 0))
               {
               this.LogInformation("OnContinue - Setze Worker Thread fort...");
               this.RunWorkerThread.Set();
               }
            if (workerThread != null)
               {
               this.LogInformation("OnContinue - Worker Thread fortgesetzt, Status = " +
                                   workerThread.ThreadState.ToString());
               }

            }
         catch (Exception Ex)
            {
            this.LogException("OnContinue", Ex);
            }
         }

      public void ServiceWorkerMethod()
         {
         int seconds = 0;
         int updateCounter;

         this.LogInformation("Worker Thread - Starte...\n"
            + $"Datenbank-Server: {MyConfig.LMSServerName}, Datenbank: {MyConfig.LMSDatabaseName}, Queue: {MyConfig.LiveQueueName}");

         // Init LiveQueueTools
         this.LQTools.LiveQueueToolsInit(MyConfig.LiveQueueName, MyConfig.LiveQueueConnectionString);

         // Datenbank Anmeldung
         this.OpenDBs();

         do
            {
            try
               {
               // Konfiguration refreshen
               this.MyConfig.Load();

               seconds = this.MyConfig.WaitSeconds;

               updateCounter = 0;

               // Live Einträge bearbeiten

               if (this.MyConfig.DevelopmentEnvironment)
                  {
                  this.LogInformation("Bearbeite Einträge um " + DateTime.Now);
                  }
               else
                  {
                  Console.Write("."); // IsAlive Hinweis für Console
                  }

               int currentUpdateCounter = 0;
               while ((currentUpdateCounter = this.ProcessQueueEntries()) > 0)
                  {
                  updateCounter += currentUpdateCounter;
                  }
               if (updateCounter >= 2)
                  {
                  this.LogInformation("Worker Thread - " + updateCounter.ToString() + " Live-Einträge erfolgreich bearbeitet");
                  }
               else if (updateCounter >= 1)
                  {
                  this.LogInformation("Worker Thread - " + updateCounter.ToString() + " Live-Eintrag erfolgreich bearbeitet");
                  }
               }
            catch (ThreadAbortException)
               {
               if (!this.ServiceExitFlag)
                  this.LogWarning("Worker Thread - Abbruch-Anforderung erhalten");
               break;
               }
            catch (Exception Ex)
               {
               if (!this.ServiceExitFlag)
                  this.LogException("ServiceWorkerMethod", Ex);
               // Service nach Fehler beenden, sollte vom Service Manager wieder automatisch neu gestartet werden
               break;
               }
            finally
               {
               }

            while ((seconds--) > 0 && !this.ServiceExitFlag && !this.ServiceErrorFlag)
               Thread.Sleep(1000);

            // Blockieren, wenn Service angehalten ist
            //this.RunWorkerThread.WaitOne();
            } while (!this.ServiceExitFlag && !this.ServiceErrorFlag);

         this.LQTools.LiveQueueToolsExit();
         // Datenbank Abmeldung
         this.CloseDBs();

         this.LogInformation("Worker Thread - Beendet");
         if (!this.ServiceExitFlag)
            {
            // Wenn Thread durch Fehler beendet wurde, Service stoppen
            this.OnStop();
            }
         }

      private int ProcessQueueEntries()
         {
         SqlTransaction LMSTransaction = null;
         string errorReason = "";
         string errorDescription = "";

         int liveUpdateCounter = 0;

         OrderCreateSyncRequest orderCreateSyncRequest = null;
         OrderUpdateSyncRequest orderUpdateSyncRequest = null;
         OrderCancelSyncRequest orderCancelSyncRequest = null;
         OrderFulfillSyncRequest orderFulfillRequest = null;
         OrderRollbackFulfillmentSyncRequest orderRollbackFulfillRequest = null;

         string messageId = "";
         string lockToken = "";
         Message message = null;

         try
            {
            Tuple<String, String, Message> queueRequest = this.LQTools.LiveQueueToolsGetLiveRequest();
            if (queueRequest != null)
               {
               Console.WriteLine(""); // IsAlive Hinweis für Console
               messageId = queueRequest.Item1;
               lockToken = queueRequest.Item2;

               message = queueRequest.Item3;

               bool success = false;
               switch (message.ContentType)
                  {
                  case nameof(OrderCreateSyncRequest):
                     orderCreateSyncRequest = message.As<OrderCreateSyncRequest>().ConvertDateTimesToCEST();

                     Console.WriteLine("Bearbeite OrderCreateSyncRequest:");
                     Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(orderCreateSyncRequest,
                         new JsonSerializerSettings
                            {
                            Formatting = Formatting.Indented,
                            }));

                     LMSTransaction = this.LMSSqlConnection.BeginTransaction();
                     switch (orderCreateSyncRequest.Type)
                        {
                        case OrderType.Supply:
                           if (!orderCreateSyncRequest.IsPermanent)
                              {
                              // Normale Verfügbarkeit
                              if (LiveDBTools.CheckAvailability(this.MyConfig, LMSTransaction, orderCreateSyncRequest, out errorReason, out errorDescription)
                                  && (success = LiveDBTools.InsertAvailability(MyConfig, LMSTransaction, orderCreateSyncRequest, out errorReason, out errorDescription)))
                                 {
                                 liveUpdateCounter++;
                                 }
                              }
                           else
                              {
                              // Dauerverfügbarkeit
                              if (LiveDBTools.CheckPermanentAvailability(this.MyConfig, LMSTransaction, orderCreateSyncRequest, out errorReason, out errorDescription)
                                  && (success = LiveDBTools.InsertPermanentAvailability(MyConfig, LMSTransaction, orderCreateSyncRequest, out errorReason, out errorDescription)))
                                 {
                                 liveUpdateCounter++;
                                 }
                              }
                           break;
                        case OrderType.Demand:
                           if (LiveDBTools.CheckDelivery(this.MyConfig, LMSTransaction, orderCreateSyncRequest, out errorReason, out errorDescription)
                                 && (success = LiveDBTools.InsertDelivery(MyConfig, LMSTransaction, orderCreateSyncRequest, out errorReason, out errorDescription)))
                              {
                              liveUpdateCounter++;
                              }
                           break;
                        }

                     if (!success)
                        {
                        this.LogWarning("Bearbeitung Queue Entry '" + messageId + "' nicht möglich, Grund: '" + errorReason + "', Beschreibung: '" + errorDescription + "'");
                        this.LQTools.LiveQueueToolsDenyLiveRequest(lockToken, errorReason, errorDescription);
                        break;
                        }
                     break;
                  case nameof(OrderUpdateSyncRequest):
                     orderUpdateSyncRequest = message.As<OrderUpdateSyncRequest>().ConvertDateTimesToCEST();

                     Console.WriteLine("Bearbeite OrderUpdateSyncRequest:");
                     Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(orderUpdateSyncRequest,
                         new JsonSerializerSettings
                            {
                            Formatting = Formatting.Indented,
                            }));

                     LMSTransaction = this.LMSSqlConnection.BeginTransaction();
                     switch (orderUpdateSyncRequest.Type)
                        {
                        case OrderType.Supply:
                           if (orderUpdateSyncRequest.IsPermanent)
                              {
                              // Dauerverfügbarkeit
                              if (success = LiveDBTools.UpdatePermanentAvailability(MyConfig, LMSTransaction, orderUpdateSyncRequest, out errorReason, out errorDescription))
                                 {
                                 liveUpdateCounter++;
                                 }
                              }
                           else
                              {
                              errorReason = ErrorReasonNotImplemented;
                              errorDescription = ErrorDescriptionNotImplemented;
                              success = false;
                              }
                           break;
                        case OrderType.Demand:
                           errorReason = ErrorReasonNotImplemented;
                           errorDescription = ErrorDescriptionNotImplemented;
                           success = false;
                           break;
                        }

                     if (!success)
                        {
                        this.LogWarning("Bearbeitung Queue Entry '" + messageId + "' nicht möglich, Grund: '" + errorReason + "', Beschreibung: '" + errorDescription + "'");
                        this.LQTools.LiveQueueToolsDenyLiveRequest(lockToken, errorReason, errorDescription);
                        break;
                        }
                     break;
                  case nameof(OrderCancelSyncRequest):
                     orderCancelSyncRequest = message.As<OrderCancelSyncRequest>().ConvertDateTimesToCEST();

                     Console.WriteLine("Bearbeite OrderCancelSyncRequest:");
                     Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(orderCancelSyncRequest,
                         new JsonSerializerSettings
                            {
                            Formatting = Formatting.Indented,
                            }));

                     LMSTransaction = this.LMSSqlConnection.BeginTransaction();
                     switch (orderCancelSyncRequest.Type)
                        {
                        case OrderType.Supply:
                           if (success = LiveDBTools.CancelAvailability(MyConfig, LMSTransaction, orderCancelSyncRequest, out errorReason, out errorDescription))
                              {
                              liveUpdateCounter++;
                              }
                           break;
                        case OrderType.Demand:
                           if (success = LiveDBTools.CancelDelivery(MyConfig, LMSTransaction, orderCancelSyncRequest, out errorReason, out errorDescription))
                              {
                              liveUpdateCounter++;
                              }
                           break;
                        }

                     if (!success)
                        {
                        this.LogWarning("Bearbeitung Queue Entry '" + messageId + "' nicht möglich, Grund: '" + errorReason + "', Beschreibung: '" + errorDescription + "'");
                        this.LQTools.LiveQueueToolsDenyLiveRequest(lockToken, errorReason, errorDescription);
                        break;
                        }
                     break;
                  case nameof(OrderFulfillSyncRequest):
                     orderFulfillRequest = message.As<OrderFulfillSyncRequest>().ConvertDateTimesToCEST();

                     Console.WriteLine("Bearbeite OrderFulfillSyncRequest:");
                     Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(orderFulfillRequest,
                         new JsonSerializerSettings
                            {
                            Formatting = Formatting.Indented,
                            }));

                     LMSTransaction = this.LMSSqlConnection.BeginTransaction();
                     if (success = LiveDBTools.FullfillEntries(MyConfig, LMSTransaction, orderFulfillRequest, out errorReason, out errorDescription))
                        {
                        liveUpdateCounter++;
                        }

                     if (!success)
                        {
                        this.LogWarning("Bearbeitung Queue Entry '" + messageId + "' nicht möglich, Grund: '" + errorReason + "', Beschreibung: '" + errorDescription + "'");
                        this.LQTools.LiveQueueToolsDenyLiveRequest(lockToken, errorReason, errorDescription);
                        break;
                        }
                     break;
                  case nameof(OrderRollbackFulfillmentSyncRequest):
                     orderRollbackFulfillRequest = message.As<OrderRollbackFulfillmentSyncRequest>();

                     Console.WriteLine("Bearbeite OrderRollbackFulfillmentSyncRequest:");
                     Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(orderRollbackFulfillRequest,
                         new JsonSerializerSettings
                            {
                            Formatting = Formatting.Indented,
                            }));

                     LMSTransaction = this.LMSSqlConnection.BeginTransaction();
                     if (success = LiveDBTools.RollbackFullfillEntries(MyConfig, LMSTransaction, orderRollbackFulfillRequest, out errorReason, out errorDescription))
                        {
                        liveUpdateCounter++;
                        }

                     if (!success)
                        {
                        this.LogWarning("Bearbeitung Queue Entry '" + messageId + "' nicht möglich, Grund: '" + errorReason + "', Beschreibung: '" + errorDescription + "'");
                        this.LQTools.LiveQueueToolsDenyLiveRequest(lockToken, errorReason, errorDescription);
                        break;
                        }
                     break;
                  default:
                     errorDescription = ErrorDescriptionMessageType + message.ContentType;
                     this.LogError("Bearbeitung Queue Entry '" + messageId + "' nicht möglich, Grund: '" + ErrorReasonMessageType + "', Beschreibung: '" + errorDescription + "'");
                     this.LQTools.LiveQueueToolsDenyLiveRequest(lockToken, ErrorReasonMessageType, errorDescription);
                     break;
                  }
#if !TEST_ONLY
               if (success)
                  {
                  success = this.LQTools.LiveQueueToolsConfirmLiveRequest(lockToken);
                  if (success)
                     {
                     LMSTransaction.Commit();
                     }
                  else
                     {
                     LMSTransaction.Rollback();
                     }
                  LMSTransaction = null;
                  }
               if (!success)
                  {
                  liveUpdateCounter = 0;
                  if (LMSTransaction != null)
                     {
                     LMSTransaction.Rollback();
                     LMSTransaction = null;
                     }
                  }
#else
               LMSTransaction.Rollback();
               LMSTransaction = null;
#endif
               }
            }
         catch (Exception ex)
            {
            this.LogException("ServiceMain", ex);
            if (LMSTransaction != null)
               {
               LMSTransaction.Rollback();
               liveUpdateCounter = 0;
               }
            if(lockToken != "")
               {
               errorDescription = ErrorDescriptionException + ex.Message;
               this.LogError("Bearbeitung Queue Entry '" + messageId + "' nicht möglich, Grund: '" + ErrorReasonException + "', Beschreibung: '" + errorDescription + "'");
               this.LQTools.LiveQueueToolsDenyLiveRequest(lockToken, ErrorReasonMessageType, errorDescription);
               }
            //this.ServiceErrorFlag = true; TODO
            }

         return liveUpdateCounter;
         }

      private string GetDBConnectionString(string vsServer, string vsDatabase, bool IntegratedSecurity,
          string vsUsername, string vsPassword, int vnTimeout)
         {
         if (IntegratedSecurity)
            return "Server=" + vsServer + ";Database=" + vsDatabase +
                   ";Integrated Security=True;Connection Timeout=" + vnTimeout.ToString();
         else
            return "Server=" + vsServer + ";Database=" + vsDatabase + ";UID=" + vsUsername + ";pwd=" + vsPassword +
                   ";Connection Timeout=" + vnTimeout.ToString();
         }

      private void InitDBs()
         {
         try
            {
            // LTMS
            this.LMSSqlConnection = new SqlConnection(this.GetDBConnectionString(this.MyConfig.LMSServerName,
                this.MyConfig.LMSDatabaseName, this.MyConfig.LMSUseIntegratedSecurity,
                this.MyConfig.LMSUserName, this.MyConfig.LMSPassword, this.MyConfig.LMSDatabaseTimeout));

            return;
            }
         catch (Exception Ex)
            {
            this.LogException("InitDBs", Ex);
            throw Ex;
            ;
            }
         }

      private void OpenDBs()
         {
         try
            {
            // LTMS
            if (this.LMSSqlConnection.State != ConnectionState.Open)
               this.LMSSqlConnection.Open();

            return;
            }
         catch (Exception Ex)
            {
            this.LogException("OpenDBs", Ex);
            throw Ex;
            }
         }

      private void CloseDBs()
         {
         try
            {
            // LMS
            if (this.LMSSqlConnection.State == ConnectionState.Open)
               {
               this.LMSSqlConnection.Close();
               }
            return;
            }
         catch (Exception Ex)
            {
            this.LogException("CloseDBs", Ex);
            throw Ex;
            }
         }

      private void LogException(string FunctionName, Exception Ex)
         {
         string LogMessage;
         LogMessage = FunctionName + ": Ausnahme aufgetreten, Message: '" + Ex.Message + "'";
         if (Ex.InnerException != null)
            {
            LogMessage += "\nInnere Ausnahme, Message: '" + Ex.InnerException.Message + "'";
            }
         LogMessage += "\nStacktrace: " + Ex.StackTrace;
         this.LogError(LogMessage);
         }

      public void LogInformation(string LogMessage)
         {
         //using (EventLog eventLog = new EventLog(EventLogName, ".", EventLogSource))
         //{
         //    eventLog.Source = EventLogSource;
         //    eventLog.WriteEntry(LogMessage, EventLogEntryType.Information);
         //}
         Log4NetLog.InfoFormat(LogMessage);

         Console.WriteLine("Information: " + LogMessage);
         }

      private void LogWarning(string LogMessage)
         {
         //using (EventLog eventLog = new EventLog(EventLogName, ".", EventLogSource))
         //{
         //    eventLog.Source = EventLogSource;
         //    eventLog.WriteEntry(LogMessage, EventLogEntryType.Warning);
         //}

         Log4NetLog.WarnFormat(LogMessage);

         Console.WriteLine("Warning: " + LogMessage);
         }

      private void LogError(string LogMessage)
         {
         //using (EventLog eventLog = new EventLog(EventLogName, ".", EventLogSource))
         //   {
         //   eventLog.Source = EventLogSource;
         //   eventLog.WriteEntry(LogMessage, EventLogEntryType.Error);
         //   }

         Log4NetLog.ErrorFormat(LogMessage);

         Console.WriteLine("Error: " + LogMessage);
         }

      }
   }