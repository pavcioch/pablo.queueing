using System;

namespace pablo.queueing.common
{
    public static class ConsoleLogger
    {
        private static void WriteCurrentTime()
        {
            Console.Write($"@{DateTime.UtcNow.ToString("hh:mm:ss.fff")} > ");
        }

        public static void Log(string message)
        {
            WriteCurrentTime();
            Console.WriteLine(message);
        }

        public static void Log(string format, params object[] args)
        {
            WriteCurrentTime();
            Console.WriteLine(format, args);
        }
    }
}
