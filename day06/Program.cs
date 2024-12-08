using System.IO;
using System.Diagnostics;

((int, int), (int, int)) findStart(char[][] lines) {
    for (int y = 0; y < lines.Length; y++) {
        for (int x = 0; x < lines[y].Length; x++) {
            if (lines[y][x] == '^') return ((x, y), (0, -1));
            if (lines[y][x] == '>') return ((x, y), (1, 0));
            if (lines[y][x] == '<') return ((x, y), (-1, 0));
            if (lines[y][x] == 'v') return ((x, y), (0, 1));
        }
    }
    Debug.Assert(false, "Unreachable");
    return ((-1, -1), (0, 0));
};
bool isValidPosition(char[][] lines, int x, int y) => y >= 0 && y < lines.Length && x >= 0 && x < lines[y].Length;

(int, int) turnRight(int vx, int vy) => (vx, vy) switch {
    (0, -1) => (1, 0),
    (1, 0)  => (0, 1),
    (0, 1)  => (-1, 0),
    (-1, 0) => (0, -1),
    _ => (0, 0),
};

void part1(string filePath) {
    char[][] lines = File.ReadAllLines(filePath).Select(val => val.ToCharArray()).ToArray();
    var ((x, y), (vx, vy)) = findStart(lines);
    int step = 0;
    do {
        while (isValidPosition(lines, x+vx, y+vy) && lines[y+vy][x+vx] == '#') {
            (vx, vy) = turnRight(vx, vy);
        }
        if (lines[y][x] != 'X') step++;
        lines[y][x] = 'X';
        y += vy;
        x += vx;
    } while (isValidPosition(lines, x, y));
    Console.WriteLine($"Part 1: {step}");
}

(int, int) advance(int x, int y, int vx, int vy) => (x+vx, y+vy);

IEnumerable<(int, int)> allPossibleNewBlocks(char[][] lines, List<(int, int)> blocks, int gx, int gy, int vx, int vy) {
    foreach (var (bx, by) in blocks) {
        var (newBx, newBy) = (vx, vy) switch {
            (0, -1) => (gx, by-1),
            (0, 1)  => (gx, by+1),
            (1, 0)  => (bx+1, gy),
            (-1, 0) => (bx-1, gy),
            _ => (-1, -1),
        };
        if (!isValidPosition(lines, newBx, newBy) || lines[newBy][newBx] != '.') continue;
        bool isPossible = (vx, vy) switch {
            (0, -1) => bx > gx && by <= gy,
            (0, 1)  => bx < gx && by >= gy,
            (1, 0)  => bx >= gx && by > gy,
            (-1, 0) => bx <= gx && by < gy,
            _ => true,
        };
        if (isPossible) {
            for (var (tempX, tempY) = advance(gx, gy, vx, vy); isValidPosition(lines, tempX, tempY); (tempX, tempY) = advance(tempX, tempY, vx, vy)) {
                if (tempX == newBx && tempY == newBy) yield return (newBx, newBy);
                else if (lines[tempY][tempX] == '#') break;
            }
        }
    }
}

bool canExit(char[][] lines, List<((int, int), (int, int))> path, int gx, int gy, int vx, int vy) {
    while (isValidPosition(lines, gx, gy)) {
        while (isValidPosition(lines, gx+vx, gy+vy) && lines[gy+vy][gx+vx] == '#') {
            (vx, vy) = turnRight(vx, vy);
        }
        (gx, gy) = advance(gx, gy, vx, vy);
        if (path.Contains(((gx, gy), (vx, vy)))) return false;
        else path.Add(((gx, gy), (vx, vy)));
    } 
    return true;
}

void part2(string filePath) {
    int result = 0;
    char[][] lines = File.ReadAllLines(filePath).Select(val => val.ToCharArray()).ToArray();

    List<(int, int)> blocks = new();
    for (int y = 0; y < lines.Length; y++) for (int x = 0; x < lines[y].Length; x++) if (lines[y][x] == '#') blocks.Add((x, y));

    var ((gx, gy), (vx, vy)) = findStart(lines);

    List<((int, int), (int, int))> path = new();
    while (isValidPosition(lines, gx, gy)) {
        path.Add(((gx, gy), (vx, vy)));
        lines[gy][gx] = 'X';
        foreach (var (newBx, newBy) in allPossibleNewBlocks(lines, blocks, gx, gy, vx, vy)) {
            int count = path.Count;
            lines[newBy][newBx] = '#';
            if (!canExit(lines, path, gx, gy, vx, vy)) {
                Console.WriteLine($"Found block: {(newBx, newBy)}");
                lines[newBy][newBx] = '0';
                result++;
            } else {
                lines[newBy][newBx] = '.';
            }
            path.RemoveRange(count, path.Count - count);
        }
        while (isValidPosition(lines, gx+vx, gy+vy) && lines[gy+vy][gx+vx] != '#') {
            (gx, gy) = advance(gx, gy, vx, vy);
            path.Add(((gx, gy), (vx, vy)));
            lines[gy][gx] = 'X';
        }
        // Console.WriteLine($"Guard: {((gx, gy), (vx, vy))}");
        if (!isValidPosition(lines, gx+vx, gy+vy)) break;
        else while (isValidPosition(lines, gx+vx, gy+vy) && lines[gy+vy][gx+vx] == '#') (vx, vy) = turnRight(vx, vy);
        path.Add(((gx, gy), (vx, vy)));
        lines[gy][gx] = 'X';
    }
    Console.WriteLine($"Part 2: {result}");
}

part1("input.txt");
part2("input.txt");
