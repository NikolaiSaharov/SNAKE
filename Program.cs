using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

public enum Boundary
{
    MaxRight = 100,
    MaxBottom = 50
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class Snake
{
    public List<Point> Body
    {
        get { return body; }
    }
    private List<Point> body;
    private Direction direction;

    public Snake()
    {
        body = new List<Point>();
        body.Add(new Point(0, 0));
        direction = Direction.Right;
    }

    public void ChangeDirection(Direction newDirection)
    {
        direction = newDirection;
    }

    public void Move()
    {
        Point head = body[0];
        Point newHead = new Point(head.X, head.Y);

        switch (direction)
        {
            case Direction.Up:
                newHead.Y--;
                break;
            case Direction.Down:
                newHead.Y++;
                break;
            case Direction.Left:
                newHead.X--;
                break;
            case Direction.Right:
                newHead.X++;
                break;
        }

        // Check if the snake is out of bounds
        if (newHead.X < 0 || newHead.X >= (int)Boundary.MaxRight || newHead.Y < 0 || newHead.Y >= (int)Boundary.MaxBottom)
        {
            Console.WriteLine("Game Over! The snake is out of bounds.");
            Environment.Exit(0);
        }

        // Check if the snake hit itself
        if (body.Contains(newHead))
        {
            Console.WriteLine("Game Over! The snake hit itself.");
            Environment.Exit(0);
        }

        body.Insert(0, newHead);
        body.RemoveAt(body.Count - 1);
    }

    public void Grow()
    {
        Point tail = body[body.Count - 1];
        body.Add(new Point(tail.X, tail.Y));
    }

    public void Draw()
    {
        foreach (Point point in body)
        {
            Console.SetCursorPosition(point.X, point.Y);
            Console.Write("*");
        }
    }
}

public class Food
{
    public Point Position { get; private set; }

    public Food()
    {
        Random random = new Random();
        Position = new Point(random.Next(0, (int)Boundary.MaxRight), random.Next(0, (int)Boundary.MaxBottom));
    }

    public void Draw()
    {
        Console.SetCursorPosition(Position.X, Position.Y);
        Console.Write("F");
    }
}

public class Game
{
    private Snake snake;
    private Food food;

    public Game()
    {
        snake = new Snake();
        food = new Food();
    }

    public void Start()
    {
        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        snake.ChangeDirection(Direction.Up);
                        break;
                    case ConsoleKey.DownArrow:
                        snake.ChangeDirection(Direction.Down);
                        break;
                    case ConsoleKey.LeftArrow:
                        snake.ChangeDirection(Direction.Left);
                        break;
                    case ConsoleKey.RightArrow:
                        snake.ChangeDirection(Direction.Right);
                        break;
                }
            }

            snake.Move();

            if (snake.Body[0].X == food.Position.X && snake.Body[0].Y == food.Position.Y)
            {
                snake.Grow();
                food = new Food();
            }

            Console.Clear();
            snake.Draw();
            food.Draw();

            Thread.Sleep(100);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
    }
}