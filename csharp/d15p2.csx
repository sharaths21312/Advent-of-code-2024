// This is not working currently
#load "utils.csx"
using System.IO;
using System.Threading;

ShortLocations[,] parsed = new ShortLocations[50, 50];
Locations[,] duplicated = new Locations[100, 50];
List<Moves> MoveList = [];

using (var file = new StreamReader("./inputs/day15.txt")) {
    var filetext = file.ReadToEnd().Split("\n\n", ss);
    foreach(var (y, line) in filetext[0].Split("\n", ss).Index()) {
        foreach(var (x, chr) in line.Index()) {
            parsed[x, y] = chr switch {
                '.' => ShortLocations.EMPTY,
                '@' => ShortLocations.ROBOT,
                '#' => ShortLocations.WALL,
                'O' => ShortLocations.BOX,
                _ => ShortLocations.BOX
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

for (int x = 0; x < 50; x++) {
    for (int y = 0; y < 50; y++) {
        if (parsed[x, y] == ShortLocations.BOX) {
            duplicated[2*x, y] = Locations.LBOX;
            duplicated[2*x + 1, y] = Locations.RBOX;
        } else if (parsed[x, y] == ShortLocations.ROBOT) {
            duplicated[2*x, y] = Locations.ROBOT;
            duplicated[2*x + 1, y] = Locations.EMPTY;
        } else {
            var l = parsed[x, y] switch {
                ShortLocations.EMPTY => Locations.EMPTY,
                ShortLocations.WALL => Locations.WALL
            };
            duplicated[2*x, y] = l;
            duplicated[2*x + 1, y] = l;
        }
    }
}

var grid = new Grid(duplicated);

foreach (var mv in MoveList) {
    grid.Move(mv);
}

long count = 0;
for (int x = 0; x < Grid.width; x++) {
    for (int y = 0; y < Grid.height; y++) {
        if (grid.MainGrid[x, y] == Locations.LBOX) {
            count += 100 * y + x;
        }
    }
}

int idx = 0;
Console.CursorVisible = false;
Clear();
Write(grid.RenderString());
ReadKey();
foreach (var mv in MoveList) {
    grid.Move(mv);
    SetCursorPosition(0, 0);
    Write(grid.RenderString());
    Write(mv.ToString() + new string(' ', 30));
    // Thread.Sleep(100);
    ReadKey();
}


class Grid {
    public const int width = 100;
    public const int height = 50;

    public Locations[,] MainGrid = new Locations[width, height];
    List<Box> Boxes = [];
    public Point Robot;

    public Grid(Locations[,] parsed)
    {
        MainGrid = parsed;
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (MainGrid[i, j] == Locations.ROBOT) {
                    Robot = new(i, j);
                }
                if (MainGrid[i, j] == Locations.LBOX) {
                    Boxes.Add(new((i, j)));
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
        Point movetup = move switch {
            Moves.UP => (0, -1),
            Moves.DOWN => (0, 1),
            Moves.LEFT => (-1, 0),
            Moves.RIGHT => (1, 0)
        };
        var moved = Robot + movetup;
        if (GetPoint(moved) == Locations.EMPTY) {
            SetPoint(Robot, Locations.EMPTY);
            Robot += movetup;
            SetPoint(Robot, Locations.ROBOT);
            return;
        } else if (GetPoint(moved) == Locations.WALL) {
            return;
        }
        // Here the point can only be some sort of box
        var box = Boxes.Where(bx => bx.p == moved || bx.p.Right == moved).First();
        if (!CheckMoveBox(box, movetup)) return;
        MoveBox(box, movetup);
        RenderBoxes();
        return;
    }

    void RenderBoxes() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++){
                if (MainGrid[x, y] == Locations.LBOX) {
                    MainGrid[x, y] = Locations.EMPTY;
                    MainGrid[x+1, y] = Locations.EMPTY;
                } else if (MainGrid[x, y] == Locations.RBOX) {
                    MainGrid[x, y] = Locations.EMPTY;
                    MainGrid[x-1, y] = Locations.EMPTY;
                }
            }
        }
        foreach (var box in Boxes) {
            SetPoint(box.p, Locations.LBOX);
            SetPoint(box.p.Right, Locations.RBOX);
        }
    }
    public string RenderString() {
        StringBuilder sb = new();
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                sb.Append(MainGrid[x, y] switch {
                    Locations.EMPTY => ".",
                    Locations.LBOX => "[]",
                    Locations.WALL => "#",
                    Locations.ROBOT => "@",
                    _ => ""
                });
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }
    
    void MoveBox(Box box, Point dir) {
        if (!CheckMoveBox(box, dir)) return;

        foreach (var b in Boxes) {
            if (b == box) continue;
            if (box.Collides(b.p + dir)) {
                MoveBox(b, dir);
            }
        }
        box.Move(dir);
        RenderBoxes();
    }

    bool CheckMoveBox(Box box, Point dir) {
        if (GetPoint(box.p + dir) == Locations.EMPTY && GetPoint(box.p.Right + dir) == Locations.EMPTY) {
            return true;
        } // Two empty spaces in front
        var c = box.p + dir;
        if (GetPoint(c) == Locations.WALL || GetPoint(c.Right) == Locations.WALL) return false;

        foreach (var b in Boxes) {
            if (b == box) continue;
            if (box.Collides(b.p + dir)) {
                if (!CheckMoveBox(b, dir)) return false;
                // If any box is blocked then don't move
            }
        }
        // None of the boxes failed, safe to move
        return true;
    }
}

record class Box {
    public Point p { get; set; }
    public Box(Point p) {
        this.p = p;
    }

    public Point MoveCheck(Point move) {
        return p + move;
    }

    public void Move(Point move) {
        p += move;
    }

    public bool Contains(Point pos) {
        return p == pos || p.Right == pos;
    }

    public bool Collides(Point pos) {
        return p == pos || p.Left == pos || p.Right == pos;
    }
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