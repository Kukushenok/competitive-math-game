using MongoDB.Driver;

namespace MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup
{
    public class MongoConnection : IMongoConnection
    {
        public IMongoDatabase Database { get; private set; }

        public IMongoClient Connection { get; private set; }
        //private IClientSessionHandle? handle;
        public MongoConnection(IMongoDatabase database, IMongoClient connection)
        {
            Database = database;
            Connection = connection;
            //handle = database.Client.StartSession();
           // handle.StartTransaction();
        }
        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
            //if(handle != null) await handle.CommitTransactionAsync();

            //handle = null;
        }
        public void Dispose()
        {
            //if (handle != null) handle.AbortTransaction();
            Database = null!;
            Connection = null!;
        }
    }
}
