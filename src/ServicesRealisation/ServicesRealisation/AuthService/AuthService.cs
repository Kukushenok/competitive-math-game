using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.Objects;
using Repositories.Objects;
using ServicesRealisation.Objects;
using ServicesRealisation.ServicesRealisation.Validator;
namespace CompetitiveBackend.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository accountRepository;
        private readonly ISessionRepository sessionRepository;
        private readonly IHashAlgorithm hashAlgorithm;
        private readonly IRoleCreator roleCreator;
        private readonly IValidator<AccountCreationData> validator;
        private readonly IRepositoryPrivilegySetting privilegySetting;
        public AuthService(
            IAccountRepository accountRepository,
            ISessionRepository sessionRepository,
            IHashAlgorithm hashAlgo,
            IRoleCreator roleCreator,
            IValidator<AccountCreationData> validator,
            IRepositoryPrivilegySetting privilegySetting)
        {
            this.accountRepository = accountRepository;
            hashAlgorithm = hashAlgo;
            this.sessionRepository = sessionRepository;
            this.roleCreator = roleCreator;
            this.validator = validator;
            this.privilegySetting = privilegySetting;
        }

        public async Task<SessionToken> GetSessionToken(string token)
        {
            SessionToken tkn = await sessionRepository.GetSessionToken(token);
            privilegySetting.SetPrivilegies(tkn);
            return tkn;
        }

        public async Task<AuthSuccessResult> LogIn(string login, string password)
        {
            string passwordHash = hashAlgorithm.Hash(password);
            if (!await accountRepository.VerifyPassword(login, passwordHash))
            {
                throw new IncorrectPasswordException();
            }

            Account acc = await accountRepository.GetAccount(login);
            string token = await sessionRepository.CreateSessionFor(acc.Id!.Value);
            string roleName = (await sessionRepository.GetSessionToken(token)).Role.ToString();
            return new AuthSuccessResult(token, roleName, acc.Id!.Value);
        }

        public async Task Register(Account data, string password)
        {
            if (!validator.IsValid(new AccountCreationData(data, password), out string? msg))
            {
                throw new InvalidArgumentsException(msg!);
            }

            string passwordHash = hashAlgorithm.Hash(password);
            try
            {
                await accountRepository.CreateAccount(new Account(data.Login, data.Email, data.Id), passwordHash, roleCreator.Create(data));
            }
            catch (FailedOperationException exp)
            {
                throw new ConflictLoginException("Conflicting login", exp);
            }
        }
    }
}
