using _24_BlizzardBasin;

var text = File.ReadAllText("input.txt");
var numSteps = Blizzard.GetNumStepsToExit(text);
Console.WriteLine("Part 1: " + numSteps);
