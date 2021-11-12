namespace ScannerSpammerDevice_User
{
    public interface IScanSpamDeviceHandler
    {
        void ReadFile(string filePath, IDataSaveStrategy saveStrategy);
    }
}