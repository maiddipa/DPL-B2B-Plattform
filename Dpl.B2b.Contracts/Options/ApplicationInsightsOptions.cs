using System.Collections.Generic;

namespace Dpl.B2b.Contracts.Options
{
    public class ApplicationInsightsOptions
    {
        public const string ApplicationInsights = "ApplicationInsights";
        public string InstrumentationKey { get; set; }
        public LogLevelOptions LogLevel { get; set; }
    }

    public class LogLevelOptions
    {
        public string Default { get; set; }
    }
}