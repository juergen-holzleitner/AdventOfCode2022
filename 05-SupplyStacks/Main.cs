using _05_SupplyStacks;

var input = File.ReadAllText("input.txt");
var result = SupplyStack.GetFinalResult(input);
Console.WriteLine($"Part 1: {result}");

result = SupplyStack.GetFinalResultPart2(input);
Console.WriteLine($"Part 2: {result}");
