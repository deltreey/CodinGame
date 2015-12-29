/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/


// game loop
while (true) {
    var inputs = readline().split(' ');
    var spaceX = parseInt(inputs[0]);
    var spaceY = parseInt(inputs[1])
    var mountains = [];
    for (var i = 0; i < 8; i++) {
        mountains.push(parseInt(readline())); // represents the height of one mountain, from 9 to 0. Mountain heights are provided from left to right.
    }
    var mountainsCopy = mountains.slice();

    // Write an action using print()
    // To debug: printErr('Debug messages...');
    var highestPeak = mountains.sort().reverse()[0];
    var highestMountain = mountainsCopy.indexOf(highestPeak);
    
    if (spaceX === highestMountain) {
        print('FIRE');
    }
    else {
        print('HOLD');
    }
}