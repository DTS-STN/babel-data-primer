using System;

namespace DataPrimer.Helpers
{
    public class ConsoleLogger : ILogInfo
    {
        public void Print(string message) {
            Console.WriteLine(message);
        }
    }
}