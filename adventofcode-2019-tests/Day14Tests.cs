using Xunit;

namespace AdventOfCode2019.Tests
{
    // public class Day14Tests : AbstractTwoPartDayTests
    // {
    //     [Theory]
    //     [InlineData("10", new string[] { "10 ORE => 1 A", "1 A => 1 FUEL" })]
    //     [InlineData("10", new string[] { "10 ORE => 10 A", "5 A => 1 FUEL" })]
    //     [InlineData("12", new string[] { "10 ORE => 10 A", "1 ORE => 1 B", "5 A, 2 B => 1 FUEL" })]
    //     [InlineData("10", new string[] { "10 ORE => 10 A", "10 ORE => 10 B", "5 A, 5 B => 1 FUEL" })]
    //     // [InlineData("31", new string[] { "10 ORE => 10 A", "1 ORE => 1 B", "7 A, 1 B => 1 C", "7 A, 1 C => 1 D", "7 A, 1 D => 1 E", "7 A, 1 E => 1 FUEL" })]
    //     // [InlineData("165", new string[] {"9 ORE => 2 A", "8 ORE => 3 B", "7 ORE => 5 C", "3 A, 4 B => 1 AB", "5 B, 7 C => 1 BC", "4 C, 1 A => 1 CA", "2 AB, 3 BC, 4 CA => 1 FUEL"})]
    //     // [InlineData("13312", new string[] {"157 ORE => 5 NZVS", "165 ORE => 6 DCFZ", "44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL", 
    //     // "12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ", "179 ORE => 7 PSHF", "177 ORE => 5 HKGWZ", "7 DCFZ, 7 PSHF => 2 XJWVT", "165 ORE => 2 GPVTF", "3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT"})]
    //     // [InlineData("180697", new string[] {"2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG", "17 NVRVD, 3 JNWZP => 8 VPVL", "53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL", 
    //     // "22 VJHF, 37 MNCFX => 5 FWMGM", "139 ORE => 4 NVRVD", "144 ORE => 7 JNWZP", "5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC", "5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV", 
    //     // "145 ORE => 6 MNCFX", "1 NVRVD => 8 CXFTF", "1 VJHF, 6 MNCFX => 4 RFSQX", "176 ORE => 6 VJHF"})]
    //     // [InlineData("2210736", new string[] {"171 ORE => 8 CNZTR", "7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL", "114 ORE => 4 BHXH",
    //     // "14 VRPVC => 6 BMBT", "6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL", "6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT", 
    //     // "15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW", "13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW", "5 BMBT => 4 WPTQ", "189 ORE => 9 KTJDG", 
    //     // "1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP", "12 VRPVC, 27 CNZTR => 2 XDBXC", "15 KTJDG, 12 BHXH => 5 XCVML", "3 BHXH, 2 VRPVC => 7 MZWV", "121 ORE => 7 VRPVC", 
    //     // "7 XCVML => 6 RJRHP", "5 BHXH, 4 VRPVC => 5 LTCX"})]
    //     public override void Test(string expectedResult, string[] input) => Execute(new Day14(), expectedResult, input);

    //     [Theory]
    //     [InlineData("nope", new string[] { "10 ORE => 10 A", "10 ORE => 10 A", "10 ORE => 10 A", "10 ORE => 10 A", "10 ORE => 10 A" })]
    //     public override void TestPartTwo(string expectedResult, string[] input) => ExecutePartTwo(new Day14(), expectedResult, input);

    //     [Fact]
    //     public void TestReactionCanReverseReact() {
    //         var reactionInput = new [] { new Chemical { Name = "ORE", Quantity = 9 } };
    //         var reactionOutput = new Chemical { Name = "A", Quantity = 2 };
    //         var reaction = new Reaction( reactionInput, reactionOutput);

