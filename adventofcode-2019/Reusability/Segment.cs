namespace AdventOfCode2019
{
    public class Segment
    {
        public Point Start { get; set; }

        public Point End { get; set; }

        public int Distance { get; set; }

        public Point FindIntersection(Segment other)
        {
            double deltaXReverseLeft = End.X - Start.X;
            double deltaYReverseLeft = End.Y - Start.Y;
            double deltaY1 = Start.Y - other.Start.Y;
            double deltaX2 = other.End.X - other.Start.X;
            double deltaX1 = Start.X - other.Start.X;
            double deltaY2 = other.End.Y - other.Start.Y;

            double denominator = deltaXReverseLeft * deltaY2 - deltaYReverseLeft * deltaX2;
            double numerator = deltaY1 * deltaX2 - deltaX1 * deltaY2;

            if (denominator == 0)
            {
                if (numerator == 0)
                {
                    if (Start.X >= other.Start.X && Start.X <= other.End.X)
                    {
                        return Start;
                    }
                    else if (other.Start.X >= Start.X && other.Start.X <= End.X)
                    {
                        return other.Start;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            double r = numerator / denominator;
            if (r < 0 || r > 1)
            {
                return null;
            }

            double s = (deltaY1 * deltaXReverseLeft - deltaX1 * deltaYReverseLeft) / denominator;
            if (s < 0 || s > 1)
            {
                return null;
            }

            return new Point { X = (int)(Start.X + r * deltaXReverseLeft), Y = (int)(Start.Y + r * deltaYReverseLeft) };
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as Segment;
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        public override int GetHashCode()
        {
            return Distance + 7 * Start.GetHashCode() + 13 * End.GetHashCode();
        }
    }
}