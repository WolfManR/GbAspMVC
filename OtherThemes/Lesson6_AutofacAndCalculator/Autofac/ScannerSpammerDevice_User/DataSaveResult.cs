using System.Collections.Generic;

namespace ScannerSpammerDevice_User
{
    public class DataSaveResult
    {
        public DataSaveResult(bool isSuccess = true)
        {
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; }
        public List<FailureInfo> Failures { get; set; }
    }
}