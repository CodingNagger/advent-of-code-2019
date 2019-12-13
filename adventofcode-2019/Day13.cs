using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019
{
    public class Day13 : TwoPartDay
    {
        const char Asteroid = '#';

        string lastPrint = "";

        Game game;

        public string Compute(string[] input)
        {
            var computer = new IntCodeComputer(IntCodeProgramParser.Parse(input));
            game = new Game(computer);
            computer.AddDelegate(game);
            game.Setup();
            return $"{game.PaintGrid()}";
        }

        public string ComputePartTwo(string[] input)
        {
            return $"";
        }


    }

    public class Game : IIntCodeComputerDelegate
    {
        private IIntCodeComputer computer;
        private SetupState setupState;

        private int tmpX;
        private int tmpY;
        private GameTile tmpTile;

        private Dictionary<Point, GameTile> gameState;

        public int BlockTilesCount => gameState.Count(t => t.Value == GameTile.Block);

        public Game(IIntCodeComputer computer)
        {
            this.computer = computer;
            computer.AddDelegate(this);
            setupState = SetupState.SetX;
            gameState = new Dictionary<Point, GameTile>();
        }

        public void HandleOutput(long output)
        {
            var instruction = (int)output;
            // Console.Write($"{setupState}");

            switch (setupState)
            {
                case SetupState.SetX:
                    tmpX = instruction;
                    Console.Write($" X - {tmpX} ");
                    break;
                case SetupState.SetY:
                    tmpY = instruction;
                    Console.Write($" Y - {tmpY} ");
                    break;
                case SetupState.SetTile:
                    var tile = (GameTile)instruction;
                    var position = new Point { X = tmpX, Y = tmpY };

                    Console.WriteLine($": Adding tile {tile}");
                    gameState.Add(position, (GameTile)instruction);

                    break;
            }

            setupState = (SetupState)((int)(setupState + 1) % 3);
        }

        public void Setup()
        {
            computer.RunIntcodeProgram();
        }

        public String PaintGrid()
        {
            var builder = new StringBuilder();
            int minX = gameState.Select(p => p.Key.X).Min();
            int maxX = gameState.Select(p => p.Key.X).Max();
            int minY = gameState.Select(p => p.Key.Y).Min();
            int maxY = gameState.Select(p => p.Key.Y).Max();

            for (var y = maxY; y >= minY; y--)
            {
                for (var x = minX; x < maxX; x++)
                {
                    var point = new Point { X = x, Y = y };

                    if (gameState.ContainsKey(point) && gameState[point] != GameTile.Empty)
                    {
                        switch (gameState[point])
                        {
                            case GameTile.Ball:
                                builder.Append('o');
                                break;
                            case GameTile.Wall:
                                builder.Append('=');
                                break;
                            case GameTile.Block:
                                builder.Append('#');
                                break;
                            case GameTile.Paddle:
                                builder.Append('_');
                                break;
                            default:
                                Console.WriteLine($"Unknown tile: {gameState[point]}");
                                builder.Append('X');
                                break;
                        }
                    }
                    else
                    {
                        builder.Append(" ");
                    }
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }

    public enum SetupState
    {
        SetX = 0,
        SetY = 1,
        SetTile = 2,
    }

    public enum GameTile
    {
        Empty = 0,
        Wall = 1,
        Block = 2,
        Paddle = 3,
        Ball = 4,
    }
}
