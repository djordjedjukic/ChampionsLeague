namespace App.Infrastructure.Query.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using FluentValidation;

    using Paramore.Darker;

    public class QueryValidationDecorator<TQuery, TResult> : IQueryHandlerDecorator<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly IEnumerable<IValidator> validators;

        public QueryValidationDecorator(IEnumerable<IValidator> validators)
        {
            this.validators = validators;
        }

        public IQueryContext Context { get; set; }

        public void InitializeFromAttributeParams(object[] attributeParams)
        {
            // nothing to do
        }

        public TResult Execute(TQuery query, Func<TQuery, TResult> next, Func<TQuery, TResult> fallback)
        {
            // todo : fallback is not used, study Darker and correct implementation

            var queryValidators = this.validators
                .Where(x => x.CanValidateInstancesOfType(query.GetType()));

            var failures = queryValidators
                .Select(v => v.Validate(query))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any()) throw new ValidationException(failures);

            var result = next(query);

            return result;
        }

        public async Task<TResult> ExecuteAsync(TQuery query,
            Func<TQuery, CancellationToken, Task<TResult>> next,
            Func<TQuery, CancellationToken, Task<TResult>> fallback,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
