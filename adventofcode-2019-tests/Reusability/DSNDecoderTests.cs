using System;
using Xunit;

namespace AdventOfCode2019.Tests
{
    public class DSNDecoderTests
    {
        [Theory]
        [InlineData("0110", 2, 2, new string[] { "0222112222120000" })]
        [InlineData("1111", 2, 2, new string[] { "22221111" })]
        [InlineData("0000", 2, 2, new string[] { "222200001111" })]
        [InlineData("0000", 2, 2, new string[] { "2222222200001111" })]
        public void TestCompute(string expectedResult, int width, int height, string[] input)  {
            Assert.Equal(expectedResult, new DSNDecoder().Decode(input, width, height));
        }

        
    }
}
