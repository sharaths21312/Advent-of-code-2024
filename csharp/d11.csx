#load "utils.csx"
using System.IO;

List<long> stonesin = [];

using (var file = new StreamReader("./inputs/day11.txt")) {
    stonesin = file.ReadToEnd().Trim()
        .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(s => long.Parse(s)).ToList();
}
Dictionary<long, long> counter = stonesin.GroupBy(n => n)
    .ToDictionary(x => x.Key, x => (long) x.Count());

var t = DateTime.Now;
for (int i = 0; i < 75; i++) {
    counter = IterateLoop(counter);
    if (i == 24) WriteLine(counter.Sum(it => it.Value));
}
WriteLine(counter.Sum(it => it.Value));
WriteLine(counter.Count());
WriteLine(DateTime.Now.Subtract(t).TotalMilliseconds);

Dictionary<long,long> IterateLoop(Dictionary<long, long> counts) {
    Dictionary<long, long> output = [];
    foreach (var (key, val) in counts) {
        if (key == 0) {
            output.AddNum(1, val);
        } else if (key.ToString().Length % 2 == 0) {
            var s = key.ToString();
            output.AddNum(long.Parse(s[..(s.Length/2)]), val);
            output.AddNum(long.Parse(s[(s.Length/2)..]), val);
        } else {
            output.AddNum(2024 * key, val);
        }
    }
    return output;
}