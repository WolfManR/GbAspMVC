using ScannerSpammerDevice;

namespace ScannerSpammerDevice_User
{
    public class ScanSpamDeviceUsingHandler : IScanSpamDeviceHandler, IScanSpamDeviceReader, IScanSpamDeviceLogger
    {
        private readonly IScanSpamDevice _scanSpamDevice;
        private ILogger _logger;
        private IDataSaveStrategy _dataSaveStrategy;

        public ScanSpamDeviceUsingHandler(IScanSpamDevice scanSpamDevice)
        {
            _scanSpamDevice = scanSpamDevice;
        }

        public void ReadFile(string filePath)
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

        public IScanSpamDeviceReader ConfigureReader() => this;
        public IScanSpamDeviceReader SetBufferSize(int bufferSize = 20)
        {
            _scanSpamDevice.BufferSize = bufferSize;
            return this;
        }

        public IScanSpamDeviceLogger ConfigureLogger() => this;
        
        public IScanSpamDeviceLogger UseConsole()
        {
            _logger = new ConsoleLogger();
            return this;
        }

        public IScanSpamDeviceReader SaveDataWith(IDataSaveStrategy saveStrategy)
        {
            _dataSaveStrategy = saveStrategy;
            return this;
        }
    }
}
