using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
void part1(string filePath) {
    Dictionary<(int, int), bool> rules = new();
    Func<int[], bool> checkUpdates = (int[] updates) => {
        for (int i = 0; i < updates.Length-1; i++) {
            for (int j = i+1; j < updates.Length; j++) {
                if (rules.ContainsKey((updates[i], updates[j]))) {
                    return false;
                }
            }
        }
        return true;
    };
    int result = 0;
    foreach (string line in File.ReadLines(filePath)) {
        if (line == "") continue;
        if (Regex.IsMatch(line, @"\d+\|\d+")) {
            int[] nums = line.Split('|').Select(x => int.Parse(x)).ToArray();
            rules[(nums[1], nums[0])] = false;
        } else {
            int[] updates = line.Split(',').Select(token => int.Parse(token)).ToArray();
            if (checkUpdates(updates)) {
                result += updates[updates.Length/2];
            }
        }
    }
    Console.WriteLine($"Part 1: {result}");

}

void part2(string filePath) {
    Dictionary<(int, int), bool> rules = new();
    Func<int[], bool> checkUpdates = (int[] updates) => {
        for (int i = 0; i < updates.Length-1; i++) {
            for (int j = i+1; j < updates.Length; j++) {
                if (rules.ContainsKey((updates[i], updates[j]))) {
                    return false;
                }
            }
        }
        return true;
    };
    int result = 0;
    foreach (string line in File.ReadLines(filePath)) {
        if (line == "") continue;
        if (Regex.IsMatch(line, @"\d+\|\d+")) {
            int[] nums = line.Split('|').Select(x => int.Parse(x)).ToArray();
            rules[(nums[1], nums[0])] = false;
        } else {
            int[] updates = line.Split(',').Select(token => int.Parse(token)).ToArray();
            if (!checkUpdates(updates)) {
                Array.Sort(updates, (int a, int b) => rules.ContainsKey((a, b)) ? 1 : a == b ? 0 : -1);
                Debug.Assert(checkUpdates(updates));
                result += updates[updates.Length/2];
            }
        }
    }
    Console.WriteLine($"Part 1: {result}");
}

part1("input.txt");
part2("input.txt");
