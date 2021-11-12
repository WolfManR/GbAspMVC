using System;
using System.Collections.Generic;
using ScannerSpammerDevice;

namespace ScannerSpammerDevice_User
{
    public class ScanSpamDeviceUsingHandler : IScanSpamDeviceHandler
    {
        private IScanSpamDevice _scanSpamDevice;
        private readonly ILogger _logger;
        private IDataSaveStrategy _dataSaveStrategy;

        public static Dictionary<Guid, ReadingState> ReadingStates { get; }
        public List<ParseHandler> ParseHandlers { get; }

        public ScanSpamDeviceUsingHandler(ILogger logger)
        {
            _logger = logger;
        }

        public void ConnectDevice(IScanSpamDevice scanSpamDevice)
        {
            _scanSpamDevice = scanSpamDevice;
        }

        public void DisconnectDevice(IScanSpamDevice scanSpamDevice)
        {
            _scanSpamDevice = scanSpamDevice;
        }

        public void ReadFile(string filePath, IDataSaveStrategy dataSaveStrategy)
        {
            _scanSpamDevice.OnFileReadEnd += ScanSpamDeviceOnOnFileReadEnd;
            _scanSpamDevice.OnDataReady += ScanSpamDeviceOnOnDataReady;
            _scanSpamDevice.ReadFile(filePath);
        }

        private void ScanSpamDeviceOnOnFileReadEnd(object? sender, ReadFileEndArgs e)
        {
            if (sender is not IScanSpamDevice device) return;
            device.OnDataReady -= ScanSpamDeviceOnOnDataReady;
            device.OnFileReadEnd -= ScanSpamDeviceOnOnFileReadEnd;
            _logger?.Log($"File read end on path {e.FilePath}");
        }

        private void ScanSpamDeviceOnOnDataReady(object? sender, ReadChunkArgs e)
        {
            _dataSaveStrategy.SaveData(e.Chunk);

            if (sender is IScanSpamDevice device)
            {
                _logger?.Log($"Device process load: {device.ProcessorLoad}, memory load: {device.MemoryLoad}");
            }
        }
    }

    public class ReadingState
    {
        public Guid Id { get; }
        public List<byte> ReadDataCache { get; }

    }

    public abstract class ParseHandler
    {
        public abstract int ExpectedDataSizeToParse { get; }
        public abstract bool CanParse(byte[] data);
        public abstract bool TryParse(byte[] data);
    }
}
