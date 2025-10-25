using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using FluentAssertions;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests
{
    public class SessionRepositoryUnitTests : IntegrationTest<ISessionRepository>
    {
        public SessionRepositoryUnitTests(ITestOutputHelper helper)
            : base(helper)
        {
        }

        [Fact]
        public async Task CreateSessionTokenForAdminSuccess()
        {
            await ExecSQLFile("accounts.sql");
            string session = await testing.CreateSessionFor(1);
            SessionToken token = await testing.GetSessionToken(session);
            token.Role.IsAdmin().Should().BeTrue();
            token.Role.IsPlayer().Should().BeFalse();
            token.TryGetAccountIdentifier(out int id).Should().BeTrue();
            id.Should().Be(1);
        }

        [Fact]
        public async Task CreateSessionTokenForPlayerSuccess()
        {
            await ExecSQLFile("accounts.sql");
            string session = await testing.CreateSessionFor(2);
            SessionToken token = await testing.GetSessionToken(session);
            token.Role.IsAdmin().Should().BeFalse();
            token.Role.IsPlayer().Should().BeTrue();
            token.TryGetAccountIdentifier(out int id).Should().BeTrue();
            id.Should().Be(2);
        }

        [Fact]
        public async Task CreateSessionTokenForFailure()
        {
            await ExecSQLFile("accounts.sql");
            await (async () => await testing.CreateSessionFor(6)).Should().ThrowExactlyAsync<MissingDataException>();
        }

        [Fact]
        public async Task CreateSessionTokenUnknown()
        {
            await ExecSQLFile("accounts.sql");
            SessionToken token = await testing.GetSessionToken("unknown");
            token.Role.IsPlayer().Should().BeFalse();
            token.Role.IsAdmin().Should().BeFalse();
            token.TryGetAccountIdentifier(out int _).Should().BeFalse();
        }

        // [Fact]
        // public async Task Dump()
        // {
        //    await Task.Delay(1000);
        //    await DoDumpings("beloved_dump");
        // }
    }
}
