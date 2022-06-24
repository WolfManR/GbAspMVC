using System;

namespace ActualUserOfScanSpamDevice.DataSaveStrategies
{
    abstract class SaveOperation
    {
        public abstract SaveDirection SaveDirection { get; }
        public abstract Type OperationType { get; }
        public abstract void Save(object entry);
    }
}