using _18_BoilingBoulders;

var input = File.ReadAllText("input.txt");
var totalSurface = Cube.GetTotalSurface(input);
Console.WriteLine("Part 2: totalSurface: " + totalSurface);
