namespace App.Infrastructure.Command.Validation
{
    using System;

    using Paramore.Brighter;

    public class AsyncCommandValidationAttribute : RequestHandlerAttribute
    {
        public AsyncCommandValidationAttribute(int step) : base(step)
        {
        }

        public override Type GetHandlerType()
        {
            return typeof(AsyncCommandValidationDecorator<>);
        }
    }
}
