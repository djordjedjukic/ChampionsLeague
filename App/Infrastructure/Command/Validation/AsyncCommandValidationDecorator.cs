namespace App.Infrastructure.Command.Validation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using FluentValidation;

    using Paramore.Brighter;

    public class AsyncCommandValidationDecorator<TRequest> : RequestHandlerAsync<TRequest> where TRequest : class, IRequest
    {
        private readonly IEnumerable<IValidator> validators;

        public AsyncCommandValidationDecorator(IEnumerable<IValidator> validators)
        {
            this.validators = validators;
        }

        public override Task<TRequest> HandleAsync(TRequest command, CancellationToken cancellationToken = new CancellationToken())
        {
            var queryValidators = this.validators
                .Where(x => x.CanValidateInstancesOfType(command.GetType()));

            var failures = queryValidators
                .Select(v => v.Validate(command))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any()) throw new ValidationException(failures);

            return base.HandleAsync(command, cancellationToken);
        }
    }
}
