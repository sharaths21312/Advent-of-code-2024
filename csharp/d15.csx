#load "utils.csx"
using System.IO;

Locations[,] parsed = new Locations[50, 50];
List<Moves> MoveList = [];

using (var file = new StreamReader("./inputs/day15.txt")) {
    var filetext = file.ReadToEnd().Split("\n\n", ss);
    foreach(var (y, line) in filetext[0].Split("\n", ss).Index()) {
        foreach(var (x, chr) in line.Index()) {
            parsed[x, y] = chr switch {
                '.' => Locations.EMPTY,
                '@' => Locations.ROBOT,
                '#' => Locations.WALL,
                'O' => Locations.BOX,
                _ => Locations.BOX
            };
        }
    }

    foreach (var chr in String.Concat(filetext[1].Split('\n', ss))) {
        MoveList.Add(chr switch {
            '^' => Moves.UP,
            'v' => Moves.DOWN,
            '<' => Moves.LEFT,
            '>' => Moves.RIGHT,
            _ => throw new Exception()
        });
    }
}

var grid = new Grid(parsed);

foreach (var mv in MoveList) {
    grid.Move(mv);
}

long count = 0;
for (int x = 0; x < Grid.width; x++) {
    for (int y = 0; y < Grid.height; y++) {
        if (grid.MainGrid[x, y] == Locations.BOX) {
            count += 100 * y + x;
        }
    }
}
WriteLine(count);


class Grid {
    public const int width = 50;
    public const int height = 50;

    public Locations[,] MainGrid = new Locations[width, height];
    public Point Robot;

    public Grid(Locations[,] parsed)
    {
        MainGrid = parsed;
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (MainGrid[i, j] == Locations.ROBOT) {
                    Robot = new(i, j);
                }
            }
        }
    }

    Locations GetPoint(Point point) {
        return MainGrid[point.X, point.Y];
    }
    void SetPoint(Point point, Locations loc) {
        MainGrid[point.X, point.Y] = loc;
    }

    public void Move(Moves move) {
        var (xmove, ymove) = move switch {
            Moves.UP => (0, -1),
            Moves.DOWN => (0, 1),
            Moves.LEFT => (-1, 0),
            Moves.RIGHT => (1, 0),
            _ => (0, 0)
        };
        Point mv = new(xmove, ymove);

        if (GetPoint(Robot + mv) == Locations.EMPTY) {
            // If the next position is empty just move
            SetPoint(Robot, Locations.EMPTY);
            Robot = Robot + mv;
            SetPoint(Robot, Locations.ROBOT);
            return;
        } 

        var cloc = Robot;
        // While not at a wall, keep seaerching forward
        while (GetPoint(cloc) != Locations.WALL) {
            cloc = cloc + mv;
            // If empty, we can move all the boxes
            if (GetPoint(cloc) == Locations.EMPTY) {
                while (GetPoint(cloc) != Locations.ROBOT) {
                    // Move all the previous boxes
                    SetPoint(cloc, Locations.BOX);
                    cloc -= mv;
                }
                // Now cloc = old robot pos
                SetPoint(cloc, Locations.EMPTY);
                SetPoint(cloc + mv, Locations.ROBOT);
                Robot += mv;
                return;
            }
        }
        // If it reaches here you just fail to move
        return;
    }
}

enum Locations {
    EMPTY,
    ROBOT,
    WALL,
    BOX
}

enum WLocations {
    EMPTY,
    ROBOT,
    WALL,
    LBOX,
    RBOX
}

enum Moves {
    UP,
    DOWN,
    LEFT,
    RIGHT
}