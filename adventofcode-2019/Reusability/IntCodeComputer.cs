using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public interface IIntCodeComputer
    {
        public long? RunIntcodeProgram(long nextInput = 0);

        public bool ShouldContinue { get; }

        public string StringOutput { get; }

        public long? LatestOutput { get; }
    }

    public interface IIntCodeComputerDelegate {
        long GetInput();

        void HandleOutput(long output);
    }

    public class IntCodeComputer : IIntCodeComputer
    {
        Queue<long> inputs;

        List<long?> outputs;
        long[] program;
        long cursor;
        long relativeBase;
        bool continuousOutput;

        IIntCodeComputerDelegate aDelegate;

        public long Noun { set { program[1] = value; } }
        public long Verb { set { program[2] = value; } }
        public bool ShouldContinue => (OperationType) program[cursor] != OperationType.Halt;

        public long FirstValue => program[0];

        public long? LatestOutput => outputs.LastOrDefault();

        public string StringOutput => string.Join(',', outputs.ToArray());

        public IntCodeComputer(int phaseSetting, long[] program, bool continuousOutput)
        {
            this.program = new long[program.Length];
            program.CopyTo(this.program, 0);
            cursor = 0;
            outputs = new List<long?>();
            inputs = new Queue<long>();
            inputs.Enqueue(phaseSetting);
            relativeBase = 0;
            this.continuousOutput = continuousOutput;
        }

        public IntCodeComputer(int phaseSetting, long[] program): this(phaseSetting, program, false)
        {
        }

        public IntCodeComputer(long[] baseValues) : this(0, baseValues)
        {
        }

        public void SetDelegate(IIntCodeComputerDelegate aDelegate) {
            inputs.Clear();
            this.aDelegate = aDelegate;
        }

        public long? RunIntcodeProgram(long nextInput = 0)
        {
            this.inputs.Enqueue(aDelegate?.GetInput() ?? nextInput);

            while (ShouldContinue)
            {
                var operationType = (OperationType) (program[cursor] % 100);
                var param1Mode = (ValueMode) ((program[cursor] / 100) % 10);
                var param2Mode = (ValueMode) ((program[cursor] / 1000) % 10);
                var param3Mode = (ValueMode) ((program[cursor] / 10000) % 10);

                if (operationType == OperationType.Add)
                {
                    Write(IndexFor(3, param3Mode), ValueFor(1, param1Mode) + ValueFor(2, param2Mode));
                    cursor += 4;
                }
                else if (operationType ==  OperationType.Multiply)
                {
                    Write(IndexFor(3, param3Mode), ValueFor(1, param1Mode) * ValueFor(2, param2Mode));
                    cursor += 4;
                }
                else if (operationType == OperationType.Store)
                {
                    Write(IndexFor(1, param1Mode),  GetDirtyInput());
                    cursor += 2;
                }
                else if (operationType == OperationType.Output)
                {
                    var output = ValueFor(1, param1Mode);
                    outputs.Add(output);
                    aDelegate?.HandleOutput(output);

                    cursor += 2;
                    
                    if (continuousOutput) return LatestOutput;
                }
                else if (operationType == OperationType.JumpIfTrue)
                {
                    cursor = ValueFor(1,  param1Mode) != 0 ? ValueFor(2, param2Mode) : (cursor + 3);
                }
                else if (operationType == OperationType.JumpIfFalse)
                {
                    cursor = ValueFor(1, param1Mode) == 0 ? ValueFor(2, param2Mode) : (cursor + 3);
                }
                else if (operationType == OperationType.Less)
                {
                    Write(IndexFor(3, param3Mode),  ValueFor(1, param1Mode) < ValueFor(2, param2Mode) ? 1 : 0);
                    cursor += 4;
                }
                else if (operationType == OperationType.Equals)
                {
                    Write(IndexFor(3, param3Mode),  ValueFor(1, param1Mode) == ValueFor(2, param2Mode) ? 1 : 0);
                    cursor += 4;
                }
                else if (operationType == OperationType.AdjustRelativeOffset) {
                    relativeBase += ValueFor(1, param1Mode);
                    cursor += 2;
                }
            }

            return LatestOutput;
        }

        void Write(long index, long entry) {
            if (IndexDoesntExist(index)) {
                IncreaseMemoryFor(index);
            }

            program[index] = entry;
        }

        long Read(long index) {
            if (IndexDoesntExist(index)) {
                return 0;
            }

            return program[index];
        }

        bool IndexDoesntExist(long index) {
            return index >= program.LongLength;
        }

        void IncreaseMemoryFor(long wishedIndex) {
            long[] biggerProgram = new long[wishedIndex * 10];
            program.CopyTo(biggerProgram, 0);
            program = biggerProgram;
        }

        long GetDirtyInput()
        {
            return aDelegate?.GetInput() ?? inputs.Dequeue();
        }

        long ValueFor(int relativePos, ValueMode mode)
        {
            return Read(IndexFor(relativePos, mode));
        }

        long IndexFor(int relativePos, ValueMode mode) {
             if (mode == ValueMode.Position)
            {
                return program[cursor + relativePos];
            }
            else if (mode == ValueMode.Immediate)
            {
                return cursor + relativePos;
            }
            else if (mode == ValueMode.Relative)
            {
                return relativeBase + program[cursor + relativePos];
            }

            throw new ArgumentException($"Why you do this? Mode: {mode}");
        }

        enum ValueMode {
            Position = 0,
            Immediate = 1,
            Relative = 2
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
            AdjustRelativeOffset = 9,
            Halt = 99
        }
    }
}