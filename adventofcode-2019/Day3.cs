using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    class Day3 : Day
    {
        private List<Cable> Cables = new List<Cable>();

        private List<Point> Intersections = new List<Point>();

        private Point Origin = new Point { X = 0, Y = 0 };

        private int minIntersectionSteps = 0;

        public override string Compute(string[] input)
        {
            foreach (var cableDefinition in input)
            {
                Cables.Add(CreateCable(cableDefinition));
            }

            Console.WriteLine($"Cables: {Cables.Count}");

            for (var i = 0; i < Cables.Count; i++)
            {
                for (var j = 0; j < Cables.Count; j++)
                {
                    if (i == j) continue;

                    Intersections.AddRange(FindIntersections(Cables[i], Cables[j]));
                }
            }

            Console.WriteLine($"Size: {Intersections.Count}");
            var smallestDistance = distance(Origin, Intersections[0]);

            foreach (var point in Intersections)
            {
                smallestDistance = Math.Min(smallestDistance, distance(Origin, point));
            }

            return $"Smallest distance: {smallestDistance} - Min steps: {minIntersectionSteps}";
        }

        private int distance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private Point[] FindIntersections(Cable a, Cable b)
        {
            List<Point> found = new List<Point>();
            minIntersectionSteps = TotalSteps(a, b);
            var iSteps = 0;
            var jSteps = 0;

            for (var i = 0; i < a.Segments.Length; i++)
            {
                iSteps += a.Segments[i].Distance;
                jSteps = 0;

                for (var j = 0; j < b.Segments.Length; j++)
                {
                    jSteps += b.Segments[j].Distance;

                    var intersection = FindIntersection(a.Segments[i], b.Segments[j]);
                    if (intersection != null && IsNotOrigin(intersection))
                    {
                        var intersectionSteps = iSteps + jSteps - distance(intersection, a.Segments[i].End) - distance(intersection, b.Segments[j].End) ;
                        minIntersectionSteps = Math.Min(minIntersectionSteps, intersectionSteps);
                        Intersections.Add(intersection);
                        Console.WriteLine($"Found ({intersection.X}, {intersection.Y}) - {intersectionSteps}");
                    }
                }
            }

            return found.ToArray();
        }

        private int TotalSteps(Cable a, Cable b) {
            return a.Segments.Sum(v => v.Distance) + b.Segments.Sum(v => v.Distance);
        }

        bool IsNotOrigin(Point point) {
            return point.X != Origin.X || point.Y != Origin.Y;
        }

        private Point FindIntersection(Segment left, Segment right)
        {
            double deltaXReverseLeft = left.End.X - left.Start.X;
            double deltaYReverseLeft = left.End.Y - left.Start.Y;
            double deltaY1 = left.Start.Y - right.Start.Y;
            double deltaX2 = right.End.X - right.Start.X;
            double deltaX1 = left.Start.X - right.Start.X;
            double deltaY2 = right.End.Y - right.Start.Y;

            double denominator = deltaXReverseLeft * deltaY2 - deltaYReverseLeft * deltaX2;
            double numerator = deltaY1 * deltaX2 - deltaX1 * deltaY2;

            if (denominator == 0)
            {
                if (numerator == 0)
                {
                    if (left.Start.X >= right.Start.X && left.Start.X <= right.End.X)
                    {
                        return left.Start;
                    }
                    else if (right.Start.X >= left.Start.X && right.Start.X <= left.End.X)
                    {
                        return right.Start;
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

            return new Point { X = (int) (left.Start.X + r * deltaXReverseLeft), Y = (int)( left.Start.Y + r * deltaYReverseLeft ) };
        }

        private Cable CreateCable(string cableDefinition)
        {
            var movements = cableDefinition.Split(',');
            List<Segment> segments = new List<Segment>();
            Point source = Origin;
            Point destination = source;
            char move;
            int shift;

            foreach (var movement in movements)
            {
                move = movement[0];
                shift = int.Parse(movement.Substring(1));

                switch (movement[0])
                {
                    case 'U':
                        destination = new Point { X = source.X, Y = source.Y + shift };
                        break;

                    case 'D':
                        destination = new Point { X = source.X, Y = source.Y - shift };
                        break;

                    case 'R':
                        destination = new Point { X = source.X + shift, Y = source.Y };
                        break;

                    case 'L':
                        destination = new Point { X = source.X - shift, Y = source.Y };
                        break;
                }

                segments.Add(new Segment { Start = source, End = destination, Distance = shift });
                source = destination;
            }

            return new Cable { Segments = segments.ToArray() };
        }

        class Cable
        {
            public Segment[] Segments { get; set; }
        }

        class Segment
        {
            public Point Start { get; set; }

            public Point End { get; set; }

            public int Distance { get; set; }
        }

        class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
