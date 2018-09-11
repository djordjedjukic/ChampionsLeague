namespace App.Infrastructure.Command.Validation
{
    using System;

    using Paramore.Brighter;

    public class CommandValidationAttribute : RequestHandlerAttribute
    {
        public CommandValidationAttribute(int step) : base(step)
        {
        }

        public override Type GetHandlerType()
        {
            return typeof(CommandValidationDecorator<>);
        }
    }
}
