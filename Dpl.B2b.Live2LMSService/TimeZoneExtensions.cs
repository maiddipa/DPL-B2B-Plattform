using Dpl.B2b.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Live2LMSService
{
    public static class TimeZoneExtensions
    {
        private static readonly TimeZoneInfo _timeZonCEST = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        public static OrderCreateSyncRequest ConvertDateTimesToCEST(this OrderCreateSyncRequest request)
        {
            request.EarliestFulfillmentDateTime = GetCEST(request.EarliestFulfillmentDateTime);
            request.LatestFulfillmentDateTime = GetCEST(request.LatestFulfillmentDateTime);

            return request;
        }

      public static OrderUpdateSyncRequest ConvertDateTimesToCEST(this OrderUpdateSyncRequest request)
         {
         request.EarliestFulfillmentDateTime = GetCEST(request.EarliestFulfillmentDateTime);
         request.LatestFulfillmentDateTime = GetCEST(request.LatestFulfillmentDateTime);

         return request;
         }

      public static OrderCancelSyncRequest ConvertDateTimesToCEST(this OrderCancelSyncRequest request)
        {
            return request;
        }

      public static OrderFulfillSyncRequest ConvertDateTimesToCEST(this OrderFulfillSyncRequest request)
         {
         request.FulfillmentDateTime = GetCEST(request.FulfillmentDateTime);

         return request;
         }

      private static DateTime GetCEST(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime.ToUniversalTime(), _timeZonCEST);
        }
    }
}
