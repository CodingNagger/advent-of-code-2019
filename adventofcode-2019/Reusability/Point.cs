using System;

namespace AdventOfCode2019
{
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

        public override string ToString() => Description;
    }
}