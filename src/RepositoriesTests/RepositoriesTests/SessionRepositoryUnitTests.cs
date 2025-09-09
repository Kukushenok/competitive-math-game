using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using FluentAssertions;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests
{
    public class SessionRepositoryUnitTests : IntegrationTest<ISessionRepository>
    {
        public SessionRepositoryUnitTests(ITestOutputHelper helper) : base(helper)
        {
            
        }
        [Fact]
        public async Task CreateSessionTokenFor_AdminSuccess()
        {
            await ExecSQLFile("accounts.sql");
            string session = await Testing.CreateSessionFor(1);
            SessionToken token = await Testing.GetSessionToken(session);
            token.Role.IsAdmin().Should().BeTrue();
            token.Role.IsPlayer().Should().BeFalse();
            token.TryGetAccountIdentifier(out int id).Should().BeTrue();
            id.Should().Be(1);
        }
        [Fact]
        public async Task CreateSessionTokenFor_PlayerSuccess()
        {
            await ExecSQLFile("accounts.sql");
            string session = await Testing.CreateSessionFor(2);
            SessionToken token = await Testing.GetSessionToken(session);
            token.Role.IsAdmin().Should().BeFalse();
            token.Role.IsPlayer().Should().BeTrue();
            token.TryGetAccountIdentifier(out int id).Should().BeTrue();
            id.Should().Be(2);
        }
        [Fact]
        public async Task CreateSessionTokenFor_Failure()
        {
            await ExecSQLFile("accounts.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.CreateSessionFor(6));
        }
        [Fact]
        public async Task CreateSessionToken_Unknown()
        {
            await ExecSQLFile("accounts.sql");
            SessionToken token = await Testing.GetSessionToken("unknown");
            token.Role.IsPlayer().Should().BeFalse();
            token.Role.IsAdmin().Should().BeFalse();
            token.TryGetAccountIdentifier(out int _).Should().BeFalse();
        }

        //[Fact]
        //public async Task Dump()
        //{
        //    await Task.Delay(1000);
        //    await DoDumpings("beloved_dump");
        //}

    }
}
