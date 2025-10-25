using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Auth;
using FluentAssertions;
using RepositoriesRealisation.Models;

namespace IntegrationalTests
{
    public class CompetitionLeaderboardTexts(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private readonly int competitionID;
        [Fact]
        public async Task GetLeaderboard()
        {
            HttpResponseMessage result = await Client.GetAsync($"/api/v1/competitions/{competitionID}/participations/");

            result.IsSuccessStatusCode.Should().BeTrue();
            PlayerParticipationDTO[] leaderboard = await result.FromJSONAsync<PlayerParticipationDTO[]>();
            leaderboard.Should().BeInDescendingOrder(x => x.Score);
        }

        protected override async Task Init()
        {
            CompetitionModel t = await Instantiate(new CompetitionModel("A", "B", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)));
            await Context.SaveChangesAsync();
            AccountModel x = await Instantiate(new AccountModel(
                new CompetitiveBackend.Core.Objects.Account(Faker.Internet.UserName()),
                "PASSWORDHASH",
                new PlayerRole()));
            AccountModel y = await Instantiate(new AccountModel(
                new CompetitiveBackend.Core.Objects.Account(Faker.Internet.UserName()),
                "PASSWORDHASH",
                new PlayerRole()));
            await Context.SaveChangesAsync();
            await Instantiate(new PlayerParticipationModel(t.Id, x.Id, 200, DateTime.UtcNow));
            await Instantiate(new PlayerParticipationModel(t.Id, y.Id, 300, DateTime.UtcNow));
            await Context.SaveChangesAsync();
        }
    }
}
