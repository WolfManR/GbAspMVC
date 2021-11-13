using ScannerSpammerDevice;

namespace ScannerSpammerDevice_User
{
    public interface IScanSpamDeviceHandler
    {
        IDataSaveStrategy DataSaveStrategy { get; set; }

        void ConnectDevice(IScanSpamDevice scanSpamDevice);
        void DisconnectDevice(IScanSpamDevice scanSpamDevice);
        void StartReadFile(IScanSpamDevice scanSpamDevice, string filePath);
    }
}