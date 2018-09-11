namespace App.Infrastructure.Telemetry
{
    using System;

    using Paramore.Brighter;

    public class CommandTelemetryAttribute : RequestHandlerAttribute
    {
        public CommandTelemetryAttribute(int step) : base(step)
        {
        }

        public override Type GetHandlerType()
        {
            return typeof(CommandTelemetryDecorator<>);
        }
    }
}
