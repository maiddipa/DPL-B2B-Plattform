using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Dpl.B2b.Contracts.Models;
using Dpl.B2b.Dal.Ltms;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Olma = Dpl.B2b.Dal.Models;

namespace Dpl.B2b.BusinessLogic.Extensions
{
    public static class UtilitiesExtensions
    {
        public static PostingAccountConditionType GetConditionType(this string termId)
        {
            switch (termId)
            {
                case "ANLALA": return PostingAccountConditionType.Pickup;
                case "DEPAB": return PostingAccountConditionType.DropOff;
                case "ANLFHA": return PostingAccountConditionType.Demand;
                case "FSD":
                case "FSI":
                case "FREIST": return PostingAccountConditionType.Supply;
                case "TRAP": return PostingAccountConditionType.Transfer;
                case "PGAUS": return PostingAccountConditionType.Voucher;
                default: return PostingAccountConditionType.Undefined;
            }
        }

        public static bool IsCorrespondingTo(this bool value, bool valueToCorrespond)
        {
            return value && valueToCorrespond;
        }

        public static List<Message> AsMessages<T>(this List<T> requests) where T : class
        {
            if (requests == null) return new List<Message>();

            var messages = new List<Message>();
            foreach (var request in requests)
            {
                var messageBody = JsonConvert.SerializeObject(request);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody))
                {
                    ContentType = typeof(T).Name
                };
                messages.Add(message);
            }

            return messages;
        }

        public static Message AsMessage<T>(this List<T> request) where T : class
        {
            var messageBody = JsonConvert.SerializeObject(request);
            return new Message(Encoding.UTF8.GetBytes(messageBody))
            {
                ContentType = typeof(T).Name
            };
        }
        
        public static T As<T>(this Message message) where T : class
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
        }

        public static string ConvertToString(this ICollection<Olma.BusinessHour> businessHours)
        {
            var businessHoursString = string.Empty;
            var culture = new CultureInfo("de-DE");
            var businessHoursGroupedByDayOfWeek =
                businessHours.GroupBy(bh => bh.DayOfWeek).OrderBy(bhg => bhg.Key).ToList();
            foreach (var businessHourGroup in businessHoursGroupedByDayOfWeek)
            {
                businessHoursString += $"{culture.DateTimeFormat.GetDayName(businessHourGroup.Key)} ";

                foreach (var businessHour in businessHourGroup)
                {
                    businessHoursString +=
                        $"{businessHour.FromTime.ToShortTimeString()} - {businessHour.ToTime.ToShortTimeString()}";

                    if (businessHour.Id != businessHourGroup.Last().Id) businessHoursString += ", ";
                }

                if (businessHourGroup.Key != businessHoursGroupedByDayOfWeek.Last().Key) businessHoursString += " / ";
            }

            return businessHoursString;
        }

        /// <summary>
        ///     Transaction must be loaded before you call GetStatus
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public static AccountingRecordStatus GetStatus(this Bookings booking)
        {
            if (booking.Transaction.CancellationId != null) return AccountingRecordStatus.Canceled;

            if (booking.Matched) return AccountingRecordStatus.Coordinated;

            if (booking.IncludeInBalance && !booking.Matched &&
                booking.ReportBookings.Any(rb => rb.Report.DeleteTime == null && rb.Report.ReportStateId == "U"))
                return AccountingRecordStatus.InCoordination;

            if (booking.IncludeInBalance) return AccountingRecordStatus.Uncoordinated;

            return AccountingRecordStatus.Provisional;
        }
    }
}