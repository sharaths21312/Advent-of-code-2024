using System.Numerics;

public static void AddNum<Tkey, Tnum>(this Dictionary<Tkey, Tnum> dict, Tkey key, Tnum val)
where Tnum: INumber<Tnum> {
    if (dict.ContainsKey(key)) {
        dict[key] += val;
    } else {
        dict[key] = val;
    }
}

double GetMsTime(DateTime t) => DateTime.Now.Subtract(t).TotalMilliseconds;

bool IsSubset<T>(IEnumerable<T> Super, IEnumerable<T> Sub)
    => !Sub.Except(Super).Any();

Dictionary<T, Tnum> CountDict<T, Tnum>(IEnumerable<T> values, Tnum one)
where Tnum: INumber<Tnum>{
    var d = new Dictionary<T, Tnum>();

    foreach (var item in values) {
        d.AddNum(item, one);
    }

    return d;
}

public static void AddToValList<Tkey, Tlist>(this Dictionary<Tkey, List<Tlist>> dict, Tkey key, Tlist item) {
    if (dict.ContainsKey(key)) {
        dict[key].Add(item);
    } else {
        dict[key] = [item];
    }
}

record LongPoint(BigInteger X, BigInteger Y)
{
    public static LongPoint operator +(LongPoint p, (BigInteger, BigInteger) dir)
        => new(p.X + dir.Item1, p.Y + dir.Item2);
    public static LongPoint operator +(LongPoint p, (long, long) dir)
        => new(p.X + dir.Item1, p.Y + dir.Item2);
    public static LongPoint operator +(LongPoint p, LongPoint dir)
        => new(p.X + dir.X, p.Y + dir.Y);
    public static LongPoint operator *(LongPoint p, BigInteger count)
        => new(p.X * count, p.Y * count);

    public LongPoint Up {get => this + (0, -1);}
    public LongPoint Down {get => this + (0, 1);}
    public LongPoint Left {get => this + (-1, 0);}
    public LongPoint Right {get => this + (1, 0);}
    public LongPoint UpLeft {get => this + (-1, -1);}
    public LongPoint DownRight {get => this + (1, 1);}
    public LongPoint DownLeft {get => this + (-1, 1);}
    public LongPoint UpRight {get => this + (1, -1);}

    public static implicit operator LongPoint((BigInteger, BigInteger) tup) => new(tup.Item1, tup.Item2);
    public static implicit operator (BigInteger, BigInteger)(LongPoint p) => (p.X, p.Y);
}

record struct Point(int X, int Y)
{
    public static Point operator +(Point p, (int, int) dir)
        => new(p.X + dir.Item1, p.Y + dir.Item2);
    public static Point operator +(Point p, Point dir)
        => new(p.X + dir.X, p.Y + dir.Y);
    public static Point operator -(Point p, Point dir)
        => new(p.X - dir.X, p.Y - dir.Y);
    public static Point operator *(Point p, int count)
        => new(p.X * count, p.Y * count);
    public static Point operator -(Point p) => new(-p.X, -p.Y);

    public Point Up { get => this + (0, -1); }
    public Point Down { get => this + (0, 1); }
    public Point Left { get => this + (-1, 0); }
    public Point Right { get => this + (1, 0); }
    public Point UpLeft { get => this + (-1, -1); }
    public Point DownRight { get => this + (1, 1); }
    public Point DownLeft { get => this + (-1, 1); }
    public Point UpRight { get => this + (1, -1); }
    public Point TurnCW { get => new(-Y, X); }
    public Point TurnACW { get => new(Y, -X); }

    public static implicit operator Point((int, int) tup) => new(tup.Item1, tup.Item2);
    public static implicit operator (int, int)(Point p) => (p.X, p.Y);

    public override string ToString() => $"X: {X}, Y: {Y}";
    public int Distance(Point p) => Math.Abs(p.X - this.X) + Math.Abs(p.Y - this.Y);
}

static class Dirs {
    public static Point UP = new(0, -1);
    public static Point DOWN = new(0, 1);
    public static Point LEFT = new(-1, 0);
    public static Point RIGHT = new(1, 0);
}

int PosMod (int num, int div) {
    return num < 0 ? (num%div) + div : (num%div);
}


long PosMod (long num, long div) {
    return num < 0 ? (num%div) + div : (num%div);
}

var ss = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
