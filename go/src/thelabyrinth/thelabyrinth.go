package main

import "fmt"
//import "os"

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/

func main() {
    // R: number of rows.
    // C: number of columns.
    // A: number of rounds between the time the alarm countdown is activated and the time the alarm goes off.
    var R, C, A int
    fmt.Scan(&R, &C, &A)
    
    for {
        // KR: row where Kirk is located.
        // KC: column where Kirk is located.
        var KR, KC int
        fmt.Scan(&KR, &KC)
        
        for i := 0; i < R; i++ {
            // ROW: C of the characters in '#.TC?' (i.e. one line of the ASCII maze).
            var ROW string
            fmt.Scan(&ROW)
        }
        
        // fmt.Fprintln(os.Stderr, "Debug messages...")
        fmt.Println("RIGHT") // Kirk's next move (UP DOWN LEFT or RIGHT).
    }
}