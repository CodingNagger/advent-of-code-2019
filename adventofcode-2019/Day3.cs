using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    class Day3 : Day
    {
        private List<Cable> Cables = new List<Cable>();

        private List<Point> Intersections = new List<Point>();

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
            var smallestDistance = Point.Origin.Distance(Intersections[0]);

            foreach (var point in Intersections)
            {
                smallestDistance = Math.Min(smallestDistance, Point.Origin.Distance(point));
            }

            return $"Smallest distance: {smallestDistance} - Min steps: {minIntersectionSteps}";
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

                    var intersection = a.Segments[i].FindIntersection(b.Segments[j]);
                    if (intersection != null && intersection.IsNotOrigin())
                    {
                        var intersectionSteps = iSteps + jSteps - intersection.Distance(a.Segments[i].End) - intersection.Distance(b.Segments[j].End) ;
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

        private Cable CreateCable(string cableDefinition)
        {
            var movements = cableDefinition.Split(',');
            List<Segment> segments = new List<Segment>();
            Point source = Point.Origin;
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
    }
}
