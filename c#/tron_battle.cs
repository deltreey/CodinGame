using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class Player
{
    public static Directions? LastDirection = null;
    public static BoardState State;
    public static ConsoleInterpreter Interpreter;
    public static int MyPlayerNumber;

    static void Main(String[] args)
    {
        Interpreter = new ConsoleInterpreter();
        State = new BoardState();
        Random randomGenerator = null;

        while (true)
        {
            // Read information from standard input
            var consoleText = Console.ReadLine();
            State.MyPlayerNumber = Interpreter.GetPlayerNumber(consoleText);
            MyPlayerNumber = State.MyPlayerNumber;
            State.UpdateBoard(Interpreter.InterpretConsole(consoleText));
            if (randomGenerator == null)
            {
                DateTime startDate = new DateTime(2014, 2, 28, 7, 0, 0);
                randomGenerator = new Random((int)startDate.Subtract(DateTime.Now).TotalSeconds);
            }

            // Compute logic here
            var bestValue = -1000;
            var availableTrips = new List<Trip>();
            for (var i = 1; i <= 4; ++i)
            {
                var trip = ComputeTrip(new Direction(i), State.Players[MyPlayerNumber].Location);
                if (trip.Successful)
                {
                    availableTrips.Add(trip);
                    Console.Error.WriteLine(trip.Value);
                    if (trip.Value > bestValue)
                    {
                        bestValue = trip.Value;
                    }
                }
            }
            //Console.Error.WriteLine("Available Directions: " + availableDirections.Count);
            //Console.Error.WriteLine("Available Locations: " + State.AvailableLocations.Count);
            if (availableTrips.Count > 0)
            {
                //Console.Error.WriteLine("Available: " + availableTrips.Count);
                var idealTrips = availableTrips.Where(t => t.Value == bestValue).ToList();
                //Console.Error.WriteLine("Ideal: " + idealTrips.Count);
                var selectedTrip = idealTrips[randomGenerator.Next(0, idealTrips.Count - 1)];

                // Console.Error.WriteLine("Debug messages...");
                //Console.Error.WriteLine("Players: " + state.Players.Count.ToString());
                //Console.Error.WriteLine("X: " + selectedTrip.OriginalLocation.X);
                //Console.Error.WriteLine("Y: " + selectedTrip.OriginalLocation.Y);
                //Console.Error.WriteLine("New X: " + selectedTrip.NewLocation.X);
                //Console.Error.WriteLine("New Y: " + selectedTrip.NewLocation.Y);

                // Write action to standard output
                Go(selectedTrip.Direction.Current);
            }
            else
            {
                Console.WriteLine("LEFT");
            }
        }
    }

    public static Trip ComputeTrip(Direction direction, Point startLocation)
    {
        var trip = new Trip(direction, startLocation, State);
        if (State.AvailableLocations.Contains(trip.NewLocation))
        {
            trip.Successful = true;
        }

        return trip;
    }

    private static void Go(Directions direction)
    {
        Console.WriteLine(direction.ToString());
    }
}

public class ConsoleInterpreter
{
    public List<TronPlayer> InterpretConsole(string input)
    {
        var result = new List<TronPlayer>();

        var numberOfLines = this.GetNumberOfLines(input);
        for (var i = 0; i < numberOfLines; ++i)
        {
            var line = Console.ReadLine();
            var elements = line.Split(' ');
            //Console.Error.WriteLine("Elements: " + elements.Length.ToString());
            var player = new TronPlayer();
            player.Location = new Point();
            player.RibbonOrigin = new Point();
            player.RibbonLocations = new List<Point>();
            player.RemovedFromGame = false;
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

                result.Add(player);
            }
        }

        return result;
    }

    public int GetPlayerNumber(string input)
    {
        var result = -1;

        var splitInput = input.Split(' ');
        if (splitInput.Length > 1)
        {
            int.TryParse(splitInput[1], out result);
        }

        return result;
    }

    private int GetNumberOfLines(string input)
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
    public List<TronPlayer> Players { get; set; }
    public List<Point> AvailableLocations { get; set; }
    public int MyPlayerNumber { get; set; }

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

    public void UpdateBoard(List<TronPlayer> players)
    {
        for (var l = 0; l < players.Count; ++l)
        {
            if (this.Players.Count <= l)
            {
                players[l].RibbonLocations.Add(players[l].RibbonOrigin);
                this.Players.Add(players[l]);
                this.AvailableLocations.Remove(players[l].RibbonOrigin);
            }
            else
            {
                this.Players[l].Location = players[l].Location;
            }
            if (players[l].Location.X == -1 && !this.Players[l].RemovedFromGame)
            {
                Console.Error.WriteLine("Removing Player " + l);
                this.AvailableLocations.AddRange(this.Players[l].RibbonLocations);
                this.AvailableLocations = this.AvailableLocations.Distinct().ToList();  //remove potential duplicates
                this.Players[l].RemovedFromGame = true;
            }
            else if (players[l].Location != players[l].RibbonOrigin && players[l].Location.X != -1)
            {
                this.AvailableLocations.Remove(players[l].Location);
                this.Players[l].RibbonLocations.Add(players[l].Location);
            }
        }
    }
}

