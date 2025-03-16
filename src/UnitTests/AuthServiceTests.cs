using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.AuthService;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.Objects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Tests.ServiceTests
{
    public class TestRole: Role
    {
        public const string NAME = "TestRoleName";
        public override bool IsAdmin() => false;
        public override bool IsPlayer() => false;
        public override string ToString() => NAME;
    }
    public class AuthServiceTests
    {
        Mock<IAccountRepository> _accountRepo = new Mock<IAccountRepository>();
        Mock<IHashAlgorithm> _algo = new Mock<IHashAlgorithm>();
        Mock<IRoleCreator> _creator = new Mock<IRoleCreator>();
        Mock<ISessionRepository> _sessionRepo = new Mock<ISessionRepository>();
        private readonly AuthService _service;
        public AuthServiceTests()
        {
            _algo.Setup(algo => algo.Hash(It.IsAny<string>())).Returns<string>(gt => $"|{gt}|");
            _algo.Setup(algo => algo.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((a, b) => $"|{a}|" == b);
            _service = new AuthService(new NullLogger<AuthService>(), _accountRepo.Object, _sessionRepo.Object, _algo.Object, _creator.Object);
        }
        [Fact]
        public async Task AuthService_LogIn_Success()
        {
            _accountRepo.Setup(x => x.GetAccount("abcd")).ReturnsAsync(new Account("abcd", "|12345|", id: 0));
            _sessionRepo.Setup(x => x.CreateSessionFor(0)).ReturnsAsync("hi");
            _sessionRepo.Setup(x => x.GetSessionToken("hi")).ReturnsAsync(new AuthenticatedSessionToken(new TestRole(), 0));
            AuthSuccessResult rw = await _service.LogIn("abcd", "12345");
            Assert.Equal("hi",rw.Token);
            Assert.Equal(0, rw.AccountID);
            Assert.Equal(TestRole.NAME, rw.RoleName);
        }
        [Fact]
        public async Task AuthService_LogIn_BadPassword()
        {
            _accountRepo.Setup(x => x.GetAccount("abcd")).ReturnsAsync(new Account("abcd", "|1234|", id: 0));
            await Assert.ThrowsAnyAsync<IncorrectPasswordException>(async () => await _service.LogIn("abcd", "12345"));
        }
        [Fact]
        public async Task AuthService_LogIn_RepositoryException()
        {
            _accountRepo.Setup(x => x.GetAccount("abcd")).ThrowsAsync(new RepositoryException());
            await Assert.ThrowsAnyAsync<RepositoryException>(async () => await _service.LogIn("abcd", "12345"));
        }
        [Fact]
        public async Task AuthService_CreateAccount_Success()
        {
            Account c = new Account("hi", "X");
            _accountRepo.Setup(x => x.CreateAccount(It.IsAny<Account>(), It.IsAny<Role>()))
                .Callback<Account, Role>((a, r) => {
                    Assert.Equal("|1234|", a.PasswordHash);
                    Assert.Equal(TestRole.NAME, r.ToString());
                    });
            _creator.Setup(x => x.Create(It.IsAny<Account>())).Returns(new TestRole());
            await _service.Register(c, "1234");
        }
        [Fact]
        public async Task AuthService_GetSessionToken()
        {
            _sessionRepo.Setup(x => x.GetSessionToken("abc")).ReturnsAsync(new AuthenticatedSessionToken(new TestRole(), 12));
            AuthenticatedSessionToken d = (AuthenticatedSessionToken)await _service.GetSessionToken("abc");
            Assert.Equal(TestRole.NAME, d.Role.ToString());
            int id;
            Assert.True(d.TryGetAccountIdentifier(out id));
            Assert.Equal(12, id);
        }
    }
}
