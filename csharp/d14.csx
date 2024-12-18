#load "utils.csx"
#r "nuget:MathNet.Numerics,5.0.0"
using System.IO;
using System.Text.RegularExpressions;

const int width = 101;
const int height = 103;

List<Robot> robots = [];

using (var file = new StreamReader("./inputs/day14.txt")) {
    Regex parser = new(@"p=(.+),(.+) v=(.+),(.+)");
    foreach (var line in file.ReadToEnd().Split("\n", ss)) {
        var mc = parser.Match(line);
        robots.Add(new(
            int.Parse(mc.Groups[1].Value),
            int.Parse(mc.Groups[2].Value),
            int.Parse(mc.Groups[3].Value),
            int.Parse(mc.Groups[4].Value)
        ));
    }
}

int safety = 0;
List<Point> results = [];
foreach (var robot in robots) {
    results.Add(Move100(robot));
}
safety = results.Count(p => p.X < width/2 && p.Y < height/2)
        * results.Count(p => p.X < width/2 && p.Y > height/2)
        * results.Count(p => p.X > width/2 && p.Y < height/2)
        * results.Count(p => p.X > width/2 && p.Y > height/2);

WriteLine(safety);
int iteration = 0;
while (true)
{
    iteration++;
    foreach (var r in robots) {
        r.Tick(width, height);
    }
    var n = robots.Max(r => CountInLine(r, robots));
    if (n > 35) { // Le manual search
        // Turns out my answer had n = 37 exactly
        Clear();
        foreach (var robot in robots) {
            SetCursorPosition(robot.x, robot.y);
            Write("#");
        }
        SetCursorPosition(0, 0);
        WriteLine((iteration, n));
        ReadKey();
    }
}

Point Move100(Robot robot) {
    var xfinal = robot.x;
    var yfinal = robot.y;
    for (int i = 0; i < 100; i++) {
        xfinal = PosMod(xfinal + robot.xmove, width);
        yfinal = PosMod(yfinal + robot.ymove, height);
    }

    return new(xfinal, yfinal);
}


int CountInLine(Robot r, List<Robot> robots) {
    return robots.Count(r2 => r2.x == r.x && Math.Abs(r.y - r2.y) < 40);
}

record Robot {
    public int x {get; set;}
    public int y {get; set;}
    public int xmove {get; set;}
    public int ymove {get; set;}
    
    public Robot(int x, int y, int xmove, int ymove) {
        this.x = x;
        this.y = y;
        this.xmove = xmove;
        this.ymove = ymove;
    }

    public void Tick(int width, int height) {
        x = PosMod(x + xmove, width);
        y = PosMod(y + ymove, height);
    }

    
    int PosMod (int num, int div) {
        return num < 0 ? (num%div) + div : (num%div);
    }
};