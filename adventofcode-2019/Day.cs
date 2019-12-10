namespace AdventOfCode2019
{
    public interface Day
    {
        string Compute(string[] input);
    }

    public interface TwoPartDay: Day {
        string ComputePartTwo(string[] input);
    }
}
