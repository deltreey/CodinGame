using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {
        string LON = Console.ReadLine();
        string LAT = Console.ReadLine();
        LON = LON.Replace(",", ".");
        decimal longitude = Convert.ToDecimal(LON);
        LAT = LAT.Replace(",", ".");
        decimal latitude = Convert.ToDecimal(LAT);
        int N = int.Parse(Console.ReadLine());
        List<Defibrillator> defibrillators = new List<Defibrillator>();

        for (int i = 0; i < N; i++)
        {
            string DEFIB = Console.ReadLine();
            Console.Error.WriteLine(DEFIB);
            defibrillators.Add(new Defibrillator(DEFIB));
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        var closest = defibrillators[0];
        long maxDist = 999999999;

        foreach (var defibrillator in defibrillators)
        {
            var xDist = (defibrillator.longitude - longitude) * Convert.ToDecimal(Math.Cos(Convert.ToDouble((latitude + defibrillator.latitude) / 2)));
            var yDist = defibrillator.latitude - latitude;
            long dist = Convert.ToInt64(Math.Sqrt(Convert.ToDouble((xDist * xDist) + (yDist * yDist))) * 6371);
            if (dist < maxDist)
            {
                maxDist = dist;
                closest = defibrillator;
            }
        }

        Console.WriteLine(closest.locationName);
    }

    public class Defibrillator
    {
        public string locationName;
        public string address;
        public string phone;
        public decimal longitude;
        public decimal latitude;

        public Defibrillator(string description)
        {
            string[] segments = description.Split(';');
            this.locationName = segments[1];
            this.address = segments[2];
            this.phone = segments[3];
            string longitudeString = segments[4];
            longitudeString = longitudeString.Replace(",", ".");
            this.longitude = Convert.ToDecimal(longitudeString);
            string latitudeString = segments[5];
            latitudeString = latitudeString.Replace(",", ".");
            this.latitude = Convert.ToDecimal(latitudeString);
        }
    }
}