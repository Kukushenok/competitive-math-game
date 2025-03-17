using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    public class BaseAuthController: ControllerBase
    {
        private ISessionRepository _sessionRepo;
        public BaseAuthController(ISessionRepository repository)
        {
            this._sessionRepo = repository;
        }
        protected async Task<SessionToken> GetToken()
        {
            if (HttpContext.TryGetTokenValue(out string token))
            {
                return await _sessionRepo.GetSessionToken(token);
            }
            return new UnauthenticatedSessionToken();
        }
    }
}
