#load "utils.csx"
using System.IO;

List<Point> bytedata = [];
const int width = 71;
const int height = 71;

using (var file = new StreamReader("./inputs/day18.txt")) {
    var lines = file.ReadToEnd().Split("\n", ss);
    bytedata = lines.Select(line => line.Split(",")
                            .Select(part => int.Parse(part)).ToArray())
                    .Select(arr => new Point(arr[0], arr[1])).ToList();
}
bool[,] field = new bool[71, 71];

var t = DateTime.Now;

for (int i = 0; i < 1024; i++) {
    var pt = bytedata[i];
    field[pt.X, pt.Y] = true;
}
bool IsFree(bool[,] field, Point pt) {
    if (pt.X < 0 || pt.Y < 0 || pt.X > 70 || pt.Y > 70) return false;
    return !field[pt.X, pt.Y];
}

PriorityQueue<(Point, int), int> Path = new();
Path.Enqueue((new(0, 0), 0), 0);
HashSet<Point> visited = [];

WriteLine(DateTime.Now.Subtract(t).TotalMilliseconds);
t = DateTime.Now;

// Solving part 1
WriteLine(PathFind(field));
WriteLine(DateTime.Now.Subtract(t).TotalMilliseconds);
t = DateTime.Now;

// Part 2, resetting field
// field = new bool[71, 71];
Point failure;

foreach (var coord in bytedata.Skip(1024)) {
    field[coord.X, coord.Y] = true;
    if (PathFind(field) == -1) {
        failure = coord;
        break;
    }
}
WriteLine(failure);
WriteLine(DateTime.Now.Subtract(t).TotalMilliseconds);

int PathFind(bool[,] field) {
    PriorityQueue<(Point, int), int> Path = new();
    Path.Enqueue((new(0, 0), 0), 0);
    HashSet<Point> visited = [];
    int success = 0;
    // Basic pathfinding algorithm, returns -1 if no paths are found.
    while (true) {
        if (Path.Count == 0) return -1;
        var (pt, steps) = Path.Dequeue();
        if (visited.Contains(pt)) continue;
        visited.Add(pt);

        if (pt == (70, 70)) {
            success = steps;
            break;
        }

        if (IsFree(field, pt.Right)) Path.Enqueue((pt.Right, steps + 1), steps + 1);
        if (IsFree(field, pt.Left )) Path.Enqueue((pt.Left , steps + 1), steps + 1);
        if (IsFree(field, pt.Up   )) Path.Enqueue((pt.Up   , steps + 1), steps + 1);
        if (IsFree(field, pt.Down )) Path.Enqueue((pt.Down , steps + 1), steps + 1);
    }
    return success;
}
