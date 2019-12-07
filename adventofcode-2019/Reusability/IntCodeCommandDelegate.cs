namespace AdventOfCode2019
{
    public interface IntCodeCommandDelegate {
            long Cursor { get; set; }

            long this[long index] { get; set; }

            long Output { set; }
        }
}