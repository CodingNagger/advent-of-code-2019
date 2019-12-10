using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public class Day10: TwoPartDay
    {
        const char Asteroid = '#';

        public string Compute(string[] input)
        {
            var asteroidsCoordinates = ParseAsteroidsCoordinates(input);
            var best = FindMonitoringStationLocationAndCounts(asteroidsCoordinates);

            return $"{best.Item1.Description} - {best.Item2}";
        }

        public string ComputePartTwo(string[] input)
        {
            var asteroidsCoordinates = ParseAsteroidsCoordinates(input);
            var monitoringStation = FindMonitoringStationLocationAndCounts(asteroidsCoordinates).Item1;
            var processed = new List<Point>() { monitoringStation };
            var shots = new List<Point>();
            var targetCount = 200;

            while (shots.Count < targetCount) {
                var potentialShots = GetVisibleAsteroidsSorted(monitoringStation, asteroidsCoordinates.Except(processed));

                foreach(var shot in potentialShots) {
                    shots.Add(shot.Value);

                    if (shots.Count == targetCount) break;
                }

                processed.AddRange(shots);
            }

            var targetShot = shots.Last();

            return $"{100*targetShot.X + targetShot.Y}";
        }

        private (Point, int) FindMonitoringStationLocationAndCounts(Point[] asteroidsCoordinates)
        {
            var best = asteroidsCoordinates.First();
            var bestCount = 0;

            foreach (var source in asteroidsCoordinates) {
                var count = GetVisibleAsteroids(source, asteroidsCoordinates).Keys.Count;
                if (count >= bestCount) {
                    best = source;
                    bestCount = count;
                }
            }

            return (best, bestCount);
        }

        private Dictionary<double, Point> GetVisibleAsteroids(Point source, IEnumerable<Point> asteroids) {
            var visibleAsteroids = new Dictionary<double, Point>();

            foreach (var asteroid in asteroids.Except(new Point[] { source })) {
                var angle = source.Angle(asteroid);
                if (!visibleAsteroids.ContainsKey(angle)) {
                    visibleAsteroids.Add(angle, asteroid);
                }
                else {
                    var d1 = source.Distance(asteroid);
                    var d2 = source.Distance(visibleAsteroids[angle]);

                    if (d1 < d2) {
                        visibleAsteroids[angle] = asteroid;
                    }
                }
            }
            return visibleAsteroids;
        }

        private Dictionary<double, Point> GetVisibleAsteroidsSorted(Point source, IEnumerable<Point> asteroids) {
            return GetVisibleAsteroids(source, asteroids)
            .Select(a => new {
                    Point = a.Value,
                    Score = a.Key > 0 ? a.Key : a.Key + 2 * Math.PI,
                })
                .OrderByDescending(a => a.Score)
                .ToDictionary(a => a.Score, v => v.Point);
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
