using ActualUserOfScanSpamDevice.DataModels;
using MongoDB.Driver;

namespace ActualUserOfScanSpamDevice.DataSaveStrategies
{
    sealed class MongoDbConnection
    {
        private const string ConnectionString = "mongodb://root:pass12345@localhost:27017";
        private readonly IMongoDatabase _database;

        public MongoDbConnection()
        {
            var mongoClient = new MongoClient(ConnectionString);
            _database = mongoClient.GetDatabase("ScanSpam");
        }

        public IMongoCollection<Image> Images => GetCollection<Image>(nameof(Image));
        public IMongoCollection<Note> Notes => GetCollection<Note>(nameof(Note));

        private IMongoCollection<T> GetCollection<T>(string name)
        {
            var collection = _database.GetCollection<T>(name.ToLowerInvariant(), new MongoCollectionSettings() { AssignIdOnInsert = true });
            return collection;
        }
    }
}