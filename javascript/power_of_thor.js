/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * ---
 * Hint: You can use the debug stream to print initialTX and initialTY, if Thor seems not follow your orders.
 **/

var inputs = readline().split(' ');
var lightX = parseInt(inputs[0]); // the X position of the light of power
var lightY = parseInt(inputs[1]); // the Y position of the light of power
var initialTX = parseInt(inputs[2]); // Thor's starting X position
var initialTY = parseInt(inputs[3]); // Thor's starting Y position
var xDirection = 'W';
var xDistance = initialTX - lightX;
if (initialTX < lightX) {
    xDirection = 'E';
    xDistance = lightX - initialTX;
}
var yDirection = 'N';
var yDistance = initialTY - lightY;
if (initialTY < lightY) {
    yDirection = 'S';
    yDistance = lightY - initialTY;
}

// game loop
while (true) {
    var remainingTurns = parseInt(readline()); // The remaining amount of turns Thor can move. Do not remove this line.

    // Write an action using print()
    // To debug: printErr('Debug messages...');
    var result = '';
    
    if (yDistance > 0) {
        result += yDirection;
        yDistance -= 1;
    }
    if (xDistance > 0) {
        result += xDirection;
        xDistance -= 1;
    }
    
    print(result); // A single line providing the move to be made: N NE E SE S SW W or NW
}