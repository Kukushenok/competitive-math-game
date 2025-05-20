using MongoDB.Driver;

namespace MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup
{
    public interface IMongoConnection: IDisposable
    {
        public IMongoDatabase Database { get; }
        public IMongoClient Connection { get; }
        public Task SaveChangesAsync();
    }
}
