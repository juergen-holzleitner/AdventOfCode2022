using _07_NoSpaceLeft;

var sut = new Device();

foreach (var line in System.IO.File.ReadLines("input.txt"))
{
  sut.ProcessLine(line.Trim('\r'));
}

var size = sut.GetSumOfFoldersAtMost(100000);
Console.WriteLine($"Part 1: {size}");

var totalSize = sut.GetRootFolderSize();
var unused = 70000000 - totalSize;
var sizeRequired = 30000000 - unused;

var smallestSize = sut.GetSmallestDirectoryWithAtLeast(sizeRequired);
Console.WriteLine($"Part 2: {smallestSize}");
