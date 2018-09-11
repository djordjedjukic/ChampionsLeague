using Core.Helpers;

namespace Core.Entities
{
    public class Standing : Entity
    {
        public Standing(string team, int points, int goals, int goalsAgainst)
        {
            Team = team;
            PlayedGames = 1;
            Points = points;
            Goals = goals;
            GoalsAgainst = goalsAgainst;
            GoalDifference = goals - goalsAgainst;
            Win = points == 3 ? 1 : 0;
            Lose = points == 0 ? 1 : 0;
            Draw = points == 1 ? 1 : 0;
        }

        public int Rank { get; set; }

        public string Team { get; set; }

        public int PlayedGames { get; set; }

        public int Points { get; set; }

        public int Goals { get; set; }

        public int GoalsAgainst { get; set; }

        public int GoalDifference { get; set; }

        public int Win { get; set; }

        public int Lose { get; set; }

        public int Draw { get; set; }

        public void UpdateStanding(Match match, bool forHomeTeam)
        {
            var points = match.Score.GetNumberOfTeamPoints(forHomeTeam);

            PlayedGames += 1;
            Points += points;
            Goals += match.Score.GetNumberOfGoals(forHomeTeam);
            GoalsAgainst += match.Score.GetNumberOfGoals(!forHomeTeam);
            GoalDifference = Goals - GoalsAgainst;
            Win += points == 3 ? 1 : 0;
            Lose += points == 0 ? 1 : 0;
            Draw += points == 1 ? 1 : 0;
        }
    }
}
