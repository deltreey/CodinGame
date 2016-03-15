package main

import "fmt"
import "os"

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/

func main() {
  // nbFloors: number of floors
  // width: width of the area
  // nbRounds: maximum number of rounds
  // exitFloor: floor on which the exit is found
  // exitPos: position of the exit on its floor
  // nbTotalClones: number of generated clones
  // nbAdditionalElevators: ignore (always zero)
  // nbElevators: number of elevators
  var nbFloors, width, nbRounds, exitFloor, exitPos, nbTotalClones, nbAdditionalElevators, nbElevators int
  fmt.Scan(&nbFloors, &width, &nbRounds, &exitFloor, &exitPos, &nbTotalClones, &nbAdditionalElevators, &nbElevators)
  fmt.Fprintln(os.Stderr, "Number of Floors: ", nbFloors);
  fmt.Fprintln(os.Stderr, "Number of Rounds: ", nbRounds);
  fmt.Fprintln(os.Stderr, "Number of Clones: ", nbTotalClones);
  fmt.Fprintln(os.Stderr, "Number of Elevators: ", nbElevators);
  fmt.Fprintln(os.Stderr, "Number of Additional Elevators: ", nbAdditionalElevators);
  fmt.Fprintln(os.Stderr, "Width: ", width);
  fmt.Fprintln(os.Stderr, "Exit Floor: ", exitFloor);
  fmt.Fprintln(os.Stderr, "Exit Position: ", exitPos);

  var potentialElevators [100]Elevator;
  var elevators []Elevator = potentialElevators[0:nbElevators];
  
  for i := 0; i < nbElevators; i++ {
    // elevatorFloor: floor on which this elevator is found
    // elevatorPos: position of the elevator on its floor
    var elevatorFloor, elevatorPos int
    fmt.Scan(&elevatorFloor, &elevatorPos)
    fmt.Fprintln(os.Stderr, "Elevator Floor: ", elevatorFloor);
    fmt.Fprintln(os.Stderr, "Elevator Position: ", elevatorPos);
    elevators[i].floor = elevatorFloor;
    elevators[i].position = elevatorPos;
  }
  for {
		var result = "WAIT";

    // cloneFloor: floor of the leading clone
    // clonePos: position of the leading clone on its floor
    // direction: direction of the leading clone: LEFT or RIGHT
    var cloneFloor, clonePos int
    var direction string
    fmt.Scan(&cloneFloor, &clonePos, &direction)
    fmt.Fprintln(os.Stderr, "Clone Floor: ", cloneFloor);
    fmt.Fprintln(os.Stderr, "Clone Position: ", clonePos);
    fmt.Fprintln(os.Stderr, "Clone Direction: ", direction);

    if cloneFloor == exitFloor {
    	if (direction == "RIGHT" && clonePos > exitPos) || (direction == "LEFT" && clonePos < exitPos) {
    		result = "BLOCK";
    	}
    } else {
    	for e := 0; e < nbElevators; e++ {
    		if elevators[e].floor == cloneFloor {
    			if (direction == "RIGHT" && clonePos > elevators[e].position) || (direction == "LEFT" && clonePos < elevators[e].position) {
	      		result = "BLOCK";
	      	}
    		}
    	}
    } 
    
    fmt.Println(result); // action: WAIT or BLOCK
  }
}

type Elevator struct {
	floor, position int
}
