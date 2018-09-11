namespace App.Features.Matches
{
    using App.Infrastructure.Query.Validation;
    using App.Infrastructure.Telemetry;
    using Core.Entities;
    using Core.UnitOfWork;
    using FluentValidation;
    using Paramore.Darker;
    using System;
    using System.Collections.Generic;

    public class GetMatches
    {
        public class Query : IQuery<Response>
        {
            public Query()
            {
            }

            public Query(DateTime? dateFrom, DateTime? dateTo, string[] groups, string[] teams)
            {
                this.DateFrom = dateFrom;
                this.DateTo = dateTo;
                this.Groups = groups;
                this.Teams = teams;
            }

            public DateTime? DateFrom { get; set; }
            
            public DateTime? DateTo { get; set; }

            public string[] Groups { get; set; }

            public string[] Teams { get; set; }
        }

        public class Response
        {
            public IEnumerable<Match> Matches { get; set; } = new List<Match>();
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                this.RuleFor(x => x.DateTo).GreaterThan(x => x.DateFrom).WithMessage("DateTo should be greater than DateFrom");
            }
        }

        public class Handler : QueryHandler<Query, Response>
        {
            private readonly IUnitOfWork uow;

            public Handler(IUnitOfWork uow)
            {
                this.uow = uow;
            }
            
            [QueryValidation(1)]
            [QueryTelemetry(2)]
            public override Response Execute(Query query)
            {
                return new Response
                {
                    Matches = this.uow.Matches.All(query.DateFrom, query.DateTo, query.Groups, query.Teams)
                };
            }
        }
    }
}
