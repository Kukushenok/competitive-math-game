using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repositories.Objects;
using Repositories.Repositories;

namespace RepositoriesRealisation
{
    internal sealed class PrivilegyConnectionStringGetter : IConnectionStringGetter, IRepositoryPrivilegySetting
    {
        private readonly IConfiguration conf;
        private readonly ILogger logger;
        private string connStringName;
        public PrivilegyConnectionStringGetter(IConfiguration conf, string defaultConnStringName, ILogger<PrivilegyConnectionStringGetter> logger)
        {
            this.conf = conf;
            connStringName = defaultConnStringName;
            this.logger = logger;
        }

        public string GetConnectionString()
        {
            return conf.GetConnectionString(connStringName)!;
        }

        public void SetPrivilegies(string privilegy)
        {
            logger.LogInformation($"Using privilegies of {privilegy} for DB");
            connStringName = privilegy;
        }
    }
}
