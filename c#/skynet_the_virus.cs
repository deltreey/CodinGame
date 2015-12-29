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
class Player
{
    public static List<Node> nodes;

    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');

        nodes = new List<Node>();

        int N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
        for (int n = 0; n < N; ++n)
        {
            nodes.Add(new Node(n));
        }
        int L = int.Parse(inputs[1]); // the number of links
        int E = int.Parse(inputs[2]); // the number of exit gateways

        for (int i = 0; i < L; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int N1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
            int N2 = int.Parse(inputs[1]);
            nodes[N1].links.Add(N2);
            nodes[N2].links.Add(N1);
            // Console.Error.WriteLine("link: " + N1 + " " + N2);
        }

        for (int i = 0; i < E; i++)
        {
            int EI = int.Parse(Console.ReadLine()); // the index of a gateway node
            nodes[EI].isGateway = true;
        }

        var gatewayNodes = nodes.Where(n => n.isGateway).ToList();

        // game loop
        while (true)
        {
            int SI = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn

            var paths = new List<List<int>>();

            foreach (var gateway in gatewayNodes)
            {
                Console.Error.WriteLine("skynet: " + SI.ToString());
                Console.Error.WriteLine("gateway: " + gateway.index.ToString());
                paths.Add(Pathfind(SI, gateway.index));
            }

            // remove empty paths where there was no way to the target
            paths = paths.Where(p => p.Count > 0).ToList();

            paths.Sort((p1, p2) => p1.Count.CompareTo(p2.Count));
            var pathToSever = paths.First();

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            var nearestGateway = pathToSever.Last();
            var linkToGateway = pathToSever[pathToSever.Count - 2];
            Console.Error.WriteLine("link to sever: " + nearestGateway.ToString() + " " + linkToGateway.ToString());

            // remove the move that gets to the node
            Console.WriteLine(nearestGateway.ToString() + " " + linkToGateway.ToString());
        }
    }

    // breadth first search
    public static List<int> Pathfind(int nodeOne, int nodeTwo)
    {
        Console.Error.WriteLine("pathfinding: " + nodeOne + " to " + nodeTwo);
        var result = new List<int>();

        int distance = 0;
        bool found = false;
        Dictionary<Node, int> traversed = new Dictionary<Node, int>();
        List<int> toTraverse = new List<int>();
        List<int> traversing = new List<int>();
        toTraverse = nodes[nodeOne].links;
        while (toTraverse.Count > 0 && !found)
        {
            distance++;
            traversing = toTraverse;
            toTraverse = new List<int>();
            foreach (var node in traversing)
            {
                if (!traversed.ContainsKey(nodes[node]))
                {
                    traversed.Add(nodes[node], distance);
                    if (node == nodeTwo)
                    {
                        found = true;
                        break;
                    }
                    else
                    {
                        toTraverse = toTraverse.Concat(nodes[node].links).ToList();
                    }
                }
            }
            Console.Error.WriteLine("found: " + found);
        }

        if (!found)
        {
            // no path found
            return result;
        }

        Console.Error.WriteLine("traversed: " + traversed.Count);
        foreach (var pair in traversed)
        {
            Console.Error.WriteLine("  node: " + pair.Key.index + " -> " + pair.Value);
        }

        result.Add(nodeTwo);
        for (var d = distance - 1; d > 0; --d)
        {
            // gather nodes 1 nearer to the target
            var nodesAtRange = traversed.Where(pair => pair.Value == d).ToDictionary(t => t.Key, t => t.Value);
            Console.Error.WriteLine("nodesAtRange: " + d + " => " + nodesAtRange.Count);
            foreach (var nearbyNode in nodesAtRange)
            {
                Console.Error.WriteLine("  node: " + nearbyNode.Key.index);
                // select the first node we find that can be accessed from the previous node in our list
                if (nearbyNode.Key.links.Contains(result.Last()))
                {
                    result.Add(nearbyNode.Key.index);
                    break;
                }
            }
            // if we get here there was no matching node
            Console.Error.WriteLine("!!!!NO MATCHING NODE!!!!");
            Console.Error.WriteLine("node: " + result.Last());
        }
        result.Add(nodeOne);
        result.Reverse();

        return result;
    }

    public class Node
    {
        public int index;
        public List<int> links;
        public bool isGateway;

        public Node(int index)
        {
            this.index = index;
            this.links = new List<int>();
            this.isGateway = false;
        }
    }
}