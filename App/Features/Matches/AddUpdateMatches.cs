namespace App.Features.Matches
{
    using System;

    using App.Infrastructure.Command.Validation;
    using App.Infrastructure.Telemetry;

    using Core.Entities;
    using Core.UnitOfWork;

    using FluentValidation;
        
    using Paramore.Brighter;
    using System.Collections.Generic;

    public class AddUpdateMatches
    {
        public class Command : ICommand
        {
            public Command()
            {

            }

            public Command(List<Match> matches)
            {
                this.Id = Guid.NewGuid();
                this.Matches = matches;
            }

            public Guid Id { get; set; }

            public List<Match> Matches { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                this.RuleForEach(x => x.Matches).SetValidator(new MatchValidator());
            }
        }

        public class Handler : RequestHandler<Command>
        {
            private readonly IUnitOfWork uow;

            public Handler(IUnitOfWork uow)
            {
                this.uow = uow;
            }

            [CommandValidation(1)]
            [CommandTelemetry(2)]
            public override Command Handle(Command command)
            {
                using (this.uow)
                {
                    foreach (var match in command.Matches)
                    {
                        Match existingMatch = this.uow.Matches.Find(match);

                        if (existingMatch != null)
                        {
                            this.uow.Matches.Update(existingMatch);
                        }
                        else
                        {
                            this.uow.Matches.Add(match);
                        }
                    }

                    this.uow.Commit();
                }

                return base.Handle(command);
            }
        }
    }
}
