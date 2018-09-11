namespace UnitOfWork.InMemory.Repositories
{
    using System.Linq;
    using System.Collections.Generic;
    using Core.Helpers;
    using Core.Entities;
    using Core.UnitOfWork.Repositories;
    using UnitOfWork.InMemory.Infrastructure;

    public class Tables : Repository<Table>, ITables
    {
        public IEnumerable<Table> Get(string[] groups)
        {
            List<Table> result = new List<Table>();

            List<Match> matches = StorageHolder.Storage.GetStorage<Match>().All();

            if (groups.Length > 0)
            {
                matches = matches
                    .Where(x => groups.Select(s => s.ToLowerInvariant()).Contains(x.Group.ToLowerInvariant())).ToList();
            }

            foreach (var match in matches)
            {
                var existingTable = result.FirstOrDefault(x => x.Group == match.Group);

                if (existingTable == null)
                {
                    Table newTable = new Table(match);
                    newTable.Standings.Add(CreateStanding(true, match));
                    newTable.Standings.Add(CreateStanding(false, match));

                    result.Add(newTable);
                }
                else
                {
                    UpdateTableStandings(existingTable, match);
                }
            }

            return result;
        }
        
        private Standing CreateStanding(bool forHomeTeam, Match match)
        {
            return new Standing(match.HomeTeam, match.Score.GetNumberOfTeamPoints(forHomeTeam),
                match.Score.GetNumberOfGoals(forHomeTeam), match.Score.GetNumberOfGoals(!forHomeTeam));
        }

        private void UpdateTableStandings(Table table, Match match)
        {
            Standing existingHomeTeamStanding =
                table.Standings.FirstOrDefault(x => x.Team == match.HomeTeam);

            Standing existingAwayTeamStanding =
                table.Standings.FirstOrDefault(x => x.Team == match.AwayTeam);

            if (existingHomeTeamStanding == null)
            {
                table.Standings.Add(CreateStanding(true, match));
            }
            else
            {
                existingHomeTeamStanding.UpdateStanding(match, true);
            }

            if (existingAwayTeamStanding == null)
            {
                table.Standings.Add(CreateStanding(false, match));
            }
            else
            {
                existingAwayTeamStanding.UpdateStanding(match, false);
            }
        }
    }
}
