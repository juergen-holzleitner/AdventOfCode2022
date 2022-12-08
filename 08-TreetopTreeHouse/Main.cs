using _08_TreetopTreeHouse;

var input = File.ReadAllText("input.txt");
var data = TreeHouse.Parse(input);

var numVisible = TreeHouse.GetNumVisible(data);

Console.WriteLine($"Part 1: {numVisible} trees are visible");
