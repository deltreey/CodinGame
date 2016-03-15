import java.util.*;
import java.io.*;
import java.math.*;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution {

    public Player playerOne;
    public Player playerTwo;
    public int round;

    public static void main(String args[]) {
        Solution solution = new Solution();
        solution.run(args);
    }

    public void run(String args[]) {
        Scanner in = new Scanner(System.in);

        int n = in.nextInt(); // the number of cards for player 1
        round = 0;
        ArrayList<String> cards = new ArrayList<String>();
        System.err.println("Player 1");
        for (int i = 0; i < n; i++) {
            String cardp1 = in.next(); // the n cards of player 1
            System.err.println(cardp1);
            cards.add(cardp1);
        }
        playerOne = new Player(cards);
        
        cards = new ArrayList<String>();
        System.err.println("Player 2");
        int m = in.nextInt(); // the number of cards for player 2
        for (int i = 0; i < m; i++) {
            String cardp2 = in.next(); // the m cards of player 2
            System.err.println(cardp2);
            cards.add(cardp2);
        }
        playerTwo = new Player(cards);

        int winner = war();
        String result;

        if (winner == 0) {
            result = "PAT";
        }
        else {
            result = Integer.toString(winner) + " " + round;
        }

        System.out.println(result);
    }

    public int war() {
        int result = 0;

        // loop until someone runs out of cards
        while (playerOne.cards.size() > 0 && playerTwo.cards.size() > 0) {
            round += 1;
            System.err.println("New Round");
            System.err.println("Player 1");
            for (int p = 0; p < playerOne.cards.size(); ++p) {
                System.err.println(playerOne.cards.get(p));
            }
            System.err.println("Player 2");
            for (int p = 0; p < playerTwo.cards.size(); ++p) {
                System.err.println(playerTwo.cards.get(p));
            }
            result = 0;
            Table table = new Table();
            String p1card = playerOne.cards.get(0);
            String p2card = playerTwo.cards.get(0);
            table.playerOneCards.add(p1card);
            table.playerTwoCards.add(p2card);
            playerOne.cards.remove(0);
            playerTwo.cards.remove(0);
            int roundResult = determineWinner(p1card, p2card);
            if (roundResult == 0) {
                while (roundResult == 0 && playerOne.cards.size() > 0 && playerTwo.cards.size() > 0) {
                    System.err.println("WAR!");
                    if (playerOne.cards.size() < 4 || playerTwo.cards.size() < 4) {
                        playerOne.cards = new ArrayList<String>();
                        playerTwo.cards = new ArrayList<String>();
                        System.err.println("A player ran out of cards while WARring");
                        result = 0;
                        break;
                    }

                    for (int i = 0; i < 3; ++i) {
                        table.playerOneCards.add(playerOne.cards.get(0));
                        playerOne.cards.remove(0);
                        table.playerTwoCards.add(playerTwo.cards.get(0));
                        playerTwo.cards.remove(0);
                    }
                    p1card = playerOne.cards.get(0);
                    p2card = playerTwo.cards.get(0);
                    table.playerOneCards.add(p1card);
                    table.playerTwoCards.add(p2card);
                    playerOne.cards.remove(0);
                    playerTwo.cards.remove(0);
                    roundResult = determineWinner(p1card, p2card);
                }
            }

            if (roundResult == 1) {
                System.err.println("Player 1 wins this round");
                for (int p = 0; p < table.playerOneCards.size(); ++p) {
                    playerOne.cards.add(table.playerOneCards.get(p));
                }
                for (int p = 0; p < table.playerTwoCards.size(); ++p) {
                    playerOne.cards.add(table.playerTwoCards.get(p));
                }
                result = 1;
            }
            else if (roundResult == 2) {
                System.err.println("Player 2 wins this round");
                for (int p = 0; p < table.playerOneCards.size(); ++p) {
                    playerTwo.cards.add(table.playerOneCards.get(p));
                }
                for (int p = 0; p < table.playerTwoCards.size(); ++p) {
                    playerTwo.cards.add(table.playerTwoCards.get(p));
                }
                result = 2;
            }
        }

        return result;
    }

    public int determineWinner(String playerOneCard, String playerTwoCard) {
        int result = 0;

        String cardNumbers[] = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        String p1cardValue = playerOneCard.substring(0, playerOneCard.length() - 1);
        String p2cardValue = playerTwoCard.substring(0, playerTwoCard.length() - 1);
        System.err.println("Player 1 Card:");
        System.err.println(playerOneCard);
        System.err.println(p1cardValue);
        System.err.println(Arrays.asList(cardNumbers).indexOf(p1cardValue));
        System.err.println("Player 2 Card:");
        System.err.println(playerTwoCard);
        System.err.println(p2cardValue);
        System.err.println(Arrays.asList(cardNumbers).indexOf(p2cardValue));

        if (Arrays.asList(cardNumbers).indexOf(p1cardValue) > Arrays.asList(cardNumbers).indexOf(p2cardValue)) {
            result = 1;
        }
        else if (Arrays.asList(cardNumbers).indexOf(p1cardValue) < Arrays.asList(cardNumbers).indexOf(p2cardValue)) {
            result = 2;
        }

        return result;
    }

    public class Player {
        public ArrayList<String> cards;

        public Player(ArrayList<String> cards) {
            this.cards = cards;
        }
    }

    public class Table {
        public ArrayList<String> playerOneCards;
        public ArrayList<String> playerTwoCards;

        public Table()
        {
            this.playerOneCards = new ArrayList<String>();
            this.playerTwoCards = new ArrayList<String>();
        }
    }
}