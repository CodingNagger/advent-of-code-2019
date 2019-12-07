using System;

namespace AdventOfCode2019
{
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

        public IntCodeComputer(long[] baseValues) : this(0, baseValues)
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
                long opcode = values[cursor] % 100;

                long input1 = -1, input2 = -1;
                long output = -1;
                long param1Mode = (values[cursor] / 100) % 10;
                long param2Mode = (values[cursor] / 1000) % 10;
                long param3Mode = (values[cursor] / 10000) % 10;

                if (opcode == ADD)
                {
                    input1 = ValueFor(1, param1Mode);
                    input2 = ValueFor(2, param2Mode);
                    output = ValueFor(3, MODE_IMMEDIATE);
                    values[output] = input1 + input2;
                    // Console.WriteLine($"Wrote {input1} + {input2} = {values[output]} to {output}");
                    cursor += 4;
                }
                else if (opcode == MULTIPLY)
                {
                    input1 = ValueFor(1, param1Mode);
                    input2 = ValueFor(2, param2Mode);
                    output = ValueFor(3, MODE_IMMEDIATE);
                    values[output] = input1 * input2;
                    // Console.WriteLine($"Wrote {input1} * {input2} = {values[output]} to {output}");
                    cursor += 4;
                }
                else if (opcode == STORE)
                {
                    input1 = GetDirtyInput();
                    output = ValueFor(1, MODE_IMMEDIATE);
                    values[output] = input1;
                    // Console.WriteLine($"Wrote {values[output]} to {output}");
                    cursor += 2;
                }
                else if (opcode == OUTPUT)
                {
                    input1 = ValueFor(1, param1Mode);
                    Console.WriteLine(input1);
                    latestOutput = input1;
                    cursor += 2;
                    return latestOutput;
                }
                else if (opcode == TRUEJUMP)
                {
                    input1 = ValueFor(1, param1Mode);
                    input2 = ValueFor(2, param2Mode);

                    if (input1 != 0)
                    {
                        cursor = input2;
                    }
                    else
                    {
                        cursor += 3;
                    }
                }
                else if (opcode == FALSEJUMP)
                {
                    input1 = ValueFor(1, param1Mode);
                    input2 = ValueFor(2, param2Mode);

                    if (input1 == 0)
                    {
                        cursor = input2;
                    }
                    else
                    {
                        cursor += 3;
                    }
                }
                else if (opcode == LESS)
                {
                    input1 = ValueFor(1, param1Mode);
                    input2 = ValueFor(2, param2Mode);
                    output = ValueFor(3, MODE_IMMEDIATE);
                    values[output] = input1 < input2 ? 1 : 0;
                    // Console.WriteLine($"Wrote {values[output]} to {output}");
                    cursor += 4;
                }
                else if (opcode == EQUALS)
                {
                    input1 = ValueFor(1, param1Mode);
                    input2 = ValueFor(2, param2Mode);
                    output = ValueFor(3, MODE_IMMEDIATE);
                    
                    values[output] = input1 == input2 ? 1 : 0;
                    // Console.WriteLine($"Wrote {values[output]} to {output}");
                    cursor += 4;
                }
            }

            return latestOutput;
        }

        private bool ShouldContinue()
        {
            halted = values[cursor] == HALT;
            return NotHalted;
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
    }
}