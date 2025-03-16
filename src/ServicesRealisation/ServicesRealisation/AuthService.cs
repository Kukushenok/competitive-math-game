using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.Objects;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace CompetitiveBackend.Services.AuthService
{
    public class PlayerRoleCreator : IRoleCreator { public Role Create(Account data) => new PlayerRole(); }
    public class SpecificRoleCreator : IRoleCreator
    {
        public Role Create(Account data)
        {
            if (data.Login == "root") return new AdminRole();
            return new PlayerRole();
        }
    }
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IHashAlgorithm _hashAlgorithm;
        private readonly IRoleCreator _roleCreator;
        public AuthService(
            ILogger<AuthService> logger,
            IAccountRepository accountRepository,
            ISessionRepository sessionRepository,
            IHashAlgorithm hashAlgo,
            IRoleCreator roleCreator)
        {
            _logger = logger;
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
            string passwordHash = _hashAlgorithm.Hash(password);
            if(CheckLogin(data.Login))
            {
                throw new BadLoginException();
            }
            await _accountRepository.CreateAccount(new Account(data.Login, passwordHash, data.Email, data.Id), _roleCreator.Create(data));
        }
        private bool CheckLogin(string login)
        {
            int len = login.Length;
            return len >= 3 && len <= 64 && !login.Contains(' ');
        }
    }
}
