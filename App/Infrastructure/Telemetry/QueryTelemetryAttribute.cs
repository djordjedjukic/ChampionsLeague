namespace App.Infrastructure.Telemetry
{
    using System;

    using Paramore.Darker.Attributes;

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class QueryTelemetryAttribute : QueryHandlerAttribute
    {
        public QueryTelemetryAttribute(int step) : base(step)
        {
        }

        public override object[] GetAttributeParams()
        {
            return new object[0];
        }

        public override Type GetDecoratorType()
        {
            return typeof(QueryTelemetryDecorator<,>);
        }
    }
}
