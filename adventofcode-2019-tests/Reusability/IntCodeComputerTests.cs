using System;
using Xunit;

namespace AdventOfCode2019.Tests
{
    public class IntCodeComputerTests
    {
        [Theory]
        [InlineData(2, new long[] { 1,0,0,0,99 })]
        [InlineData(2, new long[] { 2,3,0,3,99 })]
        [InlineData(2, new long[] { 2,4,4,5,99,0 })]
        [InlineData(30, new long[] { 1,1,1,4,99,5,6,0,99 })]
        [InlineData(3500, new long[] { 1,9,10,3,2,3,11,0,99,30,40,50 })]
        public void TestFirstPositionFromInputs(long expectedResult, long[] program)  {
            var computer = new IntCodeComputer(program);
            computer.RunIntcodeProgram();
            Assert.Equal(expectedResult, computer.FirstValue);
        }

        [Theory]
        [InlineData(999, int.MinValue, new long[] { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 })]
        [InlineData(999, 7, new long[] { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 })]
        [InlineData(1000, 8,  new long[] { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 })]
        [InlineData(1001, 9,  new long[] { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 })]
        [InlineData(1001, int.MaxValue, new long[] { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 })]
        [InlineData(1125899906842624, 1, new long[] { 104,1125899906842624,99 })]
        [InlineData(1219070632396864, 1, new long[] {1102,34915192,34915192,7,4,7,99,0})]
        [InlineData(50, 1, new long[] {109,2000,109,19,1101, 20, 30, 1985, 204, -34, 99})]

        public void TestLatestOutputFromInput(long expectedResult, int phaseSetting, long[] program) {
            var computer = new IntCodeComputer(phaseSetting, program);
            computer.RunIntcodeProgram();
            Assert.Equal(expectedResult, computer.LatestOutput);
        }

        [Theory]
        [InlineData("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99", 1, new long[] {109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99})]
        [InlineData("50", 1, new long[] {109,2000,109,19,1101, 20, 30, 1985, 204, -34, 99})]

        public void TestStringOutput(string expectedResult, int phaseSetting, long[] program) {
            var computer = new IntCodeComputer(phaseSetting, program);
            computer.RunIntcodeProgram();
            Assert.Equal(expectedResult, computer.StringOutput);
        }

        [Theory]
        [InlineData(19690720, 78, 70, new long[] { 1,0,0,3,1,1,2,3,1,3,4,3,1,5,0,3,2,1,10,19,1,19,5,23,1,6,23,27,1,27,5,31,2,31,10,35,2,35,6,39,1,39,5,43,2,43,9,47,1,47,6,51,1,13,51,55,2,9,55,59,1,59,13,63,1,6,63,67,2,67,10,71,1,9,71,75,2,75,6,79,1,79,5,83,1,83,5,87,2,9,87,91,2,9,91,95,1,95,10,99,1,9,99,103,2,103,6,107,2,9,107,111,1,111,5,115,2,6,115,119,1,5,119,123,1,123,2,127,1,127,9,0,99,2,0,14,0 })]

        public void TestNounVerbWork(long expectedResult, long noun, long verb, long[] program) {
            var computer = new IntCodeComputer(program);
            computer.Noun = noun;
            computer.Verb = verb;
            computer.RunIntcodeProgram();
            Assert.Equal(expectedResult, computer.FirstValue);
        }
    }
}
