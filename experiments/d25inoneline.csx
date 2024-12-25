Console.WriteLine(System.IO.File.ReadAllText("./inputs/day25.txt")
.Split("\n\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
.Select(str =>
    str.Split("\n")
    // One-liner for transposing via linq stolen off stackoverflow
    .Select(a => a.Select(b => Enumerable.Repeat(b, 1)))
    .Aggregate((a, b) => a.Zip(b, Enumerable.Concat))
)
.Select(
    elt => elt.Select(x => x.ToArray()).ToArray()
)
.GroupBy(
    elt => elt[0][0] == '#', // Group by whether it's a lock or key
    // Convert each lock/key from the input char[][] to int[]
    (group, locksorkeys) => locksorkeys.Select(block => block
                .Select(ln => ln.Count(x => x == '#') - 1))
                .ToArray()
) // Here we have a 2-item list, where one is a list of locks and the other a list of keys
.Chunk(2)
.Select(chunk => (chunk[0], chunk[1])) // Convert the 2 item list to a tuple
.Select(chunk => 
    chunk.Item1.SelectMany(it => chunk.Item2.Select(it2 => (it, it2)))
    // Converts the tuple into a list of every single lock and key combo
)
.First()
.Select(x => x.it.Zip(x.it2)) // Makes a tuple of corresponding lock and key pins)
.Count(x => x.All(it => it.First + it.Second <= 5))); // The main logic lol
