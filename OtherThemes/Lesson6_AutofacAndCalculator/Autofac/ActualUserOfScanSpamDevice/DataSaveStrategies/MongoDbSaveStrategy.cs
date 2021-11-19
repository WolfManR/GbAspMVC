using System;
using System.Collections.Generic;
using System.Linq;
using ScannerSpammerDevice_User;

namespace ActualUserOfScanSpamDevice.DataSaveStrategies
{
    class MongoDbSaveStrategy : IDataSaveStrategy
    {
        private readonly Dictionary<Type, SaveOperation> _saveOperations;
        
        public MongoDbSaveStrategy(IEnumerable<SaveOperation> saveOperations)
        {
            _saveOperations = saveOperations.Where(o=>o.SaveDirection == SaveDirection.Mongo).ToDictionary(o => o.OperationType);
        }

        public DataSaveResult SaveData(object data)
        {
            if (!_saveOperations.TryGetValue(data.GetType(), out var saveOperation)) return new DataSaveResult(false);
            saveOperation.Save(data);
            return new DataSaveResult();
        }
    }
}