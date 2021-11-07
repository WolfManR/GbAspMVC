using System;
using System.IO;

namespace ScannerSpammerDevice
{
    public class ScanSpamDevice : IScanSpamDevice
    {
        private static readonly Random Random = new();

        public event EventHandler<ReadChunkArgs> OnDataReady;
        public event EventHandler<ReadFileEndArgs> OnFileReadEnd;
        public double ProcessorLoad { get; private set; }
        public double MemoryLoad { get; private set; }
        public int BufferSize { get; set; } = 20;

        public void ReadFile(string filePath)
        {
            using var streamReader = new BinaryReader(File.OpenRead(filePath));
            long lastIndex = 0;
            int bufferSize = BufferSize;
            var fileLength = streamReader.BaseStream.Length;
            while (fileLength > lastIndex)
            {
                if (lastIndex + bufferSize > fileLength)
                {
                    checked
                    {
                        bufferSize = (int)(fileLength - lastIndex);
                    }
                }
                var read = streamReader.ReadBytes(bufferSize);
                OnDataReady?.Invoke(this, new ReadChunkArgs(read));
                lastIndex += bufferSize;

                ProcessorLoad = Random.NextDouble();
                MemoryLoad = Random.NextDouble();
            }

            OnFileReadEnd?.Invoke(this, new ReadFileEndArgs(filePath));
        }
    }
}
