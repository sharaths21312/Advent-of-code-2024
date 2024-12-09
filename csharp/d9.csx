using System.IO;

string file;
int[] input;
using (var f = new StreamReader("./inputs/day9.txt")) {
    file = f.ReadToEnd().Trim();
}
input = file.Select(c => int.Parse(c.ToString())).ToArray();

void Checksum() {
    List<int> sorted = [];
    for (int i = 0; i < input.Length; i++) {
        if (i%2 == 1) {
            sorted.AddRange(Enumerable.Repeat(-1, input[i]));
        } else {
            sorted.AddRange(Enumerable.Repeat(i/2, input[i]));
        }
    }
    int head = sorted.FindIndex(v => v == -1);

    while (head != -1 && head < sorted.Count) {
        if (sorted[^1] == -1) {
            sorted.RemoveAt(sorted.Count -1);
            continue;
        }
        sorted[head] = sorted[^1];
        sorted.RemoveAt(sorted.Count -1);
        head = sorted.FindIndex(v => v == -1);
    }
    long checksum = 0;

    foreach (var (idx, val) in sorted.Index()) {
        checksum += idx * val;
    }
    
    Console.WriteLine(checksum);
}

void Checksum2() {
    List<int> inls = [];
    List<Block> files = [];
    List<Block> free = [];

    for (int i = 0; i < input.Length; i++) {
        if (i%2 == 1) {
            free.Add(new Block(inls.Count, input[i], -1));
            inls.AddRange(Enumerable.Repeat(-1, input[i]));
        } else {
            files.Add(new Block(inls.Count, input[i], i/2));
            inls.AddRange(Enumerable.Repeat(i/2, input[i]));
        }
    }


    var dataarr = inls.ToArray();
    
    files.Reverse();
    foreach (var file in files) {
        var freespace = free.FirstOrDefault(b => b.Index < file.Index && b.Length >= file.Length);
        if (freespace == default) continue;

        var datablk = dataarr.AsSpan(freespace.Index, file.Length);
        var fileblk = dataarr.AsSpan(file.Index, file.Length);
        var fileval = file.Value;
        fileblk.Fill(-1);
        datablk.Fill(fileval);

        if (freespace.Length == file.Length) {
            free.Remove(freespace);
        } else {
            var i = free.IndexOf(freespace);
            free[i] = free[i] with {Index = freespace.Index + file.Length, Length = freespace.Length - file.Length};
        }
    }

    long checksum = 0;

    foreach (var (idx, val) in dataarr.Index()) {
        if (val == -1) continue;
        checksum += idx * val;
    }
    
    Console.WriteLine(checksum);
}

var t = DateTime.Now;
Checksum();
Checksum2();
Console.WriteLine(DateTime.Now.Subtract(t).TotalMilliseconds);
record Block(int Index, int Length, int Value);