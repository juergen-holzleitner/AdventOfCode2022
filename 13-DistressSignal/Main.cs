using _13_DistressSignal;

var lines = File.ReadLines("input.txt");
var sumOfIndices = Signal.GetSumOfIndices(lines);
Console.WriteLine($"part 1: sum of indices: {sumOfIndices}");

var decoderKey = Signal.GetDecoderKey(lines);
Console.WriteLine($"part 2: decoder key: {decoderKey}");
