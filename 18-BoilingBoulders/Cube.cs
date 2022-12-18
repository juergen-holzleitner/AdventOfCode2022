namespace _18_BoilingBoulders
{
  record struct Cube(int X, int Y, int Z)
  {
    internal static int GetTotalSurface(string input)
    {
      int totalSurface = 0;
      var cubes = ParseInput(input);
      var processedCubes = new List<Cube>();

      foreach (var cube in cubes)
      {
        var additional = cube.GetAdditionalSurfaces(processedCubes);
        totalSurface += additional;
        processedCubes.Add(cube);
      }
      return totalSurface;
    }

    internal static List<Cube> ParseInput(string input)
    {
      var cubes = new List<Cube>();
      foreach (var line in input.Split('\n'))
      {
        if (!string.IsNullOrWhiteSpace(line))
        {
          var cube = ParseLine(line);
          cubes.Add(cube);
        }
      }
      return cubes;
    }

    internal static Cube ParseLine(string line)
    {
      var dimensions = line.Split(',');
      var x = int.Parse(dimensions[0]);
      var y = int.Parse(dimensions[1]);
      var z = int.Parse(dimensions[2]);

      return new Cube(x, y, z);
    }

    internal int GetAdditionalSurfaces(List<Cube> processed)
    {
      int numAdditional = 6;
      foreach (var cube in processed)
      {
        if (cube == this)
          throw new ApplicationException("not expected");

        if (IsAdjacent(cube))
          numAdditional -= 2;
      }

      return numAdditional;
    }

    private bool IsAdjacent(Cube other)
    {
      foreach (var c in GetAdjacentPositions())
        if (c == other)
          return true;

      return false;
    }

    internal IEnumerable<Cube> GetAdjacentPositions()
    {
      yield return new Cube(X + 1, Y + 0, Z + 0);
      yield return new Cube(X - 1, Y + 0, Z + 0);
      yield return new Cube(X + 0, Y + 1, Z + 0);
      yield return new Cube(X + 0, Y - 1, Z + 0);
      yield return new Cube(X + 0, Y + 0, Z + 1);
      yield return new Cube(X + 0, Y + 0, Z - 1);
    }
  }

}