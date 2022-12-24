using System.Collections.Generic;

namespace _24_BlizzardBasin
{
  record struct Pos(int X, int Y)
  {
    internal Pos Move(Direction direction, Pos limits)
    {
      Pos newPos = Move(direction);

      if (newPos.X < 0)
        newPos.X = limits.X - 1;
      if (newPos.Y < 0)
        newPos.Y = limits.Y - 1;
      if (newPos.X >= limits.X)
        newPos.X = 0;
      if (newPos.Y >= limits.Y)
        newPos.Y = 0;

      return newPos;
    }

    internal readonly Pos Move(Direction direction)
    {
      return direction switch
      {
        Direction.Left => new Pos(X - 1, Y),
        Direction.Right => new Pos(X + 1, Y),
        Direction.Up => new Pos(X, Y - 1),
        Direction.Down => new Pos(X, Y + 1),
        _ => throw new ApplicationException("invalid direction")
      };
    }
  }

  enum Direction { Left, Right, Up, Down }

  class BlizzardMap
  {
    public Dictionary<Pos, List<Direction>> Blizzards { get; set; } = new();
    public Pos Size { get; internal set; }

    public Pos StartPos { get; set; }
    public Pos EndPos { get; set; }

    internal void Add(Pos pos, Direction direction)
    {
      AddInternal(Blizzards, pos, direction);
    }

    private static void AddInternal(Dictionary<Pos, List<Direction>> dictionary, Pos pos, Direction direction)
    {
      if (!dictionary.TryGetValue(pos, out List<Direction>? blizzards))
      {
        blizzards = new();
        dictionary.Add(pos, blizzards);
      }

      blizzards.Add(direction);
    }

    internal void SingleStep()
    {
      var newBlizzards = new Dictionary<Pos, List<Direction>>();

      foreach (var blizzards in Blizzards)
      {
        var pos = blizzards.Key;
        foreach (var direction in blizzards.Value)
        {
          var newPos = pos.Move(direction, Size);
          AddInternal(newBlizzards, newPos, direction);
        }
      }

      Blizzards = newBlizzards;
    }

    internal HashSet<Pos> GetNextPlayerPositions(HashSet<Pos> playerPositions, BlizzardMap blizzardMap)
    {
      var newPositions = new HashSet<Pos>();
      foreach (var pos in playerPositions)
      {
        if (IsStandingStillPossible(pos, blizzardMap))
        {
          newPositions.Add(pos);
        }

        foreach (var direction in Enum.GetValues<Direction>())
        {
          var np = pos.Move(direction);
          if (IsValidPlayerPos(np))
            newPositions.Add(np);
        }
      }
      return newPositions;
    }

    private bool IsStandingStillPossible(Pos pos, BlizzardMap blizzardMap)
    {
      return !blizzardMap.Blizzards.ContainsKey(pos);
    }

    private bool IsValidPlayerPos(Pos pos)
    {
      if (pos == StartPos)
        return true;
      if (pos == EndPos)
        return true;

      if (pos.X < 0 || pos.Y < 0)
        return false;

      if (pos.X >= Size.X || pos.Y >= Size.Y)
        return false;

      if (Blizzards.ContainsKey(pos))
        return false;

      return true;
    }
  }


  internal class Blizzard
  {
    internal static BlizzardMap Parse(string text)
    {
      var lines = text.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Trim()).ToList();

      int startPosX = GetHoleInLine(lines[0]);
      int endPosX = GetHoleInLine(lines[^1]);

      var blizzardMap = new BlizzardMap();

      for (int y = 1; y < lines.Count - 1; ++y)
      {
        var line = lines[y];
        for (int x = 1; x < line.Length - 1; ++x)
        {
          if (line[x] != '.')
          {
            var direction = ParseDirection(line[x]);
            blizzardMap.Add(new Pos(x - 1, y - 1), direction);
          }
        }
      }

