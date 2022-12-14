using _14_RegolithReservoir;

var lines = File.ReadAllText("input.txt");
var numSands = RegolithReservoir.GetNumSandsAdded(lines, false);
Console.WriteLine($"Part 1: num sand added: {numSands}");

numSands = RegolithReservoir.GetNumSandsAdded(lines, true);
Console.WriteLine($"Part 2: num sand added: {numSands}");
