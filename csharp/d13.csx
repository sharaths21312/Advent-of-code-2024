#load "utils.csx"
#r "nuget:MathNet.Numerics,5.0.0"
using System.IO;
using System.Text.RegularExpressions;
using System.Numerics;

List<Game> Games = [];

var matchnum = new Regex(@"X(\+|=)(\d+), Y(\+|=)(\d+)");
using (var file = new StreamReader("./inputs/day13.txt")) {
    foreach (var item in file.ReadToEnd().Split("\n\n", ss)) {
        var mcs = matchnum.Matches(item);
        Games.Add(new Game(
            new LongPoint(int.Parse(mcs[2].Groups[2].Value), int.Parse(mcs[2].Groups[4].Value)),
            new LongPoint(int.Parse(mcs[0].Groups[2].Value), int.Parse(mcs[0].Groups[4].Value)),
            new LongPoint(int.Parse(mcs[1].Groups[2].Value), int.Parse(mcs[1].Groups[4].Value))
        ));
    }
}

var validGames = EvaluatePossibleGames();
var multiples = validGames.Where(it => it.Value.Count > 1).ToDictionary();
var newGames = Games.Select(g => g with {Target = g.Target + (10000000000000, 10000000000000)});

WriteLine(validGames.Select(it => it.Value.Select(v => 3*v.Item1 + v.Item2).Min()).Sum());
WriteLine(EvaluatePossibleGames_2());

Dictionary<Game, List<(long, long)>> EvaluatePossibleGames() {
    Dictionary<Game, List<(long, long)>> result = [];
    foreach (var game in Games) {
        for (int a = 0; a < 100; a++) {
            for (int b = 0; b < 100; b++) {
                if (CheckGame(game, a, b)) {
                    result.AddToValList(game, (a, b));
                }
            }
        }
    }
    return result;
}


BigInteger EvaluatePossibleGames_2() {
    BigInteger result = 0;
    foreach (var game in newGames) {
        var a = game.A;
        var b = game.B;
        var t = game.Target;
        // a.x * m + b.x * n = t.x
        // a.y * m + b.y * n = t.y
        // apply cramer's rule
        var det = a.X * b.Y - a.Y * b.X;
        if (det == 0) continue; // No solution
        var detL = t.X * b.Y - t.Y * b.X;
        var detR = a.X * t.Y - a.Y * t.X;
        if (detL % det != 0 || detR % det != 0) continue; // Non integer solution

        var m = detL/det;
        var n = detR/det;
        result += 3*m + n;
    }
    return result;
}

bool CheckGame(Game game, long ACount, long BCount) {
    return game.Target == (game.A * ACount + game.B * BCount);
}
bool IsGameGreater(Game game, long Acount, long Bcount) {
    return (game.Target.X < game.A.X * Acount + game.B.X * Bcount) ||
        (game.Target.Y < game.A.Y * Acount + game.B.Y * Bcount);
}

record Game(LongPoint Target, LongPoint A, LongPoint B);