using System;

namespace AdventOfCode2019 {
    public class IntCodeComputer
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
        
        public IntCodeComputer(long[] baseValues):this(0, baseValues)
        {
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

        public long Noun { set { values[1] = value; } }
        public long Verb { set { values[2] = value; } }
        public bool NotHalted => !halted;

        public long FirstValue => values[0];

        public long? LatestOutput => latestOutput;

        public long? RunIntcodeProgram(long secondInput = 0)
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
}