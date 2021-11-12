using System;

namespace ScannerSpammerDevice
{
    public interface IScanSpamDevice : IDisposable
    {
        event EventHandler<ReadChunkArgs> OnDataReady;
        event EventHandler<ReadFileEndArgs> OnFileReadEnd;

        public double ProcessorLoad { get; }
        public double MemoryLoad { get; }
        int BufferSize { get; set; }
        bool IsReadFile { get; }

        void ReadFile(string filePath);
    }
}