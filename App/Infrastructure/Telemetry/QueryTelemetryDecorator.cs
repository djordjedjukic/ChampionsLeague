namespace App.Infrastructure.Telemetry
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    using Newtonsoft.Json;

    using Paramore.Darker;

    public class QueryTelemetryDecorator<TQuery, TResult> : IQueryHandlerDecorator<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        public IQueryContext Context { get; set; }

        public void InitializeFromAttributeParams(object[] attributeParams)
        {
            // nothing to do
        }

        public TResult Execute(TQuery query, Func<TQuery, TResult> next, Func<TQuery, TResult> fallback)
        {
            var start = Stopwatch.GetTimestamp();
            var result = next(query);
            var stop = Stopwatch.GetTimestamp();

            TelemetryClient telemetry = new TelemetryClient();
            telemetry.TrackTrace(
                "Darker",
                SeverityLevel.Information,
                this.Serialize(query, result, this.GetElapsedMilliseconds(start, stop)));

            return result;
        }

        public Task<TResult> ExecuteAsync(TQuery query, Func<TQuery, CancellationToken, Task<TResult>> next, Func<TQuery, CancellationToken, Task<TResult>> fallback, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, string> Serialize(TQuery query, TResult result, double elapsedMs)
        {
            var serializationSettings =
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            serializationSettings.Converters.Add(new ClaimsPrincipalConverter());

            return new Dictionary<string, string>
                       {
                           { "Elapsed", $"{elapsedMs:0.0000}" },
                           { "Type", query.GetType().FullName },
                           { "Query", JsonConvert.SerializeObject(query, serializationSettings) },
                           { "Result", JsonConvert.SerializeObject(result, serializationSettings) }
                       };
        }

        private double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }


    }
}
