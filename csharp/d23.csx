#load "utils.csx"
using System.IO;
using strset = System.Collections.Generic.HashSet<string>;

Dictionary<string, strset> Connections = [];
strset AllPoints = [];

using (var file = new StreamReader("./inputs/day23.txt")) {
    foreach (var line in file.ReadToEnd().Split("\n", ss)) {
        var t1 = line.Split('-', ss)[0];
        var t2 = line.Split('-', ss)[1];
        AllPoints.Add(t1);
        AllPoints.Add(t2);
        if (Connections.ContainsKey(t1)) {
            Connections[t1].Add(t2);
        } else {
            Connections[t1] = [t2];
        }
        if (Connections.ContainsKey(t2)) {
            Connections[t2].Add(t1);
        } else {
            Connections[t2] = [t1];
        }
    }
}

HashSet<Connection> TripleSets = [];
var t = DateTime.Now;

foreach (var fst in Connections) {
    var pairs = from item1 in fst.Value.Index()
                from item2 in fst.Value.Index()
                where item1.Index < item2.Index
                select (item1.Item, item2.Item);
    foreach (var (snd, thr) in pairs) {
        if (Connections[snd].Contains(thr)) {
            TripleSets.Add(new([fst.Key, snd, thr]));
        }
    }
}

WriteLine(TripleSets.Count(c => c.StartsT()));
strset MaxSet = [];
BK([], new(AllPoints), []);
WriteLine(String.Join(',', MaxSet.Order()));
WriteLine(GetMsTime(t));

// https://en.wikipedia.org/wiki/Bron-Kerbosch_algorithm
void BK(strset R, strset P, strset X) {
    if (P.Count == 0 && X.Count == 0) {
        if (R.Count > MaxSet.Count) {
            MaxSet = R.ToHashSet();
        }
    }
    while (P.Count > 0) {
        var v = P.First();
        BK(R.Union([v]).ToHashSet(),
            P.Intersect(Connections[v]).ToHashSet(),
            X.Intersect(Connections[v]).ToHashSet());
        P.Remove(v);
        X.UnionWith([v]);
    }
}

bool IsClique(IEnumerable<string> verts) {
    return verts.All(vert => IsSubset(Connections[vert], verts));
}


record class Connection {
    public string fst, snd, thr;
    public Connection(string[] inp) {
        var l = inp.Order().ToArray();
        fst = l[0];
        snd = l[1];
        thr = l[2];
    }

    public bool Contains(string inp) => inp == fst || inp == snd || inp == thr;
    public bool StartsT() => fst.StartsWith('t') || snd.StartsWith('t') || thr.StartsWith('t');
};