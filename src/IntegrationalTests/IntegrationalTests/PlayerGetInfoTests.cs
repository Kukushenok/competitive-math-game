using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Auth;
using FluentAssertions;
using RepositoriesRealisation.Models;

namespace IntegrationalTests
{
    public class PlayerGetInfoTests(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private AccountModel? tracking;
        [Fact]
        public async Task GetAccountInfo()
        {
            HttpResponseMessage r = await Client.GetAsync($"/api/v1/players/{tracking.Id}");

            r.IsSuccessStatusCode.Should().BeTrue();
            PlayerProfileDTO result = await r.FromJSONAsync<PlayerProfileDTO>();
            result.Name.Should().BeEquivalentTo(tracking.Login);
        }

        [Fact]
        public async Task GetAccountInfoFailure()
        {
            HttpResponseMessage r = await Client.GetAsync($"/api/v1/players/{tracking.Id + 10}"); // this is scary imo

            r.IsSuccessStatusCode.Should().BeFalse();
        }

        protected override async Task Init()
        {
            tracking = await Instantiate(new AccountModel(
                new CompetitiveBackend.Core.Objects.Account(Faker.Internet.UserName()),
                "PASSWORDHASH", new PlayerRole()));
        }
    }
}
