using _23_UnstableDiffusion;

var input = File.ReadAllText("input.txt");
var numFreePos = Diffusion.GetNumFreePosAfter(input, 10);
Console.WriteLine("Part 1: " + numFreePos);
