using _17_PyroclasticFlow;

var input = File.ReadAllText("input.txt");
var height = Chamber.GetHeightAfterElements(input, 2022);
Console.WriteLine("Part 1: height: " + height);
