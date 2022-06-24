using System;
using System.IO;
using System.Threading;

namespace ScannerSpammerDevice
{
    public class ScanSpamDevice : IScanSpamDevice
    {
        private static readonly Random Random = new();
        private BinaryReader _reader;
        private string _readingFilePath;

        private Timer _busyTimer;

        public ScanSpamDevice()
        {
            Id = Guid.NewGuid();
        }

        public event Action<IScanSpamDevice, ReadChunkArgs> OnDataReady;
        public event Action<IScanSpamDevice, ReadFileEndArgs> OnFileReadEnd;
        public double ProcessorLoad { get; private set; }
        public double MemoryLoad { get; private set; }
        public int BufferSize { get; set; } = 20;
        public bool IsReadFile { get; private set; }
        public Guid Id { get; }

        public void ReadFile(string filePath)
        {
            _busyTimer ??= new Timer(_ => UpdateSelfState(), null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));

            IsReadFile = true;

            _readingFilePath = filePath;
            try
            {
                _reader = new BinaryReader(File.OpenRead(filePath));
                long lastIndex = 0;
                int bufferSize = BufferSize;
                var fileLength = _reader.BaseStream.Length;
                while (fileLength > lastIndex)
                {
                    Thread.Sleep(200); // For Debug
                    if (lastIndex + bufferSize > fileLength)
                    {
                        checked
                        {
                            bufferSize = (int)(fileLength - lastIndex);
                        }
                    }
                    var read = _reader.ReadBytes(bufferSize);
                    // TODO: what to do if nobody subscribe(cache in memory or save read state and close streams)
                    OnDataReady?.Invoke(this, new ReadChunkArgs(read));
                    lastIndex += bufferSize;
                }
            }
            finally
            {
                IsReadFile = false;
                _reader.Dispose();
                _reader = null;
                _readingFilePath = null;
                OnFileReadEnd?.Invoke(this, new ReadFileEndArgs(filePath));
            }
        }

        private void UpdateSelfState()
        {
            if (!IsReadFile)
            {
                ProcessorLoad = 0;
                MemoryLoad = 0;
                return;
            }

            ProcessorLoad = GetRandomDouble();
            MemoryLoad = GetRandomDouble();
        }

        private double GetRandomDouble()
        {
            return Random.NextDouble() * 100; ;
        }

        public void Dispose()
        {
            _busyTimer?.Dispose();
            _reader.Dispose();
            OnFileReadEnd?.Invoke(this, new ReadFileEndArgs(_readingFilePath));
        }
    }
}
