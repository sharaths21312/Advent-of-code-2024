#load "utils.csx"
using System.IO;

List<uint> buyers;

using (var file = new StreamReader("./inputs/day22.txt")) {
    buyers = file.ReadToEnd().Split("\n", ss).Select(s => uint.Parse(s)).ToList();
}
var t = DateTime.Now;
// Part 1
ulong totalsum = 0uL;
foreach (var buyer in buyers) {
    uint res = buyer;
    for (int i = 0; i < 2000; i++) {
        res = Process(res);
    }
    totalsum += res;
}
WriteLine(totalsum);

Dictionary<(int, int, int, int), long> TotalCounter = [];
HashSet<(int, int, int, int)> Individual = [];
// Part 2
foreach (var buyer in buyers) {
    int d0, d1, d2, d3;
    uint res = buyer;
    (res, d0) = ProcessLD(res);
    (res, d1) = ProcessLD(res);
    (res, d2) = ProcessLD(res);
    (res, d3) = ProcessLD(res);
    TotalCounter.AddNum((d0, d1, d2, d3), res % 10);
    Individual.Add((d0, d1, d2, d3));
    for (int i = 0; i < 1996; i++) {
        (d0, d1, d2) = (d1, d2, d3);
        (res, d3) = ProcessLD(res);
        if (!Individual.Contains((d0,d1,d2,d3))) {
            Individual.Add((d0, d1, d2, d3));
            TotalCounter.AddNum((d0, d1, d2, d3), res % 10);
        }
    }
    Individual.Clear();
}
var mx = TotalCounter.MaxBy(kv => kv.Value);
WriteLine(mx.Value);
WriteLine(GetMsTime(t));

uint Process(uint input) {
    uint res = input;
    res ^= res << 6;
    res = Prune(res);
    res ^= res >> 5;
    res = Prune(res);
    res ^= res << 11;
    res = Prune(res);
    return res;
}

(uint, int) ProcessLD(uint input) {
    uint res = Process(input);
    return (res, ((int) res % 10) - ((int) input % 10));
}

uint Prune (uint x) => (x << 8) >> 8;