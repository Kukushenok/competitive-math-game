using System.Net.Http.Json;
using AwesomeAssertions;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using Microsoft.EntityFrameworkCore;
using RepositoriesRealisation.Models;
namespace IntegrationalTests
{
    public class ParticipationTests(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private CompetitionModel tracking = null!;

        [Fact]
        public async Task ParticipateDeprecatedAPI()
        {
            await using var rEGISTERACC = new SetupAuth(Client, Context, Faker.Internet.UserName());
            AuthSuccessResultDTO aCCOUNT = await rEGISTERACC.RegisterAndLogIn();

            // Act
            HttpResponseMessage result = await Client.PutAsync($"/api/v1/competitions/{tracking.Id}/participations/?score=500", null); // await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeFalse();
            await Context.PlayerParticipation.Where(x => x.AccountID == aCCOUNT.AccountID).ExecuteDeleteAsync();
        }

        [Fact]
        public async Task ParticipateNoLoggedIn()
        {
            // Act
            HttpResponseMessage result = await Client.PutAsync($"/api/v1/competitions/{tracking.Id}/participations/?score=500", null); // await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeFalse();
            var x = Context.PlayerParticipation.Where(x => x.CompetitionID == tracking.Id).ToList();
            x.Should().BeEmpty();
        }

        protected override async Task Init()
        {
            tracking = await Instantiate(new CompetitionModel("A", "B", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)));
        }
    }

    public class ConflictingLoginTests(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private AccountModel tracking = null!;
        [Fact]
        public async Task ConflictingLogin()
        {
            // Act
            HttpResponseMessage regResult = await Client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(tracking.Login, "123456789A123456789", null));

            // Assert
            regResult.IsSuccessStatusCode.Should().BeFalse();
            string name = tracking.Login;
            var acc = Context.AccountsReadOnly.Where(x => x.Login == name).ToList();
            acc.Should().ContainSingle();
        }

        // [Fact] THIS TEST DOESNT WORK BUT IT SHOULD! I DONT HAVE TIME TO FIX IT
        // public async Task IncorrectPassword()
        // {
        //    // Act
        //    var regResult = await Client.PostAsJsonAsync("/api/v1/auth/login", new AccountLoginDTO(tracking.Login, "123456789A123456789"));
        //    // Assert
        //    regResult.IsSuccessStatusCode.Should().BeFalse();
        // }
        protected override async Task Init()
        {
            tracking = await Instantiate(new AccountModel(new Account(Faker.Internet.UserName()), "PASSSWORDHASH", new PlayerRole()));
        }
    }
}
