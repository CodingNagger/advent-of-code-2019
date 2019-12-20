using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace AdventOfCode2019
{
    public class Day18 : TwoPartDay
    {
        const char EMPTY = '.';
        char[] keysToFind;
        Random random = new Random();
        Dictionary<string, Dictionary<Point, (char, int)>> cachedKeysDistances;
        Dictionary<string, int> distances;

        Dictionary<string, PathNode> cachedNodes;

        public string Compute(string[] input)
        {
            cachedKeysDistances = new Dictionary<string, Dictionary<Point, (char, int)>>();
            cachedNodes = new Dictionary<string, PathNode>();

            var points = CreatePoints(input);
            var start = points.First(p => p.Value == '@');
            keysToFind = points.Where(p => IsKey(p.Value)).Select(p => p.Value).ToArray();
            var tree = BuildTree((start.Key, start.Value), points, new List<char>(), 0);

            // return $"{tree.Tile} - {tree.Children.Count} - {BestDistance(tree, keysToFind.Length, new List<char>(), 0)}";
            return $"{BestDistance(tree, keysToFind.Length, new List<char>(), 0)}";
            return "Zbreh";
        }

        public int? BestDistance(PathNode tree, int totalKeysCount, List<char> collectedKeys, int depth) {
            var newlyCollectedKeys = new List<char>(collectedKeys);

            newlyCollectedKeys.Add(tree.Tile);
            
            var valuedChildren = tree.Children.Select(c => BestDistance(c, totalKeysCount, newlyCollectedKeys, depth+1)).Where(c => c.HasValue).ToArray();

            var value = valuedChildren.Length > 0 ? valuedChildren.Min() : tree.Distance;

            return value;
        }


        private PathNode BuildTree((Point Key, char Value) current, Dictionary<Point, char> points, List<char> collectedKeys, int distance)
        {
            Console.WriteLine($"Enter BuildTree {current.Value} - {current.Key} - {distance}");
            var mappingKey = GetNodeMappingKey(current.Key, points, distance);

            if (collectedKeys.Contains(current.Value)) {
                return null;
            }

            if (cachedNodes.ContainsKey(mappingKey)) {
                return cachedNodes[mappingKey];
            }
            else if (points.Count(p => IsKey(p.Value)) > 0) {
                var node = new PathNode(current.Value, distance);
                var newlyCollectedKeys = new List<char>(collectedKeys);
                
                if (IsKey(current.Value)) {
                    newlyCollectedKeys.Add(current.Value);
                }

                Dictionary<Point, char> visitablePoints = IsKey(current.Value) ?
                    RemoveKeyAndDoor(points, current.Value) :
                    RemovePoint(points, current.Key);

                var accessibleKeys = current.Key.GetHorizontalNeighbours()
                    .Where(n => visitablePoints.ContainsKey(n) && CanNavigate(visitablePoints[n], visitablePoints))
                    .ToArray();

                foreach (var neighbour in accessibleKeys) {
                    var neighbourResult = GetAccessibleKeysAndDistances((neighbour, points[neighbour]), visitablePoints, new List<Point> { current.Key }, distance);

                    foreach (var keyFound in neighbourResult) {
                        var child = BuildTree((keyFound.Key, keyFound.Value.Item1), visitablePoints, newlyCollectedKeys, keyFound.Value.Item2);

                        node.AddChild(child);
                    }
                }

                Console.WriteLine($"Exit BuildTree {current.Value} - {current.Key} - {distance} - Children: {string.Join(",", node.Children.Select(c => c.Tile))}-");

                cachedNodes.Add(mappingKey, node);

                return node;
            }

            return null;
        }

        private Dictionary<Point, (char, int)> GetAccessibleKeysAndDistances((Point Key, char Value) current, Dictionary<Point, char> points,
            List<Point> visitedPoints, int distance)
        {
            Console.WriteLine($"Enter GetAccessibleKeysAndDistances {current.Value} - {current.Key} - {distance} - Visited: {string.Join(';', visitedPoints)}");
            distance++;
            Dictionary<Point, char> visitablePoints;
            var result = new Dictionary<Point, (char, int)>();
            var newlyVisitedPoints = new List<Point>(visitedPoints);
            newlyVisitedPoints.Add(current.Key);

            var mappingKey = GetMappingKey(newlyVisitedPoints, points, distance);

            if (cachedKeysDistances.ContainsKey(mappingKey)) {
                return cachedKeysDistances[mappingKey];
            }
            else if (CanNavigate(current.Value, points))
            {
                if (IsKey(current.Value))
                {
                    result.Add(current.Key, (current.Value, distance));
                    visitablePoints = RemovePoint(points, current.Key);
                    Console.WriteLine($"Distance: {distance} -  Added {current.Value} because it's a key");
                }
                else
                {
                    visitablePoints = RemovePoint(points, current.Key);
                }

                Console.WriteLine($"Neighbours search");
                var neighborsResults = current.Key.GetHorizontalNeighbours()
                .Where(n => visitablePoints.ContainsKey(n) && !newlyVisitedPoints.Contains(n) && CanNavigate(visitablePoints[n], visitablePoints))
                .SelectMany(n => GetAccessibleKeysAndDistances((n, visitablePoints[n]), visitablePoints, newlyVisitedPoints, distance));

                foreach (var nResult in neighborsResults)
                {
                    if (nResult.Value.Item1 == current.Value) {
                        throw new Exception($"NEIN NEIN NEIN not happening trying should not have {current.Value} twice {distance} appeared at {mappingKey} - parent");
                    }
                    if (!result.ContainsKey(nResult.Key))
                    {
                        result.Add(nResult.Key, nResult.Value);
                    }
                }

                cachedKeysDistances.Add(mappingKey, result);
            }
            else
            {
                Console.WriteLine($"Cannot navigate {current.Key} - {current.Value}");
            }

            Console.WriteLine($"Exit GetAccessibleKeysAndDistances {current.Value} - {current.Key} - {distance}");

            return result;
        }

        private string GetNodeMappingKey(Point point, Dictionary<Point, char> points, int distance) => 
            $"{distance}-{point}-{string.Join(';', points.Select(p => p.Key).OrderBy(p => p.X).OrderBy(p => p.Y))}";
        private string GetMappingKey(List<Point> visitedPoints, Dictionary<Point, char> points, int distance) => $"{distance}-{string.Join(';', visitedPoints)}-{string.Join(';', points.Select(p => p.Key))}";

        public string ComputePartTwo(string[] input)
        {
            return "nope";
        }

        private char GetDoorForKey(char key) => ((char)((int)key - 32));

        private Dictionary<Point, char> RemoveKeyAndDoor(Dictionary<Point, char> points, char key)
        {
            Console.WriteLine($"Attempt to remove key {key} - keys: {string.Join(" ; ", points.Where(p => IsKey(p.Value) || IsDoor(p.Value)).Select(p => p.Value).ToArray())}");
            var door = GetDoorForKey(key);

            return points.Aggregate(new Dictionary<Point, char>(), (result, next) =>
            {
                if (next.Value == key || next.Value == door)
                {
                    Console.WriteLine($"Removed key or door {next.Value} at {next.Key}");
                    result.Add(next.Key, EMPTY);
                }
                else
                {
                    result.Add(next.Key, next.Value);
                }

                return result;
            });
        }

        private Dictionary<Point, char> RemovePoint(Dictionary<Point, char> points, Point point)
        {
            return points.Aggregate(new Dictionary<Point, char>(), (result, next) =>
            {
                if (next.Key.Equals(point))
                {
                    result.Add(next.Key, EMPTY);
                }
                else
                {
                    result.Add(next.Key, next.Value);
                }

                return result;
            });
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

        private bool CanNavigate(char c, Dictionary<Point, char> points) => points.Any(p => p.Value == c) && !IsWall(c) && !IsDoor(c);
        private bool IsEmpty(char c) => c == '.' || IsStart(c);
        private bool IsStart(char c) => c == '@';
        private bool IsWall(char c) => c == '#';

        private bool IsKey(char c) => c >= 'a' && c <= 'z';

        private bool IsDoor(char c) => c >= 'A' && c <= 'Z';

        private bool IsKeyForDoor(char key, char door) => 'z' - key == 'Z' - door;
    }

    public class PathNode
    {
        char tile;
        int distance;

        public List<PathNode> Children { get; private set; }
        public char Tile => tile;
        public int Distance => distance;

        public PathNode(char tile, int distance)
        {
            this.tile = tile;
            this.distance = distance;
            this.Children = new List<PathNode>();
        }

        public void AddChild(PathNode child)
        {
            if (child == null)
            {
                return;
            }

            if (child.Tile == Tile) {
                throw new Exception("You can't be your own child little inbred");
            }

            var competingChild = Children.FirstOrDefault(c => c.Tile == child.Tile);

            if (competingChild != null) {
                if (competingChild.Distance > child.Distance) {
                    Children.Remove(competingChild);
                }
                
                Children.Add(child);
            }
            else { 
                Children.Add(child);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            var other = obj as PathNode;

            return Tile.Equals(other.Tile) && Distance == other.Distance &&
                Children.Select(c => c.Tile).Except(other.Children.Select(c => c.Tile)).Count() > 0;
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new System.NotImplementedException();
            return base.GetHashCode();
        }
    }
}