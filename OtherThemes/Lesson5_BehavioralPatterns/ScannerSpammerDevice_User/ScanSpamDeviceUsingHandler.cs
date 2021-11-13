using ScannerSpammerDevice;

using System;
using System.Collections.Generic;

namespace ScannerSpammerDevice_User
{
    public class ScanSpamDeviceUsingHandler : IScanSpamDeviceHandler
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<ParseHandler> _parseHandlers;

        private static Dictionary<Guid, ReadingState> ReadingStates { get; set; }
        public IDataSaveStrategy DataSaveStrategy { get; set; }

        public ScanSpamDeviceUsingHandler(ILogger logger, IEnumerable<ParseHandler> parseHandlers)
        {
            _logger = logger;
            _parseHandlers = parseHandlers;
            ReadingStates ??= new Dictionary<Guid, ReadingState>();
        }

        public void ConnectDevice(IScanSpamDevice scanSpamDevice)
        {
            if (!ReadingStates.ContainsKey(scanSpamDevice.Id))
            {
                ReadingStates.TryAdd(scanSpamDevice.Id, new ReadingState());
            }

            scanSpamDevice.OnFileReadEnd += ScanSpamDeviceOnOnFileReadEnd;
            scanSpamDevice.OnDataReady += ScanSpamDeviceOnOnDataReady;
        }

        public void DisconnectDevice(IScanSpamDevice scanSpamDevice)
        {
            scanSpamDevice.OnFileReadEnd -= ScanSpamDeviceOnOnFileReadEnd;
            scanSpamDevice.OnDataReady -= ScanSpamDeviceOnOnDataReady;
        }

        public void StartReadFile(IScanSpamDevice scanSpamDevice, string filePath)
        {
            scanSpamDevice.ReadFile(filePath);
        }

        private void ScanSpamDeviceOnOnFileReadEnd(IScanSpamDevice sender, ReadFileEndArgs e)
        {
            _logger?.Log($"File read end on path {e.FilePath}");
            if (ReadingStates.TryGetValue(sender.Id, out var state))
            {
                state.Reset();
            }
        }

        private void ScanSpamDeviceOnOnDataReady(IScanSpamDevice sender, ReadChunkArgs e)
        {
            _logger?.Log($"Device process load: {sender.ProcessorLoad}, memory load: {sender.MemoryLoad}");

            if (DataSaveStrategy is null)
            {
                DisconnectDevice(sender);
                throw new InvalidOperationException("First set data save strategy");
            }

            object parsedData = null;

            if (ReadingStates.TryGetValue(sender.Id, out var state))
            {
                state.AppendData(e.Chunk);

                if (state.AwaitingDataParseHandler is { } parseHandler)
                {
                    var dataToParse = state.PeekData();
                    var (canParse, enoughDataToParse) = parseHandler.CanParse(dataToParse);
                    if (canParse && enoughDataToParse)
                    {
                        parsedData = parseHandler.Parse(dataToParse, out var parsedDataSize);
                        state.RemoveParsedData(parsedDataSize);
                    }
                }
            }
            else
            {
                state = new ReadingState();
                if (!ReadingStates.TryAdd(sender.Id, state))
                {
                    state = ReadingStates[sender.Id];
                }

                state.AppendData(e.Chunk);
                var dataToParse = e.Chunk;

                foreach (var parseHandler in _parseHandlers)
                {
                    var (canParse, enoughDataToParse) = parseHandler.CanParse(dataToParse);
                    if (!canParse) continue;
                    if (enoughDataToParse)
                    {
                        parsedData = parseHandler.Parse(dataToParse, out var parsedDataSize);
                        state.RemoveParsedData(parsedDataSize);
                        break;
                    }

                    state.AwaitingDataParseHandler = parseHandler;
                    break;
                }

            }

            if (parsedData is not null) DataSaveStrategy.SaveData(parsedData);
        }

        private class ReadingState
        {
            private List<byte> ReadDataCache { get; } = new();
            public ParseHandler AwaitingDataParseHandler { get; set; }

            public void AppendData(byte[] data)
            {
                ReadDataCache.AddRange(data);
            }

            public void RemoveParsedData(int length)
            {
                ReadDataCache.RemoveRange(0, length);
            }

            public byte[] PeekData()
            {
                return ReadDataCache.ToArray();
            }

            public void Reset()
            {
                ReadDataCache.Clear();
            }
        }
    }
}
