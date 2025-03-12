using CompetitiveBackend.Core.Exceptions.Services;

namespace CompetitiveBackend.Services
{
    public class BaseService
    {
    }
    public class BaseService<T>: BaseService where T: BaseService<T>
    {
        protected readonly ILogger<T> _logger;
        public BaseService(ILogger<T> logger)
        {
            this._logger = logger;
        }
    }
}
