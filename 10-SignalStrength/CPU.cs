using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10_SignalStrength
{
  internal class CPU
  {
    private int cycle = 0;
    private int registerX = 1;
    private int signalStrength = 0;

    private readonly int[] signalCycles = {20, 60, 100, 140, 180, 220 };

    private readonly StringBuilder image = new();

    internal void Execute(string instruction)
    {
      if (string.IsNullOrEmpty(instruction)) 
        return;

      const string addInstruction = "addx ";
      if (instruction.Trim() == "noop")
      {
        ++cycle;
        ProcessSignalStrength(cycle);
      }
      else if (instruction.Trim().StartsWith(addInstruction))
      {
        ProcessSignalStrength(cycle + 1);
        cycle += 2;
        ProcessSignalStrength(cycle);

        registerX += int.Parse(instruction[addInstruction.Length..]);
      }
      else
      {
        throw new ApplicationException($"invalid instruction {instruction}");
      }

    }

    private void ProcessSignalStrength(int cycle)
    {
      foreach (var x in signalCycles)
      {
        if (cycle == x)
        {
          signalStrength += x * registerX;
        }
      }

      if (cycle <= 240)
      {
        if (Math.Abs(((cycle -1 ) % 40) - registerX) <= 1)
          image.Append('#');
        else
          image.Append('.');

        if (cycle % 40 == 0)
          image.Append(Environment.NewLine);
      }
    }

    internal int GetCycle()
    {
      return cycle;
    }

    internal int GetRegisterX()
    {
      return registerX;
    }

    internal int GetSignalStrength()
    {
      return signalStrength;
    }

    internal void ProcessInput(string input)
    {
      foreach (var line in input.Split('\n'))
        Execute(line.Trim());
    }

    internal string GetImage()
    {
      return image.ToString();
    }
  }
}
