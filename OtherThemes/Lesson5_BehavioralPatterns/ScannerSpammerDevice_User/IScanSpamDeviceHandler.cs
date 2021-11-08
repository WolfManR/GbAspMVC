namespace ScannerSpammerDevice_User
{
    public interface IScanSpamDeviceHandler
    {
        void ReadFile(string filePath);
        IScanSpamDeviceReader ConfigureReader();
        IScanSpamDeviceLogger ConfigureLogger();
    }
}