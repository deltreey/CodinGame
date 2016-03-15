/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/

var moves = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
var PIT = '_';
var PATH = '#';

var idealMoveTotal = parseInt(readline());
var dangerousMoveTotal = parseInt(readline());
var numberOfMoves = parseInt(readline());

var correctMove = PATH;
var idealMove = PIT;
if (idealMoveTotal < dangerousMoveTotal) {
	correctMove = PIT;
	idealMove = PATH;
}

printErr('idealMoveTotal: ' + idealMoveTotal);
printErr('dangerousMoveTotal: ' + dangerousMoveTotal);
printErr('numberOfMoves: ' + numberOfMoves);

// game loop
while (true) {
	var result = '';

	var firstInput = readline();
	var secondInput = readline();
	var thirdInput = readline();
	var fourthInput = readline();
	var state = {
		A: firstInput,
		B: secondInput,
		C: thirdInput,
		D: fourthInput,
		E: correctMove
	};
	printErr('A: ' + firstInput);
	printErr('B: ' + secondInput);
	printErr('C: ' + thirdInput);
	printErr('D: ' + fourthInput);

	var moveCalculations = [];

	for (var i = 0; i < numberOfMoves; i++) {
		var inputs = readline().split(' ');
		var fifthInput = parseInt(inputs[0]);
		var sixthInput = parseInt(inputs[1]);
		printErr(moves[i] + ' X: ' + fifthInput);
		printErr(moves[i] + ' Y: ' + sixthInput);
		moveCalculations.push({ move: moves[i], fifth: fifthInput, sixth: sixthInput });
		var total = fifthInput + sixthInput;
		if (total === idealMoveTotal && state[moves[i]] === idealMove) {
			printErr('found ideal move: ' + moves[i]);
			result = moves[i];
		}
	}

	if (!result) {
		// print('A, B, C, D or E');
		moveCalculations = moveCalculations.sort(function (a, b) {
			return (a.fifth + a.sixth) - (b.fifth + b.sixth);
		});
		moveCalculations = moveCalculations.reverse();

		for (var f = 0; f < moveCalculations.length; ++f) {
			if (state[moveCalculations[f].move] === correctMove) {
				result = moveCalculations[f].move;
				var total = moveCalculations[f].fifth + moveCalculations[f].sixth;
				if (total !== dangerousMoveTotal && total !== idealMoveTotal) {
					break;
				}
			}
		}
	}


	
	print(result);
}


// M1L1:
// 	init1: 35
// 	init2: 28
// 	init3: 5
// 	score: 106
// M2L1:
// 	init1: 35
// 	init2: 28
// 	init3: 5
// 	score: 106
// M2L3:
// 	init1: 35
// 	init2: 28
// 	init3: 5
// 	score: 6(was 12)
// M3L1:
// 	init1: 24
// 	init2: 19
// 	init3: 5
// 	score: 126
// M4L2:
// 	init1: 13
// 	init2: 29
// 	init3: 5
// 	score: 104(was 106)
// M5L1:
// 	init1: 13
// 	init2: 29
// 	init3: 5
// 	score: 104(was 110)
// M5L3:
// 	init1: 13
// 	init2: 29
// 	init3: 5
// 	score: 104(was 110)
// M6L1:
// 	init1: 13
// 	init2: 29
// 	init3: 5
// 	score: 12
// M6L2:
// 	init1: 13
// 	init2: 29
// 	init3: 5
// 	score: 4(was 16)
// 	9e,c,19b
// M7L2:
// 	init1: 29
// 	init2: 29
// 	init3: 5
// 	score: 104(was 110)
