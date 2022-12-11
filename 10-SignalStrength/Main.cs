using _10_SignalStrength;

var sut = new CPU();

sut.ProcessInput(File.ReadAllText("input.txt"));

var signalStrength = sut.GetSignalStrength();
Console.WriteLine($"Part 1: signalStrength: {signalStrength}");

var image = sut.GetImage();
Console.WriteLine("Part 2:");
Console.Write(image);
