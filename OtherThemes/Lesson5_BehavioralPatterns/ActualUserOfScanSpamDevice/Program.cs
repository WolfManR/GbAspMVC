using ScannerSpammerDevice_User;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ScannerSpammerDevice;

const string filePath = "File.txt";

string fileContent = @"Image Title=""Spaghetti"";Content=""h4i3p2kn43on34no34in34ni6j56i4j7j57o45j5l23l3l5j2lj636l34j6"";

Note Title=""Eat"";Body=""Try Eat Something"";

Note Title=""Eat"";Body=""Try Eat Something"";

Note Title=""Eat"";Body=""Try Eat Something"";

Image Title=""Bread"";Content=""h4i3p2kn43on34no34in34ni6j56i4j7j57o45j5l23l3l5j2lj636l34j6"";

Image Title=""Eggs"";Content=""h4i3p2kn43on34no34in34ni6j56i4j7j57o45j5l23l3l5j2lj636l34j6"";
";

File.WriteAllText(filePath, fileContent, Encoding.UTF8);

List<ParseHandler> handlers = new List<ParseHandler>()
{
    new ImageParseHandler(),
    new NoteParseHandler()
};

IScanSpamDeviceHandler deviceMonitor = new ScanSpamDeviceUsingHandler(new ConsoleLogger(), handlers);

var consoleDataPublisher = new ConsoleSaveStrategy();
deviceMonitor.DataSaveStrategy = consoleDataPublisher;

var device = new ScanSpamDevice();
deviceMonitor.ConnectDevice(device);
deviceMonitor.StartReadFile(device, filePath);



class ConsoleSaveStrategy : IDataSaveStrategy
{
    public DataSaveResult SaveData(object data)
    {
        // switch data types and ways to save its
        Console.WriteLine(data.ToString());
        return new DataSaveResult();
    }
}

record Image(string Title, string Content);
record Note(string Title, string Body);

class ImageParseHandler : ParseHandler
{
    private const StringComparison Comparison = StringComparison.Ordinal;

    public override (bool CanParse, bool EnoughDataToParse) CanParse(byte[] data)
    {
        var temp = Encoding.UTF8.GetString(data);
        if (temp.StartsWith("Image") || temp.StartsWith("\r\nImage"))
        {
            var lastLineBreakIndex = temp.LastIndexOf("\r\n\r\n", Comparison);
            return (true, lastLineBreakIndex > 0 && lastLineBreakIndex <= temp.Length);
        }
        return (false, false);
    }

    public override object Parse(byte[] data, out int parsedDataSize)
    {
        
        var temp = Encoding.UTF8.GetString(data).AsSpan();

        var start = temp.IndexOf("Image", Comparison);
        var tempBlock = temp.Slice(start, temp.Length - 1);
        var end = tempBlock.IndexOf("\r\n\r\n");

        parsedDataSize = end + 3;

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

class NoteParseHandler : ParseHandler
{
    private const StringComparison Comparison = StringComparison.Ordinal;

    public override (bool CanParse, bool EnoughDataToParse) CanParse(byte[] data)
    {
        var temp = Encoding.UTF8.GetString(data);
        if (temp.StartsWith("Note") || temp.StartsWith("\r\nNote"))
        {
            var lastLineBreakIndex = temp.LastIndexOf("\r\n\r\n", Comparison);
            return (true, lastLineBreakIndex > 0 && lastLineBreakIndex <= temp.Length);
        }
        return (false, false);
    }

    public override object Parse(byte[] data, out int parsedDataSize)
    {
        parsedDataSize = 0;
        return new Note("Not Parsed", "");
    }
}