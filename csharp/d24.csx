#load "utils.csx"
#nullable enable
using System.IO;

using OpFunc = System.Func<int, int, int>;
using OpList = System.Collections.Generic.List<Operation>;
using Registers = System.Collections.Generic.Dictionary<string, int>;
Registers registers = [];
OpList ops = [];

int XOR(int n1, int n2) => 1 & (n1 ^ n2);
int OR(int n1, int n2) => n1 | n2;
int AND(int n1, int n2) => n1 & n2;

using (var file = new StreamReader("./inputs/day24.txt")) {
    var text = file.ReadToEnd().Split("\n\n", ss);
    registers = text[0].Split("\n").Select(line => line.Split(": "))
        .Select(chrs => (chrs[0], int.Parse(chrs[1]))).ToDictionary();
    
    foreach (var line in text[1].Split("\n")) {
        var split = line.Split(" ", ss);
        var it1 = split[0];
        var it2 = split[2];
        var res = split[^1];
        OpFunc op = split[1] switch {
            "XOR" => XOR,
            "OR" => OR,
            _ => AND
        };
        ops.Add(new(it1, it2, op, res, split[1]));
    }
}

var outp = Solve(registers, ops);
Dictionary<string, string> swaps = new(){
    { "z00", "z04" }
};
/*
    z19, z45, z12, z37 are not output to via an xor
*/
var outp2 = SolveWithSwaps(registers, ops, swaps);
WriteLine(Eval(outp));
WriteLine($"{EvalChr(outp, 'x'):b48}");
WriteLine($"{EvalChr(outp, 'y'):b48}");
WriteLine($"{EvalChr(outp, 'x') + EvalChr(outp, 'y'):b48}");
WriteLine($"{EvalChr(outp, 'z'):b48}");
WriteLine($"{EvalChr(outp2, 'z'):b48}");
// GetBadOutputs(ops, []);
SolveP2(ops);

void GetBadOutputs(OpList opList, Dictionary<string, string> _swaps) {
    OpList remaining = new(opList);
    Dictionary<string, string> swaps = new(_swaps);

    foreach (var kv in _swaps) {
        swaps.TryAdd(kv.Value, kv.Key); // Ensure that the swaps happen both ways
    }

    for (int i = 0; i < remaining.Count; i++) {
        var res = remaining[i].Res;
        if (swaps.ContainsKey(res)) {
            remaining[i] = remaining[i] with { Res = swaps[res] };
        }
    }
    var fails = remaining.Where(op => op.Res.StartsWith('z') && op.op != XOR);

    WriteLine();
}

void SolveP2(OpList _ops) {
    OpList ops = new(_ops);
    HashSet<string> swaps = [];
    int idx = 0;
    string? carryReg = "";
    while (registers.ContainsKey($"x{idx:00}") && swaps.Count < 8) {
        string xreg = $"x{idx:00}";
        string yreg = $"y{idx:00}";
        string zreg = $"z{idx:00}";
        if (idx == 0) {
            carryReg = FindOp(xreg, yreg, "AND" , ops)?.Res;
        } else {
            var xorreg = FindOp(xreg, yreg, "XOR", ops)?.Res;
            var andreg = FindOp(xreg, yreg, "AND", ops)?.Res;
            var carryinreg = FindOp(xorreg, carryReg, "XOR", ops)?.Res;
            if (carryinreg is null) {
                swaps.Add(xorreg!);
                swaps.Add(andreg!);
                Swap(ops, xorreg!, andreg!);
                idx = 0;
                continue;
            }
            if (carryinreg != zreg) {
                swaps.Add(carryinreg!);
                swaps.Add(zreg);
                Swap(ops, carryinreg!, zreg);
                idx = 0;
                continue;
            }
            carryinreg = FindOp(xorreg, carryReg, "AND", ops)!.Res;
            carryReg = FindOp(andreg, carryinreg, "OR", ops)?.Res;
        }
        idx++;
    }
    WriteLine(string.Join(',', swaps.Order()));
}

void Swap(OpList ops, string reg1, string reg2) {
    var (i1, op1) = ops.Index().Where(op => op.Item.Res == reg1).First();
    var (i2, op2) = ops.Index().Where(op => op.Item.Res == reg2).First();
    ops[i1] = op1 with { Res = reg2 };
    ops[i2] = op2 with { Res = reg1 };
}

Operation? FindOp(string? reg1, string? reg2, string op, OpList ops) {
    return ops.Where(f => f.Find(reg1 ?? "", reg2 ?? "", op)).FirstOrDefault();
}

Registers SolveWithSwaps(Registers registers, OpList opList, Dictionary<string, string> _swaps) {
    Registers localrs = new(registers);
    OpList remaining = new(opList);
    Dictionary<string, string> swaps = new(_swaps);

    foreach (var kv in _swaps) {
        swaps.TryAdd(kv.Value, kv.Key); // Ensure that the swaps happen both ways
    }

    for (int i = 0; i < remaining.Count; i++) {
        var res = remaining[i].Res;
        if (swaps.ContainsKey(res)) {
            remaining[i] = remaining[i] with { Res = swaps[res] };
        }
    }

    int idx = 0;
    while (remaining.Count != 0) {
        var op = remaining[idx];
        if (!(localrs.ContainsKey(op.N1)
             && localrs.ContainsKey(op.N2))) {
            idx++;
            continue;
        }
        idx = 0;
        var n1 = localrs[op.N1];
        var n2 = localrs[op.N2];
        localrs[op.Res] = op.op(n1, n2);
        remaining.Remove(op);
    }

    return localrs;
}

Registers Solve(Registers registers, OpList opList) {
    Registers localrs = new(registers);
    OpList remaining = new(opList);

    int idx = 0;
    while (remaining.Count != 0) {
        var op = remaining[idx];
        if (!(localrs.ContainsKey(op.N1)
             && localrs.ContainsKey(op.N2))) {
            idx++;
            continue;
        }
        idx = 0;
        var n1 = localrs[op.N1];
        var n2 = localrs[op.N2];
        localrs[op.Res] = op.op(n1, n2);
        remaining.Remove(op);
    }

    return localrs;
}

long EvalChr(Registers registers, char chr) {
    var filtered = registers.Where(s => s.Key.StartsWith(chr))
                    .OrderBy(s => int.Parse(s.Key[1..]))
                    .Reverse().ToArray();

    long outp = 0L;
    foreach (var item in filtered) {
        outp <<= 1;
        Debug.Assert(outp > 0);
        outp += item.Value;
    }
    return outp;
}

long Eval(Registers registers) {
    var filtered = registers.Where(s => s.Key.StartsWith('z'))
                    .OrderBy(s => int.Parse(s.Key[1..]))
                    .Reverse().ToArray();

    long outp = 0L;
    foreach (var item in filtered) {
        outp <<= 1;
        Debug.Assert(outp > 0);
        outp += item.Value;
    }
    return outp;
}

record Operation(string N1, string N2, OpFunc op, string Res, string opname) {
    public bool Find(string num1, string num2, string op) {
        return ((N1 == num1 && N2 == num2) || (N1 == num2 && N2 == num1))
                && opname == op;
    }
};