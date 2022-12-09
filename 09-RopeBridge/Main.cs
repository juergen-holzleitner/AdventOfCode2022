using _09_RopeBridge;

var input = File.ReadLines("input.txt");
var sut = new RopeBridge();

sut.MoveInput(input);

var visitedTailPositions = sut.GetVisitedTailPositions();

Console.WriteLine($"Part 1: {visitedTailPositions.Count()}");
