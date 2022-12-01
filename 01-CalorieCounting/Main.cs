using _01_CalorieCounting;

var blocks = File.ReadAllText("input.txt");
var max = CalorieCounting.GetMaxSumOfBlock(blocks);
Console.WriteLine($"max: {max}");
