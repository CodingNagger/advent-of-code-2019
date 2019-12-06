using System;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode_2019
{
    class Day5 : Day
    {
        int dirtyInput = 5;
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

        int[] values;
        int[] baseValues;
        int cursor;

        int latestOutput = -1;

        public override string Compute(string[] input)
        {
            baseValues = ParseInput(input);

            // int expectedOutput = 19690720;

            // for (int i = 0; i < 100; i++) {
            //     for (int j = 0; j < 100; j++) {
            //         noun = i; 
            //         verb = j;

            //         Initialise();
            //         RunIntcodeProgram();

            //         if (values[0] == expectedOutput) goto outside;
            //     }
            // }


            Initialise();
            RunIntcodeProgram();
            return $"{latestOutput}";

            // outside: return $"{(100 * noun) + verb}";
        }

        private void RunIntcodeProgram()
        {
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
            }
        }

        private void JumpIfTrue(Command command)
        {
            if (command.Input1 != 0) {
                cursor = command.Input2;
            }
            else {
                cursor += 3;
            }
        }

        private void JumpIfFalse(Command command)
        {
            if (command.Input1 == 0) {
                cursor = command.Input2;
            }
            else {
                cursor += 3;
            }
        }

        private void LessUpdate(Command command)
        {
            values[command.OutputIndex] = command.Input1 < command.Input2 ? 1 : 0;
            // Console.WriteLine($"Wrote {values[command.OutputIndex]} to {command.OutputIndex}");
        }

        private void EqualsUpdate(Command command)
        {
            values[command.OutputIndex] = command.Input1 == command.Input2 ? 1 : 0;
            // Console.WriteLine($"Wrote {values[command.OutputIndex]} to {command.OutputIndex}");
        }
        private void AddUpdate(Command command)
        {
            values[command.OutputIndex] = command.Input1 + command.Input2;
            // Console.WriteLine($"Wrote {command.Input1} + {command.Input2} = {values[command.OutputIndex]} to {command.OutputIndex}");
        }

        private void MultiplyUpdate(Command command)
        {
            values[command.OutputIndex] = command.Input1 * command.Input2;
            // Console.WriteLine($"Wrote {command.Input1} * {command.Input2} = {values[command.OutputIndex]} to {command.OutputIndex}");
        }

        private void StoreUpdate(Command command)
        {
            values[command.OutputIndex] = command.Input1;
            // Console.WriteLine($"Wrote {values[command.OutputIndex]} to {command.OutputIndex}");
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
            return values[cursor] != HALT;
        }

        private int[] ParseInput(string[] input)
        {
            return input[0].Split(',').Select(s => int.Parse(s.Trim())).ToArray();
        }

        private void Initialise()
        {
            values = new int[baseValues.Length];
            baseValues.CopyTo(values, 0);
            cursor = 0;

            // Console.WriteLine($"{values[225]} - {values[values[225]]}");
            // values[1] = noun;
            // values[2] = verb;
        }

        private Command CreateCommand()
        {
            int opcode = values[cursor] % 100;

            int nextCommandDiff = -1, input1 = -1, input2 = -1, output = -1;
            int param1Mode = (values[cursor] / 100) % 10;
            int param2Mode = (values[cursor] / 1000) % 10;
            int param3Mode = (values[cursor] / 10000) % 10;

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
                input1 = dirtyInput;
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
                output = ValueFor(3, MODE_IMMEDIATE); // <---- wasted 1h this morning and 2 later today not knowing why most example worked but not the input
                // turns out I misread the bit about this MF
                nextCommandDiff = 4;
            }

            return new Command { Opcode = opcode, Input1 = input1, Input2 = input2, OutputIndex = output, NextCommandDiff = nextCommandDiff };
        }

        int ValueFor(int relativePos, int mode)
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
            public int Opcode;
            public int Input1;
            public int Input2;
            public int OutputIndex;
            public int NextCommandDiff;
        }
    }
}
