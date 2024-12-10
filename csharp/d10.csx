using System.IO;

Dictionary<Point, int> topography = [];

using (var file = new StreamReader("./inputs/day10.txt")) {
    foreach (var (y, line) in file.ReadToEnd().Trim().Split("\n").Index()) {
        foreach (var (x, chr) in line.Index()) {
            topography[new Point(x, y)] = int.Parse(chr.ToString());
        }
    }
}

var t = DateTime.Now;
int count = 0;
foreach (var (coord, val) in topography) {
    if (val == 0) {
        count += getscore(coord).Count;
    }
}
Console.WriteLine(count);
int ratingsum = 0;

foreach (var (coord, val) in topography) {
    if (val == 0) {
        ratingsum += getrating(coord);
    }
}
Console.WriteLine(ratingsum);
Console.WriteLine(DateTime.Now.Subtract(t).TotalMilliseconds);

HashSet<Point> getscore(Point cpos) {
    if (topography[cpos] == 9) return [cpos];
    HashSet<Point> coords = [];

    int val = topography[cpos];
    if (topography.GetValueOrDefault(cpos with {y = cpos.y+1}) == val + 1) {
        coords.UnionWith(getscore(cpos with {y = cpos.y + 1}));
    }
    if (topography.GetValueOrDefault(cpos with {y = cpos.y - 1}) == val + 1) {
        coords.UnionWith(getscore(cpos with {y = cpos.y - 1}));
    }
    if (topography.GetValueOrDefault(cpos with {x = cpos.x + 1}) == val + 1) {
        coords.UnionWith(getscore(cpos with {x = cpos.x + 1}));
    }
    if (topography.GetValueOrDefault(cpos with {x = cpos.x - 1}) == val + 1) {
        coords.UnionWith(getscore(cpos with {x = cpos.x - 1}));
    }

    return coords;
}

int getrating(Point cpos) {
    if (topography[cpos] == 9) return 1;
    int res = 0;

    int val = topography[cpos];
    if (topography.GetValueOrDefault(cpos with {y = cpos.y+1}) == val + 1) {
        res += getrating(cpos with {y = cpos.y + 1});
    }
    if (topography.GetValueOrDefault(cpos with {y = cpos.y - 1}) == val + 1) {
        res += getrating(cpos with {y = cpos.y - 1});
    }
    if (topography.GetValueOrDefault(cpos with {x = cpos.x + 1}) == val + 1) {
        res += getrating(cpos with {x = cpos.x + 1});
    }
    if (topography.GetValueOrDefault(cpos with {x = cpos.x - 1}) == val + 1) {
        res += getrating(cpos with {x = cpos.x - 1});
    }

    return res;
}

record struct Point(int x, int y);