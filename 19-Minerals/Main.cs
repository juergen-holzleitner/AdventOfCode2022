using _19_Minerals;

var input = File.ReadAllText("input.txt");
var qualityLevel = Factory.GetQualityLevel(input);
Console.WriteLine("Part 1: " + qualityLevel);

var partTwo = Factory.GetPartTwo(input);
Console.WriteLine("Part 2: " + partTwo);
