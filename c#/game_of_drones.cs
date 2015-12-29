using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones
{
    public class Program
    {
        static void Main(string[] args)
        {
            var collection = new InputCollection(Console.ReadLine());
            while (true)
            {
                // Read information from standard input
                collection.ParseInput();

                // Compute logic here
                var myDrones = collection.Players[collection.PlayerNumber].Drones;
                if (collection.Hive == null)
                {   //set base zone
                    var droneMiddle = Points.Center(myDrones.Select(d => d.Location).ToArray());
                    var distance = 10000;
                    foreach (var zone in collection.Zones)
                    {
                        var closest = collection.GetClosestPointInZone(droneMiddle, zone);
                        var zoneDist = Points.Distance(droneMiddle, closest);
                        if (zoneDist < distance)
                        {
                            collection.Hive = zone;
                            distance = zoneDist;
                        }
                    }
                }

                if (collection.Hive.PlayerInControl != collection.PlayerNumber)
                {   //make sure I always own my base zone
                    Console.Error.WriteLine("I don't own the Hive: {0}, {1}", collection.Hive.Center.X, collection.Hive.Center.Y);
                    foreach (var drone in myDrones)
                    {
                        var closest = collection.GetClosestPointInZone(drone.Location, collection.Hive);
                        GoTo(closest);
                    }
                }
                else
                {   //expand out toward nearby zones
                    Console.Error.WriteLine("Going after enemy zones");
                    var distance = 10000;
                    var closestZone = collection.Hive;
                    foreach (var zone in collection.Zones)
                    {
                        if (zone.Center != collection.Hive.Center)
                        {
                            var zoneDist = Points.Distance(collection.Hive.Center, zone.Center);
                            if (zoneDist < distance)
                            {
                                closestZone = zone;
                                distance = zoneDist;
                            }
                        }
                    }
                    var target = closestZone;
                    var zoneDistance = distance - 200;  //remove radii from distance
                    var enemies = collection.Players.Where(p => p.PlayerNumber != collection.PlayerNumber).ToList();
                    //Group drones according to their distance from the Hive, one from each player
                    List<List<InputCollection.Drone>> enemyDroneGroups = new List<List<InputCollection.Drone>>();
                    for (var i = 0; i < collection.DroneCount; ++i)
                    {
                        enemyDroneGroups.Add(new List<InputCollection.Drone>());
                    }
                    foreach (var enemy in enemies)
                    {
                        var distancedDrones = enemy.Drones.OrderBy(d => d.DistanceFromHive).ToList();
                        for (var d = 0; d < distancedDrones.Count; ++d)
                        {
                            var drone = new InputCollection.Drone(collection, enemy.PlayerNumber);
                            //save some processing by passing the distance to the clone
                            drone.DistanceFromHive = distancedDrones[d].DistanceFromHive;
                            enemyDroneGroups[d].Add(drone);
                        }
                    }
                    var commands = new List<Point>();
                    var myDistancedDrones = myDrones.OrderBy(d => Points.Distance(d.Location, collection.GetClosestPointInZone(d.Location, target))).ToList();
                    myDistancedDrones.Reverse();
                    for (var i = 0; i < collection.DroneCount; ++i)
                    {
                        var myDroneDist = myDistancedDrones[i].DistanceFromHive;
                        var closestEnemyDroneDist = enemyDroneGroups[i][0].DistanceFromHive;
                        Console.Error.WriteLine("My Distance: {0}, Enemy Distance: {1}", myDroneDist, closestEnemyDroneDist);
                        var index = myDrones.IndexOf(myDistancedDrones[i]);
                        while (commands.Count <= index)
                        {
                            commands.Add(new Point());
                        }
                        if (myDroneDist < (closestEnemyDroneDist - 100))
                        {
                            commands[index] = collection.GetClosestPointInZone(myDistancedDrones[i].Location, target);
                        }
                        else
                        {
                            commands[index] = collection.GetClosestPointInZone(myDistancedDrones[i].Location, collection.Hive);
                        }
                    }

                    for (var c = 0; c < commands.Count; ++c)
                    {
                        GoTo(commands[c]);
                    }
                }
            }
        }

        static void GoTo(Point point)
        {
            Console.WriteLine("{0} {1}", point.X, point.Y);
        }
    }

    public class InputCollection
    {
        public List<Zone> Zones { get; set; }
        public List<Player> Players { get; set; }
        public int PlayerNumber { get; set; }
        public int PlayerCount { get; set; }
        public int DroneCount { get; set; }
        public Zone Hive { get; set; }

        public InputCollection(string input)
        {
            var inputSplit = input.Split(' ');
            var playerCount = -1;
            var playerId = -1;
            var droneCount = -1;
            var zoneCount = -1;
            if (inputSplit.Length > 3)
            {
                int.TryParse(inputSplit[0], out playerCount);
                int.TryParse(inputSplit[1], out playerId);
                int.TryParse(inputSplit[2], out droneCount);
                int.TryParse(inputSplit[3], out zoneCount);
            }
            this.PlayerCount = playerCount;
            this.Players = new List<Player>();
            for (var p = 0; p < this.PlayerCount; ++p)
            {
                this.Players.Add(new Player(p));
            }
            this.PlayerNumber = playerId;
            this.DroneCount = droneCount;
            this.Zones = new List<Zone>();
            for (var z = 0; z < zoneCount; ++z)
            {
                var zoneInput = Console.ReadLine();
                var zoneSplit = zoneInput.Split(' ');
                var zoneX = -1;
                var zoneY = -1;
                if (zoneSplit.Length > 1)
                {
                    int.TryParse(zoneSplit[0], out zoneX);
                    int.TryParse(zoneSplit[1], out zoneY);
                }
                this.Zones.Add(new Zone(zoneX, zoneY));
            }
        }

        public void ParseInput()
        {
            for (var z = 0; z < this.Zones.Count; ++z)
            {
                var input = Console.ReadLine();
                var zoneController = -1;
                int.TryParse(input, out zoneController);
                this.Zones[z].PlayerInControl = zoneController;
            }
            for (var p = 0; p < this.PlayerCount; ++p)
            {
                for (var d = 0; d < this.DroneCount; ++d)
                {
                    if (this.Players[p].Drones.Count <= d)
                    {
                        this.Players[p].Drones.Add(new Drone(this, d));
                    }
                    var input = Console.ReadLine();
                    var inputSplit = input.Split(' ');
                    var x = -1;
                    var y = -1;
                    if (inputSplit.Length > 1)
                    {
                        int.TryParse(inputSplit[0], out x);
                        int.TryParse(inputSplit[1], out y);
                    }
                    this.Players[p].Drones[d].Location = new Point(x, y);
                    //reset distance form hive so it can be recalculated
                    this.Players[p].Drones[d].DistanceFromHive = -1;
                }
            }
        }

        public Point GetClosestPointInZone(Point start, Zone zone)
        {
            Point result = null;

            var distance = 10000;
            foreach (var point in zone.EdgePoints)
            {
                var testDistance = Points.Distance(start, point);
                if (testDistance < distance)
                {
                    distance = testDistance;
                    result = point;
                }
            }

            return result;
        }

        public class Zone
        {
            public List<Point> EdgePoints { get; set; }
            public Point Center { get; set; }
            public int PlayerInControl { get; set; }
            public int Radius = 100;

            public Zone(int x, int y)
            {
                this.Center = new Point(x, y);
                this.PlayerInControl = -1;

                this.EdgePoints = new List<Point>();
                var circumference = 2 * Math.PI * this.Radius;
                //the circumference gives me the number of points around the edge of the circle
                var alpha = (2 * Math.PI) / circumference;
                //then divide 2*pi (360 degrees in radians) by the number of points I wish to calculate
                for (var p = 0; p < circumference; ++p)
                {
                    //theta is the distance between the points * the point I want to calculate
                    var theta = alpha * (p + 1);    //p+1 because p is 0 based
                    //some trig to turn radians into XY coords
                    var point = new Point((int)(Math.Cos(theta) * this.Radius), (int)(Math.Sin(theta) * this.Radius));
                    //add the center x and y to get our points on the grid
                    point.X += x;
                    point.Y += y;
                    this.EdgePoints.Add(point);
                }
            }
        }

        public class Player
        {
            public List<Drone> Drones { get; set; }
            public int PlayerNumber { get; set; }

            public Player(int playerNumber)
            {
                this.PlayerNumber = playerNumber;
                this.Drones = new List<Drone>();
            }
        }

        public class Drone
        {
            public InputCollection Collection { get; set; }
            public int PlayerNumber { get; set; }
            public Point Location { get; set; }
            private int _distanceFromHive = -1;
            public int DistanceFromHive
            {
                get
                {
                    if (this._distanceFromHive == -1)
                    {
                        var closest = this.Collection.GetClosestPointInZone(this.Location, this.Collection.Hive);
                        this._distanceFromHive = Points.Distance(this.Location, closest);
                    }

                    return _distanceFromHive;
                }
                set
                {
                    this._distanceFromHive = value;
                }
            }

            public Drone(InputCollection collection, int playerNumber)
            {
                this.Collection = collection;
                this.PlayerNumber = playerNumber;
            }
        }
    }

    public class Board
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<List<string>> Points { get; set; }

        public void Create(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Points = new List<List<string>>();
            for (var x = 0; x < this.Width; ++x)
            {
                this.Points[x] = new List<string>();
                for (var y = 0; y < this.Height; ++y)
                {
                    this.Points[x][y] = string.Empty;
                }
            }
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

    public static class Points
    {
        public static int Distance(Point p1, Point p2)
        {
            //simple distance formula.  Frankly, I'm surprised this isn't in the Math static class
            return (int)Math.Sqrt(((p2.X - p1.X) * (p2.X - p1.X)) + ((p2.Y - p1.Y) * (p2.Y - p1.Y)));
        }

        public static Point Center(params Point[] p)
        {
            Point result = null;

            var totalX = 0;
            var totalY = 0;
            foreach (var point in p)
            {
                totalX += point.X;
                totalY += point.Y;
            }
            var midX = totalX / p.Length;
            var midY = totalY / p.Length;
            //the more complex the point collection, the more relevant it is that a "Center" is best
            //defined as the average x and average y
            result = new Point(midX, midY);

            return result;
        }
    }
}
