using System.IO;
using System.Text.RegularExpressions;

var filetext = "";

using (var file = new StreamReader("./inputs/day3.txt"))
{
    filetext = file.ReadToEnd();
}

Console.WriteLine(ScanMults(filetext));
Console.WriteLine(ScanAndCheckMults(filetext));

int ScanMults(string data) {
    Regex multcheck = new(@"mul\((\d+),(\d+)\)");
    var matches = multcheck.Matches(data);

    List<int> outp = new(matches.Count);

    outp.AddRange(matches.Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)));

    return outp.Sum();
}

int ScanAndCheckMults(string data) {
    Regex multcheck = new(@"mul\((\d+),(\d+)\)");
    Regex doscheck = new(@"");
    Regex check = new(@"(?<dos>do\(\))|(?<donts>don't\(\))|(?<mult>mul\((?<mul1>\d+),(?<mul2>\d+)\))");
    

    var ms = check.Matches(data);
    List<int> acc = new(ms.Count);
    bool dos = true;

    foreach (Match m in ms) {
        if (m.Groups["donts"].Success) {
            dos = false;
        } else if (m.Groups["dos"].Success) {
            dos = true;
        } else if (m.Groups["mult"].Success && dos) {
            acc.Add(int.Parse(m.Groups["mul1"].Value) * int.Parse(m.Groups["mul2"].Value));
        }
    }
    return acc.Sum();
}