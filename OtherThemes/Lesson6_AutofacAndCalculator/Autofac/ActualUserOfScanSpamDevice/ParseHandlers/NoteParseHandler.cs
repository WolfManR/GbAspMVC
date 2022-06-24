using System;
using System.Collections.Generic;
using System.Text;
using ActualUserOfScanSpamDevice.DataModels;
using ScannerSpammerDevice_User;

namespace ActualUserOfScanSpamDevice.ParseHandlers
{
    class NoteParseHandler : ParseHandler
    {
        private const StringComparison Comparison = StringComparison.Ordinal;

        public override (bool CanParse, bool EnoughDataToParse) CanParse(byte[] data)
        {
            var temp = Encoding.UTF8.GetString(data);
            var markBlock = temp[..(temp.Length > 40 ? 40 : temp.Length)];

            if (!markBlock.Contains("Note")) return (false, false);

            var lastLineBreakIndex = temp.LastIndexOf("\r\n\r\n", Comparison);
            return (true, lastLineBreakIndex > 0 && lastLineBreakIndex <= temp.Length);
        }

        public override object Parse(byte[] data, out int parsedDataSize)
        {
            var temp = Encoding.UTF8.GetString(data).AsSpan();

            var start = temp.IndexOf("Note", Comparison);
            var end = temp.IndexOf("\r\n\r\n");
            var tempBlock = temp.Slice(start, end);

            parsedDataSize = Encoding.UTF8.GetBytes(temp[..(end + start)].ToString()).Length + 4;

            var noteBlock = tempBlock[..end];
            var noteDataBlock = noteBlock[(noteBlock.IndexOf(" ") + 1)..].ToString();

            var noteDataBlocks = noteDataBlock.Split(";", StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, string> noteData = new();

            foreach (var dataBlock in noteDataBlocks)
            {
                var pair = dataBlock.Split("=");
                noteData.Add(pair[0], pair[1].Trim('"'));
            }

            string title = noteData["Title"];
            string body = noteData["Body"];

            return new Note(title, body);
        }
    }
}