namespace AdventOfCode2019
{
    public class Day5: Day
    {
        public string Compute(string[] input)
        {
            var computer = new IntCodeComputer(5, IntCodeProgramParser.Parse(input));
            computer.RunIntcodeProgram();
            return $"{computer.LatestOutput}";
        }
    }
}
