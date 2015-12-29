using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class Player
{
    public static GameBoard Board { get; set; }

    static void Main(String[] args)
    {
        // Read init information from standard input, if any
        var consoleText = Console.ReadLine();
        //Console.Error.WriteLine(consoleText);
        var interpretedConsoleText = InterpretInitialConsole(consoleText);
        Console.Error.WriteLine(interpretedConsoleText);
        Board = new GameBoard(interpretedConsoleText);
        Console.Error.WriteLine("Board");
        Console.Error.WriteLine(Board.Ground.Count);

        while (true)
        {
            // Read information from standard input
            Board.GetPosition(Console.ReadLine());
            Console.Error.WriteLine(Board.Lander.Fuel);

            // Compute logic here

            // Console.Error.WriteLine("Debug messages...");

            // Write action to standard output
            Console.WriteLine("-20 3");
        }
    }

    private static string InterpretInitialConsole(string input)
    {
        var result = string.Empty;

        var numberOfLines = -1;
        int.TryParse(input, out numberOfLines);
        //Console.Error.WriteLine(numberOfLines);
        for (var i = 0; i < numberOfLines; ++i)
        {
            result += Console.ReadLine() + "\n";
        }
        result = result.Substring(0, result.Length - 1);

        return result;
    }
}

public class GameBoard
{
    public List<Point> Ground { get; set; }
    public Lander Lander { get; set; }

    public GameBoard(string interpretedConsoleText)
    {
        this.Ground = new List<Point>();
        var lines = interpretedConsoleText.Split('\n');
        foreach (var line in lines)
        {
            var axes = line.Split(' ');
            //Console.Error.WriteLine("Axis");
            //Console.Error.WriteLine(axes.Length);
            //for (var i = 0; i < axes.Length; ++i)
            //{
            //    Console.Error.WriteLine(i + ": " + axes[i]);
            //}
            if (axes.Length > 1)
            {
                this.Ground.Add(new Point(int.Parse(axes[0]), int.Parse(axes[1])));
            }
        }
    }

    public void GetPosition(string consoleText)
    {
        var splitText = consoleText.Split(' ');
        if (splitText.Length > 6)
        {
            this.Lander = new Lander();
            this.Lander.Position = new Point(int.Parse(splitText[0]), int.Parse(splitText[1]));
            this.Lander.HorizontalSpeed = int.Parse(splitText[2]);
            this.Lander.VerticalSpeed = int.Parse(splitText[3]);
            this.Lander.Fuel = int.Parse(splitText[4]);
            this.Lander.Angle = int.Parse(splitText[5]);
            this.Lander.Power = int.Parse(splitText[6]);
        }
    }
}

public class Lander
{
    public Point Position { get; set; }
    public int VerticalSpeed { get; set; }
    public int HorizontalSpeed { get; set; }
    public int Angle { get; set; }
    public int Power { get; set; }
    public int Fuel { get; set; }
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