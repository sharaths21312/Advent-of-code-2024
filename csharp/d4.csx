using System.IO;
using System.Text.RegularExpressions;

var filetext = "";

using (var file = new StreamReader("./inputs/day4.txt"))
{
    filetext = file.ReadToEnd().Trim();
}

Dictionary<(int, int), char> MappedFileText = [];
int height = filetext.Split("\n").Count();
int width = filetext.Split("\n").First().Length;

foreach (var (line, ypos) in filetext.Split("\n").Select((str, ind) => (str.Trim(), ind))) {
    foreach (var (chr, xpos) in line.Select((chr, ind) => (chr, ind))) {
        MappedFileText[(xpos, ypos)] = chr;
    }
}

Console.WriteLine(LoopReturn());
Console.WriteLine(DoesMapXmas2());

int LoopReturn() {
    int count = 0;

    foreach (var item in MappedFileText) {
        if (item.Value == 'X') {
            count += DoesMapXmas(item.Key.Item1, item.Key.Item2);
        }
    }

    return count;
}

int DoesMapXmas(int xpos, int ypos) {
    int result = 0;
    bool up = ypos >= 3;
    bool down = ypos < height - 3;
    bool left = xpos >= 3;
    bool right = xpos < width - 3;
    if (left) result += MatchLeft(xpos, ypos) ? 1 : 0;
    if (right) result += MatchRight(xpos, ypos)  ? 1 : 0;
    if (up) result += MatchUp(xpos, ypos) ? 1 : 0;
    if (down) result += MatchDown(xpos, ypos) ? 1 : 0;
    if (left && up) result += MatchUpLeft(xpos, ypos) ? 1 : 0;
    if (right && up) result += MatchUpRight(xpos, ypos) ? 1 : 0;
    if (left && down) result += MatchDownLeft(xpos, ypos) ? 1 : 0;
    if (right && down) result += MatchDownRight(xpos, ypos) ? 1 : 0;
    return result;
}

int DoesMapXmas2() {
    int result = 0;
    foreach (var i in MappedFileText) {
        if (i.Value == 'A') {
            result += MatchX_Mas(i.Key.Item1, i.Key.Item2);
        }
    }
    return result;
}

int MatchX_Mas(int xpos, int ypos) {
    var mft = MappedFileText;
    if (xpos == 0 || xpos == width - 1 || ypos == 0 || ypos == height - 1) return 0;
    char tl = mft[(xpos - 1, ypos - 1)];
    char tr = mft[(xpos + 1, ypos - 1)];
    char bl = mft[(xpos - 1, ypos + 1)];
    char br = mft[(xpos + 1, ypos + 1)];
    return (tl, tr, bl, br) switch {
        ('M', 'M', 'S', 'S') => 1,
        ('S', 'M', 'S', 'M') => 1,
        ('S', 'S', 'M', 'M') => 1,
        ('M', 'S', 'M', 'S') => 1,
        _ => 0,
    };
}

bool MatchUp(int xpos, int ypos) {
    return  MappedFileText[(xpos, ypos  )] == 'X' &&
            MappedFileText[(xpos, ypos-1)] == 'M' &&
            MappedFileText[(xpos, ypos-2)] == 'A' &&
            MappedFileText[(xpos, ypos-3)] == 'S';
}


bool MatchDown(int xpos, int ypos) {
    return  MappedFileText[(xpos, ypos  )] == 'X' &&
            MappedFileText[(xpos, ypos+1)] == 'M' &&
            MappedFileText[(xpos, ypos+2)] == 'A' &&
            MappedFileText[(xpos, ypos+3)] == 'S';
}


bool MatchLeft(int xpos, int ypos) {
    return  MappedFileText[(xpos  , ypos)] == 'X' &&
            MappedFileText[(xpos-1, ypos)] == 'M' &&
            MappedFileText[(xpos-2, ypos)] == 'A' &&
            MappedFileText[(xpos-3, ypos)] == 'S';
}

bool MatchRight(int xpos, int ypos) {
    return  MappedFileText[(xpos  , ypos)] == 'X' &&
            MappedFileText[(xpos+1, ypos)] == 'M' &&
            MappedFileText[(xpos+2, ypos)] == 'A' &&
            MappedFileText[(xpos+3, ypos)] == 'S';
}


bool MatchUpRight(int xpos, int ypos) {
    return  MappedFileText[(xpos  , ypos  )] == 'X' &&
            MappedFileText[(xpos+1, ypos-1)] == 'M' &&
            MappedFileText[(xpos+2, ypos-2)] == 'A' &&
            MappedFileText[(xpos+3, ypos-3)] == 'S';
}

bool MatchUpLeft(int xpos, int ypos) {
    return  MappedFileText[(xpos  , ypos  )] == 'X' &&
            MappedFileText[(xpos-1, ypos-1)] == 'M' &&
            MappedFileText[(xpos-2, ypos-2)] == 'A' &&
            MappedFileText[(xpos-3, ypos-3)] == 'S';
}

bool MatchDownLeft(int xpos, int ypos) {
    return  MappedFileText[(xpos  , ypos  )] == 'X' &&
            MappedFileText[(xpos-1, ypos+1)] == 'M' &&
            MappedFileText[(xpos-2, ypos+2)] == 'A' &&
            MappedFileText[(xpos-3, ypos+3)] == 'S';
}

bool MatchDownRight(int xpos, int ypos) {
    return  MappedFileText[(xpos  , ypos  )] == 'X' &&
            MappedFileText[(xpos+1, ypos+1)] == 'M' &&
            MappedFileText[(xpos+2, ypos+2)] == 'A' &&
            MappedFileText[(xpos+3, ypos+3)] == 'S';
}