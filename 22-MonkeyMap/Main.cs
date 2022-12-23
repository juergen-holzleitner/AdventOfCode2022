using _22_MonkeyMap;

var input = File.ReadAllText("input.txt");
var score = Map.GetFinalScore(input, 50, false);
Console.WriteLine("Part 1: " + score);

score = Map.GetFinalScore(input, 50, true);
Console.WriteLine("Part 2: " + score);
