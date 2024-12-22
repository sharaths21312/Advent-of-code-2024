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
        return pathArr.Skip(idx).Count(kv2 => {
            var (p2, d2) = kv2;
            var dist = p1.Distance(p2);
            return dist <= n && d2 - d1 >= 100 + dist;
        });
    });
}


(int, Dictionary<Point, int>) GenPath() {
    PriorityQueue<(Point, int), int> Path = new();
    Dictionary<Point, int> visited = [];
    HashSet<Point> prevVisited = [];
    visited.Add(start, 0);

    Path.Enqueue((start, 0), 0);
    
    while (Path.Count > 0) {
        var (p, dist) = Path.Dequeue();
        if (prevVisited.Contains(p)) continue;
        prevVisited.Add(p);
        if (visited.ContainsKey(p)) {
            visited[p] = Math.Min(visited[p], dist);
        } else {
            visited.Add(p, dist);
        }
        if (p == end) {
            return (dist, visited);
        }
        
        if (GetPos(p.Left) == Locs.EMPTY && !visited.ContainsKey(p.Left)) {
            Path.Enqueue((p.Left, dist+1), dist+1);
        }
        if (GetPos(p.Right) == Locs.EMPTY && !visited.ContainsKey(p.Right)) {
            Path.Enqueue((p.Right, dist+1), dist+1);
        }
        if (GetPos(p.Down) == Locs.EMPTY && !visited.ContainsKey(p.Down)) {
            Path.Enqueue((p.Down, dist+1), dist+1);
        }
        if (GetPos(p.Up) == Locs.EMPTY && !visited.ContainsKey(p.Up)) {
            Path.Enqueue((p.Up, dist+1), dist+1);
        }
    }
    return (-1, []);
}

Locs GetPos(Point p) => Grid[p.Y][p.X];


enum Locs {
    WALL,
    EMPTY
}