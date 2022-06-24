using ActualUserOfScanSpamDevice.DataModels;
using MongoDB.Driver;

namespace ActualUserOfScanSpamDevice.DataSaveStrategies
{
    sealed class MongoDbConnection
    {
        private readonly IMongoDatabase _database;

        public MongoDbConnection(string connectionString)
        {
            var mongoClient = new MongoClient(connectionString);
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