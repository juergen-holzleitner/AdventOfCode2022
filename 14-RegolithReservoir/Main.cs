using _14_RegolithReservoir;

var lines = File.ReadAllText("input.txt");
var numSands = RegolithReservoir.GetNumSandsAdded(lines);
Console.WriteLine($"Part 1: num sand added: {numSands}");
