using System;
using Xunit;

namespace AdventOfCode2019.Tests
{
    public class DSNChecksumCalculatorTests
    {
        [Theory]
        [InlineData(1, 3, 2, new string[] { "123456789012" })]
        [InlineData(6, 3, 2, new string[] { "111111000000100000111022110022" })]
        public void TestCompute(int expectedResult, int width, int height, string[] input)  {
            var calculator = new DSNChecksumCalculator();
            var result = calculator.Compute(input, width, height);
            Assert.Equal(expectedResult, result);
        }
    }
}
