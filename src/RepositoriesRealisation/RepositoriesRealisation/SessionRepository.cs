using CompetitiveBackend.Core.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoriesRealisation;
using RepositoriesRealisation.DatabaseObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace CompetitiveBackend.Repositories
{
    public class SessionRepositoryConfiguration
    {
        private IConfiguration configuration;
        public SessionRepositoryConfiguration(IConfiguration conf)
        {
            configuration = conf;
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Secrets:SessionKey"] ?? "INV3P"));
            Credentials = new SigningCredentials(Key, conf["Secrets:Algo"] ?? SecurityAlgorithms.HmacSha256);
        }
        public SymmetricSecurityKey Key { get; private set; }
        public SigningCredentials Credentials { get; private set; }
        public DateTime? Expires { get => DateTime.Now.AddHours(int.Parse(configuration["SessionTimeHrs"] ?? "24")); }
    }
    public class SessionRepository : ISessionRepository
    {
        private IDbContextFactory<BaseDbContext> contextFactory;
        private JwtSecurityTokenHandler _handler;
        private SessionRepositoryConfiguration _configuration;
        public SessionRepository(IDbContextFactory<BaseDbContext> contextFactory, SessionRepositoryConfiguration conf)
        {
            this.contextFactory = contextFactory;
            _handler = new JwtSecurityTokenHandler();
            _configuration = conf;
        }
        public async Task<string> CreateSessionFor(int accountID)
        {
            using BaseDbContext _context = await contextFactory.CreateDbContextAsync();
            AccountModel? model = await _context.Accounts.FindAsync(accountID);
            if (model == null) throw new Exceptions.MissingDataException();
            Role rl = PrivilegyRoleResolver.Resolve(model.AccountPrivilegyLevel);
            var token = new JwtSecurityToken(
                claims: new List<Claim>()
                {
                    new Claim("ID", accountID.ToString()),
                    new Claim("Role", rl.ToString()),
                    new Claim("PrivilegyLevel", model.AccountPrivilegyLevel.ToString())
                },
                expires: _configuration.Expires,
                signingCredentials: _configuration.Credentials);
            string q = _handler.WriteToken(token);
            return q;
        }

        public async Task<SessionToken> GetSessionToken(string token)
        {
            TokenValidationResult tkn = await _handler.ValidateTokenAsync(token, GetValidationParameters());
            if(tkn.Exception != null)
            {
                return new UnauthenticatedSessionToken();
            }
            try
            {
                string ID = tkn.Claims["ID"].ToString()!;
                string PrivilegyLevel = tkn.Claims["PrivilegyLevel"].ToString()!;
                return new AuthenticatedSessionToken(PrivilegyRoleResolver.Resolve(int.Parse(PrivilegyLevel)), int.Parse(ID));
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
                IssuerSigningKey = _configuration.Key 
            };
        }
    }
}
