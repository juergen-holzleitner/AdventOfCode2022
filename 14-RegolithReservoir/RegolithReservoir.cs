namespace _14_RegolithReservoir
{
  internal record struct Pos(int X, int Y);
  internal record struct Wall(List<Pos> Positions);
  internal record struct Cave(List<Wall> Walls);

  internal class RegolithReservoir
  {
    internal static int GetMaxVerticalPosition(Cave cave)
    {
      return (from w in cave.Walls
              from p in w.Positions
              select p.Y
       ).Max();
    }

    internal static bool IsPositionBlockedByWall(Pos pos, Cave cave)
    {
      foreach (var wall in cave.Walls)
      {
        if (IsPositionBlocked(pos, wall))
          return true;
      }
      return false;
    }

    private static bool IsPositionBlocked(Pos pos, Wall wall)
    {
      if (wall.Positions.Count <= 1)
        throw new ApplicationException("wall with size 1 is not expected");

      for (int n = 1; n < wall.Positions.Count; ++n)
      {
        if (IsPositionBlocked(pos, wall.Positions[n - 1], wall.Positions[n]))
          return true;
      }

      return false;
    }

    private static bool IsPositionBlocked(Pos pos, Pos startOfWall, Pos endOfWall)
    {
      if (startOfWall.X == endOfWall.X)
      {
        if (pos.X != startOfWall.X)
          return false;
        if (pos.Y < Math.Min(startOfWall.Y, endOfWall.Y))
          return false;
        if (pos.Y > Math.Max(startOfWall.Y, endOfWall.Y))
          return false;
      }
      else if (startOfWall.Y == endOfWall.Y)
      {
        if (pos.Y != startOfWall.Y)
          return false;
        if (pos.X < Math.Min(startOfWall.X, endOfWall.X))
          return false;
        if (pos.X > Math.Max(startOfWall.X, endOfWall.X))
          return false;
      }
      else
      {
        throw new ApplicationException("expected only straight walls");
      }

      return true;
    }

    internal static Cave ParseCave(string lines)
    {
      var walls = new List<Wall>();
      foreach (var line in lines.Split('\n'))
        if (!string.IsNullOrEmpty(line))
        {
          var wall = ParseLine(line);
          walls.Add(wall);
        }
      return new Cave(walls);
    }

    internal static Wall ParseLine(string line)
    {
      var wall = new Wall(new());

      var positions = line.Split("->");
      foreach (var position in positions)
      {
        var elements = position.Split(',');
        var pos = new Pos(int.Parse(elements[0]), int.Parse(elements[1]));
        wall.Positions.Add(pos);
      }
      return wall;
    }

    internal static bool IsPositionBlockedBySand(Pos pos, List<Pos> sandPositions)
    {
      return sandPositions.Contains(pos);
    }

    internal static bool AddSand(Cave cave, List<Pos> sandPositions, bool blockAtMaxPosPlusTwo, int maxVerticalPosition)
    {
      var sandPosition = new Pos(500, 0);
      if (IsPositionBlocked(sandPosition, cave, sandPositions))
        return false;

      for (; ; )
      {
        if (blockAtMaxPosPlusTwo)
        {
          if (sandPosition.Y >= maxVerticalPosition + 1)
          {
            sandPositions.Add(sandPosition);
            return true;
          }
        }
        else
        {
          if (sandPosition.Y >= maxVerticalPosition)
            return false;
        }

        var newSandPosition = sandPosition with { Y = sandPosition.Y + 1 };
        if (!IsPositionBlocked(newSandPosition, cave, sandPositions))
        {
          sandPosition = newSandPosition;
          continue;
        }

        --newSandPosition.X;
        if (!IsPositionBlocked(newSandPosition, cave, sandPositions))
        {
          sandPosition = newSandPosition;
          continue;
        }

        newSandPosition.X += 2;
        if (!IsPositionBlocked(newSandPosition, cave, sandPositions))
        {
          sandPosition = newSandPosition;
          continue;
        }

        sandPositions.Add(sandPosition);
        return true;
      }
    }

    private static bool IsPositionBlocked(Pos pos, Cave cave, List<Pos> sandPositions)
    {
      if (IsPositionBlockedByWall(pos, cave))
        return true;
      if (IsPositionBlockedBySand(pos, sandPositions))
        return true;

      return false;
    }

    internal static int GetNumSandsAdded(string lines, bool blockAtMaxPosPlusTwo)
    {
      var cave = ParseCave(lines);
      var sandPositions = new List<Pos>();
      var maxVerticalPosition = GetMaxVerticalPosition(cave);

      while (AddSand(cave, sandPositions, blockAtMaxPosPlusTwo, maxVerticalPosition)) ;

      return sandPositions.Count;
    }
  }
}