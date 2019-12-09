using System;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public class IntCodeComputer
    {
        Queue<long> inputs;

        long? latestOutput;
        long[] program;
        long cursor;

        public long Noun { set { program[1] = value; } }
        public long Verb { set { program[2] = value; } }
        public bool ShouldContinue => (OperationType) program[cursor] != OperationType.Halt;

        public long FirstValue => program[0];

        public long? LatestOutput => latestOutput;

        public IntCodeComputer(int phaseSetting, long[] program)
        {
            this.program = new long[program.Length];
            program.CopyTo(this.program, 0);
            cursor = 0;
            latestOutput = null;
            inputs = new Queue<long>();
            inputs.Enqueue(phaseSetting);
        }

        public IntCodeComputer(long[] baseValues) : this(0, baseValues)
        {
        }

        public long? RunIntcodeProgram(long nextInput = 0)
        {
            this.inputs.Enqueue(nextInput);

            while (ShouldContinue)
            {
                var operationType = (OperationType) (program[cursor] % 100);
                var param1Mode = (ValueMode) ((program[cursor] / 100) % 10);
                var param2Mode = (ValueMode) ((program[cursor] / 1000) % 10);
                var param3Mode = (ValueMode) ((program[cursor] / 10000) % 10);

                if (operationType == OperationType.Add)
                {
                    program[ValueFor(3, ValueMode.Immediate)] = ValueFor(1, param1Mode) + ValueFor(2, param2Mode);
                    cursor += 4;
                }
                else if (operationType ==  OperationType.Multiply)
                {
                    program[ValueFor(3, ValueMode.Immediate)] = ValueFor(1, param1Mode) * ValueFor(2, param2Mode);
                    cursor += 4;
                }
                else if (operationType == OperationType.Store)
                {
                    program[ValueFor(1, ValueMode.Immediate)] =  GetDirtyInput();
                    cursor += 2;
                }
                else if (operationType == OperationType.Output)
                {
                    latestOutput = ValueFor(1, param1Mode);
                    cursor += 2;
                    return latestOutput;
                }
                else if (operationType == OperationType.JumpIfTrue)
                {
                    cursor = ValueFor(1, param1Mode) != 0 ? ValueFor(2, param2Mode) : (cursor + 3);
                }
                else if (operationType == OperationType.JumpIfFalse)
                {
                    cursor = ValueFor(1, param1Mode) == 0 ? ValueFor(2, param2Mode) : (cursor + 3);
                }
                else if (operationType == OperationType.Less)
                {
                    program[ValueFor(3, ValueMode.Immediate)] = ValueFor(1, param1Mode) < ValueFor(2, param2Mode) ? 1 : 0;
                    cursor += 4;
                }
                else if (operationType == OperationType.Equals)
                {
                    program[ValueFor(3, ValueMode.Immediate)] = ValueFor(1, param1Mode) == ValueFor(2, param2Mode) ? 1 : 0;
                    cursor += 4;
                }
            }

            return latestOutput;
        }

        long GetDirtyInput()
        {
            return inputs.Dequeue();
        }

        long ValueFor(int relativePos, ValueMode mode)
        {
            if (mode == ValueMode.Position)
            {
                return program[program[cursor + relativePos]];
            }
            else if (mode == ValueMode.Immediate)
            {
                return program[cursor + relativePos];
            }

            throw new ArgumentException($"Why you do this? Mode: {mode}");
        }

        enum ValueMode {
            Position = 0,
            Immediate = 1
        }

        enum OperationType {
            Add = 1,
            Multiply = 2,
            Store = 3,
            Output = 4,
            JumpIfTrue = 5,
            JumpIfFalse = 6,
            Less = 7,
            Equals = 8,
            Halt = 99
        }
    }
}