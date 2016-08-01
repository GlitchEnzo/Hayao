namespace Vapor
{
    public static class Log
    {
        public static void Info(string message, params object[] args)
        {
            System.Console.ForegroundColor = System.ConsoleColor.Gray;
            System.Console.WriteLine(string.Format(message, args));
        }

        public static void Warning(string message, params object[] args)
        {
            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
            System.Console.WriteLine(string.Format(message, args));
        }

        public static void Error(string message, params object[] args)
        {
            System.Console.ForegroundColor = System.ConsoleColor.Red;
            System.Console.WriteLine(string.Format(message, args));
        }
    }
}
