package main

import "fmt"
import "os"

const (
	SCANNING     = iota
	DEACTIVATING = iota
	FLEEING      = iota
)

const (
	LEFT  = "LEFT"
	RIGHT = "RIGHT"
	DOWN  = "DOWN"
	UP    = "UP"
)

const (
	HOLLOW = "."
	START = "T"
	CONTROL_ROOM = "C"
	WALL = "#"
	UNSCANNED = "?"
)

type Location struct {
	state string
	previous *Location
	x int
	y int
}

type Map struct {
	height int
	width int
	locations []Location
	kirk *Location
	start *Location
	console *Location
}

func main() {
	action := SCANNING
	// fuel := 1200
	var gameMap Map
	// R: number of rows.
	// C: number of columns.
	// A: number of rounds between the time the alarm countdown is activated and the time the alarm goes off.
	var R, C, A int
	fmt.Scan(&R, &C, &A)
	gameMap.height = R
	gameMap.width = C

	for {
		var move string

		// KR: row where Kirk is located.
		// KC: column where Kirk is located.
		var KR, KC int
		fmt.Scan(&KR, &KC)

		// recreate map
		gameMap.locations = gameMap.locations[:0]
		for i := 0; i < R; i++ {
			var ROW string
			fmt.Scan(&ROW)
			fmt.Fprintln(os.Stderr, ROW)
			columns := []rune(ROW)
			for j := range columns {
				loc := Location{ state: string(columns[j]), x: j, y: i }
				gameMap.locations = append(gameMap.locations, loc)
				if (string(columns[j]) == START) {
					gameMap.start = &loc
				} else if (string(columns[j]) == CONTROL_ROOM) {
					gameMap.console = &loc
				}
			}
		}
		// locate Kirk
		gameMap.kirk = &gameMap.locations[KR * gameMap.width + KC]

		// fmt.Fprintln(os.Stderr, gameMap)
		fmt.Fprintln(os.Stderr, KR)
		fmt.Fprintln(os.Stderr, KC)

		if (action == SCANNING) {
			if (gameMap.console != nil) {
				action = DEACTIVATING
			} else {
				path := breadthFirstSearch(*gameMap.kirk, UNSCANNED, gameMap)

				move = followPath(path, *gameMap.kirk)
			}
		}
		if (action == DEACTIVATING) {
			path := breadthFirstSearch(*gameMap.console, START, gameMap)

			if (len(path) > A || path == nil) {
				// too far or no path to the control panel.  Keep scanning before we deactivate the control panel
				path := breadthFirstSearch(*gameMap.kirk, UNSCANNED, gameMap)

				move = followPath(path, *gameMap.kirk)
			} else {
				// deactivate the control panel
				path := breadthFirstSearch(*gameMap.kirk, CONTROL_ROOM, gameMap)

				move = followPath(path, *gameMap.kirk)
			}
			if ((*gameMap.kirk).x == (*gameMap.console).x && (*gameMap.kirk).y == (*gameMap.console).y) {
				// if we step on the console, we need to run now!
				action = FLEEING
			}
		}
		if (action == FLEEING) {
			path := breadthFirstSearch(*gameMap.kirk, START, gameMap)
			
			move = followPath(path, *gameMap.kirk)
		}

		fmt.Fprintln(os.Stderr, action) // what is kirk doing?
		fmt.Println(move) // Kirk's next move (UP DOWN LEFT or RIGHT).
	}
}

func followPath(path []Location, kirk Location) string {
	// fmt.Fprintln(os.Stderr, path[0])
	// fmt.Fprintln(os.Stderr, path[len(path) - 1])
	// fmt.Fprintln(os.Stderr, kirk)
	// fmt.Fprintln(os.Stderr, len(path))
	var result string

	if (kirk.x < path[len(path) - 1].x) {
		result = RIGHT
	} else if (kirk.y < path[len(path) - 1].y) {
		result = DOWN
	} else if (kirk.x > path[len(path) - 1].x) {
		result = LEFT
	} else {
		result = UP
	}

	return result
}

func breadthFirstSearch(start Location, endText string, gameMap Map) []Location {
	var result []Location

	// clear any prior searches
	for i := range gameMap.locations {
		gameMap.locations[i].previous = nil
	}

	// create the search collection
	var frontier []*Location
	frontier = append(frontier, &start)
	start.previous = new(Location)
	// fm.Fprintln(os.Stderr, gameMap.locations)

	var current = 0
	var endLocation *Location
	for {
		// don't search past the end of the collection
		if (len(frontier) <= current) {
			break;
		}

		neighbors := getNeighbors(*frontier[current], gameMap.locations)
		// add neighbors to search list if not already added
		for n := range neighbors {
			if ((*neighbors[n]).previous == nil && (*neighbors[n]).state != WALL && (endText == UNSCANNED || (*neighbors[n]).state != UNSCANNED)) {
				frontier = append(frontier, neighbors[n])
				(*neighbors[n]).previous = frontier[current]
				// early exit
				if ((*neighbors[n]).state == endText) {
					endLocation = neighbors[n]
					break
				}
			}
		}

		if (endLocation != nil) {
			break
		}

		current++
	}

	if (endLocation == nil) {
		fmt.Fprintln(os.Stderr, "Couldn't find a path")
		return result
		// fmt.Fprintln(os.Stderr, gameMap.locations)
	}

	// get the path by following the previous nodes back to start
	result = append(result, *endLocation)
	currentLocation := endLocation
	for {
		currentLocation = (*currentLocation).previous
		// don't keep going past the start of the search
		if ((*currentLocation).x == start.x && (*currentLocation).y == start.y) {
			break;
		}
		result = append(result, *currentLocation)
	}

	return result
}

func getNeighbors(current Location, locations []Location) []*Location {
	var neighbors []*Location

	for i := range locations {
		// only 4 directions, no diagonals
		if (locations[i].x == current.x) {
			if (locations[i].y == current.y + 1 || locations[i].y == current.y - 1) {
				neighbors = append(neighbors, &locations[i])
			}
		}
		if (locations[i].y == current.y) {
			if (locations[i].x == current.x + 1 || locations[i].x == current.x - 1) {
				neighbors = append(neighbors, &locations[i])
			}
		}
	}

	return neighbors
}