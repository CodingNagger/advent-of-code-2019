using System.Linq;

namespace AdventOfCode2019
{
    public static class IntCodeProgramParser
    {
        public static long[] Parse(string[] input)  =>  input[0].Split(',').Select(s => long.Parse(s.Trim())).ToArray();
    }
}
