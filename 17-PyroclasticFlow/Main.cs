using _17_PyroclasticFlow;

var input = File.ReadAllText("input.txt");
var height = Chamber.GetHeightAfterElements(input, 2022);
Console.WriteLine("Part 1: height: " + height);

height = Chamber.GetHeightAfterElementsPart2(input, 1000000000000);
Console.WriteLine("Part 2: height: " + height);
