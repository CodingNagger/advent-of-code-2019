namespace AdventOfCode2019
{
    public class Day9: Day
    {
        public string Compute(string[] input)
        {
            var computer = new IntCodeComputer(2, IntCodeProgramParser.Parse(input));
            computer.RunIntcodeProgram();
            return computer.StringOutput;
        }
    }
}
