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
        int N = int.Parse(Console.ReadLine());
        var differences = new List<int>();
        var strengths = new List<int>();
        for (int i = 0; i < N; i++)
        {
            int pi = int.Parse(Console.ReadLine());
            strengths.Add(pi);
        }
        
        strengths.Sort();
        
        var previous = strengths[0];
        for (var s = 1; s < strengths.Count(); ++s)
        {
            differences.Add(strengths[s] - previous);
            previous = strengths[s];
        }
        
        differences.Sort();

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine(differences[0].ToString());
    }
}