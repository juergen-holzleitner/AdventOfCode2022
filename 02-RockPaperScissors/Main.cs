using _02_RockPaperScissors;

var input = File.ReadLines("input.txt");
var score = RockPaperScissors.GetTotalScore(input);
Console.WriteLine($"part 1: total score {score}");

input = File.ReadLines("input.txt");
score = RockPaperScissors.GetTotalInputScore(input);
Console.WriteLine($"part 2: total score {score}");
