using System;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public class Day19 : TwoPartDay
    {
        public string Compute(string[] input)
        {
            var program = IntCodeProgramParser.Parse(input);

            var reach = 0L;
            var gridSize = 50;
            var validPoints = new List<Point>();

            // Count reachable
            for (var x = 0; x < gridSize; x++)
            {
                for (var y = 0; y < gridSize; y++)
                {
                    var computer = new IntCodeComputer(x, program);
                    var output = computer.RunIntcodeProgram(y) ?? 0;
                    reach += output;
                    Console.WriteLine($"Output: {x} - {y} - {output} - Reach: {reach}");

                    if (output > 0)
                    {
                        validPoints.Add(new Point { X = x, Y = y });
                    }
                }
            }
            return $"{reach}";
        }

        public string ComputePartTwo(string[] input)
        {
            var program = IntCodeProgramParser.Parse(input);

            var gridSize = 2000;
            var validPoints = new List<Point>();

            // Count reachable
            while (true)
            {
                for (var x = 0; x < gridSize; x++)
                {
                    for (var y = 0; y < gridSize; y++)
                    {
                        var computer = new IntCodeComputer(x, program);
                        var output = computer.RunIntcodeProgram(y) ?? 0;

                        if (output == 1)
                        {
                            computer = new IntCodeComputer(x-99, program);
                            output = computer.RunIntcodeProgram(y+99) ?? 0;

                            if (output == 1) {
                                return $"Closest {(x-99) * 10000 + (y)}";
                            }
                        }
                    }
                }
            }
        }
    }
}