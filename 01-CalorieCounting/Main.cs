using _01_CalorieCounting;

var blocks = File.ReadAllText("input.txt");
var max = CalorieCounting.GetMaxSumOfBlock(blocks);
Console.WriteLine($"1. max: {max}");

var sumOfTopThree = CalorieCounting.GetSumOfTopThree(blocks);
Console.WriteLine($"2. sum of top three: {sumOfTopThree}");
