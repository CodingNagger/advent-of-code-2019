using System;

namespace AdventOfCode2019
{
    public class Point
    {
        public static readonly Point Origin = new Point(0,0);

        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        public Point() {
        }

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

        public Point[] GetHorizontalNeighbours() {
            return new[] {
                new Point { X = X+1, Y = Y},
                new Point { X = X-1, Y = Y},
                new Point { X = X, Y = Y+1},
                new Point { X = X, Y = Y-1},
            };
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Point))
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