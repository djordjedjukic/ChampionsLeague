namespace UnitOfWork.Tests.Repositories
{
    using System.Linq;

    using AutoFixture;

    using Core.Entities;

    using FluentAssertions;

    using UnitOfWork.Tests.Infrastructure;

    using Xunit;

    public abstract class MatchesTestsBase
    {
        protected UnitOfWorkFixture Fxt { private get; set; }

        [Fact(DisplayName = "Add Matches")]
        public void AddMatchesTest()
        {
            var fixture = new Fixture();
            var matches = fixture.CreateMany<Match>(10).ToList();

            this.Fxt.Uow.Matches.Add(matches);
            this.Fxt.Uow.Commit();

            this.Fxt.Uow.Matches.Any().Should().BeTrue();
        }

        [Fact(DisplayName = "All Matches")]
        public void GetAllMatchesTest()
        {
            var fixture = new Fixture();
            var matches = fixture.CreateMany<Match>(10).ToList();
            this.Fxt.Uow.Matches.Add(matches);
            this.Fxt.Uow.Commit();

            var tables = 

            this.Fxt.Uow.Matches.All(null, null, new string[] { }, new string[] { }).Count().Should().Be(10);
        }

        [Fact(DisplayName = "All Matches For Group")]
        public void GetMatchesForGroupTest()
        {
            var fixture = new Fixture();
            var matches = fixture.CreateMany<Match>(10).ToList();

            matches.Take(5).ToList().ForEach(x => x.Group = "A");

            this.Fxt.Uow.Matches.Add(matches);
            this.Fxt.Uow.Commit();

            var tables =

                this.Fxt.Uow.Matches.All(null, null, new string[] { "A" }, new string[] { }).Count().Should().Be(5);
        }
    }
}
