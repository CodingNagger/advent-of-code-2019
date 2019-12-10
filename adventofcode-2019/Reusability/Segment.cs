using System;

namespace AdventOfCode2019
{
    public class Segment
    {
        public Point Start { get; set; }

        public Point End { get; set; }

        public int Distance { get; set; }

        public string Description => $"[{Start.Description};{End.Description}]";

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

        public bool Contains(Point p) {
            if ((Point.Origin.Distance(p) <= Point.Origin.Distance(Start)) ||
             (Point.Origin.Distance(p) >= Point.Origin.Distance(End))) {
                 return false;
            }

            if (p.X < Start.X || p.X > End.X) {
                return false;
            }

            if (p.Y < Start.Y || p.Y > End.Y) {
                return false;
            }

            if (Start.X == End.X) {
                return p.X == Start.X;
            }

            if (Start.Y == End.Y) {
                return p.Y == Start.Y;
            }

            double pX = p.X;
            double pY = p.Y;
            double sX = Start.X;
            double sY = Start.Y;
            double eX = End.X;
            double eY = End.Y;

            return ((pX - sX) / (eX - sX)) == ((pY - sY) / (eY - sY));
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

    public class Point
    {
        public static readonly Point Origin = new Point { X = 0, Y = 0 };

        public int X { get; set; }
        public int Y { get; set; }

        public string Description => $"{X},{Y}";

        public bool IsNotOrigin()
        {
            return X != Origin.X || Y != Origin.Y;
        }

        public int Distance(Point other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }

        public double Angle(Point other) {
            return Math.Atan2(X - other.X, Y - other.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as Point;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X + 13 * Y;
        }
    }
}