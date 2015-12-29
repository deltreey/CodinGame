using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Program
    {
        static void Main(String[] args)
        {
            var collection = new InputCollection(Console.ReadLine());
            var rand = new Random();

            while (true)
            {
                var output = string.Empty;

                // Read information from standard input
                collection.ParseInput();

                Console.Error.WriteLine("Control Room: {0}, Beam Location: {1}", collection.ControlRoom == null ? "none" : "found", collection.BeamLocation == null ? "none" : "found");
                Console.Error.WriteLine("Traveled: {0}, Travelable: {1}", collection.TraveledPoints.Count, collection.TravelablePoints.Count);
                Console.Error.WriteLine("Kirk: {0}, {1}", collection.Kirk.X, collection.Kirk.Y);
                // Compute logic here
                if (collection.ControlRoom != null && !collection.ReachedControlRoom && collection.IsDistanceSafe())
                {
                    output = collection.GetDirectionToTarget(collection.ControlRoom);
                    Console.Error.WriteLine("Going for Control Room: {0}, {1}", collection.ControlRoom.X, collection.ControlRoom.Y);
                }
                else if (collection.ReachedControlRoom)
                {
                    output = collection.GetDirectionToTarget(collection.BeamLocation);
                    Console.Error.WriteLine("Going for Beam Location: {0}, {1}", collection.BeamLocation.X, collection.BeamLocation.Y);
                }
                else
                {
                    var validMoves = collection.ValidMoves(collection.Kirk);
                    var nearestUnexplored = collection.GetNearestUnexploredPoint();
                    if (nearestUnexplored != null)
                    {
                        output = collection.GetDirectionToTarget(nearestUnexplored);
                    }
                    else
                    {
                        var randm = rand.Next(validMoves.Count);
                        output = collection.GetDirection(collection.Kirk, validMoves[randm]);
                    }
                }

                // Console.Error.WriteLine("Debug messages...");

                // Write action to standard output
                Console.WriteLine(output);
            }
        }

        class InputCollection
        {
            public Point Kirk { get; set; }
            public Point ControlRoom { get; set; }
            public bool ReachedControlRoom { get; set; }
            public Point BeamLocation { get; set; }
            public int AlarmTimeLeft { get; set; }
            public List<Point> TravelablePoints { get; set; }
            public List<Point> TraveledPoints { get; set; }
            public int MazeHeight { get; set; }
            public int MazeWidth { get; set; }
            public List<Column> Maze { get; set; }
            public List<Point> CheckedPoints { get; set; }

            public InputCollection(string input)
            {
                var inputSplit = input.Split(' ');
                var height = -1;
                var width = -1;
                var alarm = -1;
                if (inputSplit.Length > 2)
                {
                    int.TryParse(inputSplit[0], out height);
                    int.TryParse(inputSplit[1], out width);
                    int.TryParse(inputSplit[2], out alarm);
                }

                this.MazeHeight = height;
                this.MazeWidth = width;
                this.AlarmTimeLeft = alarm;
                this.Maze = new List<Column>();
                for (var i = 0; i < this.MazeWidth; ++i)
                {
                    this.Maze.Add(new Column());
                    for (var j = 0; j < this.MazeHeight; ++j)
                    {
                        this.Maze[i].Squares.Add(new Square
                            {
                                Point = new Point { X = i, Y = j },
                                Distance = 500
                            });
                    }
                }
                this.TravelablePoints = new List<Point>();
                this.TraveledPoints = new List<Point>();
                this.ReachedControlRoom = false;
            }

            public void ParseInput()
            {
                var inputSplit = Console.ReadLine().Split(' ');
                var kirkX = -1;
                var kirkY = -1;
                if (inputSplit.Length > 1)
                {
                    int.TryParse(inputSplit[0], out kirkY);
                    int.TryParse(inputSplit[1], out kirkX);
                }
                this.Kirk = new Point { X = kirkX, Y = kirkY };
                this.TraveledPoints.Add(new Point { X = kirkX, Y = kirkY });

                for (var y = 0; y < this.MazeHeight; ++y)
                {
                    var input = Console.ReadLine();

                    var columns = input.ToCharArray();
                    //Console.Error.WriteLine("Columns: {0}, Width: {1}", columns.Length, this.MazeWidth);
                    if (columns.Length == this.MazeWidth)
                    {
                        for (var x = 0; x < this.MazeWidth; ++x)
                        {
                            //Console.Error.WriteLine(columns[j]);
                            if (columns[x] != '?' && this.Maze[x].Squares[y].Value == '\0')
                            {
                                //Console.Error.WriteLine("Made it");
                                this.Maze[x].Squares[y].Value = columns[x];
                                if (columns[x] == '.' && !this.TravelablePoints.Contains(new Point{ X = x, Y = y }))
                                {
                                    this.TravelablePoints.Add(new Point { X = x, Y = y });
                                }
                                else if (columns[x] == 'C')
                                {
                                    this.ControlRoom = new Point { X = x, Y = y };
                                }
                                else if (columns[x] == 'T')
                                {
                                    this.BeamLocation = new Point { X = x, Y = y };
                                }
                            }
                        }
                    }
                }
            }

            public void Pathfind(Point target)
            {
                Pathfind(this.Kirk, target);
            }

            public void Pathfind(Point start, Point target)
            {
                // Find path from Kirk to target. First, get coordinates of target.
                foreach (var col in this.Maze)
                {
                    foreach (var square in col.Squares)
                    {
                        square.Distance = 500;
                    }
                }

                this.Maze[target.X].Squares[target.Y].Distance = 0;
                this.CheckedPoints = new List<Point>
                {
                    new Point { X = target.X, Y = target.Y }
                };

                var currentPoints = new List<Point>
                {
                    new Point { X = target.X, Y = target.Y }
                };

                var foundStart = false;
                while (!foundStart && currentPoints.Count > 0)
                {
                    var updatedPoints = new List<Point>();
                    //Console.Error.WriteLine("CurrentPoints: {0}, CheckedPoints: {1}", currentPoints.Count, this.CheckedPoints.Count);
                    foreach (var point in currentPoints)
                    {
                        var distance = this.Maze[point.X].Squares[point.Y].Distance;
                        //Console.Error.WriteLine("Checking Point: {0}, {1} - {2} ({3})", point.X, point.Y, distance, this.Maze[point.X].Squares[point.Y].Value);
                        var validMoves = this.ValidMoves(point);
                        foreach (var move in validMoves)
                        {
                            if (!this.CheckedPoints.Contains(move))
                            {
                                this.CheckedPoints.Add(move);
                                this.Maze[move.X].Squares[move.Y].Distance = distance + 1;
                                updatedPoints.Add(move);
                                if (start.Equals(move))
                                {
                                    foundStart = true;
                                    break;
                                }
                            }
                        }
                        if (foundStart)
                        {
                            break;
                        }
                    }
                    currentPoints = updatedPoints;
                }
            }

            public Point GetNearestUnexploredPoint()
            {
                Point result = null;

                this.CheckedPoints = new List<Point>
                {
                    new Point { X = this.Kirk.X, Y = this.Kirk.Y }
                };

                var currentPoints = new List<Point>
                {
                    new Point { X = this.Kirk.X, Y = this.Kirk.Y }
                };

                var foundUnexplored = false;
                while (!foundUnexplored && currentPoints.Count > 0)
                {
                    var updatedPoints = new List<Point>();
                    foreach (var point in currentPoints)
                    {
                        var validMoves = this.ValidMoves(point);
                        foreach (var move in validMoves)
                        {
                            if (!this.CheckedPoints.Contains(move))
                            {
                                this.CheckedPoints.Add(move);
                                var mazePoint = this.Maze[move.X].Squares[move.Y];
                                updatedPoints.Add(move);
                                if (mazePoint.Value == '\0')
                                {
                                    foundUnexplored = true;
                                    result = move;
                                    break;
                                }
                            }
                        }
                        if (foundUnexplored)
                        {
                            break;
                        }
                    }
                    currentPoints = updatedPoints;
                }

                return result;
            }

            public int GetDistanceBetween(Point start, Point end)
            {
                Pathfind(start, end);
                return this.Maze[start.X].Squares[start.Y].Distance;
            }

            public string GetDirectionToTarget(Point target)
            {
                this.Pathfind(target);
                var distance = 1000;
                var newMove = new Point();
                var validMoves = this.ValidMoves(this.Kirk);
                foreach (var move in validMoves)
                {
                    var square = this.Maze[move.X].Squares[move.Y];
                    Console.Error.WriteLine("Valid Move: {0}, {1} - {2}", move.X, move.Y, square.Distance);
                    if (square.Distance < distance)
                    {
                        newMove = move;
                        distance = square.Distance;
                    }
                }

                if (distance <= 0)
                {   //each time we reach a destination, we'll assume it's the control room for now
                    this.ReachedControlRoom = true;
                }

                Console.Error.WriteLine("Moving to: {0}, {1} - {2}", newMove.X, newMove.Y, distance);
                var result = this.GetDirection(this.Kirk, newMove);
                return result;
            }

            public bool IsDistanceSafe()
            {
                bool result = false;

                if (GetDistanceBetween(this.ControlRoom, this.BeamLocation) >= this.AlarmTimeLeft)
                {
                    result = true;
                }

                return result;
            }

            public string GetDirection(Point start, Point newLocation)
            {
                var result = string.Empty;

                if (start.X < newLocation.X)
                {
                    result = "RIGHT";
                }
                else if (start.X > newLocation.X)
                {
                    result = "LEFT";
                }
                else if (start.Y < newLocation.Y)
                {
                    result = "DOWN";
                }
                else
                {
                    result = "UP";
                }

                return result;
            }

            public List<Point> ValidMoves(Point startPoint)
            {
                var result = new List<Point>();

                //Console.Error.WriteLine("StartPoint: {0}", startPoint == null ? "none" : "found");
                var moves = new List<Point>
                {
                    new Point { X = startPoint.X, Y = startPoint.Y - 1 },
                    new Point { X = startPoint.X, Y = startPoint.Y + 1 },
                    new Point { X = startPoint.X - 1, Y = startPoint.Y },
                    new Point { X = startPoint.X + 1, Y = startPoint.Y }
                };
                foreach (var move in moves)
                {
                    //Console.Error.WriteLine("Move: {0}, {1}", move.X, move.Y);
                    if (move.X > -1 && move.Y > -1
                        && this.Maze.Count > move.X && this.Maze[move.X].Squares.Count > move.Y)
                    {
                        var square = this.Maze[move.X].Squares[move.Y];
                        if (square.Value != '#')
                        {
                            result.Add(move);
                        }
                    }
                }

                return result;
            }
        }

        public class Point
        {
            public int X;
            public int Y;
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

        public class Square
        {
            public Point Point { get; set; }
            public int Distance { get; set; }
            public char Value { get; set; }
        }

        public class Column
        {
            public List<Square> Squares { get; set; }

            public Column()
            {
                this.Squares = new List<Square>();
            }
        }
    }
}