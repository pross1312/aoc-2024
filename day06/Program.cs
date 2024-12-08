using System.Diagnostics;
using System.IO;

void part2(string filePath) {
    char[][] lines = File.ReadAllLines(filePath).Select(line => line.ToCharArray()).ToArray();
    Func<((int, int), (int, int))> findStart = () => {
        for (int row = 0; row < lines.Length; row++) {
            for (int col = 0; col < lines[row].Length; col++) {
                if (lines[row][col] == '^') return ((col, row), (0, -1));
                if (lines[row][col] == '>') return ((col, row), (1, 0));
                if (lines[row][col] == '<') return ((col, row), (-1, 0));
                if (lines[row][col] == 'v') return ((col, row), (0, 1));
            }
        }
        Debug.Assert(false, "Unreachable");
        return ((-1, -1), (0, 0));
    };
    Func<int, int, bool> isValidPosition = (int col, int row) => row >= 0 && row < lines.Length && col >= 0 && col < lines[row].Length;
    ((int x, int y), (int vx, int vy)) = findStart();

    List<((int, int), (int, int))> turningPoints = new();
    Func<int, int, (int, int)> turnRight = (_vx, _vy) => (_vx, _vy) switch {
        (0, -1) => (1, 0),
        (1, 0) => (0, 1),
        (0, 1) => (-1, 0),
        (-1, 0) => (0, -1),
        _ => (0, 0)
    };

    Func<int, int, bool> canExit = (bx, by) => {
        lines[by][bx] = '#';
        (int tempX, int tempY) = (x, y);
        (int tempVX, int tempVY) = (vx, vy);
        int count = turningPoints.Count;
        while (isValidPosition(tempX, tempY)) {
            while (isValidPosition(tempX+tempVX, tempY+tempVY) && lines[tempY+tempVY][tempX+tempVX] == '#') {
                if (turningPoints.Contains(((tempX, tempY), (tempVX, tempVY)))) {
                    lines[by][bx] = '.';
                    turningPoints.RemoveRange(count, turningPoints.Count-count);
                    Debug.Assert(turningPoints.Count == count);
                    return false;
                } else {
                    turningPoints.Add(((tempX, tempY), (tempVX, tempVY)));
                    (tempVX, tempVY) = turnRight(tempVX, tempVY);
                }
            }
            tempX += tempVX;
            tempY += tempVY;
        }
        turningPoints.RemoveRange(count, turningPoints.Count-count);
        Debug.Assert(turningPoints.Count == count);
        lines[by][bx] = '.';
        return true;
    };

    int blockCount = 0;
    do {
        while (isValidPosition(x+vx, y+vy) && lines[y+vy][x+vx] == '#') {
            turningPoints.Add(((x, y), (vx, vy)));
            (vx, vy) = turnRight(vx, vy);
            if (turningPoints.Count >= 3) {
                for (int i = 0; i < turningPoints.Count - 2; i++) {
                    ((int px, int py), var _) = turningPoints[i];
                    (int newBx, int newBy) = (vx, vy) switch {
                        (0, -1) => (x, py-1),
                        (1, 0) => (px+1, y),
                        (0, 1) => (x, py+1),
                        (-1, 0) => (px-1, y),
                        _ => (-1, -1),
                    };
                    bool isBlocked = false;
                    (int tempX, int tempY) = (x+vx, y+vy);
                    while (isValidPosition(tempX, tempY) && (tempX != newBx || tempY != newBy)) {
                        if (lines[tempY][tempX] == '#') {
                            isBlocked = true;
                            break;
                        }
                        tempX += vx;
                        tempY += vy;
                    }
                    if (isValidPosition(newBx, newBy) && !isBlocked && lines[newBy][newBx] == '.' && !canExit(newBx, newBy)) {
                        lines[newBy][newBx] = 'O';
                        blockCount++;
                    }
                }
            }
        }
        x += vx;
        y += vy;
    } while(isValidPosition(x, y));
    Console.WriteLine($"Part 2: {blockCount}");
}

part2("input.txt");
