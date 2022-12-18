using System.Diagnostics;

namespace _18_BoilingBoulders
{
  record struct BoundingBox(Cube Min, Cube Max);

  record struct Cube(int X, int Y, int Z)
  {
    internal static int GetTotalSurface(string input)
    {
      var cubes = ParseInput(input);
      return GetTotalSurface(cubes);
    }

    private static int GetTotalSurface(List<Cube> cubes)
    {
      int totalSurface = 0;
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

    internal static BoundingBox GetBoundingBox(List<Cube> cubes)
    {
      var minX = cubes.Select(c => c.X).Min();
      var minY = cubes.Select(c => c.Y).Min();
      var minZ = cubes.Select(c => c.Z).Min();

      var maxX = cubes.Select(c => c.X).Max();
      var maxY = cubes.Select(c => c.Y).Max();
      var maxZ = cubes.Select(c => c.Z).Max();

      return new BoundingBox(new Cube(minX, minY, minZ), new Cube(maxX, maxY, maxZ));
    }

    internal static CubeStatsBlock GenerateBlockWithCubeStats(List<Cube> cubes)
    {
      var boundingBox = GetBoundingBox(cubes);

      var dimX = 1 + boundingBox.Max.X - boundingBox.Min.X;
      var dimY = 1 + boundingBox.Max.Y - boundingBox.Min.Y;
      var dimZ = 1 + boundingBox.Max.Z - boundingBox.Min.Z;

      var cubeBlock = new CubeStats[dimX, dimY, dimZ];

      foreach (var cube in cubes)
      {
        var posX = cube.X - boundingBox.Min.X;
        var posY = cube.Y - boundingBox.Min.Y;
        var posZ = cube.Z - boundingBox.Min.Z;

        if (cubeBlock[posX, posY, posZ].Type != Type.Free)
          throw new ApplicationException("block should be free");

        cubeBlock[posX, posY, posZ].Type = Type.Block;
      }

      return new CubeStatsBlock(cubeBlock);
    }

    internal static int GetInnerSurface(List<Cube> cubes)
    {
      int totalInnerSurface = 0;
      var cubeStats = GenerateBlockWithCubeStats(cubes);
      for (int x = 0; x < cubeStats.Block.GetLength(0); ++x)
        for (int y = 0; y < cubeStats.Block.GetLength(1); ++y)
          for (int z = 0; z < cubeStats.Block.GetLength(2); ++z)
          {
            if (cubeStats.Block[x, y, z].Type == Type.Free)
            {
              var freeCluster = GetFreeCluser(cubeStats, new Cube(x, y, z));
              if (IsInnerBlock(cubeStats, freeCluster))
              {
                totalInnerSurface += Cube.GetTotalSurface(freeCluster);
              }
            }
          }

      return totalInnerSurface;
    }

    private static List<Cube> GetFreeCluser(CubeStatsBlock cubeStats, Cube cube)
    {
      if (cubeStats.Block[cube.X, cube.Y, cube.Z].Type != Type.Free)
        throw new Exception();

      var processing = new Queue<Cube>();
      processing.Enqueue(cube);

      var freeCluster = new List<Cube>();
      while (processing.Any())
      {
        var current = processing.Dequeue();
        if (cubeStats.Block[current.X, current.Y, current.Z].Type == Type.Free)
        {
          freeCluster.Add(current);
          cubeStats.Block[current.X, current.Y, current.Z].Type = Type.Processed;

          foreach (var ad in current.GetAdjacentPositions())
          {
            if (ad.X < 0 || ad.Y < 0 || ad.Z < 0)
              continue;

            if (ad.X >= cubeStats.Block.GetLength(0) || ad.Y >= cubeStats.Block.GetLength(1) || ad.Z >= cubeStats.Block.GetLength(2))
              continue;

            if (cubeStats.Block[ad.X, ad.Y, ad.Z].Type != Type.Free)
              continue;

            processing.Enqueue(ad);
          }
        }
      }

      return freeCluster;
    }

    private static bool IsInnerBlock(CubeStatsBlock cubeStats, List<Cube> cubes)
    {
      foreach (var cube in cubes)
      {
        if (cube.X <= 0 || cube.Y <= 0 || cube.Z <= 0)
          return false;

        if (cube.X >= cubeStats.Block.GetLength(0) - 1
          || cube.Y >= cubeStats.Block.GetLength(1) - 1
          || cube.Z >= cubeStats.Block.GetLength(2) - 1)
          return false;
      }
      return true;
    }

    internal static int GetTotalSurfacePart2(string input)
    {
      var cubes = ParseInput(input);

      var innerSurface = GetInnerSurface(cubes);
      var outerSurface = GetTotalSurface(cubes);

      return outerSurface - innerSurface;
    }
  }

  enum Type { Free, Block, Processed };
  record struct CubeStats(Type Type);

  record CubeStatsBlock(CubeStats[,,] Block);
}