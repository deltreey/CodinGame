/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/

var n = parseInt(readline()); // the number of temperatures to analyse
var temps = readline(); // the n temperatures expressed as integers ranging from -273 to 5526

temps = temps.split(' ');

temps.sort(function (a, b) {
    return (parseInt(a) - parseInt(b));
});
var highestLow = -10000;
var lowestHigh = 10000;

var i = 0;

while (temps[i] <= 0 && i < temps.length) {
    if (temps[i] === 0) {
        print('0');
        break;
    }
    ++i;
}

if (i > 0 && temps.length > 0 && temps[0]) {
    highestLow = temps[i - 1];
}
if (temps.length >= i && temps[0]) {
    lowestHigh = temps[i];
}

if (highestLow * -1 >= lowestHigh) {
    if (highestLow === -10000 && lowestHigh === 10000) {
        print('0');
    }
    else {
        print(lowestHigh);
    }
}
else {
    print(highestLow);
}
