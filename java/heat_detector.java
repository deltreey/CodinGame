import java.util.*;
import java.io.*;
import java.math.*;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player {

    public static void main(String args[]) {
        Player program = new Player();
        program.run(args);
    }
    
    public Batman batman;
    public Building building;
    public String previousDirection;
    public int leapDistanceX;
    public int leapDistanceY;
    
    public void run(String args[]) {
        
        Scanner in = new Scanner(System.in);
        int W = in.nextInt(); // width of the building.
        int H = in.nextInt(); // height of the building.
        
        int N = in.nextInt(); // maximum number of turns before game over.
        
        int X0 = in.nextInt();
        int Y0 = in.nextInt();
        
        this.batman = new Batman(X0, Y0);
        this.building = new Building(W, H);
        this.previousDirection = "";
        this.leapDistanceY = building.height;
        if (this.leapDistanceY == 0) {
            this.leapDistanceY = 1;
        }
        this.leapDistanceX = building.width;
        if (this.leapDistanceX == 0) {
            this.leapDistanceX = 1;
        }

        // game loop
        while (true) {
            String BOMBDIR = in.next(); // the direction of the bombs from batman's current location (U, UR, R, DR, D, DL, L or UL)
            
            int newX = this.batman.X;
            int newY = this.batman.Y;
            
            this.leapDistanceX = (int)Math.ceil((double)this.leapDistanceX / (double)2);
            this.leapDistanceY = (int)Math.ceil((double)this.leapDistanceY / (double)2);

            newX = towardBombX(BOMBDIR);
            newY = towardBombY(BOMBDIR);
            
            this.previousDirection = BOMBDIR;
            
            if (newX >= this.building.width) {
                newX = this.building.width - 1;
            }
            else if (newX < 0) {
                newX = 0;
            }
            
            if (newY >= this.building.height) {
                newY = this.building.height - 1;
            }
            else if (newY < 0) {
                newY = 0;
            }
            
            System.err.println("bomb: " + BOMBDIR);
            System.err.println("X: " + this.batman.X);
            System.err.println("Y: " + this.batman.Y);
            System.err.println("xDist: " + this.leapDistanceX);
            System.err.println("yDist: " + this.leapDistanceY);
            
            this.batman.X = newX;
            this.batman.Y = newY;
            
            System.out.println(newX + " " + newY);

            // Write an action using System.out.println()
            // To debug: System.err.println("Debug messages...");
        }
    }
    
    public int towardBombX(String direction) {
        int distance = this.leapDistanceX;
        if (distance == 0) {
            distance = 1;
        }
        int result = this.batman.X;
        
        if (direction.indexOf("R") > -1) {
            result = this.batman.X + distance;
        }
        else if (direction.indexOf("L") > -1) {
            result = this.batman.X - distance;
        }
        
        return result;
    }
    
    public int towardBombY(String direction) {
        int distance = this.leapDistanceY;
        if (distance == 0) {
            distance = 1;
        }
        int result = this.batman.Y;
        
        if (direction.indexOf("D") > -1) {
            result = this.batman.Y + distance;
        }
        else if (direction.indexOf("U") > -1) {
            result = this.batman.Y - distance;
        }
        
        return result;
    }
    
    public Boolean isOppositeY(String direction) {
        Boolean result = false;
        
        if (direction.indexOf("U") > -1) {
            result = this.previousDirection.indexOf("D") > -1;
        }
        else if (direction.indexOf("D") > -1) {
            result = this.previousDirection.indexOf("U") > -1;
        }
        
        return result;
    }
    
    public Boolean isOppositeX(String direction) {
        Boolean result = false;
        
        if (direction.indexOf("R") > -1) {
            result = this.previousDirection.indexOf("L") > -1;
        }
        else if (direction.indexOf("L") > -1) {
            result = this.previousDirection.indexOf("R") > -1;
        }
        
        return result;
    }
    
    public class Building {
        public int height;
        public int width;
        
        public Building(int width, int height) {
            this.height = height;
            this.width = width;
        }
    }
    
    public class Batman {
        public int X;
        public int Y;
        
        public Batman(int X, int Y) {
            this.X = X;
            this.Y = Y;
        }
    }
}