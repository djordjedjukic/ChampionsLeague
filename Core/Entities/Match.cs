namespace Core.Entities
{
    using FluentValidation;
    using System;

    public class Match : Entity
    {
        public string LeagueTitle { get; set; }

        public int Matchday { get; set; }

        public string Group { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public DateTime KickOffAt { get; set; }

        public string Score { get; set; }
    }

    public class MatchValidator : AbstractValidator<Match>
    {
        public MatchValidator()
        {
            this.RuleFor(x => x.LeagueTitle).NotEmpty();
            this.RuleFor(x => x.Matchday).NotEmpty().GreaterThan(0);
            this.RuleFor(x => x.Group).NotEmpty().Length(1);
            this.RuleFor(x => x.HomeTeam).NotEmpty();
            this.RuleFor(x => x.AwayTeam).NotEmpty();
            this.RuleFor(x => x.KickOffAt).NotEmpty();
        }
    }
}
