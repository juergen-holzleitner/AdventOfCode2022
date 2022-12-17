using _16_ProboscideaVolcanium;

var input = File.ReadAllText("input.txt");
var maxTotalPressure = Valve.GetMaxTotalPressure(input, false);
Console.WriteLine("Part 1: maxTotalPressure: " + maxTotalPressure);

maxTotalPressure = Valve.GetMaxTotalPressure(input, true);
Console.WriteLine("Part 2: maxTotalPressure with elephant: " + maxTotalPressure);
