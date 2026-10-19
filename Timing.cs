using System.Diagnostics;

namespace BugTrackerApp
{
    public static class Timing
    {
        public static TimeSpan Measure(Action action, string label)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();

            Console.WriteLine($"{label,-26} {stopwatch.Elapsed.TotalMilliseconds,12:N2} ms");
            return stopwatch.Elapsed;
        }

        public static void PrintHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine(new string('=', 70));
            Console.WriteLine(title);
            Console.WriteLine(new string('-', 70));
            Console.WriteLine($"{"Operation",-26}{"Time",12}");
            Console.WriteLine(new string('-', 70));
        }
    }
}
