#load "utils.csx"
using System.IO;

PriorityQueue<(Point, Point, long), long> queue = new();
Locations[][] grid = new Locations[141][];
List<long> Solutions = new(5);
HashSet<(Point, Point)> visited = [];
// Index by [y][x]
Locations GridPos(Point p) => grid[p.Y][p.X];
Point startpos;
Point end;

Locations SetStart(int x, int y) {
    startpos = new(x, y);
    queue.Enqueue((startpos, Dirs.RIGHT, 0), 0);
    return Locations.EMPTY;
}
// Locations SetEnd(int x, int y) {
//     end = new(x, y)
// }
using (var file = new StreamReader("./inputs/day16.txt")) {
    foreach (var (y, line) in file.ReadToEnd().Trim().Split("\n", ss).Index()) {
        grid[y] = line.Select((chr, x) => chr switch {
            'E' => Locations.END,
            'S' => SetStart(x, y),
            '#' => Locations.WALL,
            _ => Locations.EMPTY
        }).ToArray();
    }
}

var t = DateTime.Now;
// Clear();
// CursorVisible = false;
// foreach (var (y, line) in grid.Index()) {
//     foreach (var (x, chr) in line.Index()) {
//         Write(chr switch { Locations.WALL => '#', Locations.END => 'E', Locations.START => 'S', _ => '.' });
//     }
//     WriteLine();
// }
Point prevpos = startpos;
long count = 0;
while (true) {
    // Main loop
    if (queue.Count == 0) break;
    var (pos, dir, score) = queue.Dequeue();
    if (visited.Contains((pos, dir))) continue;
    // SetCursorPosition(prevpos.X, prevpos.Y);
    // Write("\x1b[37m.");
    // SetCursorPosition(0, 142);
    // Write($"{queue.Count}     ");
    // SetCursorPosition(pos.X, pos.Y);
    // Write("\x1b[35m\u2588");
    prevpos = pos;
    
    if (GridPos(pos) == Locations.END) {
        Solutions.Add(score);
        // Counting 5 solutions
        // if (Solutions.Count > 4) break;
        break;
    }

    // If dead-end then just break.
    if (GridPos(pos + dir) == Locations.WALL
        && GridPos(pos + dir.TurnCW) == Locations.WALL
        && GridPos(pos + dir.TurnACW) == Locations.WALL)
        {continue;}
    
    if (GridPos(pos + dir) != Locations.WALL) {
        queue.Enqueue((pos + dir, dir, score + 1), score + 1);
    }
    if (GridPos(pos + dir.TurnCW) != Locations.WALL) {
        queue.Enqueue((pos, dir.TurnCW, score + 1000), score + 1000);
    }
    if (GridPos(pos + dir.TurnACW) != Locations.WALL) {
        queue.Enqueue((pos, dir.TurnACW, score + 1000), score + 1000);
    }
    count++;
    visited.Add((pos, dir));
}
foreach (var item in Solutions) WriteLine(item);
WriteLine(DateTime.Now.Subtract(t).TotalMilliseconds);
t = DateTime.Now;

// Part 2
PriorityQueue<(Point, Point, HashSet<Point>, long), long> p2queue = new();

p2queue.Enqueue((startpos, Dirs.RIGHT, [startpos], 0), 0); 
Dictionary<(Point, Point), int> visitedp2 = [];
List<(Point, Point, HashSet<Point>, long)> solutionp2 = [];
count = 0;
while (true) {
    // Main loop
    if (p2queue.Count == 0) break;
    var (pos, dir, set, score) = p2queue.Dequeue();
    if (visitedp2.ContainsKey((pos, dir))) {
        if (visitedp2[(pos, dir)] > 15) continue; // A lot of the optimal solutions do overlap
        visitedp2[(pos, dir)]++;
    } else {
        visitedp2[(pos, dir)] = 1;
    }
    prevpos = pos;
    
    if (GridPos(pos) == Locations.END) {
        solutionp2.Add((pos, dir, set, score));
        if (solutionp2.Count > 20) break;
    }

    // If dead-end then just break.
    if (GridPos(pos + dir) == Locations.WALL
        && GridPos(pos + dir.TurnCW) == Locations.WALL
        && GridPos(pos + dir.TurnACW) == Locations.WALL)
        {continue;}
    
    if (GridPos(pos + dir.TurnCW) != Locations.WALL) {
        p2queue.Enqueue((pos, dir.TurnCW, new(set), score + 1000), score + 1000);
    }
    if (GridPos(pos + dir.TurnACW) != Locations.WALL) {
        p2queue.Enqueue((pos, dir.TurnACW, new(set), score + 1000), score + 1000);
    }
    if (GridPos(pos + dir) != Locations.WALL) {
        // This one is done last so that the set mutation doesn't affect the others
        set.Add(pos + dir);
        p2queue.Enqueue((pos + dir, dir, set, score + 1), score + 1);
    }
    count++;
    visited.Add((pos, dir));
}

var minsols = solutionp2.MinBy(it => it.Item4).Item4;
var minsolsets = solutionp2.Where(it => it.Item4 == minsols).Select(it => it.Item3);
var unions = minsolsets.Aggregate((agg, it) => agg.Union(it).ToHashSet());
WriteLine(unions.Count);
WriteLine(DateTime.Now.Subtract(t).TotalMilliseconds);

enum Locations {EMPTY, WALL, END, START}