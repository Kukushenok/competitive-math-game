using CompetitiveBackend.Core.Exceptions.Services;

namespace CompetitiveBackend.Services
{
    public class LogService
    {
    }
    public class LogService<T>: LogService where T: LogService<T>
    {
        protected readonly ILogger<T> _logger;
        public LogService(ILogger<T> logger)
        {
            this._logger = logger;
        }
    }
}
