using _04_CampCleanup;

var input = File.ReadLines("input.txt");
var numFullyOverlapping = CampCleanup.CountFullyOverlapping(input);
Console.WriteLine($"Part 1: number fully overlapping is {numFullyOverlapping}");
