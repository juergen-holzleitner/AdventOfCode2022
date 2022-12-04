using _03_Rucksack;

var sumOfPriorities = Rucksack.GetSumOfPriorities(File.ReadLines("input.txt"));
Console.WriteLine($"Part 1: sum of priorities is {sumOfPriorities}");

var sumOfGroup = Rucksack.GetSumOfGroupItems(File.ReadLines("input.txt"));
Console.WriteLine($"Part 2: sum of group is {sumOfGroup}");
