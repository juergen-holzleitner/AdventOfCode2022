using _06_TuningTrouble;

var input = File.ReadAllText("input.txt");
var startPos = Device.GetStartPos(input);
Console.WriteLine($"Part 1: {startPos}");
