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

Note Title=""Write a note"";Body=""Custom not description"";

Note Title=""Read all text"";Body=""Read all notes"";

Image Title=""Bread"";Content=""h4i3p328h09igo23hvo9jw9340vijv48303hbrj84jg9vj4iv43b34ib43jb0934jg436l34j6"";

Image Title=""Eggs"";Content=""h4i3p2kn43on34no34in34ni6j56i4j7onbe9wpogjj84j09vkmv32iu4b39jv2ova8ugl4hvkk84gbjleb349jbn349b4j3b8943bier8943bhefuidbhr9pegjl43hj57o45j5l23l3l5j2lj636l34j6"";

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