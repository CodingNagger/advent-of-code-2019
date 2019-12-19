using Xunit;

namespace AdventOfCode2019.Tests
{
    public class Day18Tests : AbstractTwoPartDayTests
    {

        [Theory]
        [InlineData("8", new [] { "#########","#b.A.@.a#","#########" })]
        [InlineData("86", new [] { "########################","#f.D.E.e.C.b.A.@.a.B.c.#","######################.#","#d.....................#","########################" })]
        [InlineData("132", new [] {"########################","#...............b.C.D.f#","#.######################","#.....@.a.B.c.d.A.e.F.g#","########################"})]
        // [InlineData("136", new [] { "#################","#i.G..c...e..H.p#","########.########","#j.A..b...f..D.o#","########@########","#k.E..a...g..B.n#","########.########","#l.F..d...h..C.m#","#################" })]
        [InlineData("81", new [] { "########################","#@..............ac.GI.b#","###d#e#f################","###A#B#C################","###g#h#i################","########################" })]
        public override void Test(string expectedResult, string[] input) => Execute(new Day18(), expectedResult, input);

        [Theory]
        [InlineData("nope", new string[] { "10 ORE => 10 A", "10 ORE => 10 A", "10 ORE => 10 A", "10 ORE => 10 A", "10 ORE => 10 A" })]
        public override void TestPartTwo(string expectedResult, string[] input) => ExecutePartTwo(new Day18(), expectedResult, input);
    }
}
