using MongoDB.Driver;

namespace Reporter.DbContexts
{
    public class DatabaseConnection
    {
        private readonly IMongoDatabase _database;

        public DatabaseConnection(string connectionString, string database)
        {
            MongoClient client = new(connectionString);
            this._database = client.GetDatabase(database);
        }
    
        public IMongoCollection<TModel> Set<TModel>(string collectionName)
        {
            return _database.GetCollection<TModel>(collectionName);
        }
    }
}