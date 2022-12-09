using _09_RopeBridge;

var ropeWithTwo = new RopeBridge(2);
ropeWithTwo.MoveInput(File.ReadLines("input.txt"));
var visitedTailPositions = ropeWithTwo.GetVisitedTailPositions();
Console.WriteLine($"Part 1: {visitedTailPositions.Count()}");

var ropeWithTen = new RopeBridge(10);
ropeWithTen.MoveInput(File.ReadLines("input.txt"));
visitedTailPositions = ropeWithTen.GetVisitedTailPositions();
Console.WriteLine($"Part 2: {visitedTailPositions.Count()}");
