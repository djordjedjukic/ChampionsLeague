namespace App.Infrastructure.Query.Validation
{
    using System;

    using Paramore.Darker.Attributes;

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class QueryValidationAttribute : QueryHandlerAttribute
    {
        public QueryValidationAttribute(int step) : base(step)
        {
        }

        public override object[] GetAttributeParams()
        {
            return new object[0];
        }

        public override Type GetDecoratorType()
        {
            return typeof(QueryValidationDecorator<,>);
        }
    }
}
