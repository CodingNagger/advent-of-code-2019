using System;
using System.IO;
using System.Diagnostics;

namespace adventofcode_2019
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Day day = new Day6();
            // var input = new string[] {"K)L","COM)B","C)D","D)E","E)F","B)G","G)H","D)I","E)J","J)K","B)C" };
            // var input = new string[] {"COM)B","B)C","C)D","D)E","E)F","B)G","G)H","D)I","E)J","J)K","K)L","K)YOU","I)SAN" };
            var input = File.ReadAllLines("./input/day6.txt");
            Console.WriteLine(day.Compute(input));

            stopWatch.Stop();

            DisplayDuration(stopWatch);
        }

        static void DisplayDuration(Stopwatch stopWatch) {
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
        }
    }
}
