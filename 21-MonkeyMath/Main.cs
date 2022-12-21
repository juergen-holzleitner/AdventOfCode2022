using _21_MonkeyMath;

var input = File.ReadAllText("input.txt");
var result = MonkeyMath.GetResultOf("root", input);
Console.WriteLine("Part 1: " + result);

var humanValue = MonkeyMath.GetHumanValue("root", "humn", input);
Console.WriteLine("Part 2: " + humanValue);
