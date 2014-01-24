using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class Player
{
    public static Direction? LastDirection = null;
    public static BoardState State;
    public static int MyPlayerNumber;

    static void Main(String[] args)
    {
        State = new BoardState();

        while (true)
        {
            // Read information from standard input
            var consoleText = Console.ReadLine();
            var interpretedConsole = InterpretConsole(consoleText);
            State.UpdateBoard(interpretedConsole);

            // Compute logic here
            var randomGenerator = new Random(DateTime.Now.Millisecond);
            var availableDirections = new List<Direction>();
            for (var i = 1; i <= 4; ++i)
            {
                var trip = ComputeTrip((Direction)i);
                if (trip.Successful)
                {
                    availableDirections.Add((Direction)i);
                }
            }
            //Console.Error.WriteLine("Available: " + availableDirections.Count);
            if (availableDirections.Count > 0)
            {
                var selectedDirection = availableDirections[(randomGenerator.Next(1, availableDirections.Count)) - 1];
                var selectedTrip = ComputeTrip(selectedDirection);

                // Console.Error.WriteLine("Debug messages...");
                //Console.Error.WriteLine("Players: " + state.Players.Count.ToString());
                //Console.Error.WriteLine("X: " + selectedTrip.OriginalLocation.X);
                //Console.Error.WriteLine("Y: " + selectedTrip.OriginalLocation.Y);
                //Console.Error.WriteLine("New X: " + selectedTrip.NewLocation.X);
                //Console.Error.WriteLine("New Y: " + selectedTrip.NewLocation.Y);

                // Write action to standard output
                Go(selectedDirection);
            }
        }
    }

    private static string InterpretConsole(string input)
    {
        var result = string.Empty;

        var numberOfLines = GetNumberOfLines(input);
        MyPlayerNumber = GetPlayerNumber(input);
        for (var i = 0; i < numberOfLines; ++i)
        {
            result += Console.ReadLine() + "\n";
        }
        result = result.Substring(0, result.Length - 1);

        return result;
    }

    private static int GetNumberOfLines(string input)
    {
        var result = -1;

        var splitInput = input.Split(' ');
        if (splitInput.Length > 0)
        {
            int.TryParse(splitInput[0], out result);
        }

        return result;
    }

    private static int GetPlayerNumber(string input)
    {
        var result = -1;

        var splitInput = input.Split(' ');
        if (splitInput.Length > 1)
        {
            int.TryParse(splitInput[1], out result);
        }

        return result;
    }

    private static Trip ComputeTrip(Direction direction)
    {
        var mainAxis = (direction == Direction.UP || direction == Direction.DOWN) ? Axis.Y : Axis.X;
        var trip = new Trip(mainAxis, (direction == Direction.DOWN || direction == Direction.RIGHT), State.Players[MyPlayerNumber].Location);
        if (State.AvailableLocations.Contains(trip.NewLocation))
        {
            trip.Successful = true;
        }

        return trip;
    }

    private static void Go(Direction direction)
    {
        Console.WriteLine(direction.ToString());
    }
}

public class BoardState
{
    public List<TronPlayer> Players { get; set; }
    public List<Point> AvailableLocations { get; set; }

    public BoardState()
    {
        this.Players = new List<TronPlayer>();
        this.AvailableLocations = new List<Point>();
        for (var i = 0; i < 30; ++i)
        {
            for (var j = 0; j < 20; ++j)
            {
                this.AvailableLocations.Add(new Point(i, j));
            }
        }
    }

    public void UpdateBoard(string interpretedInput)
    {
        //Console.Error.WriteLine(interpretedInput);
        var lines = interpretedInput.Split('\n');
        //Console.Error.WriteLine("Lines: " + lines.Length.ToString());

        for (var l = 0; l < lines.Length; ++l)
        {
            var elements = lines[l].Split(' ');
            //Console.Error.WriteLine("Elements: " + elements.Length.ToString());
            var player = new TronPlayer();
            player.Location = new Point();
            player.RibbonOrigin = new Point();
            if (elements.Length == 4)
            {
                var originX = -1;
                var originY = -1;
                int.TryParse(elements[0], out originX);
                int.TryParse(elements[1], out originY);
                var currentX = -1;
                var currentY = -1;
                int.TryParse(elements[2], out currentX);
                int.TryParse(elements[3], out currentY);

                player.Location.X = currentX;
                player.Location.Y = currentY;
                player.RibbonOrigin.X = originX;
                player.RibbonOrigin.Y = originY;
            }
            if (this.Players.Count <= l)
            {
                this.Players.Add(player);
                this.AvailableLocations.Remove(player.RibbonOrigin);
            }
            else
            {
                this.Players[l].Location = player.Location;
            }
            this.AvailableLocations.Remove(player.Location);
        }
    }
}

public class TronPlayer
{
    public Point Location {get;set;}
    public Point RibbonOrigin { get; set; }
}

public class Point
{
    public int X {get;set;}
    public int Y {get;set;}

    public override bool Equals(object obj)
    {
        var result = false;

        if (obj == null)
        {
            result = false;
        }
        else
        {
            var point = obj as Point;
            if (point.X == this.X && point.Y == this.Y)
            {
                result = true;
            }
        }

        return result;
    }

    public override int GetHashCode()
    {
        return this.X ^ this.Y;
    }

    public Point()
    { }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}

public enum Direction
{
    UP = 1,
    LEFT = 2,
    DOWN = 3,
    RIGHT = 4
}

public class Trip
{
    public Point NewLocation { get; set; }
    public Point OriginalLocation { get; set; }
    public bool Successful { get; set; }

    public Trip(Axis axis, bool positiveDirection, Point currentLocation)
    {
        this.Successful = false;
        this.OriginalLocation = currentLocation;
        if (axis == Axis.X)
        {
            if (positiveDirection)
            {
                this.NewLocation = new Point(currentLocation.X + 1, currentLocation.Y);
            }
            else
            {
                this.NewLocation = new Point(currentLocation.X - 1, currentLocation.Y);
            }
        }
        else
        {
            if (positiveDirection)
            {
                this.NewLocation = new Point(currentLocation.X, currentLocation.Y + 1);
            }
            else
            {
                this.NewLocation = new Point(currentLocation.X, currentLocation.Y - 1);
            }
        }
    }
}

public enum InverseDirection
{
    DOWN = 1,
    RIGHT = 2,
    UP = 3,
    LEFT = 4
}

public enum Axis
{
    X = 0,
    Y = 1
}