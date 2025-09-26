using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Auth;
using FluentAssertions;
using RepositoriesRealisation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationalTests
{
    public class CompetitionLeaderboardTexts(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private int competitionID;
        [Fact]
        public async Task GetLeaderboard()
        {
            var result = await Client.GetAsync($"/api/v1/competitions/{competitionID}/participations/");

            result.IsSuccessStatusCode.Should().BeTrue();
            var leaderboard = await result.FromJSONAsync<PlayerParticipationDTO[]>();
            leaderboard.Should().BeInDescendingOrder(x => x.Score);
        }
        protected override async Task Init()
        {
            var T = await Instantiate(new CompetitionModel("A", "B", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)));
            await Context.SaveChangesAsync();
            var X = await Instantiate(new AccountModel(
                new CompetitiveBackend.Core.Objects.Account(Faker.Internet.UserName()),
                "PASSWORDHASH", new PlayerRole()
                ));
            var Y = await Instantiate(new AccountModel(
                new CompetitiveBackend.Core.Objects.Account(Faker.Internet.UserName()),
                "PASSWORDHASH", new PlayerRole()
                ));
            await Context.SaveChangesAsync();
            await Instantiate(new PlayerParticipationModel(T.Id, X.Id, 200, DateTime.UtcNow));
            await Instantiate(new PlayerParticipationModel(T.Id, Y.Id, 300, DateTime.UtcNow));
            await Context.SaveChangesAsync();
        }
    }
}
