﻿using _12_HillClimbing;

var input = HillClimbing.Parse(File.ReadAllText("input.txt"));
var steps = HillClimbing.GetMinStepsFromStartToEnd(input);
Console.WriteLine($"Part 1: {steps}");
