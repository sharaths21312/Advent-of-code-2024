using System.IO;

string[] filetext = [];
List<(long, int[])> Operations = [];

using (var file = new StreamReader("./inputs/day7.txt")) {
    filetext = file.ReadToEnd().Trim().Split("\n");
}

foreach (var line in filetext) {
    long key = long.Parse(line.Split(":")[0]);
    int[] values = line.Split(":")[1].Split(" ").Where(x => x.Length != 0)
        .Select(x => int.Parse(x.Trim())).ToArray();

    Operations.Add((key, values));
}

long soln = 0;
foreach (var (num, line) in Operations) {
    soln += Evaluate(num, line.AsSpan(), 0) ? num : 0; 
}

Console.WriteLine(soln);

soln = 0;
foreach (var (num, line) in Operations) {
    soln += Evaluate2(num, line.AsSpan(), 0) ? num : 0; 
}

Console.WriteLine(soln);

bool Evaluate(long target, Span<int> nums, long acc) {
    if (nums.Length < 1) return target == acc;

    for (var op = 0; op < 2; op++) {
        long next = op switch {
            0 => acc + nums[0],
            1 => acc * nums[0],
            _ => acc
        };

        if (Evaluate(target, nums[1..], next)) return true;
    }

    return false;
}

bool Evaluate2(long target, Span<int> nums, long acc) {
    if (nums.Length < 1) return target == acc;

    for (var op = 0; op < 3; op++) {
        long next = op switch {
            0 => acc + nums[0],
            1 => acc * nums[0],
            _ => long.Parse(String.Concat(acc.ToString(), nums[0].ToString()))
        };

        if (Evaluate2(target, nums[1..], next)) return true;
    }

    return false;
}