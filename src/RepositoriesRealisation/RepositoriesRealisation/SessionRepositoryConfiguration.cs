using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace CompetitiveBackend.Repositories
{
    public class SessionRepositoryConfiguration
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
        public DateTime? Expires { get 
            {
                double val = 24.0;
                double.TryParse(configuration["SessionTimeHrs"] ?? "24", out val);
                return DateTime.UtcNow.AddHours(val);
            }
        }
    }
}
