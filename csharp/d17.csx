#load "utils.csx"
using System.IO;
using System.Numerics;

long regA = 0L;
long regB = 0L;
long regC = 0L;
int[] instructions;

long pow2 (int inp) {
    return 1L << inp;
}
long pow2 (long inp) {
    int conv = (int) inp;
    return 1L << conv;
}

using (var file = new StreamReader("./inputs/day17.txt")) {
    var filedata = file.ReadToEnd().Split("\n", ss);

    regA = long.Parse(filedata[0].Split(":", ss)[^1]);
    regB = long.Parse(filedata[1].Split(":", ss)[^1]);
    regC = long.Parse(filedata[1].Split(":", ss)[^1]);

    instructions = filedata[3].Split(":", ss)[^1].Split(",").Select(str => int.Parse(str)).ToArray();
}

var t = DateTime.Now;
var i = 0;
var outputs = RunProgram(regA, regB, regC, instructions);
WriteLine(String.Join(',', outputs.Select(n => n.ToString())));
// PrintProgram(instructions);

var soln2 = OptimiseRec(instructions, 0, 0).Min();
WriteLine(soln2);
WriteLine(String.Join(',', RunProgram(soln2, 0L, 0L, instructions).Select(n => n.ToString())));
WriteLine(DateTime.Now.Subtract(t).TotalMilliseconds);


int[] RunProgram(long inRegA, long inRegB, long inRegC, int[] instructions) {
    List<int> outputs = [];
    long regA = inRegA;
    long regB = inRegB;
    long regC = inRegC;
    int i = 0;
    while (i < instructions.Length - 1) {
        var instr = (INS) instructions[i];
        var literal = instructions[i+1];
        var combo = literal switch {
            4 => regA,
            5 => regB,
            6 => regC,
            var x => x
        };
        
        switch (instr) {
            case INS.ADV:
                regA = regA / pow2(combo);
                goto default;
            case INS.BXL:
                regB = regB ^ literal;
                goto default;
            case INS.BST:
                regB = PosMod(combo, 8);
                goto default;
            case INS.JNZ:
                if (regA == 0) {
                    goto default;
                }
                i = literal;
                break;
            case INS.BXC:
                regB = regB ^ regC;
                goto default;
            case INS.OUT:
                outputs.Add((int) PosMod(combo, 8L));
                goto default;
            case INS.BDV:
                regB = regA / pow2(combo);
                goto default;
            case INS.CDV:
                regC = regA / pow2(combo);
                goto default;
            default:
                i += 2;
                break;
        }
    }
    return outputs.ToArray();
}

/*
    My program:
    0 B = A mod 8
    2 B = B XOR 3
    4 C = A floordiv 2^B = unk
    6 B = B XOR 5
    8 A = A floordiv 2^3 = 8
    10 B = B XOR C
    12 Output B
    14 If A = 0 jump to 0

    this is basically 
    while (a != 0) {
        B = last three binary digits of A
        B = B ^ 0b011 (3)
        C = A >> B
        A = A >> 3
        B = B ^ 0b101
        B = B XOR C
        output B
    }
    A = 3x output in binary digits these are 16 inputs, so A ~ 70,368,744,177,664
    running this in reverse, if I want output x
    Bf = x
    => B XOR C = X
    => ((last 3 A) ^ 0b110) XOR (A >> (3 ^ last 3 A)) = x
    starting at the end with A = 0 should give a result?
*/
long lastN (long inp, int n) {
    return inp % pow2(n);
}




List<long> OptimiseRec(int[] instructions, long cval, int idx) {
    // 2,4,1,3,7,5,1,5,0,3,4,2,5,5,3,0
    if (idx > instructions.Length) return [];
    List<long> res = [];
    var inc = cval * 8;
    for (int l3 = 0; l3 < 8; l3++) {
        var tmpres = RunProgram(inc + l3, 0L, 0L, instructions);
        if (tmpres.SequenceEqual(instructions.TakeLast(idx + 1))) {
            if (idx + 1 == instructions.Length) res.Add(inc + l3);
            res.AddRange(OptimiseRec(instructions, inc + l3, idx + 1));
        }
    }
    return res;
}

void PrintProgram (int[] instructions) {
    int i = 0;
    while (i < instructions.Length - 1) {
        var instr = (INS) instructions[i];
        var literal = instructions[i+1];
        var combo = literal switch {
            4 => "A",
            5 => "B",
            6 => "C",
            var x => x.ToString()
        };
        int temp;
        switch (instr) {
            case INS.ADV:
                WriteLine($"{i} A = A floordiv 2^{combo} = {(int.TryParse(combo, out temp) ? pow2(temp) : "unk")}");
                goto default;
            case INS.BXL:
                WriteLine($"{i} B = B XOR {literal}");
                goto default;
            case INS.BST:
                WriteLine($"{i} B = {combo} mod 8");
                goto default;
            case INS.JNZ:
                WriteLine($"{i} If A = 0 jump to {literal}");
                goto default;
            case INS.BXC:
                WriteLine($"{i} B = B XOR C");
                goto default;
            case INS.OUT:
                WriteLine($"{i} Output {combo}");
                goto default;
            case INS.BDV:
                WriteLine($"{i} B = A floordiv 2^{combo} = {(int.TryParse(combo, out temp) ? pow2(temp) : "unk")}");
                goto default;
            case INS.CDV:
                WriteLine($"{i} C = A floordiv 2^{combo} = {(int.TryParse(combo, out temp) ? pow2(temp) : "unk")}");
                goto default;
            default:
                i += 2;
                break;
        }
    }
}

enum INS {
    ADV,
    BXL,
    BST,
    JNZ,
    BXC,
    OUT,
    BDV,
    CDV
}