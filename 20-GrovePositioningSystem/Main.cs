using _20_GrovePositioningSystem;

var input = File.ReadAllText("input.txt");
var groveCoordinate = Grove.GetGroveCoordinate(input);
Console.WriteLine("Part 1: " + groveCoordinate);
