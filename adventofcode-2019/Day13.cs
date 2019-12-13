using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AdventOfCode2019
{
    public class Day13 : TwoPartDay
    {
        Game game;

        public string Compute(string[] input)
        {
            var computer = new IntCodeComputer(IntCodeProgramParser.Parse(input));
            game = new Game(computer);
            game.Setup();
            return $"{game.BlockTilesCount}";
        }

        public string ComputePartTwo(string[] input)
        {
            var program = IntCodeProgramParser.Parse(input);
            program[0] = 2;
            var computer = new IntCodeComputer(program);
            game = new Game(computer);
            computer.SetDatasource(game);
            game.Setup();
            return $"Game over: {game.Score}";
        }
    }

    public class Game : IIntCodeComputerDelegate, IIntCodeComputerDatasource
    {
        private IIntCodeComputer computer;
        private SetupState setupState;
        private int tmpX;
        private int tmpY;
        private Dictionary<Point, GameTile> gameState;
        private long score;
        private JoystickOrientation orientation;
        private long ballX = -1;
        private long padX = -1;


        public int BlockTilesCount => gameState.Count(t => t.Value == GameTile.Block);
        public long Score => score;

        public Game(IIntCodeComputer computer)
        {
            this.computer = computer;
            computer.AddDelegate(this);
            setupState = SetupState.SetX;
            gameState = new Dictionary<Point, GameTile>();
            orientation = JoystickOrientation.Neutral;
        }

        public long GetInput()
        {
            return (long)orientation;
        }

        public long GetBallX()
        {
            if (gameState.Count == 0) return 0;

            return gameState.FirstOrDefault(t => t.Value == GameTile.Ball).Key.X;
        }

        public long GetPadX()
        {
            if (gameState.Count == 0) return 0;

            return gameState.FirstOrDefault(t => t.Value == GameTile.Paddle).Key.X;
        }

        public void HandleOutput(long output)
        {
            var instruction = (int)output;

            switch (setupState)
            {
                case SetupState.SetX:
                    tmpX = instruction;
                    setupState = SetupState.SetY;
                    break;
                case SetupState.SetY:
                    tmpY = instruction;
                    setupState = SetupState.SetTile;
                    break;
                case SetupState.SetTile:
                    if (tmpX == -1 && tmpY == 0)
                    {
                        UpdateScreen();
                        score = instruction;
                    }
                    else
                    {
                        var tile = (GameTile)instruction;
                        var position = new Point { X = tmpX, Y = tmpY };

                        if (gameState.ContainsKey(position))
                            gameState[position] = tile;
                        else
                            gameState.Add(position, tile);

                        if (tile == GameTile.Ball)
                        {
                            ballX = GetBallX();
                        }

                        if (tile == GameTile.Paddle)
                        {
                            padX = GetPadX();
                        }

                        if (ballX >= 0 && padX >= 0) {
                            orientation = padX == ballX ? JoystickOrientation.Neutral : padX > ballX ? JoystickOrientation.Left : JoystickOrientation.Right;
                            UpdateScreen();
                        }
                    }

                    setupState = SetupState.SetX;

                    break;
            }
        }

        public void Setup()
        {
            computer.RunIntcodeProgram();
        }

        public void UpdateScreen()
        {
            Thread.Sleep(1);
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(PaintGrid());
        }

        public String PaintGrid()
        {
            if (gameState.Count == 0)
            {
                return "";
            }
            var builder = new StringBuilder();

            builder.Append($"Score: {score}\nJoystick: {orientation}\n\n");

            int minX = gameState.Select(p => p.Key.X).Min();
            int maxX = gameState.Select(p => p.Key.X).Max();
            int minY = gameState.Select(p => p.Key.Y).Min();
            int maxY = gameState.Select(p => p.Key.Y).Max();

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
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

    public enum JoystickOrientation
    {
        Left = -1,
        Neutral = 0,
        Right = 1,
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
