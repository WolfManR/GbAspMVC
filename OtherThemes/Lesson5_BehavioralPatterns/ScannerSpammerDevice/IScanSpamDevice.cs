using System;

namespace ScannerSpammerDevice
{
    public interface IScanSpamDevice
    {
        event EventHandler<ReadChunkArgs> OnDataReady;
        event EventHandler<ReadFileEndArgs> OnFileReadEnd;

        public double ProcessorLoad { get; }
        public double MemoryLoad { get; }
        int BufferSize { get; set; }

        void ReadFile(string filePath);
    }
}