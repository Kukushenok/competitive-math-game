using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.Objects;
using Microsoft.Extensions.Logging;
namespace CompetitiveBackend.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IHashAlgorithm _hashAlgorithm;
        private readonly IRoleCreator _roleCreator;
        public AuthService(
            IAccountRepository accountRepository,
            ISessionRepository sessionRepository,
            IHashAlgorithm hashAlgo,
            IRoleCreator roleCreator)
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
            await _accountRepository.CreateAccount(new Account(data.Login, passwordHash, data.Email, data.Id), _roleCreator.Create(data));
        }
    }
}
