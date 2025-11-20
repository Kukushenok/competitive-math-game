using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using CompetitiveBackend.Core.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RepositoriesRealisation;
using RepositoriesRealisation.DatabaseObjects;
using RepositoriesRealisation.Models;
namespace CompetitiveBackend.Repositories
{
    internal sealed class SessionRepository : ISessionRepository
    {
        private readonly IDbContextFactory<BaseDbContext> contextFactory;
        private readonly JwtSecurityTokenHandler handler;
        private readonly SessionRepositoryConfiguration configuration;
        public SessionRepository(IDbContextFactory<BaseDbContext> contextFactory, SessionRepositoryConfiguration conf)
        {
            this.contextFactory = contextFactory;
            handler = new JwtSecurityTokenHandler();
            configuration = conf;
        }

        public async Task<string> CreateSessionFor(int accountID)
        {
            using BaseDbContext context = await contextFactory.CreateDbContextAsync();
            AccountModel? model = await context.AccountsReadOnly.FindAsync(accountID) ?? throw new Exceptions.MissingDataException();
            Role rl = PrivilegyRoleResolver.Resolve(model.AccountPrivilegyLevel);
            var token = new JwtSecurityToken(
                claims:
                [
                    new("ID", accountID.ToString(CultureInfo.InvariantCulture)),
                    new("Role", rl.ToString()),
                    new("PrivilegyLevel", model.AccountPrivilegyLevel.ToString(CultureInfo.InvariantCulture)),
                ],
                expires: configuration.Expires,
                signingCredentials: configuration.Credentials);
            string q = handler.WriteToken(token);
            return q;
        }

        public async Task<SessionToken> GetSessionToken(string token)
        {
            TokenValidationResult tkn = await handler.ValidateTokenAsync(token, GetValidationParameters());
            if (tkn.Exception != null)
            {
                return new UnauthenticatedSessionToken();
            }

            try
            {
                string iD = tkn.Claims["ID"].ToString()!;
                string privilegyLevel = tkn.Claims["PrivilegyLevel"].ToString()!;
                return new AuthenticatedSessionToken(PrivilegyRoleResolver.Resolve(int.Parse(privilegyLevel, CultureInfo.InvariantCulture)), int.Parse(iD, CultureInfo.InvariantCulture));
            }
            catch
            {
                return new UnauthenticatedSessionToken();
            }
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = configuration.Key,
            };
        }
    }
}
