using Castle.Core.Logging;
using CompetitiveBackend.Core.Exceptions.Repository;
using CompetitiveBackend.Core.Exceptions.Services;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.LogicComponents;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.AccountSevice;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.ServiceTests
{
    public class AuthServiceTests
    {
        private readonly AuthService _service;
        public AuthServiceTests()
        {
            Mock<IAccountRepository> _accountRepo = new Mock<IAccountRepository>();
            Mock<IHashAlgorithm> algo = new Mock<IHashAlgorithm>();
            Mock<IRoleCreator> creator = new Mock<IRoleCreator>();
            Mock<ISessionRepository> repo = new Mock<ISessionRepository>();
            repo.Setup(repo => repo.CreateSessionFor(It.IsAny<int>())).Returns<int>(a => Task.FromResult(a.ToString()));
            repo.Setup(repo => repo.GetSessionToken(It.IsAny<string>())).Returns<string>(a => Task.FromResult((SessionToken)new AuthenticatedSessionToken(new PlayerRole(), int.Parse(a))));
            creator.Setup(algo => algo.Create(It.IsAny<Account>())).Returns<Account>(a => a.Login == "root" ? new AdminRole() : new PlayerRole());
            algo.Setup(algo => algo.Hash(It.IsAny<string>())).Returns<string>(gt => $"|{gt}|");
            algo.Setup(algo => algo.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((a, b) => $"|{a}|" == b);
            List<Account> accounts = new List<Account>()
            {
                new Account("abcd", "|12345|", id: 0),
                new Account("cerf", "|12345|", id: 1),
                new Account("root", "|12345|", id: 2),
                new Account("aaaa", "|12345|", id: 3),
            };
            _accountRepo.Setup(a => a.CreateAccount(It.IsAny<Account>(), It.IsAny<Role>())).Returns(() => Task.CompletedTask);

            _accountRepo.Setup(a => a.GetAccount(It.IsAny<string>())).Returns<string>(gt => Task.FromResult(accounts.Find(x => x.Login == gt) ?? throw new MissingDataException()));


            _service = new AuthService(new NullLogger<AuthService>(), _accountRepo.Object, repo.Object, algo.Object, creator.Object);
        }
        [Fact]
        public async Task SSS()
        {
            AuthSuccessResult rw = await _service.LogIn("abcd", "12345");
            Assert.Equal("0", rw.Token);
            Assert.Equal("Player", rw.RoleName);
        }
        [Fact]
        public async Task BAD()
        {
            await Assert.ThrowsAnyAsync<ServiceException>(async () => await _service.LogIn("abcd", "123"));
            return;
        }
        [Fact]
        public async Task BAD2()
        {
            await Assert.ThrowsAnyAsync<ServiceException>(async () => await _service.LogIn("abcdef", "12345"));
            return;
        }
    }
}
