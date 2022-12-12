using _11_Monkey;

var monkeysPart1 = MonkeyStuff.Parse(File.ReadAllText("input.txt"));
var monkeyBusiness = MonkeyStuff.GetMonkeyBusiness(monkeysPart1, 20);
Console.WriteLine($"Part 1: monkey business: {monkeyBusiness}");

var monkeysPart2 = MonkeyStuff.Parse(File.ReadAllText("input.txt"));
monkeyBusiness = MonkeyStuff.GetMonkeyBusinessWithoutDiv(monkeysPart2, 10000);
Console.WriteLine($"Part 2: monkey business: {monkeyBusiness}");
