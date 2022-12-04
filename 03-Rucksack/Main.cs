using _03_Rucksack;

var inputs = File.ReadLines("input.txt");
var sumOfPriorities = Rucksack.GetSumOfPriorities(inputs);
Console.WriteLine($"Part 1: sum of priorities is {sumOfPriorities}");
