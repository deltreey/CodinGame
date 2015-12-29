/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/

var MESSAGE = readline();
printErr('message: ' + MESSAGE);

function unarySegment(character, count) {
    var result = '';

    if (character === '0') {
        result = '00 ';
    }
    else if (character === '1') {
        result = '0 ';
    }

    for (var c = 0; c < count; ++c) {
        result += '0';
    }
    
    return result;
}

var binary = {
    ' ': '0100000',
    '!': '0100001',
    '"': '0100010',
    '#': '0100011',
    '$': '0100100',
    '%': '0100101',
    '&': '0100110',
    '\'': '0100111',
    '(': '0101000',
    ')': '0101001',
    '*': '0101010',
    '+': '0101011',
    ',': '0101100',
    '-': '0101101',
    '.': '0101110',
    '/': '0101111',
    '0': '0110000',
    '1': '0110001',
    '2': '0110010',
    '3': '0110011',
    '4': '0110100',
    '5': '0110101',
    '6': '0110110',
    '7': '0110111',
    '8': '0111000',
    '9': '0111001',
    ':': '0111010',
    ';': '0111011',
    '<': '0111100',
    '=': '0111101',
    '>': '0111110',
    '?': '0111111',
    '@': '1000000',
    'A': '1000001',
    'B': '1000010',
    'C': '1000011',
    'D': '1000100',
    'E': '1000101',
    'F': '1000110',
    'G': '1000111',
    'H': '1001000',
    'I': '1001001',
    'J': '1001010',
    'K': '1001011',
    'L': '1001100',
    'M': '1001101',
    'N': '1001110',
    'O': '1001111',
    'P': '1010000',
    'Q': '1010001',
    'R': '1010010',
    'S': '1010011',
    'T': '1010100',
    'U': '1010101',
    'V': '1010110',
    'W': '1010111',
    'X': '1011000',
    'Y': '1011001',
    'Z': '1011010',
    '[': '1011011',
    '\\': '1011100',
    ']': '1011101',
    '^': '1011110',
    '_': '1011111',
    '`': '1100000',
    'a': '1100001',
    'b': '1100010',
    'c': '1100011',
    'd': '1100100',
    'e': '1100101',
    'f': '1100110',
    'g': '1100111',
    'h': '1101000',
    'i': '1101001',
    'j': '1101010',
    'k': '1101011',
    'l': '1101100',
    'm': '1101101',
    'n': '1101110',
    'o': '1101111',
    'p': '1110000',
    'q': '1110001',
    'r': '1110010',
    's': '1110011',
    't': '1110100',
    'u': '1110101',
    'v': '1110110',
    'w': '1110111',
    'x': '1111000',
    'y': '1111001',
    'z': '1111010',
    '{': '1111011',
    '|': '1111100',
    '}': '1111101',
    '~': '1111110'
}

var binaryOutput = '';

for (var m = 0; m < MESSAGE.length; ++m) {
    binaryOutput += binary[MESSAGE[m]];
}
printErr('binary: ' + binaryOutput);

var unaryOutput = '';

var character = '0';
var count = 0;
for (var b = 0; b < binaryOutput.length; ++b) {
    if (count === 0) {
        character = binaryOutput[b];
        count = 1;
    }
    else {
        if (binaryOutput[b] !== character) {
            unaryOutput += ' ' + unarySegment(character, count);
            character = binaryOutput[b];
            count = 1;
        }
        else {
            ++count;
        }
    }
}

if (count !== 0) {
    unaryOutput += ' ' + unarySegment(character, count);
}

// remove the space at the beginning
unaryOutput = unaryOutput.substr(1);

// Write an action using print()
// To debug: printErr('Debug messages...');

print(unaryOutput);