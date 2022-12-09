namespace _09_RopeBridge
{
  public enum Direction { Left, Right, Up, Down };
  public record struct Motion(Direction Direction, int Steps);

  public record struct Position(int X, int Y);

  internal class RopeBridge
  {
    private readonly Position[] ropePosition;
    private readonly HashSet<Position> visitedTailPositions = new();

    public RopeBridge(int numKnots = 2)
    {
      ropePosition = new Position[numKnots];
      MoveTail();
    }

    internal static Motion Parse(string line)
    {
      if (string.IsNullOrEmpty(line))
        throw new ArgumentException($"{nameof(line)} has invalid value {line}");

      var parts = line.Split(' ');
      return parts[0] switch
      {
        "U" => new Motion(Direction.Up, int.Parse(parts[1])),
        "R" => new Motion(Direction.Right, int.Parse(parts[1])),
        "L" => new Motion(Direction.Left, int.Parse(parts[1])),
        "D" => new Motion(Direction.Down, int.Parse(parts[1])),
        _ => throw new ArgumentException($"{nameof(line)} has invalid value {line}"),
      };
    }

    internal Position GetHeadPosition()
    {
      return ropePosition[0];
    }

    internal Position GetTailPosition()
    {
      return ropePosition[^1];
    }

    internal void Move(Direction direction)
    {
      switch (direction)
      {
        case Direction.Left:
          --ropePosition[0].X;
          break;
        case Direction.Right:
          ++ropePosition[0].X;
          break;
        case Direction.Up:
          --ropePosition[0].Y;
          break;
        case Direction.Down:
          ++ropePosition[0].Y;
          break;
        default:
          throw new ApplicationException("unexpected state");
      }
      MoveTail();
    }

    private void MoveTail()
    {
      for (int n = 1; n < ropePosition.Length; ++n)
      {
        int diffX = ropePosition[n - 1].X - ropePosition[n].X;
        int diffY = ropePosition[n - 1].Y - ropePosition[n].Y;
        if (Math.Abs(diffX) >= 2 || Math.Abs(diffY) >= 2)
        {
          ropePosition[n].X += Math.Sign(diffX);
          ropePosition[n].Y += Math.Sign(diffY);
        }
      }

      visitedTailPositions.Add(ropePosition[^1]);
    }

    internal void Move(Motion motion)
    {
      for (int n = 0; n < motion.Steps; ++n)
      {
        Move(motion.Direction);
      }
    }

    internal void MoveInput(IEnumerable<string> input)
    {
      foreach (var line in input)
      {
        if (!string.IsNullOrEmpty(line))
        {
          var motion = Parse(line);
          Move(motion);
        }
      }
    }

    internal IEnumerable<Position> GetVisitedTailPositions()
    {
      return visitedTailPositions;
    }
  }
}
