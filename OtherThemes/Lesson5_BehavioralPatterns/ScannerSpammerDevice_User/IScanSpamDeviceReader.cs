namespace ScannerSpammerDevice_User
{
    public interface IScanSpamDeviceReader
    {
        IScanSpamDeviceReader SaveDataWith(IDataSaveStrategy saveStrategy);
        IScanSpamDeviceReader SetBufferSize(int bufferSize = 20);
        IScanSpamDeviceLogger ConfigureLogger();
    }
}