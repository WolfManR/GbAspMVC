namespace ScannerSpammerDevice_User
{
    public interface IScanSpamDeviceLogger
    {
        IScanSpamDeviceLogger UseConsole();
        IScanSpamDeviceReader ConfigureReader();
    }
}