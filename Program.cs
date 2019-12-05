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

            Day day = new Day5();
            // var input = new string[] { "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99" };
            var input = File.ReadAllLines("./input/day5.txt");
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
