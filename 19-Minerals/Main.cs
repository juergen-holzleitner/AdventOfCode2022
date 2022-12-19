using _19_Minerals;

var input = File.ReadAllText("input.txt");
var qualityLevel = Factory.GetQualityLevel(input);
Console.WriteLine("Part 1: " + qualityLevel);
