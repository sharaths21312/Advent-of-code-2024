using System.IO;

var filetext = "";
Dictionary<(int, int), bool> Positions = [];

using (var file = new StreamReader("./inputs/day6.txt")) {
    filetext = file.ReadToEnd();
}

int xinit, yinit;
foreach (var (y, line) in filetext.Split("\n").Index()) {
    foreach (var (x, chr) in line.Index()) {
        Positions.Add((x, y), chr == '#');
        if (chr == '^') {xinit = x; yinit = y;}
    }
}
var (x, y) = (xinit, yinit);
var t = DateTime.Now;
Travel(x, y, Positions, 0, -1);
Travel2();
Console.WriteLine(DateTime.Now.Subtract(t).TotalMilliseconds);

List<Point> travelled = [];

List<Point> Travel (int xin, int yin,  Dictionary<(int, int), bool> Positions, int dxin = 0, int dyin = -1) {
    Point current = new(xin, yin, dxin, dyin);
    List<Point> visits = [current];
    HashSet<(int, int)> Pos = [current.pos];

    while (Positions.ContainsKey(MovePoint(current).pos)) {
        if (Positions[MovePoint(current).pos]) {
            current = RotatePoint(current);
            visits.Add(current);
        } else {
            current = MovePoint(current);
            visits.Add(current);
            Pos.Add(current.pos);
        }
    }
    Console.WriteLine(Pos.Count);
    return visits;
}

bool DoesTravelLoop (int xin, int yin, int dxin, int dyin, Dictionary<(int, int), bool> Obstacles) {
    Point current = new(xin, yin, dxin, dyin);
    HashSet<(int, int, int, int)> visits = [current.tup];

    while (Positions.ContainsKey(MovePoint(current).pos))
    {
        if (Obstacles[MovePoint(current).pos]) {
            current = RotatePoint(current);
        } else {
            current = MovePoint(current);
        }
        if (visits.Contains(current.tup)) {
            return true;
        }
        visits.Add(current.tup);
    }

    return false;
}


void Travel2() {
    var visits = Travel(x, y, Positions);
    HashSet<(int, int)> blockingpos = [];
    bool loop = false;
    foreach (var visit in visits.Skip(1)) {
        var adjusted = new Dictionary<(int, int), bool>(Positions) { [visit.pos] = true };
        loop = DoesTravelLoop(xinit, yinit, 0, -1, adjusted);
        if (loop) blockingpos.Add(visit.pos);
    }
    Console.WriteLine(blockingpos.Count);
}


record Point(int x, int y, int dx, int dy) {
    public (int, int) pos {get => (x, y);}
    public (int, int) dir {get => (dx, dy);}
    public (int, int, int, int) tup {get => (x, y, dx, dy);}
};
Point MovePoint(Point p) => p with { x = p.x + p.dx, y = p.y + p.dy };
Point RotatePoint(Point p) => p with { dx = -p.dy, dy = +p.dx };