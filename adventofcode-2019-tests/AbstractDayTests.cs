using System;
using AdventOfCode2019;
using Xunit;

namespace AdventOfCode2019.Tests
{
    public abstract class AbstractDayTests
    {
        protected void Execute(Day day, string expectedResult, string[] input)
        {
            var result = day.Compute(input);
            Assert.Equal(expectedResult, result);
        }

        public abstract void Test(string expectedResult, string[] input);
    }
}
