using CompetitiveBackend.Core.Exceptions.Repository;
using CompetitiveBackend.Core.Exceptions.Services;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.LogicComponents;
using CompetitiveBackend.Repositories;
namespace CompetitiveBackend.Services.AccountSevice
{
    public class AuthService : BaseService<AuthService>, IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IHashAlgorithm _hashAlgorithm;
        private readonly IRoleCreator _roleCreator;
        public AuthService(
            ILogger<AuthService> logger,
            IAccountRepository accountRepository,
            ISessionRepository sessionRepository,
            IHashAlgorithm hashAlgo,
            IRoleCreator roleCreator): base(logger)
        {
            _accountRepository = accountRepository;
            _hashAlgorithm = hashAlgo;
            _sessionRepository = sessionRepository;
            _roleCreator = roleCreator;
        }

        public Task<SessionToken> GetSessionToken(string token)
        {
            return _sessionRepository.GetSessionToken(token);
        }

        public async Task<AuthSuccessResult> LogIn(string login, string password)
        {
            try
            {
                Account acc = await _accountRepository.GetAccount(login);

                if (!_hashAlgorithm.Verify(password, acc.PasswordHash))
                {
                    throw new ServiceException<AuthService>();
                }
                string token = await _sessionRepository.CreateSessionFor(acc.Id!.Value);
                string roleName = (await _sessionRepository.GetSessionToken(token)).Role.ToString();
                return new AuthSuccessResult(token, roleName, acc.Id!.Value);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError($"Log in failure: {ex}");
                throw new ServiceException<AuthService>();
            }
        }

        public async Task Register(Account data, string password)
        {
            try
            {
                string passwordHash = _hashAlgorithm.Hash(password); // account is ref type so it changes data :(
                await _accountRepository.CreateAccount(new Account(data.Login, passwordHash, data.Email, data.Id), _roleCreator.Create(data));
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, $"Unable to register user with login {data.Login}");
                throw new ServiceException<AuthService>();
            }
        }
    }
}
