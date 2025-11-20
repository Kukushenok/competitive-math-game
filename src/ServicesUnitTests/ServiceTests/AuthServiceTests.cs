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

namespace ServicesUnitTests.ServiceTests
{
    public class TestRole : Role
    {
        public const string NAME = "TestRoleName";
        public override bool IsAdmin()
        {
            return false;
        }

        public override bool IsPlayer()
        {
            return false;
        }

        public override string ToString()
        {
            return NAME;
        }
    }

    public class AuthServiceTests
    {
        private readonly Mock<IAccountRepository> accountRepo = new();
        private readonly Mock<IHashAlgorithm> algo = new();
        private readonly Mock<IRoleCreator> creator = new();
        private readonly Mock<IRepositoryPrivilegySetting> setting = new();
        private readonly Mock<ISessionRepository> sessionRepo = new();
        public AuthServiceTests()
        {
            algo.Setup(algo => algo.Hash(It.IsAny<string>())).Returns<string>(gt => $"|{gt}|");
            algo.Setup(algo => algo.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((a, b) => $"|{a}|" == b);
        }

        [Fact]
        public async Task AuthServiceLogInSuccess()
        {
            // Arrange
            MockValidator<AccountCreationData> validator = new MockValidatorBuilder<AccountCreationData>().Build();
            accountRepo.Setup(x => x.GetAccount("abcd")).ReturnsAsync(new Account("abcd", id: 0));
            sessionRepo.Setup(x => x.CreateSessionFor(0)).ReturnsAsync("hi");
            accountRepo.Setup(x => x.VerifyPassword("abcd", "|12345|")).ReturnsAsync(true);
            sessionRepo.Setup(x => x.GetSessionToken("hi")).ReturnsAsync(new AuthenticatedSessionToken(new TestRole(), 0));
            var service = new AuthService(accountRepo.Object, sessionRepo.Object, algo.Object, creator.Object, validator, setting.Object);

            // Act
            AuthSuccessResult rw = await service.LogIn("abcd", "12345");

            // Assert
            Assert.Equal("hi", rw.Token);
            Assert.Equal(0, rw.AccountID);
            Assert.Equal(TestRole.NAME, rw.RoleName);
        }

        [Fact]
        public async Task AuthServiceLogInBadPassword()
        {
            // Arrange
            MockValidator<AccountCreationData> validator = new MockValidatorBuilder<AccountCreationData>().Build();
            accountRepo.Setup(x => x.VerifyPassword("abcd", "1234")).ReturnsAsync(false);
            accountRepo.Setup(x => x.GetAccount("abcd")).ReturnsAsync(new Account("abcd", id: 0));
            var service = new AuthService(accountRepo.Object, sessionRepo.Object, algo.Object, creator.Object, validator, setting.Object);

            // Act Assert
            await Assert.ThrowsAnyAsync<IncorrectPasswordException>(async () => await service.LogIn("abcd", "12345"));
        }

        [Fact]
        public async Task AuthServiceLogInRepositoryException()
        {
            // Arrange
            MockValidator<AccountCreationData> validator = new MockValidatorBuilder<AccountCreationData>().Build();
            accountRepo.Setup(x => x.GetAccount("abcd")).ThrowsAsync(new RepositoryException());
            var service = new AuthService(accountRepo.Object, sessionRepo.Object, algo.Object, creator.Object, validator, setting.Object);

            // Act Assert
            await Assert.ThrowsAsync<IncorrectPasswordException>(async () => await service.LogIn("abcd", "12345"));
        }

        [Fact]
        public async Task AuthServiceCreateAccountSuccess()
        {
            // Arrange
            var c = new Account("hi", "X");
            MockValidator<AccountCreationData> validator = new MockValidatorBuilder<AccountCreationData>()
                .CheckEtalon(new AccountCreationData(c, "1234")).Build();
            accountRepo.Setup(x => x.CreateAccount(It.IsAny<Account>(), "1234", It.IsAny<Role>()));
            creator.Setup(x => x.Create(It.IsAny<Account>())).Returns(new TestRole());
            var service = new AuthService(accountRepo.Object, sessionRepo.Object, algo.Object, creator.Object, validator, setting.Object);

            // Act
            await service.Register(c, "1234");

            // Assert
            validator.CheckWasCalled();
        }

        [Fact]
        public async Task AuthServiceCreateAccountFailure()
        {
            // Arrange
            var c = new Account("hi", "X");
            MockValidator<AccountCreationData> validator = new MockValidatorBuilder<AccountCreationData>().FailByDefault().Build();
            accountRepo.Setup(x => x.CreateAccount(It.IsAny<Account>(), "1234", It.IsAny<Role>()));
            creator.Setup(x => x.Create(It.IsAny<Account>())).Returns(new TestRole());
            var service = new AuthService(accountRepo.Object, sessionRepo.Object, algo.Object, creator.Object, validator, setting.Object);

            // Act Assert
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await service.Register(c, "1234"));
        }

        [Fact]
        public async Task AuthServiceGetSessionToken()
        {
            // Arrange
            sessionRepo.Setup(x => x.GetSessionToken("abc")).ReturnsAsync(new AuthenticatedSessionToken(new TestRole(), 12));
            MockValidator<AccountCreationData> validator = new MockValidatorBuilder<AccountCreationData>().Build();
            var service = new AuthService(accountRepo.Object, sessionRepo.Object, algo.Object, creator.Object, validator, setting.Object);

            // Act
            var d = (AuthenticatedSessionToken)await service.GetSessionToken("abc");

            // Assert
            bool valid = d.TryGetAccountIdentifier(out int id);
            Assert.True(valid);
            Assert.Equal(TestRole.NAME, d.Role.ToString());
            Assert.Equal(12, id);
        }
    }
}
