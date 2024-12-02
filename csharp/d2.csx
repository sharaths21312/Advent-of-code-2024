using System.IO;
using System.IO.IsolatedStorage;
using System.Text.RegularExpressions;

List<List<int>> numbers = [];

using (var file = new StreamReader("./inputs/day2.txt")) {
    string line = "";
    while (!file.EndOfStream) {
        numbers.Add(Regex.Matches(file.ReadLine(), @"\d+")
            .Select(m => int.Parse(m.Value)).ToList()
        );
    }
}

Console.WriteLine(numbers.Count(line => IsSafe(line)));

Console.WriteLine(numbers.Count(line => IsSafe2(line)));

bool IsSafe(List<int> numbers) {
    var isPos = (numbers[0] - numbers[1]) > 0;
    for (int i = 1; i < numbers.Count; i++) {
        int diff = numbers[i-1] - numbers[i];
        if ((diff > 0 ^ isPos) || Math.Abs(diff) > 3 || Math.Abs(diff) < 1) {
            return false;
        }
    }
    return true;
}

bool IsSafe2(List<int> ns) {

    if (IsSafe(ns)) return true;


    if (Math.Sign(ns[0] - ns[1]) != Math.Sign(ns[1] - ns[2])) {
        if (Math.Sign(ns[0] - ns[1]) == Math.Sign(ns[1] - ns[3])) {
            return IsSafe(ns.Where((v, i) => i != 2).ToList());
        } else if (Math.Sign(ns[0] - ns[2]) == Math.Sign(ns[2]-ns[3])) {
            return IsSafe(ns.Where((v, i) => i != 1).ToList());
        } else {
            return IsSafe(ns.Where((v, i) => i != 0).ToList());
        }
    } else {
        for (int i = 0; i < ns.Count; i++) {
            if (IsSafe(ns.Where((v, idx) => i != idx).ToList())) return true;
        }
    }

    return false;
}