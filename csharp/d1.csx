using System.IO;
using System.Text.RegularExpressions;

string filedata = "";
List<int> firstcol = [];
List<int> secondcol = [];

using (var filehandle = new StreamReader("./inputs/day1.txt"))
{
    filedata = filehandle.ReadToEnd().Trim();
}

Run();

void Run() {
    firstcol = [];
    secondcol = [];
    foreach (var line in filedata.Split("\n")) {
        firstcol.Add(int.Parse(line.Split()[0]));
        secondcol.Add(int.Parse(line.Split()[^1]));
    }
    firstcol.Sort();
    secondcol.Sort();
    var mergedsum = firstcol.Zip(secondcol)
                    .Select(x => Math.Abs(x.First - x.Second))
                    .Sum();
    Console.WriteLine($"Part 1: {mergedsum}");
    var sumprod = firstcol.Select(x => x * secondcol.Where(y => x == y).Count()).Sum();
    Console.WriteLine($"Part 2: {sumprod}");
}
