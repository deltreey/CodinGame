import java.util.*;
import java.io.*;
import java.math.*;

/**
 * Don't let the machines win. You are humanity's last hope...
 **/
class Player {
    public ArrayList<Node> nodes;

    public static void main(String args[]) {
        Player player = new Player();
        player.run();
    }
    
    public void run() {
        Scanner in = new Scanner(System.in);
        int width = in.nextInt(); // the number of cells on the X axis
        int height = in.nextInt(); // the number of cells on the Y axis
        in.nextLine();
        
        nodes = new ArrayList<Node>();
        
        for (int i = 0; i < height; i++) {
            String line = in.nextLine(); // width characters, each either 0 or .
            for (int l = 0; l < line.length(); ++l) {
                if (line.charAt(l) == '0') {
                    nodes.add(new Node(l, i));
                }
            }
        }
        
        for (int n = 0; n < nodes.size(); ++n) {
            Node current = nodes.get(n);
            System.err.println("x: " + current.X + ", y: " + current.Y);
            System.out.println(current.X + " " + current.Y + " " + getRightNode(current) + " " + getBottomNode(current));
        }
    }
    
    public String getRightNode(Node node) {
        ArrayList<Node> nodesOnRight = new ArrayList<Node>();
        
        for (int n = 0; n < this.nodes.size(); ++n) {
            Node current = nodes.get(n);
            if (current.X > node.X && current.Y == node.Y) {
                nodesOnRight.add(current);
            }
        }
        
        if (nodesOnRight.size() == 0) {
            return "-1 -1";
        }
        
        Collections.sort(nodesOnRight, new XComparator());
        
        return nodesOnRight.get(0).X + " " + nodesOnRight.get(0).Y;
    }
    
    public class XComparator implements Comparator<Node> {
        @Override
        public int compare(Node node1, Node node2) {
            return node1.X - node2.X;
        }
    }
    
    public String getBottomNode(Node node) {
        ArrayList<Node> nodesUnder = new ArrayList<Node>();
        
        for (int n = 0; n < this.nodes.size(); ++n) {
            Node current = nodes.get(n);
            if (current.X == node.X && current.Y > node.Y) {
                nodesUnder.add(current);
            }
        }
        
        if (nodesUnder.size() == 0) {
            return "-1 -1";
        }
        
        Collections.sort(nodesUnder, new YComparator());
        
        return nodesUnder.get(0).X + " " + nodesUnder.get(0).Y;
    }
    
    public class YComparator implements Comparator<Node> {
        @Override
        public int compare(Node node1, Node node2) {
            return node1.Y - node2.Y;
        }
    }
    
    public class Node {
        public int X;
        public int Y;
        
        public Node(int x, int y) {
            this.X = x;
            this.Y = y;
        }
    }
}