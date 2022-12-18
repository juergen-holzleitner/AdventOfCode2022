using _18_BoilingBoulders;

var input = File.ReadAllText("input.txt");
var totalSurface = Cube.GetTotalSurface(input);
Console.WriteLine("Part 1: total surface: " + totalSurface);

totalSurface = Cube.GetTotalSurfacePart2(input);
Console.WriteLine("Part 2: total outer surface: " + totalSurface);
