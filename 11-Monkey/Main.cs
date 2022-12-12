using _11_Monkey;

var monkeys = MonkeyStuff.Parse(File.ReadAllText("input.txt"));
var monkeyBusiness = MonkeyStuff.GetMonkeyBusiness(monkeys, 20);
Console.WriteLine($"Part 1: monkey business: {monkeyBusiness}");
