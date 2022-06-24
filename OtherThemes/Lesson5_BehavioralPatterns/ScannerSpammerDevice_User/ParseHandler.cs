namespace ScannerSpammerDevice_User
{
    public abstract class ParseHandler
    {
        public abstract (bool CanParse, bool EnoughDataToParse) CanParse(byte[] data);
        public abstract object Parse(byte[] data, out int parsedDataSize);
    }
}