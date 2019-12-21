using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day18 : TwoPartDay
    {
        const char EMPTY = '.';
        private Dictionary<Point, char> map;

        public string Compute(string[] input)
        {
            map = CreatePoints(input);
            var start = map.First(p => p.Value == '@').Key;

            return $"{BestDistance(start, GetKeysPositions(), new Dictionary<string, int>())}";
        }

        private Point[] GetKeysPositions() => map.Where(p => IsKey(p.Key)).Select(p => p.Key).ToArray();

        public int BestDistance(Point start, Point[] keysLeft, Dictionary<string, int> cache)
        {
            Console.WriteLine($"ENTER Best distance from {start} - {map[start]}");
            var path = $"{map[start]}";
            
            if (keysLeft.Length == 0)
            {
                // Console.WriteLine($"NOKEYS Best distance from {start}");
                return 0;
            }

            var cachingKey = GenerateCachingKey(start, keysLeft);
            if (cache.ContainsKey(cachingKey))
            {
                Console.WriteLine($"CACHED Best Distance update from {start} - {cache[cachingKey]}");
                return cache[cachingKey];
            }

            var bestDistance = int.MaxValue;
            char bestDistanceChar = '0';

            foreach (var key in DirectlyReachableKeys(start, keysLeft, new Point[0], new Dictionary<string, IEnumerable<Point>>()))
            {
                Console.WriteLine($"Dealing with key {key} - {map[key]} from {map[start]}");
                var distanceToKey = Distance(start, key, keysLeft);

                if (distanceToKey.HasValue) {
                    var distanceKeyToEnd = BestDistance(key, keysLeft.Except(new[] { key }).ToArray(), cache);

                    Console.WriteLine($"Distance update with key {key} - {map[key]} from {map[start]} - {bestDistance} - {distanceToKey.Value} - {distanceKeyToEnd} - {distanceKeyToEnd+distanceKeyToEnd}");
                    bestDistance = Math.Min(bestDistance, distanceToKey.Value + distanceKeyToEnd);
                    bestDistanceChar = map[key];
                    Console.WriteLine($"Distance update with key {key} - {map[key]} from {map[start]} - {bestDistance} - {distanceToKey.Value} - {distanceKeyToEnd} - {distanceKeyToEnd+distanceKeyToEnd}");
                }
                else {
                    Console.WriteLine($"Not Distance update between {map[start]} and {bestDistanceChar}");
                }
            }

            cache.Add(cachingKey, bestDistance);
            Console.WriteLine($"EXIT Best Distance update from {start} - {map[start]} - {bestDistance} - path: {map[start]}{bestDistanceChar}");
            return bestDistance;
        }

        private int? Distance(Point s, Point t, Point[] keysLeft)
        {
            Console.WriteLine($"ENTER Calculate distance from {s} to {t}");
            AStarPoint current = null;
            var start = new AStarPoint(s);
            var target = new AStarPoint(t);
            var visitable = new List<AStarPoint>();
            var visited = new List<AStarPoint>();
            int g = 0;

            visitable.Add(start);

            while (visitable.Count > 0)
            {
                // Console.WriteLine($"LOOPCURRENT {g} Calculate distance from {s} to {t} - {current}");
                var lowestF = visitable.Min(v => v.F);
                current = visitable.First(v => v.F == lowestF);


                visited.Add(current);
                // Console.WriteLine($"VISITEDCURRENT {g} Calculate distance from {s} to {t} - {current} - {string.Join(';', visitable)}");
                visitable.Remove(current);
                // Console.WriteLine($"NOMOREVISIT {g} Calculate distance from {s} to {t} - {current} - {string.Join(';', visited)}");

                if (visited.FirstOrDefault(v => target.Equals(v)) != null)
                {
                    // Console.WriteLine($"BREAK {g} - {visited.FirstOrDefault(v => v.Equals(target))?.G} Calculate distance from {s} to {t} - {current} - {string.Join(';', visited)}");
                    break;
                }

                var explorableNeighbours = GetExplorableNeighbours(current, keysLeft, visited)
                    .Select(n => new AStarPoint(n))
                    .ToArray();

                // Console.WriteLine($"NEIGHBOURS {g} Calculate distance from {s} to {t} - {current} - {string.Join(';', explorableNeighbours.ToList())}");

                g++;

                foreach (var neighbour in explorableNeighbours)
                {
                    // Console.WriteLine($"LOOPNEIGHBOUR {g} Calculate distance from {s} to {t} - {current} - {neighbour}");
                    if (visitable.Contains(neighbour))
                    {
                        if (g + neighbour.H < neighbour.F)
                        {
                            neighbour.G = g;
                            neighbour.Parent = current;
                        }
                    }
                    else
                    {
                        neighbour.G = g;
                        neighbour.H = Math.Abs(target.X - neighbour.X) + Math.Abs(target.Y - neighbour.Y);
                        visitable.Add(neighbour);
                        // Console.WriteLine($"ADDVISITABLE {g} Calculate distance from {s} to {t} - {current} - {neighbour}");
                    }
                }
            }

            // Console.WriteLine($"EXIT {g} Calculate distance from {s} to {t} - {current} - {visited.FirstOrDefault(v => v.Equals(target))?.G}");

            return visited.FirstOrDefault(v => v.Equals(target))?.G;
        }

        private IEnumerable<Point> GetExplorableNeighbours(Point current, Point[] keysLeft, IEnumerable<Point> visited)
        {
            return current.GetHorizontalNeighbours().Where(n => CanNavigate(n, keysLeft) && !visited.Contains(n));
        }

        private IEnumerable<Point> DirectlyReachableKeys(Point start, Point[] keysLeft, IEnumerable<Point> visitedPoints, Dictionary<string, IEnumerable<Point>> cache)
        {
            if (IsKey(start)) Console.WriteLine($"Enter DirectlyReachableKeys {start} {map[start]}  - Keys left: {string.Join(';', keysLeft.Select(k => map[k])) } - Visited:  {string.Join(';', visitedPoints.Select(k => k))}");

            var cachingKey = GenerateCachingKey(start, keysLeft);

            if (cache.ContainsKey(cachingKey))
            {
                if (IsKey(start)) Console.WriteLine($"Returning cached value for {cachingKey}");
                return cache[cachingKey];
            }

            var result = new List<Point>();
            var visited = new List<Point>(visitedPoints);

            if (CanNavigate(start, keysLeft))
            {
                if (IsKey(start) && keysLeft.Contains(start))
                {
                    Console.WriteLine($"DirectlyReachableKeys found key {start}");
                    result.Add(start);
                }
                else
                {
                    visited.Add(start);

                    var neighbourResults = GetExplorableNeighbours(start, keysLeft, visited).SelectMany(n => DirectlyReachableKeys(n, keysLeft, visited, cache));

                    if (IsKey(start)) {
                        Console.WriteLine($"Neighbours candidates from {map[start]} {start}: {string.Join(";", start.GetHorizontalNeighbours().ToList())}");
                        Console.WriteLine($"Neighbours returned from {map[start]} {start}: {string.Join(";", neighbourResults)}");
                    }
                    foreach (var neighbourResult in neighbourResults)
                    {
                        if (!result.Contains(neighbourResult))
                        {
                            if (IsKey(start)) Console.WriteLine($"Add neighbour {neighbourResult}-{map[neighbourResult]} from {start}-{map[start]}");
                            result.Add(neighbourResult);
                        }
                        else {
                            if (IsKey(start)) Console.WriteLine($"Already have neighbour {neighbourResult}-{map[neighbourResult]} from {start}-{map[start]}");
                        }
                    }
                }
            }

            cache.Add(cachingKey, result);

            if (IsKey(start)) Console.WriteLine($"Exit DirectlyReachableKeys {start} {map[start]}  - Returning: {string.Join(';', result.Select(k => map[k])) }");

            return result;
        }

        private string GenerateCachingKey(Point start, Point[] keys)
        {
            return $"{start}-{string.Join(';', keys.ToList())}";
        }

        public string ComputePartTwo(string[] input)
        {
            return "nope";
        }

        private char GetDoorForKey(char key) => ((char)((int)key - 32));

        private List<Point> RemoveKeyAndDoor(IEnumerable<Point> points, Point key)
        {
            var doorKey = GetDoorForKey(map[key]);
            var result = RemovePoint(points, key);

            var door = map.FirstOrDefault(m => m.Value == doorKey).Key;

            if (door != null)
            {
                result = RemovePoint(points, door);
            }

            return result;
        }

        private List<Point> RemovePoint(IEnumerable<Point> points, Point point)
        {
            return points.Except(new[] { point }).ToList();
        }

        private Dictionary<Point, char> CreatePoints(string[] input)
        {
            var map = input.Select(i => i.ToArray()).ToArray();
            var points = new Dictionary<Point, char>();

            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    points.Add(new Point { X = x, Y = y }, map[y][x]);
                }
            }

            return points;
        }

        private bool CanNavigate(Point current, Point[] keysLeft) => map.ContainsKey(current) && !DoorsLeft(keysLeft).Contains(current) && !IsWall(map[current]);
        private bool IsStart(char c) => c == '@';
        private bool IsWall(char c) => c == '#';

        private bool IsKey(Point current) => map.ContainsKey(current) && map[current] >= 'a' && map[current] <= 'z';

        private bool IsDoor(char c) => c >= 'A' && c <= 'Z';

        private bool IsValidKeyDoorPair(char key, char door) => 'z' - key == 'Z' - door;

        private Point[] DoorsLeft(Point[] keysLeft) => map.Where(p => IsDoor(p.Value) && keysLeft.Any(k => IsValidKeyDoorPair(map[k], p.Value))).Select(p => p.Key).ToArray();
    }

    public class AStarPoint : Point
    {
        public int F => G + H;
        public int G { get; set; }
        public int H { get; set; }

        public AStarPoint Parent { get; set; }

        public AStarPoint(Point point) : base(point.X, point.Y) { }
    }
}