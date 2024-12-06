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

    Queue<(int, int)> blocks = new();
    int blockCount = 0;
    do {
        if (isValidPosition(x+vx, y+vy) && lines[y+vy][x+vx] == '#') {
            blocks.Enqueue((x+vx, y+vy));
            (vx, vy) = (vx, vy) switch {
                (0, -1) => (1, 0),
                (1, 0) => (0, 1),
                (0, 1) => (-1, 0),
                (-1, 0) => (0, -1),
                _ => (0, 0)
            };




            if (blocks.Count == 3) {
                (int bx, int by) = blocks.Dequeue();
                (int newBx, int newBy) = (vx, vy) switch {
                    (0, -1) => (x, by-1),
                    (1, 0) => (bx+1, y),
                    (0, 1) => (x, by+1),
                    (-1, 0) => (bx-1, y),
                    _ => (-1, -1),
                };
                Console.WriteLine($"Old {(bx, by)}, New {(newBx, newBy)}");
                if (isValidPosition(newBx, newBy) && lines[newBy][newBx] != 'O') {
                    (int tempX, int tempY) = (x+vx, y+vy);
                    bool isBlocked = false;
                    while (isValidPosition(tempX, tempY) && (tempX != newBx || tempY != newBy)) {
                        if (lines[tempY][tempX] == '#') {
                            isBlocked = true;
                            break;
                        }
                        tempX += vx;
                        tempY += vy;
                    }
                    if (!isBlocked) {
                        blockCount++;
                        lines[newBy][newBx] = 'O';
                    }
                }
            }
        }
        x += vx;
        y += vy;
    } while(isValidPosition(x, y));
    Console.WriteLine($"{String.Join('\n', lines.Select(line => new String(line)).ToArray())}");
    Console.WriteLine($"Part 2: {blockCount}");
}

part2("sample.txt");
