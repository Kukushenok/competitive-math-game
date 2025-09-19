using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using FluentAssertions;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using CompetitiveBackend.Controllers;
namespace RepositoriesTests.RepositoriesTests
{
    public class CompetitionE2ETests : E2ETest
    {
        public CompetitionE2ETests(ITestOutputHelper helper) : base(helper)
        {
            
        }
        [Fact]
        public async Task CreateSessionTokenFor_AdminSuccess()
        {
            // Arrange
            await ExecSQLFile("competitions.sql");

            // Act
            var result = await Client.GetAsync($"/{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/0");


            var response = await Client.GetAsync("/swagger/v1/swagger.json");

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();


        }

    }
}
