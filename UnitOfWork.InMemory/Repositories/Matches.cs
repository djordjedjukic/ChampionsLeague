namespace UnitOfWork.InMemory.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Entities;
    using Core.UnitOfWork.Repositories;
    using UnitOfWork.InMemory.Infrastructure;

    public class Matches : Repository<Match>, IMatches
    {
        public IEnumerable<Match> All(DateTime? dateFrom, DateTime? dateTo, string[] groups, string[] teams)
        {
            List<Match> matches = StorageHolder.Storage.GetStorage<Match>().All();

            if (dateFrom.HasValue)
            {
                matches = matches.Where(x => x.KickOffAt > dateFrom).ToList();
            }
            if (dateTo.HasValue)
            {
                matches = matches.Where(x => x.KickOffAt < dateTo).ToList();
            }
            if (groups?.Length > 0)
            {
                matches = matches.Where(x => groups.Select(s => s.ToLowerInvariant()).Contains(x.Group.ToLowerInvariant())).ToList();
            }
            if (teams?.Length > 0)
            {
                matches = matches.Where(x =>
                    teams.Select(s => s.ToLowerInvariant()).Contains(x.HomeTeam.ToLowerInvariant()) ||
                    teams.Select(s => s.ToLowerInvariant()).Contains(x.AwayTeam.ToLowerInvariant())).ToList();
            }

            return matches;
        }

        public Match Find(Match match)
        {
            return StorageHolder.Storage.GetStorage<Match>().All(x =>
                x.LeagueTitle == match.LeagueTitle && x.HomeTeam == match.HomeTeam &&
                x.AwayTeam == match.AwayTeam).FirstOrDefault();
        }

        public Match FindLatestMatch(string group)
        {
            return StorageHolder.Storage.GetStorage<Match>().All(x =>
                x.Group == group).OrderByDescending(x => x.Matchday).FirstOrDefault();
        }
    }
}
