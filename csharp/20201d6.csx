#load "utils.csx"
using System.IO;
using dil = System.Collections.Generic.Dictionary<int, long>;
dil school = [];
using (var file = new StreamReader("./inputs/2021d6.txt")) {
    school = CountDict(file.ReadToEnd().Split(",", ss).Select(it => int.Parse(it)), 1L);
}

dil t = new(school);
for (int i = 0; i < 256; i++) {
    if (i == 80) WriteLine(t.Sum(it => it.Value));
    t = Iterate(t);
}
WriteLine(t.Sum(it => it.Value));
dil Iterate(dil current) {
    dil result = [];

    foreach (var (k, v) in current) {
        if (k == 0) {
            result.AddNum(6, v);
            result.AddNum(8, v);
        } else {
            result.AddNum(k-1, v);
        }
    }

    return result;
}