using System;
using Xunit;

namespace AdventOfCode2019.Tests
{
    public class Day10Tests : AbstractDayTests
    {
        [Theory]
        [InlineData("3,4 - 8", new string[] { ".#..#",".....","#####","....#","...##" })]
        [InlineData("5,8 - 33", new string[] { "......#.#.","#..#.#....",".#######.",".#.#.###..",".#..#.....","..#....#.#","#..#....#.",".##.#..###","##...#..#.",".#....####" })]
        public override void Test(string expectedResult, string[] input) => Execute(new Day10(), expectedResult, input);

        [Theory]
        [InlineData(10, new string[] { ".#..#",".....","#####","....#","...##" })]
        public void ParseAsteroidsCoordinatesAndCheckCount(int expectedCount, string[] input) {
            var points = new Day10().ParseAsteroidsCoordinates(input);
            Assert.Equal(expectedCount, points.Length);
        }
    }
}
