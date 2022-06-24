using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActualUserOfScanSpamDevice.DataModels;
using ScannerSpammerDevice_User;

namespace ActualUserOfScanSpamDevice.ParseHandlers
{
    class ImageParseHandler : ParseHandler
    {
        private const StringComparison Comparison = StringComparison.Ordinal;

        public override (bool CanParse, bool EnoughDataToParse) CanParse(byte[] data)
        {
            var temp = Encoding.UTF8.GetString(data);
            var markBlock = temp[..(temp.Length > 40 ? 40 : temp.Length)];

            if (!markBlock.Contains("Image")) return (false, false);

            var lastLineBreakIndex = temp.LastIndexOf("\r\n\r\n", Comparison);
            return (true, lastLineBreakIndex > 0 && lastLineBreakIndex <= temp.Length);
        }

        public override object Parse(byte[] data, out int parsedDataSize)
        {
            var temp = Encoding.UTF8.GetString(data).AsSpan();

            var start = temp.IndexOf("Image", Comparison);
            var end = temp.IndexOf("\r\n\r\n") - 1;
            var tempBlock = temp.Slice(start, end);

            parsedDataSize = Encoding.UTF8.GetBytes(temp[..(end+start)].ToString()).Length + 4;

            var imageBlock = tempBlock[..end];
            var imageDataBlock = imageBlock.ToString().Split(" ").LastOrDefault();
            if (imageDataBlock is null) return null;

            var imageDataBlocks = imageDataBlock.Split(";", StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, string> imageData = new();

            foreach (var dataBlock in imageDataBlocks)
            {
                var pair = dataBlock.Split("=");
                imageData.Add(pair[0], pair[1].Trim('"'));
            }

            string title = imageData["Title"];
            string content = imageData["Content"];

            return new Image(title, content);
        }
    }
}