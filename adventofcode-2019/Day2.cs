using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day2 : Day
    {
        public override string Compute(string[] input) {
           long noun = 0, verb = 0;
           long expectedOutput = 19690720;
           long[] program = IntCodeProgramParser.Parse(input);

            for (noun = 0; noun < 100; noun++) {
                for (verb = 0; verb < 100; verb++) {
                    var computer = new IntCodeComputer(program);
                    computer.Noun = noun;
                    computer.Verb = verb;
                    computer.RunIntcodeProgram();

                    if (computer.FirstValue == expectedOutput) goto outside;
                }
            }

            outside: return $"{(100 * noun) + verb}";
        }
    }
}
