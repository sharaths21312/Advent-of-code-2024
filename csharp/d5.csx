using System.IO;
using System.Text.RegularExpressions;

int[][] conds;
int[][] updates;

using (var file = new StreamReader("./inputs/day5.txt")) {
    string[] filearr = file.ReadToEnd().Split("\n\n");
    conds = filearr[0].Trim().Split("\n").Select(line => line.Split("|")
                      .Select(num => int.Parse(num)).ToArray()).ToArray();
    updates = filearr[1].Trim().Split("\n").Select(line => line.Split(",")
                      .Select(num => int.Parse(num)).ToArray()).ToArray();;
}

System.Console.WriteLine(SumOrdered());

int SumOrdered() {
    int result = 0;
    bool success = true;

    foreach (var update in updates) {
        success = true;
        foreach (var cond in conds) {
            if (update.Contains(cond[0]) && update.Contains(cond[1])) {
                if (Array.IndexOf(update, cond[0]) > Array.IndexOf(update, cond[1])) {
                    success = false;
                }
            }
        }
        if (success) result += update[(update.Length - 1)/2];
    }


    return result;
}

Console.WriteLine(SumIncorrect());

int SumIncorrect() {
    int result = 0;
    List<int[]> BadUpds = [];
    Dictionary<int, List<int>> mappedConds = [];

    foreach (var cond in conds) {
        if (mappedConds.ContainsKey(cond[0])) {
            mappedConds.GetValueOrDefault(cond[0])?.Add(cond[1]);
        } else {
            mappedConds.Add(cond[0], [cond[1]]);
        }
    }


    var comparer = Comparer<int>.Create((int num1, int num2) => {
        if (num1 == num2) return 0;
        return mappedConds[num1].Contains(num2) ? -1 : 1 ;
    });

    foreach (var update in updates) {
        foreach (var cond in conds) {
            if (update.Contains(cond[0]) && update.Contains(cond[1])) {
                if (Array.IndexOf(update, cond[0]) > Array.IndexOf(update, cond[1])) {
                    Array.Sort(update, comparer);
                    result += update[(update.Length - 1)/2];
                    break;
                }
            }
        }
        
    }

    return result;
}