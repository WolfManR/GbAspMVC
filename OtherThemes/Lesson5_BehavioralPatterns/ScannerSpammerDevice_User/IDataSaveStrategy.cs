namespace ScannerSpammerDevice_User
{
    public interface IDataSaveStrategy
    {
        DataSaveResult SaveData(object data);
    }
}