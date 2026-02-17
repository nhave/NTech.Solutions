using NTech.Solutions.Tests.Fixtures;

namespace NTech.Solutions.Tests.UnitTests
{
    public class DatabaseTests(DatabaseFixture db) : IClassFixture<DatabaseFixture>
    {
        [Fact]
        public async Task UserRegistry()
        {

        }
    }
}
