using System;
using System.Threading;

namespace AdventOfCode2019
{
    public class Day11: TwoPartDay, IIntCodeComputerDelegate
    {
        const char Asteroid = '#';

        string lastPrint = "";

        Robot robot;

        public string Compute(string[] input)
        {
            var brain = new IntCodeComputer(IntCodeProgramParser.Parse(input));
            robot = new Robot(brain, PanelColor.Black);
            brain.AddDelegate(robot);
            brain.SetDatasource(robot);
            
            robot.Run();
            return $"{robot.UniquePanelsPaintedCount}";
        }

        public string ComputePartTwo(string[] input)
        {
            var brain = new IntCodeComputer(IntCodeProgramParser.Parse(input));
            robot = new Robot(brain, PanelColor.White);
            brain.AddDelegate(robot);
            brain.AddDelegate(this);
            brain.SetDatasource(robot);
            
            robot.Run();
            return $"";
        }

        public void HandleOutput(long output)
        {
            Thread.Sleep(30);
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(robot.PaintIdentifier());
        }
    }
}
