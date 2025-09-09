using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using RepositoriesRealisation;
using RepositoriesRealisation.DatabaseObjects;
using Xunit.Abstractions;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories.Exceptions;
namespace RepositoriesTests.RepositoriesTests
{
    [Collection("sample2")]
    public class AccountRepositoryUnitTests : IntegrationTest<IAccountRepository>
    {
        public AccountRepositoryUnitTests(ITestOutputHelper helper) : base(helper)
        {
        }

        [Fact]
        public async Task CreateAccountTest()
        {
            using var context = await GetContext();
            AccountModel etalon = new AccountModel(new Account("amongus"), null!, new AdminRole());
            await Testing.CreateAccount(new Account("amongus"), "1234567", new AdminRole());

            context.AccountsReadOnly.ToList().Should().ContainSingle().Which.Should().BeEquivalentTo(etalon, options => options.Excluding(x => x.Id).Excluding(x=>x.Profile));
            await DoDumpings(nameof(CreateAccountTest));
        }

        [Fact]
        public async Task GetAccount_Erroneous_Login()
        {
            await ExecSQLFile("accounts.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.GetAccount("clongus"));
        }
        [Fact]
        public async Task GetAccount_Erroneous_Id()
        {
            await ExecSQLFile("accounts.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.GetAccount(4));
        }
        [Fact]
        public async Task GetAccount_Success_Login()
        {
            await ExecSQLFile("accounts.sql");
            Account c = await Testing.GetAccount("amongus");
            c.Should().NotBeNull().And.BeEquivalentTo(new Account("amongus", null, 1));
        }
        [Fact]
        public async Task GetAccount_Success_ID()
        {
            await ExecSQLFile("accounts.sql");
            Account c = await Testing.GetAccount(2);
            c.Should().NotBeNull().And.BeEquivalentTo(new Account("trollface", null, 2));
        }
    }
}