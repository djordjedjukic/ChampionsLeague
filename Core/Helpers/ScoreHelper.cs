namespace Core.Helpers
{
    using System;

    public static class ScoreHelper
    {
        public static int GetNumberOfTeamPoints(this string matchResult, bool forHomeTeam)
        {
            var result = matchResult.Split(':');
            if (result[0] != result[1])
            {
                if (forHomeTeam)
                {
                    return Convert.ToInt32(result[0]) > Convert.ToInt32(result[1]) ? 3 : 0;
                }
                else
                {
                    return Convert.ToInt32(result[0]) < Convert.ToInt32(result[1]) ? 3 : 0;
                }
            }

            return 1;
        }

        public static int GetNumberOfGoals(this string matchResult, bool homeTeam)
        {
            var result = matchResult.Split(':');

            return homeTeam ? Convert.ToInt32(result[0]) : Convert.ToInt32(result[1]);
        }
    }
}
