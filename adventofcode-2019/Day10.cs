using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public class Day10: Day
    {
        const char Empty = '.';
        const char Asteroid = '#';

        public override string Compute(string[] input)
        {
            var asteroidsCoordinates = ParseAsteroidsCoordinates(input);
            var best = asteroidsCoordinates.First();
            var bestCount = 0;

            foreach (var p1 in asteroidsCoordinates) {
                var angles = new List<double>();
                foreach (var p2 in asteroidsCoordinates) {
                    if (p1.Equals(p2)) continue;

                    var angle = p1.Angle(p2);
                    if (!angles.Contains(angle)) {
                        angles.Add(angle);
                    }
                }

                var count = angles.Count;
                if (count >= bestCount) {
                    Console.WriteLine($"New best {p1.Description} - {count}");
                    best = p1;
                    bestCount = angles.Count;
                }
                else {
                    Console.WriteLine($"Nope {p1.Description} - {count}");
                }
            }

            return $"{best.Description} - {bestCount}";
        }

        public Point[] ParseAsteroidsCoordinates(string[] input) {
            var asteroidsCoordinates = new List<Point>();

            for (var y = 0; y < input.Length; y++) {
                for (var x = 0; x < input[y].Length; x++) {
                    if (input[y][x] == Asteroid) {
                        asteroidsCoordinates.Add(new Point { X = x, Y = y});
                    }
                }
            }

            return asteroidsCoordinates.ToArray();
        }
    }
}
