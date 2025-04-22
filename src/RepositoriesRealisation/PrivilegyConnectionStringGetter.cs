using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repositories.Objects;
using Repositories.Repositories;

namespace RepositoriesRealisation
{
    internal class PrivilegyConnectionStringGetter: IConnectionStringGetter, IRepositoryPrivilegySetting
    {
        private IConfiguration conf;
        private ILogger _logger;
        private string connStringName;
        public PrivilegyConnectionStringGetter(IConfiguration conf, string defaultConnStringName, ILogger<PrivilegyConnectionStringGetter> logger)
        {
            this.conf = conf;
            this.connStringName = defaultConnStringName;
            _logger = logger;
        }
        public string GetConnectionString()
        {
            return conf.GetConnectionString(connStringName)!;
        }

        public void SetPrivilegies(string privilegy)
        {
            _logger.LogInformation($"Using privilegies of {privilegy} for DB");
            connStringName = privilegy;
        }
    }
}
