using System;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public class IntCodeComputer
    {
        public IntCodeComputer(int phaseSetting, long[] baseValues)
        {
            this.values = new long[baseValues.Length];
            baseValues.CopyTo(this.values, 0);
            cursor = 0;
            latestOutput = null;
            inputs = new Queue<long>();
            inputs.Enqueue(phaseSetting);
        }

        public IntCodeComputer(long[] baseValues) : this(0, baseValues)
        {
        }

        Queue<long> inputs;

        long? latestOutput;
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

        public long Noun { set { values[1] = value; } }
        public long Verb { set { values[2] = value; } }
        public bool ShouldContinue => values[cursor] != HALT;

        public long FirstValue => values[0];

        public long? LatestOutput => latestOutput;

        public long? RunIntcodeProgram(long nextInput = 0)
        {
            this.inputs.Enqueue(nextInput);

            while (ShouldContinue)
            {
                long opcode = values[cursor] % 100;
                long param1Mode = (values[cursor] / 100) % 10;
                long param2Mode = (values[cursor] / 1000) % 10;
                long param3Mode = (values[cursor] / 10000) % 10;

                if (opcode == ADD)
                {
                    values[ValueFor(3, MODE_IMMEDIATE)] = ValueFor(1, param1Mode) + ValueFor(2, param2Mode);
                    cursor += 4;
                }
                else if (opcode == MULTIPLY)
                {
                    values[ValueFor(3, MODE_IMMEDIATE)] = ValueFor(1, param1Mode) * ValueFor(2, param2Mode);
                    cursor += 4;
                }
                else if (opcode == STORE)
                {
                    values[ValueFor(1, MODE_IMMEDIATE)] =  GetDirtyInput();
                    cursor += 2;
                }
                else if (opcode == OUTPUT)
                {
                    latestOutput = ValueFor(1, param1Mode);
                    cursor += 2;
                    return latestOutput;
                }
                else if (opcode == TRUEJUMP)
                {
                    cursor = ValueFor(1, param1Mode) != 0 ? ValueFor(2, param2Mode) : (cursor + 3);
                }
                else if (opcode == FALSEJUMP)
                {
                    cursor = ValueFor(1, param1Mode) == 0 ? ValueFor(2, param2Mode) : (cursor + 3);
                }
                else if (opcode == LESS)
                {
                    values[ValueFor(3, MODE_IMMEDIATE)] = ValueFor(1, param1Mode) < ValueFor(2, param2Mode) ? 1 : 0;
                    cursor += 4;
                }
                else if (opcode == EQUALS)
                {
                    values[ValueFor(3, MODE_IMMEDIATE)] = ValueFor(1, param1Mode) == ValueFor(2, param2Mode) ? 1 : 0;
                    cursor += 4;
                }
            }

            return latestOutput;
        }

        long GetDirtyInput()
        {
            return inputs.Dequeue();
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