using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.BusinessLogic.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns true if two date ranges intersect.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="intersectingStartDate"></param>
        /// <param name="intersectingEndDate"></param>
        /// <returns></returns>
        public static bool Intersects(this DateTime startDate, DateTime endDate, DateTime intersectingStartDate, DateTime intersectingEndDate)
        {
            return (intersectingEndDate >= startDate && intersectingStartDate <= endDate);
        }

        /// <summary>
        /// Determines if a date is within a given date rang
        /// </summary>
        /// <param name="value"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static bool IsInRange(this DateTime value, DateTime beginDate, DateTime endDate)
        {
            return (value >= beginDate && value <= endDate);
        }
        /// <summary>
        /// Returns true if the date value is on weekend.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsWeekend(this DateTime value)
        {
            return (value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday);
        }

        public static DateTime AddWorkDay(this DateTime value)
        {
            switch (value.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    value = value.AddDays(3);
                    break;
                case DayOfWeek.Saturday:
                    value = value.AddDays(2);
                    break;
                default:
                    value = value.AddDays(1);
                    break;
            }

            return value;
        }
        public static DateTime SubstractWorkDay(this DateTime value)
        {
            switch (value.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    value = value.AddDays(-3);
                    break;
                case DayOfWeek.Sunday:
                    value = value.AddDays(-2);
                    break;
                default:
                    value = value.AddDays(-1);
                    break;
            }

            return value;
        }
    }
}
