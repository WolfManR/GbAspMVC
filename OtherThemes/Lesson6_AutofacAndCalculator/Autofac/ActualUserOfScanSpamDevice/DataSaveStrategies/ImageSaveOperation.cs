using System;
using ActualUserOfScanSpamDevice.DataModels;

namespace ActualUserOfScanSpamDevice.DataSaveStrategies
{
    class ImageSaveOperation : SaveOperation
    {
        private readonly MongoDbConnection _connection;

        public ImageSaveOperation(MongoDbConnection connection)
        {
            _connection = connection;
        }

        public override SaveDirection SaveDirection { get; } = SaveDirection.Mongo;
        public override Type OperationType { get; } = typeof(Image);

        public override void Save(object entry)
        {
            if (entry is not Image entity) return;

            _connection.Images.InsertOne(entity);
        }
    }
}