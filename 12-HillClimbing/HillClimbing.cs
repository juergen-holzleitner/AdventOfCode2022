namespace _12_HillClimbing
{
  internal record struct Pos(int Row, int Column);

  internal record Input(List<List<int>> Highmap, Pos Start, Pos End);

  internal class HillClimbing
  {
    internal static int GetMinStepsFromAnyStartToEnd(Input input)
    {
      int? bestMinSteps = null;

      for (int row = 0; row < input.Highmap.Count; ++row)
        for (int col = 0; col < input.Highmap[row].Count; ++col)
        {
          if (input.Highmap[row][col] == 0)
          {
            var newInput = input with { Start = new Pos(row, col) };
            var minSteps = GetMinStepsFromStartToEnd(newInput);
            if (minSteps.HasValue)
            {
              if (!bestMinSteps.HasValue || bestMinSteps!.Value > minSteps.Value)
                bestMinSteps = minSteps.Value;
            }
          }
        }

      return bestMinSteps!.Value;
    }

    internal static int? GetMinStepsFromStartToEnd(Input input)
    {
      var minReachable = new List<List<int?>>();
      for (int row = 0; row < input.Highmap.Count; ++row)
      {
        var rowValues = new List<int?>();
        for (int col = 0; col < input.Highmap[row].Count; ++col)
        {
          rowValues.Add(null);
        }
        minReachable.Add(rowValues);
      }

      minReachable[input.Start.Row][input.Start.Column] = 0;
      var itemsToProcess = new Queue<Pos>();
      itemsToProcess.Enqueue(input.Start);

      while (itemsToProcess.Any())
      {
        var item = itemsToProcess.Dequeue();

        var nextHigh = input.Highmap[item.Row][item.Column] + 1;
        var nextReachable = minReachable[item.Row][item.Column]!.Value + 1;

        if (item.Row > 0 && input.Highmap[item.Row - 1][item.Column] <= nextHigh)
        {
          if (!minReachable[item.Row - 1][item.Column].HasValue || minReachable[item.Row - 1][item.Column]!.Value > nextReachable)
          {
            minReachable[item.Row - 1][item.Column] = nextReachable;
            itemsToProcess.Enqueue(new Pos(item.Row - 1, item.Column));
          }
        }

        if (item.Row < input.Highmap.Count - 1 && input.Highmap[item.Row + 1][item.Column] <= nextHigh)
        {
          if (!minReachable[item.Row + 1][item.Column].HasValue || minReachable[item.Row + 1][item.Column]!.Value > nextReachable)
          {
            minReachable[item.Row + 1][item.Column] = nextReachable;
            itemsToProcess.Enqueue(new Pos(item.Row + 1, item.Column));
          }
        }

        if (item.Column > 0 && input.Highmap[item.Row][item.Column - 1] <= nextHigh)
        {
          if (!minReachable[item.Row][item.Column - 1].HasValue || minReachable[item.Row][item.Column - 1]!.Value > nextReachable)
          {
            minReachable[item.Row][item.Column - 1] = nextReachable;
            itemsToProcess.Enqueue(new Pos(item.Row, item.Column - 1));
          }
        }

        if (item.Column < input.Highmap[item.Row].Count - 1 && input.Highmap[item.Row][item.Column + 1] <= nextHigh)
        {
          if (!minReachable[item.Row][item.Column + 1].HasValue || minReachable[item.Row][item.Column + 1]!.Value > nextReachable)
          {
            minReachable[item.Row][item.Column + 1] = nextReachable;
            itemsToProcess.Enqueue(new Pos(item.Row, item.Column + 1));
          }
        }

      }

      return minReachable[input.End.Row][input.End.Column];
    }

    internal static Input Parse(string inputString)
    {
      var highMap = new List<List<int>>();
      var start = new Pos(-1, -1);
      var end = new Pos(-1, -1);

      foreach (var line in inputString.Split('\n'))
        if (!string.IsNullOrWhiteSpace(line))
        {
          var row = new List<int>();
          foreach (var ch in line.Trim())
          {
            if (ch == 'S')
            {
              start = new Pos(highMap.Count, row.Count);
              row.Add('a' - 'a');
            }
            else if (ch == 'E')
            {
              end = new Pos(highMap.Count, row.Count);
              row.Add('z' - 'a');
            }
            else if (ch >= 'a' && ch <= 'z')
              row.Add(ch - 'a');
            else
              throw new ApplicationException($"invalid input character {ch}");
          }
          highMap.Add(row);
        }

      return new Input(highMap, start, end);
    }
  }
}
