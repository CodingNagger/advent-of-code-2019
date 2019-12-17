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
            var computer = new IntCodeComputer(program);
            var inputer = new Inputer(computer);
            computer.SetDatasource(inputer);
            inputer.Run();
            return $"{inputer.Output}";
        }
    }

    public class Inputer: IIntCodeComputerDatasource, IIntCodeComputerDelegate {
        private long[] instructions;
        private int cursor;
        private IIntCodeComputer brain;

        private long lastOutput;

        public Inputer(IIntCodeComputer brain) {
            this.brain = brain;
            brain.AddDelegate(this);
            instructions = " A,B,B,C,A,B,C,A,B,C\nL,6,R,12,L,4,L,6\nR,6,L,6,R,12\nL,6,L,10,L,10,R,6\nn\n".ToCharArray().Select(c => (long) c).ToArray();
            cursor = 0;
        }

        public void Run() {
            brain.RunIntcodeProgram();
        }
        
        public long GetInput() {
            var next = instructions[cursor];
            cursor++;
            return next;
        }

        public void HandleOutput(long output) {
            lastOutput = output;
        }

        public long Output => lastOutput;
    }

    public class ScaffoldRobot : IIntCodeComputerDelegate
    {
        private IIntCodeComputer brain;
        private int row;
        private int column;
        private Dictionary<Point, char> map;

        public int AlignmentParamsSum => map.Where(m => m.Value == '#').Where(m => IsIntersection(m.Key)).Select(p => p.Key.X * p.Key.Y).Sum();

        public ScaffoldRobot(IIntCodeComputer brain)
        {
            this.brain = brain;
            brain.AddDelegate(this);
            map = new Dictionary<Point, char>();
        }

        public void HandleOutput(long output)
        {
            var c = (char) (int) output;

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
    }
}
