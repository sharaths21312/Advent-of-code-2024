#load "utils.csx"
using System.IO;

HashSet<Lock> Locks = [];
HashSet<Key> Keys = [];

using (var file = new StreamReader("./inputs/day25.txt")) {
    foreach (var schematic in file.ReadToEnd().Split("\n\n", ss)) {
        var lines = schematic.Split("\n");
        var tmp = lines.Transpose();
        if (lines[0][0] == '#') {
            var heights = tmp.Select(it => it.Count(chr => chr == '#') - 1).ToArray();
            Locks.Add(new(heights));
        } else {
            var heights = tmp.Select(it => it.Count(chr => chr == '#') - 1).ToArray();
            Keys.Add(new(heights));
        }
    }
}

var count = Locks.Sum(lck => Keys.Count(key => LockKeyMatch(lck, key)));
WriteLine(count);

bool LockKeyMatch(Lock lck, Key key) {
    return lck.Heights.Zip(key.Heights)
            .Select(lk => lk.First + lk.Second)
            .All(num => num <= 5);
}

record Lock(int[] Heights) {
    public override string ToString()
    {
        return String.Join(',', Heights);
    }
};
record Key(int[] Heights) {
    public override string ToString()
    {
        return String.Join(',', Heights);
    }
};