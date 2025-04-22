using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.Objects;
using Repositories.Objects;
using ServicesRealisation.Objects;
using ServicesRealisation.ServicesRealisation.Validator;
namespace CompetitiveBackend.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IHashAlgorithm _hashAlgorithm;
        private readonly IRoleCreator _roleCreator;
        private readonly IValidator<AccountCreationData> _validator;
        private readonly IRepositoryPrivilegySetting _privilegySetting;
        public AuthService(
            IAccountRepository accountRepository,
            ISessionRepository sessionRepository,
            IHashAlgorithm hashAlgo,
            IRoleCreator roleCreator,
            IValidator<AccountCreationData> validator,
            IRepositoryPrivilegySetting privilegySetting)
        {
            _accountRepository = accountRepository;
            _hashAlgorithm = hashAlgo;
            _sessionRepository = sessionRepository;
            _roleCreator = roleCreator;
            _validator = validator;
            _privilegySetting = privilegySetting;
        }

        public async Task<SessionToken> GetSessionToken(string token)
        {
            SessionToken tkn = await _sessionRepository.GetSessionToken(token);
            _privilegySetting.SetPrivilegies(tkn);
            return tkn;
        }

        public async Task<AuthSuccessResult> LogIn(string login, string password)
        {
            Account acc = await _accountRepository.GetAccount(login);

            if (!_hashAlgorithm.Verify(password, acc.PasswordHash))
            {
                throw new IncorrectPasswordException();
            }
            string token = await _sessionRepository.CreateSessionFor(acc.Id!.Value);
            string roleName = (await _sessionRepository.GetSessionToken(token)).Role.ToString();
            return new AuthSuccessResult(token, roleName, acc.Id!.Value);
        }

        public async Task Register(Account data, string password)
        {
            if (!_validator.IsValid(new AccountCreationData(data, password), out string? msg))
                throw new InvalidArgumentsException(msg!);
            string passwordHash = _hashAlgorithm.Hash(password);
            await _accountRepository.CreateAccount(new Account(data.Login, passwordHash, data.Email, data.Id), _roleCreator.Create(data));
        }
    }
}
