namespace UnitOfWork.InMemory.Tests.Repositories
{
    using UnitOfWork.InMemory;
    using UnitOfWork.Tests.Infrastructure;
    using UnitOfWork.Tests.Repositories;
    using Xunit;

    [Trait("InMemory Matches repository", "")]
    public class MatchesTests : MatchesTestsBase
    {
        public MatchesTests()
        {
            this.Fxt = new UnitOfWorkFixture(new InMemoryUnitOfWork());
        }
    }
}
