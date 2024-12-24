#load "utils.csx"
using System.IO;

using OpFunc = System.Func<int, int, int>;
using OpList = System.Collections.Generic.List<Operation>;
using Registers = System.Collections.Generic.Dictionary<string, int>;
Registers registers = [];
OpList ops = [];

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
            "XOR" => (int n1, int n2) => 1 & (n1 ^ n2),
            "OR" => (int n1, int n2) => n1 | n2,
            _ => (int n1, int n2) => n1 & n2
        };
        ops.Add(new(it1, it2, op, res));
    }
}

var outp = Solve(registers, ops);
WriteLine(Eval(outp));

Registers Solve(Registers registers, OpList opArr) {
    Registers localrs = new(registers);
    OpList remaining = new(opArr);

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

record Operation(string N1, string N2, OpFunc op, string Res);