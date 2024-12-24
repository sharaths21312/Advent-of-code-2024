#load "utils.csx"
using System.IO;

using Keypad = System.Collections.Generic.Dictionary<Point, char>;
using CacheDict = System.Collections.Concurrent.ConcurrentDictionary<(char currentKey, char nextKey, int depth), long>;
// Using a ConcurrentDictionary here as it has a .GetOrAdd method;

string[] input;
using (var file = new StreamReader("./inputs/day21.txt")) {
    input = file.ReadToEnd().Split("\n", ss);
}
var t = DateTime.Now;
WriteLine(Solve(input, 2));
WriteLine(Solve(input, 25));
WriteLine(GetMsTime(t));

Keypad ParsePad(string inp) {
    var lines = inp.Split("\n");
    // Have the keypads as a grid
    return (from x in Enumerable.Range(0, lines[0].Length)
            from y in Enumerable.Range(0, lines.Length)
            select (new Point(x, -y), lines[y][x])).ToDictionary();
}

long Solve(string[] inp, int depth) {
    var keypad1 = ParsePad("789\n456\n123\n 0A");
    var keypad2 = ParsePad(" ^A\n<v>");
    var keypadarr = Enumerable.Repeat(keypad2, depth).Prepend(keypad1).ToArray();
    CacheDict cache = [];
    long outp = 0;

    foreach (var line in inp) {
        var num = long.Parse(line[..^1]); // Remove the final A
        outp += num * EncodeAllKeys(line, cache, keypadarr);
    }

    return outp;
}


long EncodeAllKeys(string inpkeys, CacheDict cache, Keypad[] keypads) {
    if (keypads.Length == 0) {
        return inpkeys.Length; // If there are no keypads then this is just entered by the human
    }
    var ckey = 'A';
    var len = 0L;

    foreach (var nextkey in inpkeys) {
        len += EncodeSingleKey(ckey, nextkey, keypads, cache);
        ckey = nextkey;
    }
    return len;
}

long EncodeSingleKey(char current, char next, Keypad[] keypads, CacheDict cache) {
    // keypads.length serves as an alias for the depth we're at
    return cache.GetOrAdd((current, next, keypads.Length), _ => {
        var cost = long.MaxValue;
        var kp = keypads[0];

        var npos = kp.Single(kv => kv.Value == next).Key;
        var cpos = kp.Single(kv => kv.Value == current).Key;

        var dx = npos.X - cpos.X;
        var dy = npos.Y - cpos.Y;

        var horizstr = new string(dx < 0 ? '<' : '>', Math.Abs(dx));
        var vertstr = new string(dy < 0 ? 'v' : '^', Math.Abs(dy));

        // If the corner here is not empty then the path is safe
        if (kp[(cpos.X, npos.Y)] != ' ') {
            cost = Math.Min(cost, EncodeAllKeys($"{vertstr}{horizstr}A", cache, keypads[1..]));
        }
        if (kp[(npos.X, cpos.Y)] != ' ') {
            cost = Math.Min(cost, EncodeAllKeys($"{horizstr}{vertstr}A", cache, keypads[1..]));
        }

        return cost;
    });
}