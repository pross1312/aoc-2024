package main
import (
	"fmt"
	"math"
	"io/ioutil"
	"os"
	"strings"
	"strconv"
	"sort"
)

func part1(file_path string) {
	data, err := ioutil.ReadFile(file_path);
	if err != nil {
		os.Exit(1);
	}
	content := string(data);
	lines := strings.SplitAfter(content, "\n");
	list1 := make([]int, 0, 10)
	list2 := make([]int, 0, 10)
	for _, line := range lines {
		if line == "" {
			break;
		}
		tokens := strings.SplitN(line, " ", 2)
		i, err := strconv.Atoi(strings.TrimSpace(tokens[0]))
		if err == nil {
			list1 = append(list1, i)
		}
		i, err = strconv.Atoi(strings.TrimSpace(tokens[1]))
		if err == nil {
			list2 = append(list2, i)
		}
	}
	sort.Slice(list1, func(i, j int) bool {
		return list1[i] < list1[j]
	});
	sort.Slice(list2, func(i, j int) bool {
		return list2[i] < list2[j]
	});
	sum := 0;
	for i := 0; i < len(list1); i++ {
		sum += int(math.Abs(float64(list1[i] - list2[i])));
	}
	fmt.Println("Part 1:", sum);
}

func part2(file_path string) {
	data, err := ioutil.ReadFile(file_path);
	if err != nil {
		os.Exit(1);
	}
	content := string(data);
	lines := strings.SplitAfter(content, "\n");
	list := make([]int, 0, 100);
	hashmap := make(map[int]int);
	for _, line := range lines {
		if line == "" {
			break;
		}
		tokens := strings.SplitN(line, " ", 2)
		i, err := strconv.Atoi(strings.TrimSpace(tokens[0]))
		if err != nil {
			fmt.Println(err);
			os.Exit(1);
		} else {
			list = append(list, i);
			if _, ok := hashmap[i]; !ok {
				hashmap[i] = 0;
			}
		}
		i, err = strconv.Atoi(strings.TrimSpace(tokens[1]))
		if err != nil {
			fmt.Println(err);
			os.Exit(1);
		} else {
			if _, ok := hashmap[i]; ok {
				hashmap[i] = hashmap[i] + 1;
			} else {
				hashmap[i] = 1;
			}
		}
	}
	sum := 0;
	for _, x := range list {
		sum += x * hashmap[x];
	}
	fmt.Println("Part 2:", sum);
}

func main() {
	const FILE_PATH = "input.txt";
	part1(FILE_PATH);
	part2(FILE_PATH);
}
