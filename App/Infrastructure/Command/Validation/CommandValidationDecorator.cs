namespace App.Infrastructure.Command.Validation
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;

    using Paramore.Brighter;    

    public class CommandValidationDecorator<TRequest> : RequestHandler<TRequest> where TRequest : class, IRequest
    {
        private readonly IEnumerable<IValidator> validators;

        public CommandValidationDecorator(IEnumerable<IValidator> validators)
        {
            this.validators = validators;
        }

        public override TRequest Handle(TRequest command)
        {
            var queryValidators = this.validators
                .Where(x => x.CanValidateInstancesOfType(command.GetType()));

            var failures = queryValidators
                .Select(v => v.Validate(command))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any()) throw new ValidationException(failures);

            return base.Handle(command);
        }
    }
}