      blizzardMap.Size = new Pos(lines[0].Length - 2, lines.Count - 2);
      blizzardMap.StartPos = new Pos(startPosX - 1, -1);
      blizzardMap.EndPos = new Pos(endPosX - 1, lines.Count - 2);

      return blizzardMap;
    }

    private static Direction ParseDirection(char ch)
    {
      return ch switch
      {
        '>' => Direction.Right,
        '<' => Direction.Left,
        '^' => Direction.Up,
        'v' => Direction.Down,
        _ => throw new ApplicationException("invalid char: " + ch)
      };
    }

    private static int GetHoleInLine(string line)
    {
      int startPosX = 0;
      while (line[startPosX] == '#')
        ++startPosX;
      return startPosX;
    }

    internal static int GetNumStepsToExit(string text)
    {
      var blizzardMap = Parse(text);

      var playerPositions = new HashSet<Pos> { blizzardMap.StartPos };

      int numSteps = 0;

      PrintSetup(numSteps, blizzardMap, playerPositions);

      while (!playerPositions.Contains(blizzardMap.EndPos))
      {
        blizzardMap.SingleStep();
        playerPositions = blizzardMap.GetNextPlayerPositions(playerPositions, blizzardMap);
        ++numSteps;

        PrintSetup(numSteps, blizzardMap, playerPositions);
      }

      return numSteps;
    }

    private static void PrintSetup(int numSteps, BlizzardMap blizzardMap, HashSet<Pos> playerPositions)
    {
      /*
      Console.WriteLine("\n\nStep " + numSteps + '\n');

      for (int y = 0; y <= blizzardMap.Size.Y + 1; ++y)
      {
        for (int x = 0; x <= blizzardMap.Size.X + 1; ++x)
        {
          var pos = new Pos(x - 1, y - 1);

          if (playerPositions.Contains(pos))
          {
            if (blizzardMap.Blizzards.ContainsKey(pos))
              throw new ApplicationException("player and blizzard at the same position");

            Console.Write('E');
          }
          else
          {
            if (blizzardMap.Blizzards.TryGetValue(pos, out List<Direction>? list))
            {
              if (list.Count > 1)
                Console.Write(list.Count);
              else
                Console.Write(GetDirectionChar(list.Single()));
            }
            else
            {
              if (pos == blizzardMap.EndPos || pos == blizzardMap.StartPos)
                Console.Write('.');
              else if (pos.X < 0 || pos.X >= blizzardMap.Size.X
                || pos.Y < 0 || pos.Y >= blizzardMap.Size.Y)
                Console.Write('#');
              else
                Console.Write('.');
            }
          }
        }

        Console.WriteLine();
      }
      */
    }

    private static char GetDirectionChar(Direction direction)
    {
      return direction switch
      {
        Direction.Left => '<',
        Direction.Right => '>',
        Direction.Up => '^',
        Direction.Down => 'v',
        _ => throw new ApplicationException("invalid direction")
      };
    }

    internal static int GetNumStepsToExitBackAndExitAgain(string text)
    {
      var blizzardMap = Parse(text);

      int numSteps = 0;

      var playerPositions = new HashSet<Pos> { blizzardMap.StartPos };
      while (!playerPositions.Contains(blizzardMap.EndPos))
      {
        blizzardMap.SingleStep();
        playerPositions = blizzardMap.GetNextPlayerPositions(playerPositions, blizzardMap);
        ++numSteps;
      }

      playerPositions = new() { blizzardMap.EndPos };
      while (!playerPositions.Contains(blizzardMap.StartPos))
      {
        blizzardMap.SingleStep();
        playerPositions = blizzardMap.GetNextPlayerPositions(playerPositions, blizzardMap);
        ++numSteps;
      }

      playerPositions = new () { blizzardMap.StartPos };
      while (!playerPositions.Contains(blizzardMap.EndPos))
      {
        blizzardMap.SingleStep();
        playerPositions = blizzardMap.GetNextPlayerPositions(playerPositions, blizzardMap);
        ++numSteps;
      }

      return numSteps;
    }
  }
}