public class TronPlayer
{
    public Point Location {get;set;}
    public List<Point> RibbonLocations { get; set; }
    public List<Trip> AvailableTrips
    {
        get
        {
            var availableTrips = new List<Trip>();
            for (var i = 1; i <= 4; ++i)
            {
                var trip = Player.ComputeTrip(new Direction(i), this.Location);
                if (trip.Successful)
                {
                    availableTrips.Add(trip);
                }
            }
            return availableTrips;
        }
    }
    public List<Point> AvailableLocations
    {
        get
        {
            var result = new List<Point>();

            var availableTrips = new List<Trip>();
            for (var i = 1; i <= 4; ++i)
            {
                var trip = Player.ComputeTrip(new Direction(i), this.Location);
                if (trip.Successful)
                {
                    availableTrips.Add(trip);
                }
            }

            foreach (var trip in availableTrips)
            {
                result.Add(trip.NewLocation);
            }

            return result;
        }
    }
    public Point RibbonOrigin { get; set; }
    public bool RemovedFromGame { get; set; }
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

    public bool Equals(Point point)
    {
        var result = false;

        if (point.X == this.X && point.Y == this.Y)
        {
            result = true;
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

public class Direction
{
    public Directions Current { get; set; }
    public Directions Inverse
    {
        get
        {
            var result = Directions.DOWN;

            switch((int)this.Current)
            {
                case 1:
                    result = Directions.DOWN;
                    break;
                case 2:
                    result = Directions.RIGHT;
                    break;
                case 3:
                    result = Directions.UP;
                    break;
                case 4:
                    result = Directions.LEFT;
                    break;
                default:
                    break;
            }

            return result;
        }
    }
    public Axis Axis
    {
        get
        {
            return (this.Current == Directions.UP || this.Current == Directions.DOWN) ? Axis.Y : Axis.X;
        }
    }
    public bool IsPositive
    {
        get
        {
            return this.Current == Directions.DOWN || this.Current == Directions.RIGHT;
        }
    }

    public Direction(Directions current)
    {
        this.Current = current;
    }

    public Direction(int current)
    {
        this.Current = (Directions)current;
    }
}

public class Trip
{
    public static int ExitValue = 1000;
    public static int KillPlayerValue = 2100;
    public static int EnterTrapValue = -400;
    public static int MaxDeadEndCheck = 50;
    public static int MaxSpaceCheck = 1000;

    public Direction Direction { get; set; }
    public Point NewLocation { get; set; }
    public Point OriginalLocation { get; set; }
    public bool Successful { get; set; }
    private int value = -1;
    public BoardState State { get; set; }
    public int Value
    {
        get
        {
            if (this.value == -1)
            {
                this.value = this.AssessValue(this.State);
            }

            return this.value;
        }
    }
    
    public Trip(Direction direction, Point currentLocation, BoardState state)
    {
        this.Direction = direction;
        this.Successful = false;
        this.OriginalLocation = currentLocation;
        this.State = state;
        if (direction.Axis == Axis.X)
        {
            if (direction.IsPositive)
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
            if (direction.IsPositive)
            {
                this.NewLocation = new Point(currentLocation.X, currentLocation.Y + 1);
            }
            else
            {
                this.NewLocation = new Point(currentLocation.X, currentLocation.Y - 1);
            }
        }
    }

    public int AssessValue(BoardState state)
    {
        int result = 0;

        var availableTrips = new List<Trip>();
        for (var i = 1; i <= 4; ++i)
        {
            var trip = Player.ComputeTrip(new Direction(i), this.NewLocation);
            if (trip.Successful)
            {
                availableTrips.Add(trip);
            }
        }
        //FIRST HEURISTIC: avoid dead ends that are 1 block away
        result += ExitHeuristic(availableTrips.Count);
        //SECOND HEURISTIC: Distance to dead ends down 1 block alleys
        result += DeadEndDistanceHeuristic(availableTrips);
        //THIRD HEURISTIC: Kill players this move if possible
        result += KillPlayerHeuristic(state, availableTrips);
        //FOURTH HEURISTIC: Avoid moves that allow players to kill me on their next move
        result += AvoidTrapsHeuristic(state, availableTrips);
        //FIFTH HEURISTIC: Select the direction with the most available points on the board
        result += MaxSpaceHeuristic(state, availableTrips);

        return result;
    }

    public int ExitHeuristic(int availableTripCount)
    {
        return availableTripCount > 0 ? ExitValue : 0;
    }

    public int DeadEndDistanceHeuristic(List<Trip> availableTrips)
    {
        var result = 0;

        if (availableTrips.Count == 1)
        {
            var latestAvailableDirections = availableTrips.Select(t => t.Direction).ToList();
            var latestTrip = this;
            var counter = 0;
            while (latestAvailableDirections.Count == 1 && counter <= MaxDeadEndCheck)
            {
                counter++;
                var inverse = latestAvailableDirections[0].Inverse;
                latestTrip = Player.ComputeTrip(latestAvailableDirections[0], latestTrip.NewLocation);
                latestAvailableDirections = new List<Direction>();
                for (var i = 1; i <= 4; ++i)
                {
                    if ((Directions)i != inverse)
                    {
                        var nextTrip = Player.ComputeTrip(new Direction(i), latestTrip.NewLocation);
                        if (nextTrip.Successful)
                        {
                            latestAvailableDirections.Add(nextTrip.Direction);
                        }
                    }
                }
            }
            if (latestAvailableDirections.Count == 0)
            {
                //Console.Error.WriteLine("Counter: " + counter);
                // when faced with dead ends, select the one that's the furthest away
                result = counter;
            }
        }

        return result;
    }

    public int KillPlayerHeuristic(BoardState state, List<Trip> availableTrips)
    {
        var result = 0;

        for (var i = 0; i < state.Players.Count; ++i)
        {
            if (i != state.MyPlayerNumber)
            {
                if (state.Players[i].AvailableLocations.Count == 1)
                {
                    //Console.Error.WriteLine("Player " + i + " has only one exit");
                    if (this.NewLocation.Equals(state.Players[i].AvailableLocations[0]))
                    {
                        //if we can block off a player's only possible direction, always do that, killing the player will provide us with an exit
                        result += KillPlayerValue;
                    }
                }
                
            }
        }

        return result;
    }

    public int AvoidTrapsHeuristic(BoardState state, List<Trip> availableTrips)
    {
        var result = 0;

        for (var i = 0; i < state.Players.Count; ++i)
        {
            if (i != state.MyPlayerNumber)
            {
                if (availableTrips.Count == 1)
                {
                    //Console.Error.WriteLine("Trip will result in only 1 exit");
                    var playerTrips = state.Players[i].AvailableTrips;
                    if (playerTrips.Exists(a => a.NewLocation.Equals(availableTrips[0].NewLocation)))
                    {
                        //if a player could kill us after our next move, don't go that way
                        result += EnterTrapValue;
                    }
                }
            }
        }

        return result;
    }

    public int MaxSpaceHeuristic(BoardState state, List<Trip> availableTrips)
    {
        var result = 0;

        if (availableTrips.Count > 0)
        {
            var points = 0;
            var spaces = new List<Point>(state.AvailableLocations);
            var spacesChecked = 0;
            foreach (var player in state.Players)
            {
                spaces.Remove(player.Location);
            }
            var availableLocations = availableTrips.Select(t => t.NewLocation);
            foreach (var location in availableLocations)
            {
                
                spaces.Remove(location);
                var locationsToCheck = new List<Point>();
                locationsToCheck.Add(location);
                while (locationsToCheck.Count > 0 && spacesChecked <= MaxSpaceCheck)
                {
                    for (var i = 1; i <= 4; ++i)
                    {
                        var trip = new Trip(new Direction(i), locationsToCheck[0], state);
                        if (spaces.Contains(trip.NewLocation))
                        {
                            spacesChecked++;
                            points++;
                            locationsToCheck.Add(trip.NewLocation);
                            spaces.Remove(trip.NewLocation);
                        }
                    }
                    locationsToCheck.Remove(locationsToCheck[0]);
                    if (spaces.Count == 0)
                    {
                        break;
                    }
                }

                if (spaces.Count == 0 || spacesChecked == MaxSpaceCheck)
                {
                    break;
                }
            }
            result += points;
        }

        return result;
    }
}

public enum Directions
{
    UP = 1,
    LEFT = 2,
    DOWN = 3,
    RIGHT = 4
}

public enum Axis
{
    X = 0,
    Y = 1
}