    //         Assert.True(reaction.CanReverseReact(new Chemical { Name = "A", Quantity = 2 }));
    //         Assert.True(reaction.CanReverseReact(new Chemical { Name = "A", Quantity = 3 }));
    //         Assert.False(reaction.CanReverseReact(new Chemical { Name = "B", Quantity = 2 }));
    //         Assert.False(reaction.CanReverseReact(new Chemical { Name = "ORE", Quantity = 1000 }));
    //     }

    //     [Fact]
    //     public void TestReactionReverseReact() {
    //         var reactionInput = new [] { new Chemical { Name = "ORE", Quantity = 9 } };
    //         var reactionOutput = new Chemical { Name = "A", Quantity = 2 };

    //         Assert.Equal(new Chemical { Name = "ORE", Quantity = 9 }, new Reaction( reactionInput, reactionOutput).ReverseReact(new Chemical { Name = "A", Quantity = 2 })[0]);
    //         Assert.Equal(new Chemical { Name = "ORE", Quantity = 18 }, new Reaction( reactionInput, reactionOutput).ReverseReact(new Chemical { Name = "A", Quantity = 3 })[0]);
    //         Assert.Equal(new Chemical { Name = "ORE", Quantity = 45 }, new Reaction( reactionInput, reactionOutput).ReverseReact(new Chemical { Name = "A", Quantity = 10 })[0]);
    //         Assert.Equal(new Chemical[0], new Reaction( reactionInput, reactionOutput).ReverseReact(new Chemical { Name = "B", Quantity = 2 }));
    //         Assert.Equal(new Chemical[0], new Reaction( reactionInput, reactionOutput).ReverseReact(new Chemical { Name = "ORE", Quantity = 1000 }));
    //     }

    //     [Fact]
    //     public void TestReactionReverseReactRoundsUp() {
    //         var reactionInput = new [] { new Chemical { Name = "ORE", Quantity = 10 } };
    //         var reactionOutput = new Chemical { Name = "A", Quantity = 10 };

    //         Assert.Equal(new Chemical { Name = "ORE", Quantity = 30 }, new Reaction( reactionInput, reactionOutput).ReverseReact(new Chemical { Name = "A", Quantity = 28 })[0]);
    //         Assert.Equal(new Chemical { Name = "ORE", Quantity = 30 }, new Reaction( reactionInput, reactionOutput).ReverseReact(new Chemical { Name = "A", Quantity = 30 })[0]);
    //         Assert.Equal(new Chemical { Name = "ORE", Quantity = 40 }, new Reaction( reactionInput, reactionOutput).ReverseReact(new Chemical { Name = "A", Quantity = 38 })[0]);
    //     }

    //     [Fact]
    //     public void TestReactionReverseReactMultipleInputs() {
    //         var reactionInput = new [] { new Chemical { Name = "A", Quantity = 7 }, new Chemical { Name = "B", Quantity = 1 } };
    //         var reactionOutput = new Chemical { Name = "C", Quantity = 1 };
    //         var reaction = new Reaction( reactionInput, reactionOutput);
    //         var expectedResult = new [] { new Chemical { Name = "A", Quantity = 21 }, new Chemical { Name = "B", Quantity = 3 } };

    //         Assert.Equal(expectedResult, reaction.ReverseReact(new Chemical { Name = "C", Quantity = 3 }));
    //     }

    //     [Fact]
    //     public void TestParseSimpleReaction() {
    //         var data = "10 ORE => 10 A";
    //         var expectedResult = new Reaction( new [] { new Chemical { Name = "ORE", Quantity = 10 } }, new Chemical { Name = "A", Quantity = 10 });
    //         Assert.Equal(expectedResult, Reaction.Parse(data));
    //     }

    //     [Theory]
    //     [InlineData(10, "ORE")]
    //     [InlineData(8, "A")]
    //     [InlineData(58, "FUEL")]
    //     public void TestParseChemical(int quantity, string name) {
    //         Assert.Equal(new Chemical { Name = name, Quantity = quantity }, Chemical.Parse($"{quantity} {name}"));
    //     }
    // }
}
