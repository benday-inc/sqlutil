using Benday.SqlUtils.Api;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.WpfUi.ViewModel
{
    public class AppInsightsTelemetryService : ITelemetryService
    {
        private TelemetryClient _Client;

        public AppInsightsTelemetryService(TelemetryClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client", "Argument cannot be null.");
            }

            _Client = client;
        }

        private Dictionary<string, string> ArgsToDictionary(string[] args)
        {
            Dictionary<string, string> returnValue = new Dictionary<string, string>();

            if (args == null || args.Length == 0)
            {
                // do nothing
            }
            else if (args.Length == 1)
            {
                returnValue.Add(args[0], args[0]);
            }
            else if (args.Length == 2)
            {
                returnValue.Add(args[0], args[1]);
            }
            else if (args.Length == 3)
            {
                returnValue.Add(args[0], args[1]);
                returnValue.Add(args[2], args[2]);
            }
            else if (args.Length == 4)
            {
                returnValue.Add(args[0], args[1]);
                returnValue.Add(args[2], args[3]);
            }
            else
            {
                returnValue.Add(args[0], args[1]);
                returnValue.Add(args[2], args[3]);
                returnValue.Add(nameof(AppInsightsTelemetryService), $"{nameof(ArgsToDictionary)} skipped args when arg count was {args.Length}");
            }

            return returnValue;
        }

        public void TrackEvent(string name, params string[] args)
        {
            var properties = ArgsToDictionary(args);

            _Client.TrackEvent(name, properties);
        }

        public void TrackException(Exception ex, params string[] args)
        {
            var properties = ArgsToDictionary(args);

            _Client.TrackException(ex, properties);
        }

        public void Flush()
        {
            _Client.Flush();
        }
    }
}
