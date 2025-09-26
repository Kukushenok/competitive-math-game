using Allure.Net.Commons;
using Allure.Xunit.Attributes.Steps;
using Bogus;
using CompetitiveBackend.BackendUsage.Objects;
using FluentAssertions;
using RepositoriesRealisation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationalTests
{
    public class SetupAuth(HttpClient Client, BaseDbContext Context, string name) : IAsyncDisposable
    {
        private const string USER_PASSWORD = "123456789A123456789";
        private bool registered = false;
        public async ValueTask DisposeAsync()
        {
            if (!registered) return;
            await AllureApi.Step("Remove account", async () =>
            {
                var acc = Context.AccountsReadOnly.Where(x => x.Login == name).ToList();
                Context.AccountsReadOnly.RemoveRange(acc);
                await Context.SaveChangesAsync();
                registered = false;
            });
        }

        public async Task<AuthSuccessResultDTO> RegisterAccount()
        {
            return await AllureApi.Step("Register account", async () =>
            {
                var regResult = await Client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(name, USER_PASSWORD, null));
                regResult.IsSuccessStatusCode.Should().BeTrue();
                registered = true;
                return await regResult.FromJSONAsync<AuthSuccessResultDTO>();
            });
        }
        public async Task<AuthSuccessResultDTO> RegisterAndLogIn()
        {
            var x = await RegisterAccount();
            Client.DefaultRequestHeaders.Add("Bearer", x.Token);
            return x;
        }
    }
    public class AuthIntegrationTests(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private const string USER_PASSWORD = "123456789A123456789";
        [Fact]
        public async Task CreateAccount()
        {
            string name = Faker.Internet.UserName();
            await RegisterAccount(name);
            var acc = Context.AccountsReadOnly.Where(x => x.Login == name).ToList();
            acc.Should().ContainSingle().Which.Login.Should().Be(name);
            await RemoveAccount(name);
        }
        [AllureStep]
        private async Task<AuthSuccessResultDTO> RegisterAccount(string name)
        {
            var regResult = await Client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(name, USER_PASSWORD, null));
            regResult.IsSuccessStatusCode.Should().BeTrue();
            return await regResult.FromJSONAsync<AuthSuccessResultDTO>();
        }
        [AllureStep]
        private async Task RemoveAccount(string name)
        {
            var acc = Context.AccountsReadOnly.Where(x => x.Login == name).ToList();
            Context.AccountsReadOnly.RemoveRange(acc);
            await Context.SaveChangesAsync();
        }
        [Theory]
        [InlineData("")]
        [InlineData("thats my login!")]
        [InlineData("****")]
        public async Task CreateAccount_BadNickname(string name)
        {
            // Act
            var regResult = await Client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(name, USER_PASSWORD, null));
            // Assert
            regResult.IsSuccessStatusCode.Should().BeFalse();
            var acc = Context.AccountsReadOnly.Where(x => x.Login == name).ToList();
            acc.Should().BeEmpty();
        }
        [Theory]
        [InlineData("")]
        [InlineData("123456")]
        [InlineData("abcd")]
        public async Task CreateAccount_BadPassword(string password)
        {
            // Arrange
            string name = Faker.Internet.UserName();
            // Act
            var regResult = await Client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(name, password, null));
            // Assert
            regResult.IsSuccessStatusCode.Should().BeFalse();
            var acc = Context.AccountsReadOnly.Where(x => x.Login == name).ToList();
            acc.Should().BeEmpty();
        }
        [Fact]
        public async Task LogIn()
        {
            // Arrange
            string name = Faker.Internet.UserName();
            await RegisterAccount(name);
            // Act
            var logResult = await Client.PostAsJsonAsync("/api/v1/auth/login", new AccountLoginDTO(name, USER_PASSWORD));
            // Assert
            var res = await logResult.FromJSONAsync<AuthSuccessResultDTO>();
            var id = res.AccountID;
            var acc = Context.AccountsReadOnly.Where(x => x.Id == id).ToList();
            acc.Should().ContainSingle().Which.Login.Should().Be(name);
            await RemoveAccount(name);
        }
    }
}
