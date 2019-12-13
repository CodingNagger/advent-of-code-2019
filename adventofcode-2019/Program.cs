using System;
using System.IO;
using System.Diagnostics;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            TwoPartDay day = new Day13();
            var input = File.ReadAllLines("./input/day13.txt");     
            // Console.WriteLine(day.Compute(input));  
            Console.WriteLine(day.ComputePartTwo(input));

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
