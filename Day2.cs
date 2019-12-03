using System;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode_2019
{
    class Day2 : Day
    {
        const int ADD = 1;
        const int MULTIPLY = 2;
        const int HALT = 99;

        int[] values;
        int[] baseValues;
        int cursor;

        int noun;
        int verb;

        public override string Compute(string[] input) {
            baseValues = ParseInput(input);

            int expectedOutput = 19690720;

            for (int i = 0; i < 100; i++) {
                for (int j = 0; j < 100; j++) {
                    noun = i; 
                    verb = j;

                    Initialise();
                    RunIntcodeProgram();

                    if (values[0] == expectedOutput) goto outside;
                }
            }

            outside: return $"{(100 * noun) + verb}";
        }

        private void RunIntcodeProgram() {
            while(ShouldContinue()) {
                int command = values[cursor];

                if (command == ADD) {
                    AddUpdate();
                }
                else if (command == MULTIPLY) {
                    MultiplyUpdate();
                }

                StepForward();
            }
        }

        private void AddUpdate() {
            values[values[cursor+3]] = values[values[cursor+1]] + values[values[cursor+2]];
        }

        private void MultiplyUpdate() {
            values[values[cursor+3]] = values[values[cursor+1]] * values[values[cursor+2]];
        }

        private void StepForward() {
            cursor += 4;
        }

        private bool ShouldContinue() {
            return values[cursor] != HALT;
        }

        private int[] ParseInput(string[] input) {
            return input[0].Split(',').Select(s => int.Parse(s.Trim())).ToArray();
        }

        private void Initialise() {
            values = new int[baseValues.Length];
            baseValues.CopyTo(values, 0);
            cursor = 0;
            values[1] = noun;
            values[2] = verb;
        }
    }
}
