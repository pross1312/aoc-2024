package main;
import (
    "fmt"
    "math"
    "os"
    "io/ioutil"
    "slices"
    "strings"
    "strconv"
)

func assert(err error) {
    if err != nil {
        fmt.Println(err);
        os.Exit(1);
    }
}

func str_array_to_int(str_array []string) []int {
    result := make([]int, 0, len(str_array));
    for _, str := range str_array {
        val, err := strconv.Atoi(strings.TrimSpace(str));
        assert(err);
        result = append(result, val);
    }
    return result;
}

func gap_int(a, b int) int {
    return int(math.Abs(float64(a - b)));
}

func is_safe(arr []int) bool {
    is_sorted := slices.IsSorted(arr);
    slices.Reverse(arr);
    is_sorted = is_sorted || slices.IsSorted(arr);
    slices.Reverse(arr);
    if !is_sorted { return false; }
    for i := 1; i < len(arr); i++ {
        gap := int(math.Abs(float64(arr[i] - arr[i-1])));
        if gap == 0 || gap > 3 {
            return false;
        }
    }
    return true;
}

func part1(file_path string) {
    data, err := ioutil.ReadFile(file_path);
    assert(err);
    result := 0;
    for _, line := range strings.Split(string(data), "\n") {
        if line == "" { continue; }
        levels := str_array_to_int(strings.Split(line, " "));
        if is_safe(levels) { result++; }
    }
    fmt.Println("Part 1:", result);
}

func part2(file_path string) {
    data, err := ioutil.ReadFile(file_path);
    assert(err);
    result := 0;
    for _, line := range strings.Split(string(data), "\n") {
        if line == "" { continue; }
        levels := str_array_to_int(strings.Split(line, " "));
        if is_safe(levels) {
            result++;
        } else {
            for i, _ := range levels {
                new_levels := slices.Delete(slices.Clone(levels), i, i+1);
                if is_safe(new_levels) { result++; break; }
            }
        }
    }
    fmt.Println("Part 1:", result);
}

func main() {
    const INPUT_PATH = "input.txt";
    part1(INPUT_PATH);
    part2(INPUT_PATH);
}
