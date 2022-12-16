using _16_ProboscideaVolcanium;

var input = File.ReadAllText("input.txt");
var maxTotalPressure = Valve.GetMaxTotalPressure(input);
Console.WriteLine("Part 1: maxTotalPressure: " + maxTotalPressure);
