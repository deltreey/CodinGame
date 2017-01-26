use std::io;

macro_rules! print_err {
    ($($arg:tt)*) => (
        {
            use std::io::Write;
            writeln!(&mut ::std::io::stderr(), $($arg)*).ok();
        }
    )
}

macro_rules! parse_input {
    ($x:expr, $t:ident) => ($x.trim().parse::<$t>().unwrap())
}

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
fn main() {
    let mut result: i32 = 0;

    let mut input_line = String::new();
    io::stdin().read_line(&mut input_line).unwrap();
    let n = parse_input!(input_line, i32);
    print_err!("{}", n);
    let mut inputs = String::new();

    let mut values = Vec::new();

    io::stdin().read_line(&mut inputs).unwrap();
    for i in inputs.split(" ") {
        let v = parse_input!(i, i32);
        values.push(v);
    }
    
    let mut largest: i32 = -999999999;
    let mut smallest: i32 = 999999999;
    for v in values {
        if v > largest {
            largest = v;
            smallest = largest;
        }
        if v < smallest {
            smallest = v;
            if smallest - largest < result {
                result = smallest - largest;
            }
        }
    }
    
    println!("{}", result);
}
