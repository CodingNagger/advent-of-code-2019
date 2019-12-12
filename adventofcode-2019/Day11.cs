using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public class Day11: TwoPartDay
    {
        const char Asteroid = '#';

        public string Compute(string[] input)
        {
            var brain = new IntCodeComputer(IntCodeProgramParser.Parse(input));
            var robot = new Robot(brain, PanelColor.Black);
            brain.SetDelegate(robot);
            brain.SetDatasource(robot);
            
            robot.Run();
            return $"{robot.UniquePanelsPaintedCount}";
        }

        public string ComputePartTwo(string[] input)
        {
            var brain = new IntCodeComputer(IntCodeProgramParser.Parse(input));
            var robot = new Robot(brain, PanelColor.White);
            brain.SetDelegate(robot);
            brain.SetDatasource(robot);
            
            robot.Run();
            return $"{robot.PaintIdentifier()}";
        }
    }
}
