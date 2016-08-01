namespace Vapor
{
    public static class Log
    {
        public static void Info(string message, params object[] args)
        {
            System.Console.WriteLine(string.Format(message, args));
        }

        public static void Error(string message, params object[] args)
        {
            System.Console.WriteLine(string.Format(message, args));
        }
    }
}
