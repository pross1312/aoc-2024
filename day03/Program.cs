using System.IO;
using System.Text.RegularExpressions;

void part1(string input_path) {
    string content = File.ReadAllText(input_path);
    string pattern = @"mul\(\d+\,\d+\)";
    int result = 0;
    foreach (ValueMatch match in Regex.EnumerateMatches(content, pattern)) {
        int tmp = 1;
        ReadOnlySpan<char> expression = content.AsSpan(match.Index, match.Length);
        foreach (ValueMatch val in Regex.EnumerateMatches(expression, @"\d+")) {
            tmp *= int.Parse(expression.Slice(val.Index, val.Length));
        }
        result += tmp;
    }
    Console.WriteLine(result);
}

void part2(string input_path) {
    string content = File.ReadAllText(input_path);
    string pattern = @"(mul\(\d+\,\d+\)|do\(\)|don't\(\))";
    bool is_enabled = true;
    int result = 0;
    foreach (ValueMatch match in Regex.EnumerateMatches(content, pattern)) {
        ReadOnlySpan<char> expression = content.AsSpan(match.Index, match.Length);
        if (expression.Equals("don't()".AsSpan(), StringComparison.Ordinal)) is_enabled = false;
        if (expression.Equals("do()".AsSpan(), StringComparison.Ordinal)) is_enabled = true;
        else if (is_enabled) {
            int tmp = 1;
            foreach (ValueMatch val in Regex.EnumerateMatches(expression, @"\d+")) {
                tmp *= int.Parse(expression.Slice(val.Index, val.Length));
            }
            result += tmp;
        }
    }
    Console.WriteLine($"{result}");
}

part1("input.txt");
part2("input.txt");
