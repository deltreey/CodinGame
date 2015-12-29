using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(String[] args)
        {
            while (true)
            {
                var output = "HOLD";

                // Read information from standard input
                var collection = new InputCollection(Console.ReadLine());

                // Compute logic here
                var tallestMountainIndex = collection.Mountains.IndexOf(collection.Mountains.Max());
                if (collection.Ship.X == tallestMountainIndex)
                {
                    output = "FIRE";
                }

                // Console.Error.WriteLine("Debug messages...");

                // Write action to standard output
                Console.WriteLine(output);
            }
        }

        class InputCollection
        {
            public Point Ship { get; set; }
            public bool HasFired = false;
            public List<int> Mountains { get; set; }

            public InputCollection(string input)
            {
                var inputSplit = input.Split(' ');
                var shipX = -1;
                var shipY = -1;
                if (inputSplit.Length > 1)
                {
                    int.TryParse(inputSplit[0], out shipX);
                    int.TryParse(inputSplit[1], out shipY);
                }
                this.Ship = new Point { X = shipX, Y = shipY };

                this.Mountains = new List<int>();
                for (var i = 0; i < 8; ++i)
                {
                    var mountain = Console.ReadLine();
                    var mountainHeight = -1;
                    int.TryParse(mountain, out mountainHeight);
                    this.Mountains.Add(mountainHeight);
                }
            }
        }

        public class Point
        {
            public int X;
            public int Y;
        }
    }
}