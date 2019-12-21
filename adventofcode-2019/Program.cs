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

            TwoPartDay day = new Day18();
            // var input = File.ReadAllLines("./input/day18.txt");
            
            // var input = new [] { "#########","#b.A.@.a#","#########" };
            var input = new [] { "########################","#f.D.E.e.C.b.A.@.a.B.c.#","######################.#","#d.....................#","########################" };
            //  var input = new [] {"########################","#...............b.C.D.f#","#.######################","#.....@.a.B.c.d.A.e.F.g#","########################"};
            // var input = new [] { "#################","#i.G..c...e..H.p#","########.########","#j.A..b...f..D.o#","########@########","#k.E..a...g..B.n#","########.########","#l.F..d...h..C.m#","#################" };
            // var input = new [] { "########################","#@..............ac.GI.b#","###d#e#f################","###A#B#C################","###g#h#i################","########################" };
            Console.WriteLine(day.Compute(input));  
            // Console.WriteLine(day.ComputePartTwo(input));  

            stopWatch.Stop();

            DisplayDuration(stopWatch);
        }

        static void DisplayDuration(Stopwatch stopWatch) {
            Console.WriteLine("RunTime " + DisplayUtils.DisplayValue(stopWatch));
        }
    }
}
