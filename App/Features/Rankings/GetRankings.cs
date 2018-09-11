namespace App.Features.Rankings
{
    using App.Infrastructure.Telemetry;
    using Core.Entities;
    using Core.UnitOfWork;
    using Paramore.Darker;
    using System.Collections.Generic;
    using System.Linq;

    public class GetRankings
    {
        public class Query : IQuery<Response>
        {
            public Query()
            {
                this.Groups = new string[] { };
            }

            public Query(string[] groups)
            {
                this.Groups = groups;
            }

            public string[] Groups { get; set; }
        }

        public class Response
        {
            public IEnumerable<Table> Tables { get; set; } = new List<Table>();
        }

        public class Handler : QueryHandler<Query, Response>
        {
            private readonly IUnitOfWork uow;

            public Handler(IUnitOfWork uow)
            {
                this.uow = uow;
            }
            
            [QueryTelemetry(1)]
            public override Response Execute(Query query)
            {
                var response = new Response
                {
                    Tables = this.uow.Tables.Get(query.Groups)
                };

                foreach (var table in response.Tables)
                {
                    table.Matchday = this.uow.Matches.FindLatestMatch(table.Group).Matchday;

                    table.Standings = table.Standings
                        .OrderByDescending(y => y.Points)
                        .ThenByDescending(y => y.Goals)
                        .ThenByDescending(y => y.GoalDifference).ToList();

                    for (int i = 0; i < table.Standings.Count; i++)
                    {
                        table.Standings[i].Rank = i + 1;
                    }
                }

                return response;
            }
        }
    }
}
