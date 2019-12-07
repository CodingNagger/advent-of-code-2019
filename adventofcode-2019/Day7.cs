using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day7 : Day
    {
        long[] baseValues;

        long maxOutput = 0;

        public override string Compute(string[] input)
        {
            // baseValues = ParseInput(input);
            // var a = new IntCodeComputer(5, baseValues);
            // return $"{a.RunIntcodeProgram(0)}";

            baseValues = ParseInput(input);

            int[] phaseSettings = new int[] { 5, 6, 7, 8, 9 };


            foreach (var permuation in phaseSettings.Permutations())
            {
                Console.WriteLine("New permutation");

                var a = new IntCodeComputer(permuation[0], baseValues);
                var b = new IntCodeComputer(permuation[1], baseValues);
                var c = new IntCodeComputer(permuation[2], baseValues);
                var d = new IntCodeComputer(permuation[3], baseValues);
                var e = new IntCodeComputer(permuation[4], baseValues);

                long? outputE = 0;

                while (e.NotHalted) {
                    var outputA = a.RunIntcodeProgram(outputE.HasValue ? outputE.Value : 0 ) ?? 0;
                    var outputB = b.RunIntcodeProgram(outputA) ?? 0;
                    var outputC = c.RunIntcodeProgram(outputB) ?? 0;
                    var outputD = d.RunIntcodeProgram(outputC) ?? 0;
                    outputE = e.RunIntcodeProgram(outputD);
                }

                if (outputE.HasValue) {
                     maxOutput = Math.Max(maxOutput, outputE.Value);
                }
               
            }

            return $"{maxOutput}";
        }

        private long[] ParseInput(string[] input)
        {
            return input[0].Split(',').Select(s => long.Parse(s.Trim())).ToArray();
        }
    }

    class IntCodeComputer
    {
        public IntCodeComputer(int phaseSetting, long[] baseValues)
        {
            this.phaseSetting = phaseSetting;
            this.values = new long[baseValues.Length];
            baseValues.CopyTo(this.values, 0);
            cursor = 0;
            sentPhaseSetting = false;
            halted = false;
        }

        long? latestOutput = null;
        long[] values;
        long cursor;
        const int ADD = 1;
        const int MULTIPLY = 2;
        const int STORE = 3;
        const int OUTPUT = 4;
        const int TRUEJUMP = 5;
        const int FALSEJUMP = 6;
        const int LESS = 7;
        const int EQUALS = 8;

        const int HALT = 99;

        const int MODE_POSITION = 0;
        const int MODE_IMMEDIATE = 1;

        int phaseSetting;
        long secondInput;
        bool sentPhaseSetting;

        private bool halted;
        public bool NotHalted => !halted;

        public long? RunIntcodeProgram(long secondInput)
        {
            this.secondInput = secondInput;

            while (ShouldContinue())
            {
                // Console.WriteLine($"Index {cursor} - {values[cursor]}");
                Command command = CreateCommand();
                // Console.WriteLine($"Opcode: {command.Opcode}");

                if (command.Opcode == ADD)
                {
                    AddUpdate(command);
                }
                else if (command.Opcode == MULTIPLY)
                {
                    MultiplyUpdate(command);
                }
                else if (command.Opcode == STORE)
                {
                    StoreUpdate(command);
                }
                else if (command.Opcode == OUTPUT)
                {
                    OutputParameter(command);
                }
                else if (command.Opcode == TRUEJUMP)
                {
                    JumpIfTrue(command);
                }
                else if (command.Opcode == FALSEJUMP)
                {
                    JumpIfFalse(command);
                }
                else if (command.Opcode == LESS)
                {
                    LessUpdate(command);
                }
                else if (command.Opcode == EQUALS)
                {
                    EqualsUpdate(command);
                }

                StepForward(command);

                if (command.Opcode == OUTPUT)
                {
                    return latestOutput;
                }
            }

            return latestOutput;
        }

        private void JumpIfTrue(Command command)
        {
            if (command.Input1 != 0)
            {
                cursor = command.Input2;
            }
            else
            {
                cursor += 3;
            }
        }

        private void JumpIfFalse(Command command)
        {
            if (command.Input1 == 0)
            {
                cursor = command.Input2;
            }
            else
            {
                cursor += 3;
            }
        }

        private void LessUpdate(Command command)
        {
            values[command.OutputIndex] = command.Input1 < command.Input2 ? 1 : 0;
            Console.WriteLine($"Wrote {values[command.OutputIndex]} to {command.OutputIndex}");
        }

        private void EqualsUpdate(Command command)
        {
            values[command.OutputIndex] = command.Input1 == command.Input2 ? 1 : 0;
            Console.WriteLine($"Wrote {values[command.OutputIndex]} to {command.OutputIndex}");
        }
        private void AddUpdate(Command command)
        {
            values[command.OutputIndex] = command.Input1 + command.Input2;
            Console.WriteLine($"Wrote {command.Input1} + {command.Input2} = {values[command.OutputIndex]} to {command.OutputIndex}");
        }

        private void MultiplyUpdate(Command command)
        {
            values[command.OutputIndex] = command.Input1 * command.Input2;
            Console.WriteLine($"Wrote {command.Input1} * {command.Input2} = {values[command.OutputIndex]} to {command.OutputIndex}");
        }

        private void StoreUpdate(Command command)
        {
            values[command.OutputIndex] = command.Input1;
            Console.WriteLine($"Wrote {values[command.OutputIndex]} to {command.OutputIndex}");
        }

        private void OutputParameter(Command command)
        {
            Console.WriteLine(command.Input1);
            latestOutput = command.Input1;
        }

        private void StepForward(Command command)
        {
            // Console.WriteLine($"move to {cursor + command.NextCommandDiff} - +{command.NextCommandDiff}");
            cursor += command.NextCommandDiff;
        }

        private bool ShouldContinue()
        {
            halted = values[cursor] == HALT;
            return NotHalted;
        }

        private Command CreateCommand()
        {
            long opcode = values[cursor] % 100;

            long input1 = -1, input2 = -1;
            long nextCommandDiff = -1, output = -1;
            long param1Mode = (values[cursor] / 100) % 10;
            long param2Mode = (values[cursor] / 1000) % 10;
            long param3Mode = (values[cursor] / 10000) % 10;

            if (opcode == ADD || opcode == MULTIPLY)
            {
                nextCommandDiff = 4;
                input1 = ValueFor(1, param1Mode);
                input2 = ValueFor(2, param2Mode);
                output = ValueFor(3, MODE_IMMEDIATE);
            }
            else if (opcode == STORE)
            {
                nextCommandDiff = 2;
                input1 = GetDirtyInput();
                output = ValueFor(1, MODE_IMMEDIATE);
            }
            else if (opcode == OUTPUT)
            {
                nextCommandDiff = 2;
                input1 = ValueFor(1, param1Mode);
            }
            else if (opcode == TRUEJUMP || opcode == FALSEJUMP)
            {
                input1 = ValueFor(1, param1Mode);
                input2 = ValueFor(2, param2Mode);
                nextCommandDiff = 0;
            }
            else if (opcode == LESS || opcode == EQUALS)
            {
                input1 = ValueFor(1, param1Mode);
                input2 = ValueFor(2, param2Mode);
                output = ValueFor(3, MODE_IMMEDIATE);
                nextCommandDiff = 4;
            }

            return new Command { Opcode = opcode, Input1 = input1, Input2 = input2, OutputIndex = output, NextCommandDiff = nextCommandDiff };
        }

        long GetDirtyInput()
        {
            if (sentPhaseSetting)
            {
                return secondInput;
            }

            sentPhaseSetting = true;
            return phaseSetting;
        }

        long ValueFor(int relativePos, long mode)
        {
            if (mode == MODE_POSITION)
            {
                return values[values[cursor + relativePos]];
            }
            else if (mode == MODE_IMMEDIATE)
            {
                return values[cursor + relativePos];
            }

            throw new ArgumentException($"Why you do this? Mode: {mode}");
        }

        struct Command
        {
            public long Opcode;
            public long Input1;
            public long Input2;
            public long OutputIndex;
            public long NextCommandDiff;
        }
    }

    public static class PermutationExtension
    {
        public static IEnumerable<T[]> Permutations<T>(this IEnumerable<T> source)
        {
            var sourceArray = source.ToArray();
            var results = new List<T[]>();
            Permute(sourceArray, 0, sourceArray.Length - 1, results);
            return results;
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }

        private static void Permute<T>(T[] elements, int recursionDepth, int maxDepth, ICollection<T[]> results)
        {
            if (recursionDepth == maxDepth)
            {
                results.Add(elements.ToArray());
                return;
            }

            for (var i = recursionDepth; i <= maxDepth; i++)
            {
                Swap(ref elements[recursionDepth], ref elements[i]);
                Permute(elements, recursionDepth + 1, maxDepth, results);
                Swap(ref elements[recursionDepth], ref elements[i]);
            }
        }
    }
}
