#load "utils.csx"
using System.IO;

Dictionary<Point, char> farm = [];

using (var file = new StreamReader("./inputs/day12.txt")) {
    foreach (var (y, line) in file.ReadToEnd().Split("\n", ss).Index()) {
        foreach (var (x, chr) in line.Index()) {
            farm[new Point(x, y)] = chr;
        }
    }
}

Dictionary<char, int> areas = [];
Dictionary<char, int> perimiters = [];
List<Region> regions = [];

var validPoints = farm.Keys.ToHashSet();
var t = DateTime.Now;

while (validPoints.Count > 0) {
    var p = validPoints.FirstOrDefault();
    regions.Add(new Region(farm[p], GenerateRegion(p)));
}
var price = regions.Sum(r => r.Area * r.Perimiter);
WriteLine(price);
WriteLine(GetMsTime(t));
t = DateTime.Now;
long discountedprice = 0;
foreach (var region in regions) {
    discountedprice += region.Area * CountCorners(region);
}
WriteLine(discountedprice);
WriteLine(GetMsTime(t));

// The number of edges in a region equals the number of corners
// 4 possible corners:
// upleft => left and up in region and upleft not in region, or left and up not in region
// (l & u & !ul) | (!l & !u)
int CountCorners(Region region) {
    int count = 0;

    foreach (Point p in region) {
        count += CheckCorner(region, p.Up, p.Left, p.UpLeft);
        count += CheckCorner(region, p.Up, p.Right, p.UpRight);
        count += CheckCorner(region, p.Down, p.Left, p.DownLeft);
        count += CheckCorner(region, p.Down, p.Right, p.DownRight);
    }

    return count;
}
int CheckCorner(Region region, Point p1, Point p2, Point p3) {
    bool c1 = region.Contains(p1);
    bool c2 = region.Contains(p2);
    bool c12 = region.Contains(p3);
    
    return ((c1 && c2 && !c12) || (!c1 && !c2) ) ? 1 : 0;
}

HashSet<Point> GenerateRegion(Point start) {
    HashSet<Point> output = [start];
    char val = farm[start];
    validPoints.Remove(start);

    var up = start.Up;
    var down = start.Down;
    var left = start.Left;
    var right = start.Right;

    if (Check(val, up)) output.UnionWith(GenerateRegion(up));
    if (Check(val, down)) output.UnionWith(GenerateRegion(down));
    if (Check(val, left)) output.UnionWith(GenerateRegion(left));
    if (Check(val, right)) output.UnionWith(GenerateRegion(right));

    return output;
}
bool Check(char chr, Point point) {
    return farm.GetValueOrDefault(point) == chr && validPoints.Contains(point);
}
record Region (char Chr, HashSet<Point> Points){
    public long Area {get => Points.Count;}
    public long Perimiter {get => GetPerimiter();}
    public int MinX { get => Points.Min(p => p.X); }
    public int MaxX { get => Points.Max(p => p.X); }
    public int MinY { get => Points.Min(p => p.Y); }
    public int MaxY { get => Points.Max(p => p.Y); }

    long GetPerimiter () {
        long pm = 0;
        foreach (var p in Points) {
            if (!Points.Contains(p.Up)) pm++;
            if (!Points.Contains(p.Left)) pm++;
            if (!Points.Contains(p.Right)) pm++;
            if (!Points.Contains(p.Down)) pm++;
        }
        return pm;
    }

    public bool Contains (Point p) => Points.Contains(p);

    public IEnumerator<Point> GetEnumerator() => Points.GetEnumerator();
}

record class Edge(List<Point> P, MatchDirection Dir);
enum MatchDirection {
    LEFT, RIGHT, UP, DOWN
}