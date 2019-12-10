using System;
using AdventOfCode2019;
using Xunit;

namespace AdventOfCode2019.Tests
{
    public abstract class AbstractTwoPartDayTests: AbstractDayTests
    {
        protected void ExecutePartTwo(TwoPartDay day, string expectedResult, string[] input)
        {
            var result = day.ComputePartTwo(input);
            Assert.Equal(expectedResult, result);
        }

        public abstract void TestPartTwo(string expectedResult, string[] input);
    }
}
