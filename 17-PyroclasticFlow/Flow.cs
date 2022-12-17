namespace _17_PyroclasticFlow
{
  public enum ShapeType { Dash, Plus, L, I, Square };

  public enum MoveType { Left, Right };

  record struct Pos(int X, long Y);

  record Shape
  {
    public Shape(ShapeType shapeType, Pos pos)
    {
      ShapeType = shapeType;
      Pos = pos;
    }

    public ShapeType ShapeType { get; set; }
    public Pos Pos { get; set; }

    internal void SetPos(Pos newPos)
    {
      Pos = newPos;
    }
  }

  internal class Flow
  {
    internal static IEnumerable<MoveType> GetMoves(string input)
    {
      for (; ; )
      {
        foreach (var ch in input.Trim())
        {
          yield return ch switch
          {
            '<' => MoveType.Left,
            '>' => MoveType.Right,
            _ => throw new ApplicationException("unexpected character")
          };
        }
      }
    }

    internal static IEnumerable<Pos> GetShapeElements(ShapeType shapeType, Pos pos)
    {
      switch (shapeType)
      {
        case ShapeType.Dash:
          yield return pos;
          yield return new Pos(pos.X + 1, pos.Y);
          yield return new Pos(pos.X + 2, pos.Y);
          yield return new Pos(pos.X + 3, pos.Y);
          break;
        case ShapeType.Plus:
          yield return new Pos(pos.X + 1, pos.Y);
          yield return new Pos(pos.X + 0, pos.Y + 1);
          yield return new Pos(pos.X + 1, pos.Y + 1);
          yield return new Pos(pos.X + 2, pos.Y + 1);
          yield return new Pos(pos.X + 1, pos.Y + 2);
          break;
        case ShapeType.L:
          yield return pos;
          yield return new Pos(pos.X + 1, pos.Y);
          yield return new Pos(pos.X + 2, pos.Y);
          yield return new Pos(pos.X + 2, pos.Y + 1);
          yield return new Pos(pos.X + 2, pos.Y + 2);
          break;
        case ShapeType.I:
          yield return pos;
          yield return new Pos(pos.X, pos.Y + 1);
          yield return new Pos(pos.X, pos.Y + 2);
          yield return new Pos(pos.X, pos.Y + 3);
          break;
        case ShapeType.Square:
          yield return pos;
          yield return new Pos(pos.X + 1, pos.Y);
          yield return new Pos(pos.X, pos.Y + 1);
          yield return new Pos(pos.X + 1, pos.Y + 1);
          break;
        default:
          throw new ApplicationException("unexpected");
      }
    }

    internal static IEnumerable<ShapeType> GetShapes()
    {
      for (; ; )
      {
        foreach (var shapeType in Enum.GetValues<ShapeType>())
        {
          yield return shapeType;
        }
      }
    }
  }
}