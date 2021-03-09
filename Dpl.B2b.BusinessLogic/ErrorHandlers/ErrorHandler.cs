using System;
using System.Collections.Generic;
using Dpl.B2b.Contracts.Localizable;
using Dpl.B2b.Contracts.Localizable.ErrorHandlers.Messages.Errors.Common;
using Dpl.B2b.Contracts.Options;
using Microsoft.Extensions.Options;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json;

namespace Dpl.B2b.BusinessLogic.ErrorHandlers
{
    public class ErrorHandler
    {
        private List<ILocalizableMessage> Messages { get; } = new List<ILocalizableMessage>();
        private readonly TelemetryClient _telemetryClient;

        private ErrorHandler()
        {
            var options = ServiceLocator.Current.GetInstance<IOptions<ApplicationInsightsOptions>>();
            var applicationInsightsOptions = options.Value;
            _telemetryClient = new TelemetryClient(new TelemetryConfiguration(applicationInsightsOptions.InstrumentationKey));
        }

        public ErrorHandler TranslateException(Exception exception)
        {
            switch (exception)
            {
                case NullReferenceException _:
                    Messages.Add(new TechnicalError());
                    break;
                case TimeoutException _:
                    Messages.Add(new TemporaryError());
                    break;
                default:
                    Messages.Add(new GeneralError());
                    break;
            }

            return this;
        }

        public ErrorHandler AddMessage(ILocalizableMessage message)
        {
            Messages.Add(message);
            return this;
        }

        public static ErrorHandler Create()
        {
            return new ErrorHandler();
        }

        public void LogException(Exception exception, object customData = null)
        {
            var customDataDictionary = new Dictionary<string, string>();
            if (customData != null)
            {
                string customDataString;
                try
                {
                    customDataString = JsonConvert.SerializeObject(customData);
                }
                catch (Exception)
                {
                    customDataString = "Object serialization failed.";
                }
                
                customDataDictionary = new Dictionary<string, string>{{"data", customDataString}};
            }
            _telemetryClient.TrackException(exception, customDataDictionary);
        }

        public ServiceState GetServiceState()
        {
            var serviceState = new ServiceState();

            if (Messages.Count == 0) Messages.Add(new GeneralError());

            foreach (var message in Messages) serviceState.StateItems.Add(message);

            return serviceState;
        }
    }
}