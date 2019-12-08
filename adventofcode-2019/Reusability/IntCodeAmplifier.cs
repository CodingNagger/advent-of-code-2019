using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public class IntCodeAmplifier
    {
        IntCodeComputer[] computers;

        public IntCodeAmplifier(int[] phaseSettings, long[] program)
        {
            computers = phaseSettings.Select(setting => new IntCodeComputer(setting, program)).ToArray();
        }

        public long Run()
        {
            long? output = 0;

            while (computers.Last().ShouldContinue)
            {
                for (var i = 0; i < computers.Length; i++)
                {
                    output = computers[i].RunIntcodeProgram(output.HasValue ? output.Value : 0) ?? 0;
                }
            }

            return output.Value;
        }
    }
}
