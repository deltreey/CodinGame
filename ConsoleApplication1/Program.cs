using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class Player
{
    static void Main(String[] args)
    {
        var lastDirection = string.Empty;
        while (true)
        {
            // Read information from standard input
            var consoleText = Console.ReadLine();
            var numberOfLines = GetNumberOfLines(consoleText);
            consoleText = string.Empty;
            for (var i = 0; i < numberOfLines; ++i)
            {
                consoleText += Console.ReadLine() + "\n";
            }
            consoleText = consoleText.Substring(0, consoleText.Length - 1);
            var state = GetBoardState(consoleText);
            // Compute logic here

            // Console.Error.WriteLine("Debug messages...");
            Console.Error.WriteLine("Players: " + state.Players.Count.ToString());

            // Write action to standard output
            var randomGenerator = new Random(DateTime.Now.Millisecond);
            int rand = randomGenerator.Next(1, 4);

            switch (rand)
            {
                case 1:
                    if (lastDirection != "DOWN")
                    {
                        lastDirection = "UP";
                    }
                    break;
                case 2:
                    if (lastDirection != "UP")
                    {
                        lastDirection = "DOWN";
                    }
                    break;
                case 3:
                    if (lastDirection != "RIGHT")
                    {
                        lastDirection = "LEFT";
                    }
                    break;
                case 4:
                    if (lastDirection != "LEFT")
                    {
                        lastDirection = "RIGHT";
                    }
                    break;
                default:
                    break;
            }
            Console.WriteLine(lastDirection);
        }
    }

    private static BoardState GetBoardState(string input)
    {
        var results = new BoardState();

        results.Players = new List<TronPlayer>();
        Console.Error.WriteLine(input);
        var lines = input.Split('\n');
        //Console.Error.WriteLine("Lines: " + lines.Length.ToString());
        foreach (var line in lines)
        {
            var elements = line.Split(' ');
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
            results.Players.Add(player);
        }

        return results;
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
}

public class BoardState
{
    public List<TronPlayer> Players {get;set;}
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
}