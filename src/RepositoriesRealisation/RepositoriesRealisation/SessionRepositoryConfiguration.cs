using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace CompetitiveBackend.Repositories
{
    public class SessionRepositoryConfiguration
    {
        private readonly IConfiguration configuration;
        public SessionRepositoryConfiguration(IConfiguration conf)
        {
            configuration = conf;
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Secrets:SessionKey"] ?? new string('N', 128)));
            Credentials = new SigningCredentials(Key, conf["Secrets:Algo"] ?? SecurityAlgorithms.HmacSha256);
        }

        public SymmetricSecurityKey Key { get; private set; }
        public SigningCredentials Credentials { get; private set; }
        public DateTime? Expires => double.TryParse(configuration["SessionTimeHrs"] ?? "24", out double val)
                    ? DateTime.UtcNow.AddHours(val)
                    : DateTime.UtcNow.AddHours(24);
    }
}
