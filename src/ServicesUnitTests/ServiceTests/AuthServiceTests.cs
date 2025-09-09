using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.AuthService;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.Objects;
using Moq;
using Repositories.Objects;
using ServicesRealisation.Objects;
using ServicesRealisation.ServicesRealisation.Validator;
using System.ComponentModel.DataAnnotations;

namespace ServicesUnitTests.ServiceTests
{
    public class TestRole : Role
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
        Mock<IRepositoryPrivilegySetting> _setting = new Mock<IRepositoryPrivilegySetting>();
        Mock<ISessionRepository> _sessionRepo = new Mock<ISessionRepository>();
        MockValidator<AccountCreationData> _validator = new MockValidator<AccountCreationData>();
        private readonly AuthService _service;
        public AuthServiceTests()
        {
            _algo.Setup(algo => algo.Hash(It.IsAny<string>())).Returns<string>(gt => $"|{gt}|");
            _algo.Setup(algo => algo.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((a, b) => $"|{a}|" == b);
            _service = new AuthService(_accountRepo.Object, _sessionRepo.Object, _algo.Object, _creator.Object, _validator, _setting.Object);
        }
        [Fact]
        public async Task AuthService_LogIn_Success()
        {
            // Arrange
            _validator.Reset();
            _accountRepo.Setup(x => x.GetAccount("abcd")).ReturnsAsync(new Account("abcd", id: 0));
            _sessionRepo.Setup(x => x.CreateSessionFor(0)).ReturnsAsync("hi");
            _accountRepo.Setup(x => x.VerifyPassword("abcd", "|12345|")).ReturnsAsync(true);
            _sessionRepo.Setup(x => x.GetSessionToken("hi")).ReturnsAsync(new AuthenticatedSessionToken(new TestRole(), 0));

            // Act
            AuthSuccessResult rw = await _service.LogIn("abcd", "12345");

            // Assert
            Assert.Equal("hi", rw.Token);
            Assert.Equal(0, rw.AccountID);
            Assert.Equal(TestRole.NAME, rw.RoleName);
        }
        [Fact]
        public async Task AuthService_LogIn_BadPassword()
        {
            // Arrange
            _validator.Reset();
            _accountRepo.Setup(x => x.VerifyPassword("abcd", "1234")).ReturnsAsync(false);
            _accountRepo.Setup(x => x.GetAccount("abcd")).ReturnsAsync(new Account("abcd", id: 0));

            // Act Assert
            await Assert.ThrowsAnyAsync<IncorrectPasswordException>(async () => await _service.LogIn("abcd", "12345"));
        }
        [Fact]
        public async Task AuthService_LogIn_RepositoryException()
        {
            // Arrange
            _validator.Reset();
            _accountRepo.Setup(x => x.GetAccount("abcd")).ThrowsAsync(new RepositoryException());

            // Act Assert
            await Assert.ThrowsAsync<IncorrectPasswordException>(async () => await _service.LogIn("abcd", "12345"));
        }
        [Fact]
        public async Task AuthService_CreateAccount_Success()
        {
            // Arrange
            Account c = new Account("hi", "X");
            _validator.Reset(new AccountCreationData(c, "1234"));
            _accountRepo.Setup(x => x.CreateAccount(It.IsAny<Account>(), "1234", It.IsAny<Role>()));
            _creator.Setup(x => x.Create(It.IsAny<Account>())).Returns(new TestRole());

            // Act
            await _service.Register(c, "1234");

            // Assert
            _validator.Check();
        }
        [Fact]
        public async Task AuthService_CreateAccount_Failure()
        {
            // Arrange
            Account c = new Account("hi", "X");
            _validator.Reset(new AccountCreationData(c, "1234"), true);
            _accountRepo.Setup(x => x.CreateAccount(It.IsAny<Account>(), "1234", It.IsAny<Role>()));
            _creator.Setup(x => x.Create(It.IsAny<Account>())).Returns(new TestRole());

            // Act
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.Register(c, "1234"));
        }
        [Fact]
        public async Task AuthService_GetSessionToken()
        {
            // Arrange
            _sessionRepo.Setup(x => x.GetSessionToken("abc")).ReturnsAsync(new AuthenticatedSessionToken(new TestRole(), 12));

            // Act
            AuthenticatedSessionToken d = (AuthenticatedSessionToken)await _service.GetSessionToken("abc");
            bool valid = d.TryGetAccountIdentifier(out int id);

            // Assert
            Assert.True(valid);
            Assert.Equal(TestRole.NAME, d.Role.ToString());
            Assert.Equal(12, id);
        }
    }
}
