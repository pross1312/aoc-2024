using System.IO;
using System.Diagnostics;

bool check(ulong testValue, List<ulong> numbers) {
    // Console.WriteLine($"{String.Join(", ", numbers)}");
    Debug.Assert(numbers.Count != 0);
    if (numbers.Count == 1) return numbers[numbers.Count-1] == testValue;
    if (numbers[numbers.Count-1] >= testValue) return false;

    var (a, b) = (numbers[numbers.Count-2], numbers[numbers.Count-1]);
    numbers.RemoveRange(numbers.Count-2, 2);
    numbers.Add(a+b);
    if (check(testValue, numbers)) return true;
    numbers.RemoveAt(numbers.Count-1);
    numbers.Add(a*b);
    if (check(testValue, numbers)) return true;
    numbers.RemoveAt(numbers.Count-1);
    numbers.Add(a);
    numbers.Add(b);
    return false;
}

void part1(string filePath) {
    ulong result = 0;
    foreach (string line in File.ReadLines(filePath)) {
        string[] tokens = line.Split(":", 2);
        ulong testValue = ulong.Parse(tokens[0]);
        List<ulong> testNumbers = tokens[1].Trim().Split(" ").Select(x => ulong.Parse(x)).Reverse().ToList();
        Console.WriteLine($"{testValue}: {String.Join(", ", testNumbers)} - {check(testValue, testNumbers)}");
        if (check(testValue, testNumbers)) result += testValue;
    }
    Console.WriteLine($"Part 1: {result}");
}

part1("sample.txt");
