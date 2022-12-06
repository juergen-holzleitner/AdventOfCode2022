using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _05_SupplyStacks
{
  internal class SupplyStack
  {
    internal static char ParseCrate(string crate)
    {
      return crate[1];
    }

    internal static IEnumerable<char> ParseLine(string input)
    {
      foreach (var crate in input.Split(' '))
        yield return ParseCrate(crate);
    }

    internal static bool IsSeparatorLine(string line)
    {
      return line.StartsWith(" 1 ");
    }

    internal static IEnumerable<string> GetLinesUntilSeparator(string input)
    {
      foreach (var line in input.Split('\n'))
      {
        if (IsSeparatorLine(line)) 
          yield break;
        yield return line.Trim('\r');
      }
    }

    internal static Stack<char>[] ParseInitialStack(string input)
    {
      var stacks = new Stack<char>[9];
      for (int n = 0; n < stacks.Length; ++n)
      {
        stacks[n] = new Stack<char>();
      }

      var lines = GetLinesUntilSeparator(input);

      foreach (var line in lines.Reverse())
      {
        for (int n = 0; n < stacks.Length; ++n)
        {
          var supply = GetSupplyAt(line, n);
          if (supply.HasValue)
          {
            stacks[n].Push(supply.Value);
          }
        }
      }

      return stacks;
    }

    internal static char? GetSupplyAt(string line, int position)
    {
      var charPos = position * 4 + 1;
      if (charPos - 1 < 0 || charPos + 1 >= line.Length) 
        return null;
      if (line[charPos - 1] != '[')
        return null;
      if (line[charPos + 1] != ']')
        return null;

      return line[charPos];
    }

    internal record struct Instruction(int Num, int From, int To);

    internal static Instruction ParseInstruction(string instruction)
    {
      var r = new Regex(@"move (?<Num>\d+) from (?<From>\d+) to (?<To>\d+)");
      var match = r.Match(instruction);
      if (match.Success)
      {
        var num = int.Parse(match.Groups["Num"].Value);
        var from = int.Parse(match.Groups["From"].Value);
        var to = int.Parse(match.Groups["To"].Value);

        return new Instruction(num, from, to);
      }
      throw new ApplicationException("Can't parse instruction " + instruction);
    }

    internal static Stack<char>[] ProcessInput(string input)
    {
      var stacks = ParseInitialStack(input);

      bool separatorFound = false;
      foreach (var line in input.Split('\n'))
      {
        if (!separatorFound)
        {
          if (!IsSeparatorLine(line))
            continue;

          separatorFound = true;
          continue;
        }

        if (string.IsNullOrWhiteSpace(line))
          continue;

        var instruction = ParseInstruction(line);
        for (int n = 0; n < instruction.Num; ++n)
        {
          stacks[instruction.To - 1].Push(stacks[instruction.From - 1].Pop());
        }
      }

      return stacks;
    }

    internal static string GetFinalResult(string input)
    {
      var stacks = ProcessInput(input);
      var sb = new StringBuilder();
      for (int n = 0; n < stacks.Length; ++n)
      {
        if (stacks[n].TryPop(out char supply))
          sb.Append(supply);
      }
      return sb.ToString();
    }
  }
}
