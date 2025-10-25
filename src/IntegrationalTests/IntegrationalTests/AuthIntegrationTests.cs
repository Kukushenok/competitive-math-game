using System.Net.Http.Json;
using Allure.Net.Commons;
using Allure.Xunit.Attributes.Steps;
using Bogus;
using CompetitiveBackend.BackendUsage.Objects;
using FluentAssertions;
using RepositoriesRealisation;

namespace IntegrationalTests
{
    public class SetupAuth(HttpClient client, BaseDbContext context, string name) : IAsyncDisposable
    {
        private const string USERPASSWORD = "123456789A123456789";
        private bool registered;
        public async ValueTask DisposeAsync()
        {
            if (!registered)
            {
                return;
            }

            await AllureApi.Step("Remove account", async () =>
            {
                var acc = context.AccountsReadOnly.Where(x => x.Login == name).ToList();
                context.AccountsReadOnly.RemoveRange(acc);
                await context.SaveChangesAsync();
                registered = false;
            });
        }

        public async Task<AuthSuccessResultDTO> RegisterAccount()
        {
            return await AllureApi.Step("Register account", async () =>
            {
                HttpResponseMessage regResult = await client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(name, USERPASSWORD, null));
                regResult.IsSuccessStatusCode.Should().BeTrue();
                registered = true;
                return await regResult.FromJSONAsync<AuthSuccessResultDTO>();
            });
        }

        public async Task<AuthSuccessResultDTO> RegisterAndLogIn()
        {
            AuthSuccessResultDTO x = await RegisterAccount();
            client.DefaultRequestHeaders.Add("Bearer", x.Token);
            return x;
        }
    }

    public class AuthIntegrationTests(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private const string USERPASSWORD = "123456789A123456789";
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
            HttpResponseMessage regResult = await Client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(name, USERPASSWORD, null));
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
        [InlineData("_")]
        public async Task CreateAccountBadNickname(string name)
        {
            // Act
            HttpResponseMessage regResult = await Client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(name, USERPASSWORD, null));

            // Assert
            regResult.IsSuccessStatusCode.Should().BeFalse();
            var acc = Context.AccountsReadOnly.Where(x => x.Login == name).ToList();
            acc.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData("123456")]
        [InlineData("abcd")]
        public async Task CreateAccountBadPassword(string password)
        {
            // Arrange
            string name = Faker.Internet.UserName();

            // Act
            HttpResponseMessage regResult = await Client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(name, password, null));

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
            HttpResponseMessage logResult = await Client.PostAsJsonAsync("/api/v1/auth/login", new AccountLoginDTO(name, USERPASSWORD));

            // Assert
            AuthSuccessResultDTO res = await logResult.FromJSONAsync<AuthSuccessResultDTO>();
            int id = res.AccountID;
            var acc = Context.AccountsReadOnly.Where(x => x.Id == id).ToList();
            acc.Should().ContainSingle().Which.Login.Should().Be(name);
            await RemoveAccount(name);
        }
    }
}
