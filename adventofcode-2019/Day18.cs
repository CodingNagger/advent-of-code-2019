using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day18 : TwoPartDay
    {
        private char[] keysToFind;

        private Dictionary<String, int> bestDistancesFound;
        private Dictionary<Point, KeysDistanceState> pointWithKeysCollected;

        public string Compute(string[] input)
        {
            var points = CreatePoints(input);
            PopulateKeysToFind(points);
            var start = points.First(p => p.Value == '@').Key;

            var answer = ShortestPath(start, new List<char>(), points, new List<Point>(), 0);
            // Console.WriteLine($"steps = {answer} ; Found: {GetBestDistanceKey()} ; Keys to find {string.Join(',', keysToFind)}");
            return $"{answer}";
        }

        public string ComputePartTwo(string[] input)
        {
            return "nope";
        }

        // shortest path between two positions

        // sum(@ to a etc... @ to lastPoint ) shortest path from origin to a then a to b etc sum all these shortest paths
        private int? ShortestPath(Point currentPosition, List<char> collectedKeys, Dictionary<Point, char> points, List<Point> visitedPoints, int distance)
        {
            var newlyCollectedKeys = new List<char>(collectedKeys);
            var newlyVisitedPoints = new List<Point>(visitedPoints);
            var newPoints = new Dictionary<Point, char>(points);

            // if (HasBetterPoint(currentPosition, newlyCollectedKeys, distance)) {
            //     Console.WriteLine($"Better point {currentPosition} found with {pointWithKeysCollected[currentPosition].KeysFound}>{collectedKeys.Count} keys and {pointWithKeysCollected[currentPosition].Distance}<{distance} distance");
            //     return null;
            // }

            if (CheckAllKeysCollected(newlyCollectedKeys)) {
                // // Console.WriteLine("Why are you here? All keys already caught");
                return null;
            }

            if (newPoints.Count == visitedPoints.Count) {
                // // Console.WriteLine("Why are you here? All points already visited");
                return null;
            }

            if (!CanExplore(currentPosition, newPoints, newlyVisitedPoints))
            {
                // Console.WriteLine($"Cannot explore {currentPosition} - {distance} - {GetDistanceKey(newlyCollectedKeys)}");
                return null;
            }

            var tile = newPoints[currentPosition];

            // Console.WriteLine($"Distance so far {distance} - position {tile} - {currentPosition} - Collected {newlyCollectedKeys.Count}/{keysToFind.Length} {GetDistanceKey(newlyCollectedKeys)}");

            if (InTooDeep(newlyCollectedKeys, distance)) {
                // Console.WriteLine("Too deep");
                return null;
            }

            if (IsKey(tile))
            {
                if (newlyCollectedKeys.Contains(tile))
                {
                    // Console.WriteLine("Already been here");
                    return null;
                }

                


       
                    // Console.WriteLine($"Can open door from key: {tile} - {GetDistanceKey(newlyCollectedKeys)} - {distance}");
                    newlyCollectedKeys.Add(tile);
                    // Console.Write($"Distance: {distance} ");
                    newPoints = RemoveKeyAndDoor(newPoints, tile);
                    newlyVisitedPoints.Clear();

                    UpdateBestDistance(newlyCollectedKeys, distance);
                    UpdateBestPoint(currentPosition, newlyCollectedKeys, distance);

                    if (CheckAllKeysCollected(newlyCollectedKeys))
                    {
                        Console.WriteLine($"Done - {string.Join(" -> ", newlyCollectedKeys)} - {distance}");
                        
                        return distance;
                    }

                    newlyVisitedPoints.Clear();

            }
            else if (IsDoor(tile))
            {
                // // Console.WriteLine($"Visited door: {tile} - {distance} - {currentPosition}");
                return null;
            }
            else {
                // // Console.WriteLine($"Visit: Wtf is this? {tile} - {distance} - {currentPosition}");
            }

            newlyVisitedPoints.Add(currentPosition);

            var neighbours = GetVisitableNeighbours(currentPosition, newPoints, newlyVisitedPoints);

            // Console.WriteLine($"To explore: {string.Join(" ; ", neighbours)}");

            var neighboursSearches = neighbours.Select(n => ShortestPath(n, new List<char>(newlyCollectedKeys), newPoints, new List<Point> { currentPosition}, distance + 1)).Where(v => v.HasValue).ToArray();

            if (neighboursSearches.Count() > 0)
            {
                // Console.WriteLine($"Back from {neighboursSearches.Count()} neighbours {currentPosition} - distance {distance} - {GetDistanceKey(newlyCollectedKeys)} - {neighboursSearches.Min()}");
                return neighboursSearches.Min();
            }

            return null;
        }

        private bool HasBetterPoint(Point position, List<char> newlyCollectedKeys, int distance) {
            return pointWithKeysCollected.ContainsKey(position) && 
                IsBetterState(pointWithKeysCollected[position], new KeysDistanceState { Distance = distance, KeysFound = newlyCollectedKeys.Count });
        }

        private bool IsBetterState(KeysDistanceState a, KeysDistanceState b) {
            return a.Distance <= b.Distance && a.KeysFound > b.KeysFound;
        }

        private void UpdateBestPoint(Point position, List<char> newlyCollectedKeys, int distance)
        {
            var state = new KeysDistanceState { Distance = distance, KeysFound = newlyCollectedKeys.Count };
            if (pointWithKeysCollected.ContainsKey(position)) {
                if (IsBetterState(state, pointWithKeysCollected[position])) {
                    pointWithKeysCollected[position] = state;
                }
            }
            else {
                pointWithKeysCollected.Add(position, state);
            }
        }

        private List<Point> GetVisitableNeighbours(Point currentPosition, Dictionary<Point, char> points, List<Point> visitedPoints)
        {
            return currentPosition.GetHorizontalNeighbours().Where(n => CanExplore(n, points, visitedPoints)).ToList();
        }

        private char GetDoorForKey(char key) => ((char)((int)key - 32));

        private bool CanExplore(Point position, Dictionary<Point, char> points, List<Point> visitedPoints) =>
            points.ContainsKey(position) && !visitedPoints.Contains(position) && Passage(points[position]);

        private bool Passage(char tile)
        {
            return !IsWall(tile);
        }

        private Dictionary<Point, char> RemoveKeyAndDoor(Dictionary<Point, char> points, char key)
        {
            var door = GetDoorForKey(key);
            // Console.WriteLine($"Attempting to remove keys: {key} and {door}");
            points[points.First(v => v.Value == key).Key] = '.';

            var doorKey = points.Where(v => v.Value == door).Select(v => v.Key).FirstOrDefault();
            if (doorKey != null && points.ContainsKey(doorKey))
            {
                points[doorKey] = '.';
            }

            return points;
        }

        private int BestDistanceSoFar()
        {
            return bestDistancesFound[GetBestDistanceKey()];
        }

        private bool CheckAllKeysCollected(IEnumerable<char> newlyCollectedKeys) => keysToFind.Except(newlyCollectedKeys).Count() == 0;

        private bool InTooDeep(List<char> collectedKeys, int distance)
        {
            if (bestDistancesFound.Count() == 0 || collectedKeys.Count == 0)
            {
                return false;
            }

            var bestDistanceKey = GetDistanceKey(collectedKeys);
            var biggestBestDistanceKey = GetBestDistanceKey();
            var biggestDistanceKey = GetKeysFromDistanceKey(bestDistanceKey);
            var allKeysCollected = CheckAllKeysCollected(biggestDistanceKey);

            if (bestDistancesFound.ContainsKey(biggestBestDistanceKey) && biggestBestDistanceKey.Length > bestDistanceKey.Length && (bestDistancesFound[biggestBestDistanceKey] <= distance  ))
            {
                // // Console.WriteLine($"Stop here motherfucker- Best key {biggestBestDistanceKey}:{bestDistancesFound[biggestBestDistanceKey]}  - Current {bestDistanceKey}:{bestDistancesFound[bestDistanceKey]}");
                return true;
            }

            if (bestDistancesFound.ContainsKey(bestDistanceKey))
            {
                var tooDeep = biggestBestDistanceKey.Length > bestDistanceKey.Length && bestDistancesFound[bestDistanceKey] < distance;
                // Console.WriteLine($"{(tooDeep ? "In Too Deep" : "Carry on my wayward son")} - Current key {bestDistanceKey} - Best found {bestDistancesFound[bestDistanceKey]} - Current {distance}");
                return tooDeep;
            }

            return false;
        }

        private void UpdateBestDistance(List<char> collectedKeys, int distance)
        {
            var distanceKey = GetDistanceKey(collectedKeys);
            var bestKeys = GetKeysFromDistanceKey(GetBestDistanceKey());

            if (bestKeys.Count() - collectedKeys.Count() > 0) {
                return;
            }

            if (bestDistancesFound.ContainsKey(distanceKey)) {
                if (bestDistancesFound[distanceKey] > distance) {
                    bestDistancesFound[distanceKey] = distance;
                }
            }
            else {
                bestDistancesFound.Add(distanceKey, distance);
            }
        }

        private string GetBestDistanceKey() => bestDistancesFound.Count > 0 ? bestDistancesFound.Aggregate((a, b) => a.Key.Length > b.Key.Length && a.Value <= b.Value ? a : b).Key : string.Empty;
        private string GetDistanceKey(List<char> collectedKeys) => 
            bestDistancesFound.Count > 0 ? 
                bestDistancesFound.FirstOrDefault(d => d.Key.Length == collectedKeys.Count && collectedKeys.All(c => d.Key.Contains(c))).Key ?? string.Join(',', collectedKeys) : 
                string.Join(',', collectedKeys);
        private char[] GetKeysFromDistanceKey(string distanceKey) => distanceKey.Length > 0 ? distanceKey.Split(",").Select(c => c[0]).ToArray() : new char[0];

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

        private void PopulateKeysToFind(Dictionary<Point, char> points)
        {
            keysToFind = points.Where(p => IsKey(p.Value)).Select(p => p.Value).ToArray();
            bestDistancesFound = new Dictionary<string, int>();
            pointWithKeysCollected = new Dictionary<Point, KeysDistanceState>();
        }

        private bool IsWall(char c) => c == '#';

        private bool IsKey(char c) => c >= 'a' && c <= 'z';

        private bool IsDoor(char c) => c >= 'A' && c <= 'Z';

        private bool IsKeyForDoor(char key, char door) => 'z' - key == 'Z' - door;
    }

    struct KeysDistanceState {
        public int KeysFound;
        public int Distance;
    }
}