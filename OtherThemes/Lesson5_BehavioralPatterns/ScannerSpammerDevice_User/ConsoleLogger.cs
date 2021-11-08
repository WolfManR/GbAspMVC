using System;

namespace ScannerSpammerDevice_User
{
    internal class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"{message}");
        }
    }
}