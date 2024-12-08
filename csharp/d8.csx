using System.IO;

Dictionary<(int, int), char> file = [];
Dictionary<char, List<(int, int)>> antenna = [];

using (var f = new StreamReader("./inputs/day8.txt")) {
    foreach (var (y, line) in f.ReadToEnd().Split("\n").Index()) {
        foreach (var (x, chr) in line.Index()) {
            file.Add((x, y) , chr);
            if (chr != '.') {
                if (antenna.ContainsKey(chr)) {
                    antenna[chr].Add((x, y));
                } else {
                    antenna[chr] = [(x, y)];
                }
            }
        }
    }
}

HashSet<(int, int)> antinodes = [];

foreach (var (type, ps) in antenna) {
    int xdiff, ydiff;
    (int, int) pos1, pos2;
    for (int i = 0; i < ps.Count; i++) {
        for (int k = i + 1; k < ps.Count; k++) {
            xdiff = ps[k].Item1 - ps[i].Item1;
            ydiff = ps[k].Item2 - ps[i].Item2;
            pos1 = (ps[i].Item1 + 2*xdiff, ps[i].Item2 + 2*ydiff);
            pos2 = (ps[i].Item1 - xdiff, ps[i].Item2 - ydiff);
            if (file.ContainsKey(pos1)) {
                antinodes.Add(pos1);
            }
            if (file.ContainsKey(pos2)) {
                antinodes.Add(pos2);
            }
        }
    }
}
Console.WriteLine(antinodes.Count);

foreach (var (type, ps) in antenna) {
    int xdiff, ydiff, xstep, ystep, gcd, backwards, forwards;
    (int, int) postest;
    if (ps.Count == 0) continue;
    for (int i = 0; i < ps.Count; i++) {
        for (int k = i + 1; k < ps.Count; k++) {
            xdiff = ps[k].Item1 - ps[i].Item1;
            ydiff = ps[k].Item2 - ps[i].Item2;
            gcd = Math.Abs(GCD(xdiff, ydiff));
            xstep = xdiff/gcd;
            ystep = ydiff/gcd;
            for (int m = -50; m <= 50; m++) {
                postest = (ps[i].Item1 + xstep * m, ps[i].Item2 + ystep * m);
                if (!file.ContainsKey(postest)) continue;
                antinodes.Add(postest);
            }
        }
    }
}
Console.WriteLine(antinodes.Count); // 991
int GCD(int a, int b) {
    int temp = 0;
    while (b != 0) {
        temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}