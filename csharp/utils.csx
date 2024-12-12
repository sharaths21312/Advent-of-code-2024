using System.Numerics;

public static void AddNum<Tkey, Tnum>(this Dictionary<Tkey, Tnum> dict, Tkey key, Tnum val)
where Tnum: INumber<Tnum> {
    if (dict.ContainsKey(key)) {
        dict[key] += val;
    } else {
        dict[key] = val;
    }
}

record Point(int x, int y)
{
    public static Point operator +(Point p, (int, int) dir)
        => p with { x = p.x + dir.Item1, y = p.y + dir.Item2 };

    public Point Up {get => this + (0, -1);}
    public Point Down {get => this + (0, 1);}
    public Point Left {get => this + (-1, 0);}
    public Point Right {get => this + (1, 0);}
    public Point UpLeft {get => this + (-1, -1);}
    public Point DownRight {get => this + (1, 1);}
    public Point DownLeft {get => this + (-1, 1);}
    public Point UpRight {get => this + (1, -1);}

    public static implicit operator Point((int, int) tup) => new(tup.Item1, tup.Item2);
    public static implicit operator (int, int)(Point p) => (p.x, p.y);
}