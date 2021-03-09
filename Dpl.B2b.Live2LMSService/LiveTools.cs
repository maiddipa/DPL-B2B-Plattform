using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dpl.B2b.Contracts.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Primitives;
using Newtonsoft.Json;

namespace Live2LMSService
   {
   public class LiveQueueTools
      {
      private string mQueueName = "";
      private string mConnectionString = "";

      //public QueueClient LiveRequestQueueClient { get; set; }
      public MessageReceiver LiveMessageReceiver { get; set; }

      // Endpoint=sb://dpllivesyncdvit.servicebus.windows.net/;SharedAccessKeyName=Manager;SharedAccessKey=BcqZ0kNRCUJx2I0BqeXZwVpare7XjHb38/fQMACPujs=;
      public bool LiveQueueToolsInit(string QueueName, string ConnectionString)
         {
         this.mQueueName = QueueName;
         this.mConnectionString = ConnectionString;

         //var connectionStringBuilder = new ServiceBusConnectionStringBuilder(this.mEndpoint, this.mQueueName, this.mSasKeyName, this.mSasKey);
         LiveMessageReceiver = new MessageReceiver(this.mConnectionString, this.mQueueName, ReceiveMode.PeekLock, RetryPolicy.Default, 1);

         return true;
         }

      public bool LiveQueueToolsExit()
         {
         this.LiveMessageReceiver?.CloseAsync().GetAwaiter().GetResult();
         this.LiveMessageReceiver = null;

         return true;
         }

      // https://github.com/Azure/azure-service-bus/blob/master/samples/DotNet/GettingStarted/BasicSendReceiveTutorialwithFilters/BasicSendReceiveTutorialWithFilters/SendReceive.cs      
      public Tuple<String, String, Message> LiveQueueToolsGetLiveRequest()
         {
         Tuple<String, String, Message> returnTuple = null;

         try
            {
            IList<Message> messages = this.LiveMessageReceiver.ReceiveAsync(1, TimeSpan.FromSeconds(2)).GetAwaiter().GetResult();
            // Note the extension class which is serializing an deserializing messages and testing messages is null or 0.
            if (messages.Any())
               {
               foreach (var message in messages) // Should be only one here
                  {
                  //IDictionary<string, object> myUserProperties = message.UserProperties;
                  //Console.WriteLine($"StoreId={myUserProperties["StoreId"]}");

                  returnTuple = new Tuple<String, String, Message>(message.MessageId, message.SystemProperties.LockToken, message);
                  return returnTuple;
                  }
               }
            else
               {
               return null;
               }
            }
         catch (Exception ex)
            {
            Console.WriteLine(ex.ToString());
            throw ex;
            }

         return returnTuple;
         }

      public bool LiveQueueToolsConfirmLiveRequest(string lockToken)
         {
         this.LiveMessageReceiver.CompleteAsync(lockToken).GetAwaiter().GetResult();

         return true;
         }

      public bool LiveQueueToolsDenyLiveRequest(string lockToken, string reason, string description)
         {
         this.LiveMessageReceiver.DeadLetterAsync(lockToken, reason, description).GetAwaiter().GetResult();

         return true;
         }
      }
   public static class Extensions
      {
      public static T As<T>(this Message message) where T : class
         {
         return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
         }
      public static Message AsMessage(this object obj)
         {
         return new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj)));
         }

      public static bool Any(this IList<Message> collection)
         {
         return collection != null && collection.Count > 0;
         }
      }

   }
