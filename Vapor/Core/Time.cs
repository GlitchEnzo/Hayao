namespace Vapor
{
    using System.Diagnostics;

    public class Time
    {
        public static float DeltaTime;

        private static Stopwatch stopwatch = new Stopwatch();

        public static void Update()
        {
            stopwatch.Stop();

            DeltaTime = stopwatch.ElapsedMilliseconds / 1000.0f;

            stopwatch.Restart();
        }
    }
}
