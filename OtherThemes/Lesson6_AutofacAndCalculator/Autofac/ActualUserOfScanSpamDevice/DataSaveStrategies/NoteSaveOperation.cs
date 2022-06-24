using System;
using ActualUserOfScanSpamDevice.DataModels;

namespace ActualUserOfScanSpamDevice.DataSaveStrategies
{
    sealed class NoteSaveOperation : SaveOperation
    {
        private readonly MongoDbConnection _connection;

        public NoteSaveOperation(MongoDbConnection connection)
        {
            _connection = connection;
        }

        public override SaveDirection SaveDirection { get; } = SaveDirection.Mongo;
        public override Type OperationType { get; } = typeof(Note);

        public override void Save(object entry)
        {
            if (entry is not Note entity) return;

            _connection.Notes.InsertOne(entity);
        }
    }
}