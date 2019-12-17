using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day17 : TwoPartDay
    {
        public string Compute(string[] input)
        {
            var program = IntCodeProgramParser.Parse(input);
            var robot = new ScaffoldRobot(new IntCodeComputer(program));
            robot.Run();
            return $"{robot.AlignmentParamsSum}";
        }

        public string ComputePartTwo(string[] input)
        {
            var program = IntCodeProgramParser.Parse(input);
            program[0] = 2;
            var robot = new ScaffoldRobot(new IntCodeComputer(program));
            robot.Run();
            return $"{robot.AlignmentParamsSum}";
        }
    }

    public class ScaffoldRobot : IIntCodeComputerDelegate
    {
        private IIntCodeComputer brain;
        private int row;
        private int column;
        private Dictionary<Point, char> map;
        private string optimalPath;
        private long lastOutput;

        public int AlignmentParamsSum => map.Where(m => m.Value == '#').Where(m => IsIntersection(m.Key)).Select(p => p.Key.X * p.Key.Y).Sum();

        public ScaffoldRobot(IIntCodeComputer brain)
        {
            this.brain = brain;
            brain.AddDelegate(this);
            map = new Dictionary<Point, char>();
            lastOutput = -1;
        }

        public void HandleOutput(long output)
        {
            var c = (char)output;
            Console.Write(c);

            var currentPoint = new Point { X = column, Y = row };

            if (c == 10)
            {
                row++;
                column = 0;
            }
            else
            {
                map.Add(currentPoint, c);
                column++;
            }
        }

        public bool IsIntersection(Point p)
        {
            var potentialIntersections = new[] {
                new Point { X = p.X+1, Y = p.Y},
                new Point { X = p.X-1, Y = p.Y},
                new Point { X = p.X, Y = p.Y+1},
                new Point { X = p.X, Y = p.Y-1},
            };

            return potentialIntersections.All(i => map.ContainsKey(i) && map[i] == '#');
        }

        public void Run()
        {
            brain.RunIntcodeProgram();
        }

        public string FindOptimalPath() {
            var path = new List<char>();

            return string.Join(',', path);
        }
    }
}
