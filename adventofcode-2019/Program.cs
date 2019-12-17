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

            TwoPartDay day = new Day14();
            var input = File.ReadAllLines("./input/day14.txt");
            // Console.WriteLine(day.Compute(input));  
            Console.WriteLine(day.ComputePartTwo(input));  

            stopWatch.Stop();

            DisplayDuration(stopWatch);
        }

        static void DisplayDuration(Stopwatch stopWatch) {
            Console.WriteLine("RunTime " + DisplayUtils.DisplayValue(stopWatch));
        }
    }
}
