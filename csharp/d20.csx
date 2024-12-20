#load "utils.csx"
using System.Collections.Concurrent;
using System.IO;

var Grid = new Locs[141][];
Point start, end;

using (var file = new StreamReader("./inputs/day20.txt")) {
    foreach (var (i, l) in file.ReadToEnd().Split("\n", ss).Index()) {
        Grid[i] = new Locs[141];
        foreach (var (j, chr) in l.Index()) {
            Grid[i][j] = chr switch {
                '#' => Locs.WALL,
                _ => Locs.EMPTY
            };
            if (chr == 'S') start = new(j, i);
            if (chr == 'E') end = new(j, i);
        }
    }
}

var t = DateTime.Now;
var (normalpath, pathsvisited) = GenPath();
var pathArr = pathsvisited.OrderBy(kv => kv.Value).ToArray();
WriteLine(normalpath);
WriteLine(Solve(2));
WriteLine(Solve(20));
WriteLine(GetMsTime(t));

long Solve(int n) {
    return pathArr.Index().AsParallel().Sum(ikv => {
        var (idx, (p1, d1)) = ikv;
        var remaining = pathArr.AsSpan(idx + 1);
        long res = 0;
        foreach (var (p2, d2) in remaining) {
            var dist = p1.Distance(p2);
            if (dist <= n) {
                if (d2 - d1 >= 100 + dist) {
                    res++;
                }
            }
        }
        return res;
    });
}


(int, Dictionary<Point, int>) GenPath() {
    PriorityQueue<(Point, Dictionary<Point, int>, int), int> Path = new();
    Dictionary<Point, int> visited = [];
    HashSet<Point> prevVisited = [];
    visited.Add(start, 0);

    Path.Enqueue((start, visited, 0), 0);
    
    while (Path.Count > 0) {
        var (p, dict, dist) = Path.Dequeue();
        if (prevVisited.Contains(p)) continue;
        prevVisited.Add(p);
        if (dict.ContainsKey(p)) {
            dict[p] = Math.Min(dict[p], dist);
        } else {
            dict.Add(p, dist);
        }
        if (p == end) {
            return (dist, dict);
        }
        
        if (GetPos(p.Left) == Locs.EMPTY && !dict.ContainsKey(p.Left)) {
            var d = dict.ToDictionary();
            Path.Enqueue((p.Left, d, dist+1), dist+1);
        }
        if (GetPos(p.Right) == Locs.EMPTY && !dict.ContainsKey(p.Right)) {
            var d = dict.ToDictionary();
            Path.Enqueue((p.Right, d, dist+1), dist+1);
        }
        if (GetPos(p.Down) == Locs.EMPTY && !dict.ContainsKey(p.Down)) {
            var d = dict.ToDictionary();
            Path.Enqueue((p.Down, d, dist+1), dist+1);
        }
        if (GetPos(p.Up) == Locs.EMPTY && !dict.ContainsKey(p.Up)) {
            var d = dict.ToDictionary();
            Path.Enqueue((p.Up, d, dist+1), dist+1);
        }
    }
    return (-1, []);
}

Locs GetPos(Point p) => Grid[p.Y][p.X];


enum Locs {
    WALL,
    EMPTY
}