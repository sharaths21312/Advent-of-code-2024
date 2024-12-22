// This is not working currently
#load "utils.csx"
using System.IO;
using System.Threading;

Locations[,] MainGrid = new Locations[100, 50];
Point robot = new();
List<Moves> MoveList = [];

using (var file = new StreamReader("./inputs/day15.txt")) {
    var filetext = file.ReadToEnd().Split("\n\n", ss);
    MoveList = string.Concat(filetext[1].Split("\n")).Select(
        chr => chr switch {
            '^' => Moves.UP,
            'v' => Moves.DOWN,
            '<' => Moves.LEFT,
            '>' => Moves.RIGHT,
            _ => Moves.RIGHT
        }
    ).ToList();

    var strarr = filetext[0].Split("\n").Select(l => l.ToCharArray()).ToArray();

    for (int i = 0; i < 50; i++) {
        for (int k = 0; k < 50; k++) {
            MainGrid[i*2, k] = strarr[k][i] switch {
                '#' => Locations.WALL,
                'O' => Locations.LBOX,
                '.' => Locations.EMPTY,
                '@' => Locations.ROBOT,
                _ => Locations.EMPTY
            };
            MainGrid[i*2+1, k] = strarr[k][i] switch {
                '#' => Locations.WALL,
                'O' => Locations.RBOX,
                '.' => Locations.EMPTY,
                '@' => Locations.EMPTY,
                _ => Locations.EMPTY
            };
            if (strarr[k][i] == '@') robot = new(i*2, k);
        }
    }
}

Console.Clear();
CursorVisible = false;
// PrintGrid();

foreach(var m in MoveList) {
    IterateGrid(m);
    // ReadKey();
}
PrintGrid();
long sum = 0;
for (int i = 0; i < 50; i++) {
    for (int k = 0; k < 100; k++) {
        if (MainGrid[k, i] == Locations.LBOX) {
            sum += k + i*100;
        }
    }
}
WriteLine(sum);

void PrintGrid() {
    Console.SetCursorPosition(0, 0);
    StringBuilder buffer = new();
    for (int i = 0; i < 50; i++) {
        for (int k = 0; k < 100; k++) {
            buffer.Append(MainGrid[k,i] switch {
                Locations.LBOX => '[',
                Locations.RBOX => ']',
                Locations.ROBOT => '@',
                Locations.WALL => '#',
                _ => '.'
            });
        }
        buffer.AppendLine();
    }
    Console.Write(buffer.ToString());
}

void IterateGrid(Moves m) {
    Point dir = m switch {
        Moves.UP => (0, -1),
        Moves.DOWN => (0, 1),
        Moves.LEFT => (-1, 0),
        _ => (1, 0)
    };
    if (!CheckMove(robot + dir, dir)) return;
    if (GetGrid(robot + dir) != Locations.EMPTY) {
        MoveRec(robot + dir, dir);
    }

    SetGrid(robot, Locations.EMPTY);
    robot += dir;
    SetGrid(robot, Locations.ROBOT);
}

void MoveRec(Point p, Point dir) {
    if (GetGrid(p) == Locations.EMPTY) return;
    Point Lpoint = new();
    Point Rpoint = new();
    if (GetGrid(p) == Locations.LBOX) {
        Lpoint = p;
        Rpoint = p.Right;
    } else if (GetGrid(p) == Locations.RBOX) {
        Rpoint = p;
        Lpoint = p.Left;
    }
    if (p == Lpoint && dir == (1, 0)) {
        MoveRec(Rpoint + dir, dir);
    } else if (p == Rpoint && dir == (-1, 0)) {
        MoveRec(Lpoint + dir, dir);
    }
    else {
        if (GetGrid(Lpoint + dir) == Locations.LBOX
            || GetGrid(Lpoint + dir) == Locations.RBOX) {
            MoveRec(Lpoint + dir, dir);
        }
        if (GetGrid(Rpoint + dir) == Locations.LBOX
            || GetGrid(Rpoint + dir) == Locations.RBOX) {
            MoveRec(Rpoint + dir, dir);
        }
    }

    SetGrid(Lpoint, Locations.EMPTY);
    SetGrid(Rpoint, Locations.EMPTY);
    SetGrid(Lpoint + dir, Locations.LBOX);
    SetGrid(Rpoint + dir, Locations.RBOX);
}

bool CheckMove(Point p, Point dir) {
    if (GetGrid(p) == Locations.EMPTY) return true;
    if (GetGrid(p) == Locations.WALL) return false;
    if (GetGrid(p + dir) == Locations.WALL) return false;

    Point Lpoint = new();
    Point Rpoint = new();
    if (GetGrid(p) == Locations.LBOX) {
        Lpoint = p;
        Rpoint = p.Right;
    } else if (GetGrid(p) == Locations.RBOX) {
        Rpoint = p;
        Lpoint = p.Left;
    }
    if (GetGrid(Lpoint + dir) == Locations.EMPTY 
            && GetGrid(Rpoint + dir) == Locations.EMPTY) {
        return true;
    }

    if (GetGrid(Lpoint + dir) == Locations.WALL 
            || GetGrid(Rpoint + dir) == Locations.WALL) {
        return false;
    }
    if (p == Lpoint && dir == (1, 0)) {
        return CheckMove(Rpoint + dir, dir);
    } else if (p == Rpoint && dir == (-1, 0)) {
        return CheckMove(Lpoint + dir, dir);
    } // If the thing is moving horizontally we have to account for that and skip one
    return CheckMove(Lpoint + dir, dir) && CheckMove(Rpoint + dir, dir);
}

Locations GetGrid(Point p) {
    return MainGrid[p.X, p.Y];
}
void SetGrid(Point p, Locations l) {
    MainGrid[p.X, p.Y] = l;
}

enum ShortLocations {
    EMPTY,
    ROBOT,
    WALL,
    BOX
}
enum Locations {
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