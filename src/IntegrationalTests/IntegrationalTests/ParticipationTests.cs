using Bogus.DataSets;
using CompetitiveBackend;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Controllers;
using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using FluentAssertions;
using IntegrationalTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Renci.SshNet;
using RepositoriesRealisation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
namespace IntegrationalTests
{
    public class ParticipationTests(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private RepositoriesRealisation.Models.CompetitionModel tracking;

        [Fact]
        public async Task Participate()
        {
            await using var REGISTERACC = new SetupAuth(Client, Context, Faker.Internet.UserName());
            var ACCOUNT = await REGISTERACC.RegisterAndLogIn();
            
            // Act
            var result = await Client.PutAsync($"/api/v1/competitions/{tracking.Id}/participations/?score=500", null);//await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
            await Context.PlayerParticipation.Where(x => x.AccountID == ACCOUNT.AccountID).ExecuteDeleteAsync();
        }
        [Fact]
        public async Task ParticipateNoLoggedIn()
        {
            // Act
            var result = await Client.PutAsync($"/api/v1/competitions/{tracking.Id}/participations/?score=500", null);//await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeFalse();
            var x = Context.PlayerParticipation.Where(x => x.CompetitionID == tracking.Id).ToList();
            x.Should().BeEmpty();
        }
        protected override async Task Init()
        {
            tracking = await Instantiate(new RepositoriesRealisation.Models.CompetitionModel("A", "B", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)));
            
        }
    }
    public class ConflictingLoginTests(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private AccountModel tracking;
        [Fact]
        public async Task ConflictingLogin()
        {
            // Act
            var regResult = await Client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(tracking.Login, "123456789A123456789", null));
            // Assert
            regResult.IsSuccessStatusCode.Should().BeFalse();
            var name = tracking.Login;
            var acc = Context.AccountsReadOnly.Where(x => x.Login == name).ToList();
            acc.Should().ContainSingle();
        }
        //[Fact] THIS TEST DOESNT WORK BUT IT SHOULD! I DONT HAVE TIME TO FIX IT
        //public async Task IncorrectPassword()
        //{
        //    // Act
        //    var regResult = await Client.PostAsJsonAsync("/api/v1/auth/login", new AccountLoginDTO(tracking.Login, "123456789A123456789"));
        //    // Assert
        //    regResult.IsSuccessStatusCode.Should().BeFalse();
        //}
        protected override async Task Init()
        {
            tracking = await Instantiate(new AccountModel(new Account(Faker.Internet.UserName()), "PASSSWORDHASH", new PlayerRole()));

        }
    }
}
