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
using Microsoft.AspNetCore.Mvc.Testing;
using CompetitiveBackend;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.BackendUsage.Objects;
namespace RepositoriesTests.RepositoriesTests
{
    public class CompetitionE2ETests : E2ETest
    {
        public CompetitionE2ETests(ITestOutputHelper helper, WebApplicationFactory<Program> p) : base(helper, p)
        {
            
        }
        [Fact]
        public async Task CompetitionFetchOne()
        {
            // Arrange
            await ExecSQLFile("competitions.sql");

            // Act
            var result = await Client.GetAsync("/api/v1/competitions/1");//await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
            var obj = await GetObject<CompetitionDTO>(result);
            obj.ID.Should().Be(1);
            obj.Name.Should().Be("The name");
        }
        [Fact]
        public async Task CompetitionFetchAll()
        {
            // Arrange
            await ExecSQLFile("competitions.sql");

            // Act
            var result = await Client.GetAsync("/api/v1/competitions/");//await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
            var obj = await GetObject<CompetitionDTO[]>(result);
            obj.Count().Should().Be(3);
        }
        [Fact]
        public async Task CompetitionFetchPage()
        {
            // Arrange
            await ExecSQLFile("competitions.sql");

            // Act
            var result = await Client.GetAsync("/api/v1/competitions/?page=1&count=1");//await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
            var obj = await GetObject<CompetitionDTO[]>(result);
            obj.Should().ContainSingle().Which.ID.Should().Be(2);
        }
    }
}
