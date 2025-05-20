
using Microsoft.Extensions.Logging;
using MongoDBRepositoryRealisation.RepositoriesImplementation;
using MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup;

namespace MongoDBRepositoryRealisation
{
    internal class BaseRepository<T>
    {
        private IMongoConnectionCreator creator;
        private IAutoIncrementManager? _autoIncrementManager;
        protected ILogger<T> _logger;
        public BaseRepository(IMongoConnectionCreator contextFactory, ILogger<T> logger, IAutoIncrementManager? manager = null)
        {
            this.creator = contextFactory;
            this._autoIncrementManager = manager;
            _logger = logger;
        }
        protected Task<IMongoConnection> GetMongoConnection() => Task.FromResult(creator.Create());
        protected async Task<int> NewID(IMongoConnection conn)
        {
            if (_autoIncrementManager == null) throw new NotSupportedException("You should require AutoIncrementManager");
            if (conn == null)
            {
                using var C = creator.Create();
                return await _autoIncrementManager.GetID<T>(C);
            }
            else
                return await _autoIncrementManager.GetID<T>(conn);
        }
    }
}
