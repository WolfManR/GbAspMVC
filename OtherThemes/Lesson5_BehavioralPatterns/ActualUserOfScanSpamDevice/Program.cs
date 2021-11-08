using System;
using System.Text;
using ScannerSpammerDevice;
using ScannerSpammerDevice_User;

IScanSpamDeviceHandler deviceMonitor = new ScanSpamDeviceUsingHandler(new ScanSpamDevice());
deviceMonitor
    .ConfigureReader().SaveDataWith(new ConsoleSaveStrategy()).SetBufferSize(60)
    .ConfigureLogger().UseConsole();
deviceMonitor.ReadFile("File.txt");


class ConsoleSaveStrategy : IDataSaveStrategy
{
    public DataSaveResult SaveData(byte[] data)
    {
        Console.WriteLine(Encoding.UTF8.GetString(data));
        return new DataSaveResult();
    }
}