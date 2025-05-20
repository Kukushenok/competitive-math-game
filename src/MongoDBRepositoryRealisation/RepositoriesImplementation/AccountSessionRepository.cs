using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Model;
using MongoDB.Driver;
using MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBRepositoryRealisation.RepositoriesImplementation
{
    internal class StringPrivilegyRoleResolver
    {
        public static string Resolve(Role rl)
        {
            return rl.ToString();
        }
        public static Role Resolve(string rl)
        {
            if (rl == "Player") return new PlayerRole();
            if (rl == "Admin") return new AdminRole();
            throw new FailedOperationException("No such role supported");
        }
    }
    internal class SessionRepositoryConfiguration
    {
        private IConfiguration configuration;
        public SessionRepositoryConfiguration(IConfiguration conf)
        {
            configuration = conf;
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Secrets:SessionKey"] ?? new string('N', 128)));
            Credentials = new SigningCredentials(Key, conf["Secrets:Algo"] ?? SecurityAlgorithms.HmacSha256);
        }
        public SymmetricSecurityKey Key { get; private set; }
        public SigningCredentials Credentials { get; private set; }
        public DateTime? Expires
        {
            get
            {
                double val = 24.0;
                double.TryParse(configuration["SessionTimeHrs"] ?? "24", out val);
                return DateTime.UtcNow.AddHours(val);
            }
        }
    }
    internal class SessionRepository : BaseRepository<SessionRepository>, ISessionRepository
    {
        private JwtSecurityTokenHandler _handler;
        private SessionRepositoryConfiguration _configuration;
        public SessionRepository(IMongoConnectionCreator contextFactory, ILogger<SessionRepository> logger, SessionRepositoryConfiguration conf) : base(contextFactory, logger)
        {
            _handler = new JwtSecurityTokenHandler();
            _configuration = conf;
        }

        public async Task<string> CreateSessionFor(int accountID)
        {
            using var _context = await GetMongoConnection();
            AccountEntity? model = (await _context.Account().FindAsync(x => x.Id == accountID)).SingleOrDefault();
            if (model == null) throw new MissingDataException();
            Role rl = StringPrivilegyRoleResolver.Resolve(model.PrivilegyName);
            var token = new JwtSecurityToken(
                claims: new List<Claim>()
                {
                    new Claim("ID", accountID.ToString()),
                    new Claim("Role", rl.ToString()),
                    new Claim("PrivilegyLevel", model.PrivilegyName)
                },
                expires: _configuration.Expires,
                signingCredentials: _configuration.Credentials);
            string q = _handler.WriteToken(token);
            return q;
        }

        public async Task<SessionToken> GetSessionToken(string token)
        {
            TokenValidationResult tkn = await _handler.ValidateTokenAsync(token, GetValidationParameters());
            if (tkn.Exception != null)
            {
                return new UnauthenticatedSessionToken();
            }
            try
            {
                string ID = tkn.Claims["ID"].ToString()!;
                string PrivilegyLevel = tkn.Claims["PrivilegyLevel"].ToString()!;
                return new AuthenticatedSessionToken(StringPrivilegyRoleResolver.Resolve(PrivilegyLevel), int.Parse(ID));
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
