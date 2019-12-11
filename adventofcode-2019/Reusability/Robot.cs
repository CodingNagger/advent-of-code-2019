using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public class Robot : IIntCodeComputerDelegate
    {
        private IIntCodeComputer brain;
        private RobotDirection direction;
        private Dictionary<Point, PanelColor> paintedPanels;
        private Point position;
        private bool shouldPaint;
        private PanelColor startPanelColor;
        public int UniquePanelsPaintedCount => paintedPanels.Count;

        public Robot(IIntCodeComputer brain, PanelColor startPanelColor)
        {
            this.brain = brain;
            this.direction = RobotDirection.North;
            this.paintedPanels = new Dictionary<Point, PanelColor>();
            this.position = Point.Origin;
            this.shouldPaint = true;
            this.startPanelColor = startPanelColor;
        }

        public long GetInput()
        {
            return (long)GetCameraInput();
        }

        public void HandleOutput(long output)
        {
            if (shouldPaint)
            {
                Paint((PanelColor)output);
            }
            else
            {
                UpdateDirection((RobotRotation)output);
                Move();
            }

            shouldPaint = !shouldPaint;
        }

        public void Run()
        {
            brain.RunIntcodeProgram();
        }

        public String PaintIdentifier()
        {
            var builder = new StringBuilder();
            int minX = paintedPanels.Select(p => p.Key.X).Min();
            int maxX = paintedPanels.Select(p => p.Key.X).Max();
            int minY = paintedPanels.Select(p => p.Key.Y).Min();
            int maxY = paintedPanels.Select(p => p.Key.Y).Max();

            for (var y = maxY; y >= minY; y--)
            {
                for (var x = minX; x < maxX; x++)
                {
                    var point = new Point { X = x, Y = y };

                    if (paintedPanels.ContainsKey(point) && paintedPanels[point] == PanelColor.White)
                    {
                        builder.Append(" ");
                    }
                    else
                    {
                        builder.Append("█");
                    }
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }

        PanelColor GetCameraInput()
        {
            if (paintedPanels.Count == 0)
            {
                return startPanelColor;
            }

            if (paintedPanels.ContainsKey(position))
            {
                return paintedPanels[position];
            }

            return PanelColor.Black;
        }

        void Paint(PanelColor colorCode)
        {
            if (paintedPanels.ContainsKey(position))
            {
                paintedPanels[position] = colorCode;
            }
            else
            {
                paintedPanels.Add(position, colorCode);
            }
        }

        void UpdateDirection(RobotRotation rotation)
        {
            var old = this.direction;

            if (rotation == RobotRotation.Left)
            {
                switch (direction)
                {
                    case RobotDirection.East:
                        direction = RobotDirection.South;
                        break;

                    case RobotDirection.North:
                        direction = RobotDirection.East;
                        break;

                    case RobotDirection.South:
                        direction = RobotDirection.West;
                        break;

                    case RobotDirection.West:
                        direction = RobotDirection.North;
                        break;
                }
            }
            else
            {
                switch (direction)
                {
                    case RobotDirection.East:
                        direction = RobotDirection.North;
                        break;

                    case RobotDirection.North:
                        direction = RobotDirection.West;
                        break;

                    case RobotDirection.South:
                        direction = RobotDirection.East;
                        break;

                    case RobotDirection.West:
                        direction = RobotDirection.South;
                        break;
                }
            }
        }

        void Move()
        {
            switch (direction)
            {
                case RobotDirection.East:
                    position = new Point { X = position.X - 1, Y = position.Y };
                    break;

                case RobotDirection.North:
                    position = new Point { X = position.X, Y = position.Y + 1 };
                    break;

                case RobotDirection.South:
                    position = new Point { X = position.X, Y = position.Y - 1 };
                    break;

                case RobotDirection.West:
                    position = new Point { X = position.X + 1, Y = position.Y };
                    break;
            }
        }
    }

    enum RobotRotation
    {
        Left = 0,
        Right = 1,
    }

    enum RobotDirection
    {
        West = '<',
        North = '^',
        East = '>',
        South = 'v',
    }

    public enum PanelColor
    {
        Black = 0,
        White = 1,
    }
}
