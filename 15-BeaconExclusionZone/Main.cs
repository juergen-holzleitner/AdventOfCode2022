using _15_BeaconExclusionZone;

var lines = File.ReadAllText("input.txt");

var notPositions = Zone.GetNotPositions(lines, 2000000);
Console.WriteLine($"Part 1: {notPositions}");
