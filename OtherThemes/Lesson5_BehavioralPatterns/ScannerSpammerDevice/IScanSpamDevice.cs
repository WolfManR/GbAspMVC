using System;

namespace ScannerSpammerDevice
{
    public interface IScanSpamDevice : IDisposable
    {
        event Action<IScanSpamDevice, ReadChunkArgs> OnDataReady;
        event Action<IScanSpamDevice, ReadFileEndArgs> OnFileReadEnd;

        public double ProcessorLoad { get; }
        public double MemoryLoad { get; }
        int BufferSize { get; set; }
        bool IsReadFile { get; }
        Guid Id { get; }

        void ReadFile(string filePath);
    }
}