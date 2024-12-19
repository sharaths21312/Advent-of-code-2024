#load "utils.csx"
using System.Collections.Concurrent;
using System.IO;

string[] towels;
string[] inputs;
ConcurrentDictionary<string, long> cache = new() { [string.Empty] = 1L };
// Dictionary<string, long> cache = new() { [string.Empty] = 1L };

using (var file = new StreamReader("./inputs/day19.txt")) {
    var filetext = file.ReadToEnd().Split("\n\n", ss);
    towels = filetext[0].Split(",", ss);
    inputs = filetext[1].Split("\n", ss);
}

var t = DateTime.Now;
long result = inputs.AsParallel().Count(inp => NumPossible(inp) > 0);
// long result = inputs.Count(inp => NumPossible(inp) > 0);
WriteLine(result);
long result2 = inputs.AsParallel().Sum(NumPossible);
// long result2 = inputs.Sum(NumPossible);
WriteLine(result2);
WriteLine(GetMsTime(t));

long NumPossible(string remainder) {
    if (cache.ContainsKey(remainder)) return cache[remainder];

    cache[remainder] = towels
            .Where(remainder.StartsWith)
            .Sum(towel => NumPossible(remainder[towel.Length..]));

    return cache[remainder];
}