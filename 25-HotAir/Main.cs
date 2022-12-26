using _25_HotAir;

var text = File.ReadAllText("input.txt");
var sum = Snafu.GetSumOfAsSnafu(text);
Console.WriteLine("Part 1: " + sum);
