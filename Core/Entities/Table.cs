namespace Core.Entities
{
    using System.Collections.Generic;

    public class Table : Entity
    {
        public Table(Match match)
        {
            LeagueTitle = match.LeagueTitle;
            Group = match.Group;
            Standings = new List<Standing>();
        }

        public string LeagueTitle { get; set; }

        public int Matchday { get; set; }

        public string Group { get; set; }

        public List<Standing> Standings { get; set; }
    }
}
