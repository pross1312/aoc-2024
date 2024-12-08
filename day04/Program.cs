using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

// NOTE: this is for evil math iteration, which is not a smart answer
// int countOccurence(ReadOnlySpan<char> sequence) {
//     int count = 0;
//     string pattern = "XMAS";
//     for (int i = 0; i < sequence.Length - 3; i++) {
//         bool equal = true;
//         for (int j = 0; j < 4; j++) {
//             if (sequence[i+j] != pattern[j]) {
//                 equal = false;
//                 break;
//             }
//         }
//         if (equal) { count++; continue; }
//         equal = true;
//         for (int j = 0; j < 4; j++) {
//             if (sequence[i+j] != pattern[3-j]) {
//                 equal = false;
//                 break;
//             }
//         }
//         if (equal) { count++; continue; }
//     }
//     return count;
// }

int countXmas(string[] lines, int row, int col) {
    int result = 0;
    if (col < lines[0].Length-3) {
        if ((lines[row][col] == 'X' && lines[row][col+1] == 'M' && lines[row][col+2] == 'A' && lines[row][col+3] == 'S') ||
           (lines[row][col] == 'S' && lines[row][col+1] == 'A' && lines[row][col+2] == 'M' && lines[row][col+3] == 'X')) result++;
    }

    if (row < lines.Length-3) {
        if ((lines[row][col] == 'X' && lines[row+1][col] == 'M' && lines[row+2][col] == 'A' && lines[row+3][col] == 'S') ||
           (lines[row][col] == 'S' && lines[row+1][col] == 'A' && lines[row+2][col] == 'M' && lines[row+3][col] == 'X')) result++;
    }

    if (row < lines.Length-3 && col < lines[0].Length-3) {
        if ((lines[row][col] == 'X' && lines[row+1][col+1] == 'M' && lines[row+2][col+2] == 'A' && lines[row+3][col+3] == 'S') ||
           (lines[row][col] == 'S' && lines[row+1][col+1] == 'A' && lines[row+2][col+2] == 'M' && lines[row+3][col+3] == 'X')) result++;
    }

    if (row < lines.Length-3 && col >= 3) {
        if ((lines[row][col] == 'X' && lines[row+1][col-1] == 'M' && lines[row+2][col-2] == 'A' && lines[row+3][col-3] == 'S') ||
           (lines[row][col] == 'S' && lines[row+1][col-1] == 'A' && lines[row+2][col-2] == 'M' && lines[row+3][col-3] == 'X')) result++;
    }
    return result;
}

void part1(string filePath) {
    string[] lines = File.ReadAllLines(filePath);
    Debug.Assert(lines.Length == lines[0].Length);
    int result = 0;

    // NOTE: Use some evil math to iterate diagonally
    // foreach (string line in lines) {
    //     result += countOccurence(line);
    // }
    // Span<char> verticalSequence = stackalloc char[lines.Length];
    // for (int i = 0; i < lines[0].Length; i++) {
    //     for (int j = 0; j < lines.Length; j++) {
    //         verticalSequence[j] = lines[j][i];
    //     }
    //     result += countOccurence(verticalSequence);
    // }
    // Span<char> diagonalSequence = stackalloc char[lines.Length + lines[0].Length - 1];
    // Span<char> diagonalSequence2 = stackalloc char[lines.Length + lines[0].Length - 1];

    // for (int col = 0; col < lines[0].Length; col++) {
    //     diagonalSequence.Clear();
    //     diagonalSequence2.Clear();
    //     for (int i = 0; i < lines[0].Length-col; i++) {
    //         diagonalSequence[i] = lines[i][i+col];
    //         if (col != 0) diagonalSequence2[i] = lines[i+col][i];
    //     }
    //     result += countOccurence(diagonalSequence);
    //     if (col != 0) result += countOccurence(diagonalSequence2);
    // }
    // for (int row = 0; row < lines.Length; row++) {
    //     diagonalSequence.Clear();
    //     diagonalSequence2.Clear();
    //     for (int i = 0; i < row+1; i++) {
    //         diagonalSequence[i] = lines[row-i][i];
    //         if (row != lines.Length-1) diagonalSequence2[i] = lines[lines.Length-1 - i][lines[0].Length-1 - (row - i)];
    //     }
    //     result += countOccurence(diagonalSequence);
    //     if (row != lines.Length-1) result += countOccurence(diagonalSequence2);
    // }

    for (int row = 0; row < lines.Length; row++) {
        for (int col = 0; col < lines[0].Length; col++) {
            result += countXmas(lines: lines, row: row, col: col);
        }
    }
    Console.WriteLine($"{result}");
}

// NOTE: this function does not validate position
bool isX_mas(string[] lines, int row, int col) {
    Func<bool> checkDiag = () => (lines[row-1][col-1] == 'M' && lines[row+1][col+1] == 'S') || (lines[row-1][col-1] == 'S' && lines[row+1][col+1] == 'M');
    var checkSubDiag = () => (lines[row-1][col+1] == 'M' && lines[row+1][col-1] == 'S') || (lines[row-1][col+1] == 'S' && lines[row+1][col-1] == 'M');
    return lines[row][col] == 'A' && checkDiag() && checkSubDiag();
}

void part2(string filePath) {
    string[] lines = File.ReadAllLines(filePath);
    int result = 0;
    for (int row = 1; row < lines.Length-1; row++) {
        for (int col = 1; col < lines[0].Length-1; col++) {
            if (isX_mas(lines, row, col)) result++;
        }
    }
    Console.WriteLine($"{result}");
}

part1("input.txt");
part2("input.txt");
