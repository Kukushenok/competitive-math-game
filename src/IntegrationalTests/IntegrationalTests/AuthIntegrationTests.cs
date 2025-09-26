using CompetitiveBackend.BackendUsage.Objects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationalTests
{
    public class AuthIntegrationTests(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private const string USER_PASSWORD = "123456789A123456789";
        [Fact]
        public async Task CreateAccount()
        {
            string name = Faker.Internet.UserName();
            var regResult = await Client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(name, USER_PASSWORD, null));
            regResult.IsSuccessStatusCode.Should().BeTrue();


            var acc = Context.AccountsReadOnly.Where(x => x.Login == name).ToList();
            acc.Should().ContainSingle().Which.Login.Should().Be(name);
            Context.AccountsReadOnly.RemoveRange(acc);
            await Context.SaveChangesAsync();
        }
        protected override Task Init()
        {
            return base.Init();
        }
    }
}
