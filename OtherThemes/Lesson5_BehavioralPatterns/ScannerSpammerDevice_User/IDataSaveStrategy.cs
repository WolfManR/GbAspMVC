namespace ScannerSpammerDevice_User
{
    public interface IDataSaveStrategy
    {
        DataSaveResult SaveData(byte[] data);
    }
}