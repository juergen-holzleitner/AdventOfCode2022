namespace _09_RopeBridge
{
  public enum Direction { Left, Right, Up, Down };
  public record struct Motion(Direction Direction, int Steps);

  public record struct Position(int X, int Y);

  internal class RopeBridge
  {
    private Position headPosition;
    private Position tailPosition;
    private HashSet<Position> visitedTailPositions = new();

    public RopeBridge()
    {
      MoveTail();
    }

    internal static Motion Parse(string line)
    {
      if (string.IsNullOrEmpty(line))
        throw new ArgumentException($"{nameof(line)} has invalid value {line}");

      var parts = line.Split(' ');
      switch (parts[0])
      {
        case "U":
          return new Motion(Direction.Up, int.Parse(parts[1]));
        case "R":
          return new Motion(Direction.Right, int.Parse(parts[1]));
        case "L":
          return new Motion(Direction.Left, int.Parse(parts[1]));
        case "D":
          return new Motion(Direction.Down, int.Parse(parts[1]));
      }

      throw new ArgumentException($"{nameof(line)} has invalid value {line}");
    }

    internal Position GetHeadPosition()
    {
      return headPosition;
    }

    internal Position GetTailPosition()
    {
      return tailPosition;
    }

    internal void Move(Direction direction)
    {
      switch (direction)
      {
        case Direction.Left:
          --headPosition.X;
          break;
        case Direction.Right:
          ++headPosition.X;
          break;
        case Direction.Up:
          --headPosition.Y;
          break;
        case Direction.Down:
          ++headPosition.Y;
          break;
        default:
          throw new ApplicationException("unexpected state");
      }
      MoveTail();
    }

    private void MoveTail()
    {
      int diffX = headPosition.X - tailPosition.X;
      int diffY = headPosition.Y - tailPosition.Y;
      if (Math.Abs(diffX) >= 2 || Math.Abs(diffY) >= 2)
      {
        tailPosition.X += Math.Sign(diffX);
        tailPosition.Y += Math.Sign(diffY);
      }

      visitedTailPositions.Add(tailPosition);
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
