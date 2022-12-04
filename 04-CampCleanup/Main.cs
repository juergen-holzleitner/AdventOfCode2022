using _04_CampCleanup;

var numFullyOverlapping = CampCleanup.CountFullyOverlapping(File.ReadLines("input.txt"));
Console.WriteLine($"Part 1: number fully overlapping is {numFullyOverlapping}");

var numOverlapping = CampCleanup.CountOverlapping(File.ReadLines("input.txt"));
Console.WriteLine($"Part 2: number overlapping is {numOverlapping}");
