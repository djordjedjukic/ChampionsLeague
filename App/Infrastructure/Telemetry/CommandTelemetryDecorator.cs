namespace App.Infrastructure.Telemetry
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    using Newtonsoft.Json;

    using Paramore.Brighter;

    public class CommandTelemetryDecorator<TRequest> : RequestHandler<TRequest> where TRequest : class, IRequest
    {
        public override TRequest Handle(TRequest command)
        {
            var start = Stopwatch.GetTimestamp();
            TRequest response = base.Handle(command);
            var stop = Stopwatch.GetTimestamp();

            TelemetryClient telemetry = new TelemetryClient();
            telemetry.TrackTrace(
                "Brighter",
                SeverityLevel.Information,
                this.Serialize(command, this.GetElapsedMilliseconds(start, stop)));

            return response;
        }

        private Dictionary<string, string> Serialize(TRequest request, double elapsedMs)
        {
            var serializationSettings =
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            serializationSettings.Converters.Add(new ClaimsPrincipalConverter());

            return new Dictionary<string, string>
                       {
                           { "Elapsed", $"{elapsedMs:0.0000}" },
                           { "Type", request.GetType().FullName },
                           { "Request", JsonConvert.SerializeObject(request, serializationSettings) },
                       };
        }

        private double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }
    }
}
