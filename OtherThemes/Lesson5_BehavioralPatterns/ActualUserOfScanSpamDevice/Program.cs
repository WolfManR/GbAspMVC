using ScannerSpammerDevice_User;

using System;
using System.Text;

IScanSpamDeviceHandler deviceMonitor = new ScanSpamDeviceUsingHandler(new ConsoleLogger());


var consoleDataPublisher = new ConsoleSaveStrategy();
deviceMonitor.ReadFile("File.txt", consoleDataPublisher);


class ConsoleSaveStrategy : IDataSaveStrategy
{
    public DataSaveResult SaveData(byte[] data)
    {
        Console.WriteLine(Encoding.UTF8.GetString(data));
        return new DataSaveResult();
    }
}