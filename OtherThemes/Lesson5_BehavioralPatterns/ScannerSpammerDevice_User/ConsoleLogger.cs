using System;

namespace ScannerSpammerDevice_User
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"{message}");
        }
    }
}