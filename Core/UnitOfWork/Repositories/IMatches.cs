namespace Core.UnitOfWork.Repositories
{
    using System;
    using System.Collections.Generic;
    using Core.Entities;

    public interface IMatches : IRepository<Match>
    {
        Match Find(Match match);

        Match FindLatestMatch(string group);

        IEnumerable<Match> All(DateTime? dateFrom, DateTime? dateTo, string[] groups, string[] teams);
    }
}
