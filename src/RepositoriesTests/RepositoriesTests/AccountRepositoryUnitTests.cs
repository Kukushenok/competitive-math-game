using AwesomeAssertions;
using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using RepositoriesRealisation;
using RepositoriesRealisation.Models;
using Xunit.Abstractions;
namespace RepositoriesTests.RepositoriesTests
{
    [Collection("sample2")]
    public class AccountRepositoryUnitTests : IntegrationTest<IAccountRepository>
    {
        public AccountRepositoryUnitTests(ITestOutputHelper helper)
            : base(helper)
        {
        }

        [Fact]
        public async Task CreateAccountTest()
        {
            using BaseDbContext context = await GetContext();
            var etalon = new AccountModel(new Account("amongus"), null!, new AdminRole());
            await testing.CreateAccount(new Account("amongus"), "1234567", new AdminRole());

            context.AccountsReadOnly.ToList().Should().ContainSingle().Which.Should().BeEquivalentTo(etalon, options => options.Excluding(x => x.Id).Excluding(x => x.Profile));
            await DoDumpings(nameof(CreateAccountTest));
        }

        [Fact]
        public async Task GetAccountErroneousLogin()
        {
            await ExecSQLFile("accounts.sql");
            await Assert.ThrowsAnyAsync<RepositoryException>(async () => await testing.GetAccount("clongus"));
        }

        [Fact]
        public async Task GetAccountErroneousId()
        {
            await ExecSQLFile("accounts.sql");
            await Assert.ThrowsAnyAsync<MissingDataException>(async () => await testing.GetAccount(4));
        }

        [Fact]
        public async Task GetAccountSuccessLogin()
        {
            await ExecSQLFile("accounts.sql");
            Account c = await testing.GetAccount("amongus");
            c.Should().NotBeNull().And.BeEquivalentTo(new Account("amongus", null, 1));
        }

        [Fact]
        public async Task GetAccountSuccessID()
        {
            await ExecSQLFile("accounts.sql");
            Account c = await testing.GetAccount(2);
            c.Should().NotBeNull().And.BeEquivalentTo(new Account("trollface", null, 2));
        }
    }
}