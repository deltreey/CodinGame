/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
 
var characters = {};

var width = parseInt(readline());
var height = parseInt(readline());
var text = readline();

var alphabet = [];
for (var a = 0; a < 27; ++a) {
    alphabet[a] = [];
}

for (var i = 0; i < height; i++) {
    var row = readline();

    for (var c = 0; c < 27; ++c) {
        var letterRow = [];
        for (var l = 0 + (c * width); l < width + (c * width); ++l) {
            letterRow.push(row[l]);
        }
        alphabet[c].push(letterRow.join(''));
    }
}

printErr('width: ' + width);
printErr('height: ' + height);
printErr(alphabet[0]);
printErr('text: ' + text);
// [
//    [
//      ' # '
//    ]
// ]


for (var h = 0; h < height; ++h) {
    var outputRow = '';
    for (var n = 0; n < text.length; ++n) {
        switch (text[n].toLowerCase()) {
            case 'a':
                outputRow += alphabet[0][h];
                break;
            case 'b':
                outputRow += alphabet[1][h];
                break;
            case 'c':
                outputRow += alphabet[2][h];
                break;
            case 'd':
                outputRow += alphabet[3][h];
                break;
            case 'e':
                outputRow += alphabet[4][h];
                break;
            case 'f':
                outputRow += alphabet[5][h];
                break;
            case 'g':
                outputRow += alphabet[6][h];
                break;
            case 'h':
                outputRow += alphabet[7][h];
                break;
            case 'i':
                outputRow += alphabet[8][h];
                break;
            case 'j':
                outputRow += alphabet[9][h];
                break;
            case 'k':
                outputRow += alphabet[10][h];
                break;
            case 'l':
                outputRow += alphabet[11][h];
                break;
            case 'm':
                outputRow += alphabet[12][h];
                break;
            case 'n':
                outputRow += alphabet[13][h];
                break;
            case 'o':
                outputRow += alphabet[14][h];
                break;
            case 'p':
                outputRow += alphabet[15][h];
                break;
            case 'q':
                outputRow += alphabet[16][h];
                break;
            case 'r':
                outputRow += alphabet[17][h];
                break;
            case 's':
                outputRow += alphabet[18][h];
                break;
            case 't':
                outputRow += alphabet[19][h];
                break;
            case 'u':
                outputRow += alphabet[20][h];
                break;
            case 'v':
                outputRow += alphabet[21][h];
                break;
            case 'w':
                outputRow += alphabet[22][h];
                break;
            case 'x':
                outputRow += alphabet[23][h];
                break;
            case 'y':
                outputRow += alphabet[24][h];
                break;
            case 'z':
                outputRow += alphabet[25][h];
                break;
            default:
                outputRow += alphabet[26][h];
                break;
            
        }
    }
    print(outputRow);
}