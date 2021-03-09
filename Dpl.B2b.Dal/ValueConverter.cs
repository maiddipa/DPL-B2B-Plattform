using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Dal
{
    public static class ValueConverters
    {

        private static readonly TimeZoneInfo _timeZonCEST = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        public static ValueConverter CestToUtcConverter = new ValueConverter<DateTime, DateTime>(v => v.ToUniversalTime(), v => TimeZoneInfo.ConvertTimeToUtc(v, _timeZonCEST));
    }
}
