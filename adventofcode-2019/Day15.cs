using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public class Day15 : TwoPartDay
    {
        public string Compute(string[] input)
        {
            var computer = new IntCodeComputer(IntCodeProgramParser.Parse(input));
            var explorer = new Explorer(computer, Point.Origin);
            computer.SetDatasource(explorer);

            try
            {
                while (true)
                {
                    explorer.Explore();
                }
            }
            catch (Explorer.QuestCompletedException)
            {
                var map = explorer.Map;
                return $"{explorer.Distance}";
            }
        }

        public string ComputePartTwo(string[] input)
        {
            var maxMinutes = 0;
            var program = IntCodeProgramParser.Parse(input);

            for (int i = 0; i < 1000; i++) // using stats for the filfiest hack in history of filthy hacks, it will eventually give the right answer
            {
                var computer = new IntCodeComputer(program);
                var explorer = new Explorer(computer, Point.Origin);
                computer.SetDatasource(explorer);

                try
                {
                    while (true)
                    {
                        explorer.Explore();
                    }
                }
                catch (Explorer.QuestCompletedException)
                {
                    var filled = explorer.Map.Where(p => p.Value == Block.OxygenSystem).Select(p => p.Key).Distinct().ToList();
                    var fillable = explorer.Map.Where(p => p.Value != Block.Wall).Select(p => p.Key).Distinct().ToList();
                    var minutes = 0;

                    while (fillable.Count > filled.Count)
                    {
                        var toFill = new List<Point>();
                        foreach (var filledPoint in filled)
                        {
                            foreach (var potentialFill in explorer.GetPotentialPointsInfo(filledPoint).Select(ff => ff.Value))
                            {
                                if (!filled.Contains(potentialFill) && fillable.Contains(potentialFill))
                                {
                                    toFill.Add(potentialFill);
                                }
                            }
                        }

                        foreach (var pointToFill in toFill.Distinct())
                        {
                            filled.Add(pointToFill);
                        }

                        minutes++;
                        // Console.WriteLine($"{minutes} minutes - {filled.Count}/{fillable.Count} filled");
                    }

                    maxMinutes = Math.Max(maxMinutes, minutes);
                    Console.WriteLine($"{minutes} vs {maxMinutes} minutes - {filled.Count}/{fillable.Count} filled");
                }
            }

            return $"{maxMinutes}";
        }
    }

    public class Explorer : IIntCodeComputerDelegate, IIntCodeComputerDatasource
    {
        private IIntCodeComputer computer;
        Dictionary<Point, Block> map;
        Dictionary<Point, int> distances;
        Direction direction;
        Point currentPosition;
        private int distance;
        Random random = new Random();

        public Dictionary<Point, Block> Map => map;
        public int Distance => distances[map.First(m => m.Value == Block.OxygenSystem).Key];

        public Explorer(IIntCodeComputer computer, Point start)
        {
            this.computer = computer;
            computer.AddDelegate(this);

            direction = Direction.North;
            currentPosition = start;
            map = new Dictionary<Point, Block>();
            map.Add(currentPosition, Block.Empty);
            distance = 0;
            distances = new Dictionary<Point, int>();
            distances.Add(currentPosition, 0);
        }

        public void Explore()
        {
            computer.RunIntcodeProgram();
        }

        public long GetInput()
        {
            Point potentialPoint;
            Direction potentialDirection;
            KeyValuePair<Direction, Point>[] info;

            info = GetPotentialPointsInfo().Where(kvp => !map.ContainsKey(kvp.Value) || map[kvp.Value] != Block.Wall).ToArray();

            if (info.Length > 0)
            {
                var nextMove = info[random.Next(0, info.Length)];
                potentialDirection = nextMove.Key;
                potentialPoint = nextMove.Value;
                // Console.WriteLine($"Moving {direction} towards {potentialPoint} from {currentPosition}");
                direction = potentialDirection;


            }
            // else {
            //     Console.WriteLine($"Moving {direction}");
            // }

            return (long)direction;
        }

        public Dictionary<Direction, Point> GetPotentialPointsInfo()
        {
            return GetPotentialPointsInfo(currentPosition);
        }

        public Dictionary<Direction, Point> GetPotentialPointsInfo(Point start)
        {
            return Enum.GetValues(typeof(Direction)).Cast<Direction>().ToDictionary(d => d, d => GetPointForDirectionFromPoint(d, start));
        }

        public void HandleOutput(long output)
        {
            var code = (StatusCode)output;

            var pointOfInterest = GetPointForDirection();
            // Console.WriteLine($"Action: {code}");

            distance++;

            switch (code)
            {
                case StatusCode.OxygenSystemFound:
                    currentPosition = pointOfInterest;
                    if (!map.ContainsKey(pointOfInterest))
                    {
                        map.Add(currentPosition, Block.OxygenSystem);
                        // Console.WriteLine($"Added {Block.OxygenSystem} at {currentPosition} - distance: {distance}");
                    }
                    // else Console.WriteLine($"Back to {Block.OxygenSystem} - {map[pointOfInterest]} at {pointOfInterest} - distance: {distance}");
                    break;
                case StatusCode.PathFound:
                    currentPosition = pointOfInterest;
                    if (!map.ContainsKey(pointOfInterest))
                    {
                        map.Add(currentPosition, Block.Empty);

                        // Console.WriteLine($"Added {Block.Empty} at {currentPosition} - distance: {distance}");
                    }
                    // else Console.WriteLine($"Back to {Block.Empty} - {map[pointOfInterest]} at {pointOfInterest} - distance: {distance}");
                    break;
                case StatusCode.WallFound:
                    if (map.ContainsKey(pointOfInterest))
                    {
                        // Console.WriteLine($"Back to {Block.Wall} - {map[pointOfInterest]} at {pointOfInterest} - distance: {distance}");
                    }
                    else map.Add(pointOfInterest, Block.Wall);
                    // Console.WriteLine($"Added {Block.Wall} at {pointOfInterest} - distance: {distance}");
                    break;
                default:
                    throw new Exception("WTF");
            }

            if (code != StatusCode.WallFound)
            {
                if (!distances.ContainsKey(pointOfInterest))
                {
                    distances.Add(pointOfInterest, distance);
                    // Console.WriteLine($"First pass to {pointOfInterest} - distance: {distances[pointOfInterest]} vs {distance}");
                }
                else
                {
                    // Console.WriteLine($"Return to block at {pointOfInterest} - distance: {distances[pointOfInterest]} vs {distance}");
                    distances[pointOfInterest] = Math.Min(distances[pointOfInterest], distance);
                    distance = distances[pointOfInterest];
                }
            }
            else
            {
                distance--;
            }

            if (map.Any(m => m.Value == Block.OxygenSystem))
            {
                throw new QuestCompletedException();
            }
        }

        private Point GetPointForDirection()
        {
            return GetPointForDirection(direction);
        }

        private Point GetPointForDirection(Direction direction)
        {
            return GetPointForDirectionFromPoint(direction, currentPosition);
        }

        public Point GetPointForDirectionFromPoint(Direction direction, Point start)
        {
            switch (direction)
            {
                case Direction.North: return new Point { X = start.X, Y = start.Y - 1 };
                case Direction.South: return new Point { X = start.X, Y = start.Y + 1 };
                case Direction.East: return new Point { X = start.X - 1, Y = start.Y };
                case Direction.West: return new Point { X = start.X + 1, Y = start.Y };
            }

            throw new Exception("Really should not be here");
        }

        public class QuestCompletedException : Exception
        {

        }
    }

    public enum StatusCode
    {
        WallFound = 0,
        PathFound = 1,
        OxygenSystemFound = 2,
    }

    public enum Direction
    {
        North = 1,
        South = 2,
        West = 3,
        East = 4,
    }

    public enum Block
    {
        Empty = 0,
        Wall = 1,
        OxygenSystem = 2,
    }
}
