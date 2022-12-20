using _20_GrovePositioningSystem;

var input = File.ReadAllText("input.txt");
var groveCoordinate = Grove.GetGroveCoordinate(input, 1, 1);
Console.WriteLine("Part 1: " + groveCoordinate);

groveCoordinate = Grove.GetGroveCoordinate(input, 811589153, 10);
Console.WriteLine("Part 2: " + groveCoordinate);
