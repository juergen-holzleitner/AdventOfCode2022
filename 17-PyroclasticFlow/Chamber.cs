using System.Runtime.CompilerServices;

namespace _17_PyroclasticFlow
{
  internal class Chamber
  {
    private readonly IEnumerator<MoveType> nextMoves;
    private readonly IEnumerator<ShapeType> shapeTypes;

    public Shape? CurrentShape { get; private set; }

    public List<Shape> StoppedShapes { get; private init; } = new();

    public int CurrentTotalHeight { get; private set; }

    private const int chamberWidth = 7;

    public Chamber(string input)
    {
      nextMoves = Flow.GetMoves(input).GetEnumerator();
      shapeTypes = Flow.GetShapes().GetEnumerator();
    }


    internal void DoStepUntilNextStopped()
    {
      while (!DoStep()) ;
    }

    internal bool DoStep()
    {
      if (CurrentShape is null)
      {
        shapeTypes.MoveNext();
        CurrentShape = new Shape(shapeTypes.Current, new Pos(2, CurrentTotalHeight + 3));
      }

      nextMoves.MoveNext();
      var newPos = GetNewPosFromMove(CurrentShape.Pos, nextMoves.Current);
      if (IsPosValid(newPos, CurrentShape.ShapeType))
        CurrentShape.SetPos(newPos);

      newPos = CurrentShape.Pos with { Y = CurrentShape.Pos.Y - 1 };
      if (IsPosValid(newPos, CurrentShape.ShapeType))
        CurrentShape.SetPos(newPos);
      else
      {
        int newHight = CurrentShape.Pos.Y + GetShapeHeight(CurrentShape.ShapeType);
        if (newHight > CurrentTotalHeight)
          CurrentTotalHeight = newHight;

        StoppedShapes.Add(CurrentShape);
        CurrentShape = null;
        
        return true;
      }

      return false;
    }

    private bool IsPosValid(Pos newPos, ShapeType shapeType)
    {
      if (newPos.X < 0)
        return false;

      if (newPos.X + GetShapeWitdh(shapeType) > chamberWidth)
        return false;

      if (newPos.Y < 0)
        return false;

      foreach (var shape in StoppedShapes)
      {
        if (IsShapeCollision(shape, shapeType, newPos))
          return false;
      }

      return true;
    }

    private bool IsShapeCollision(Shape shape, ShapeType shapeType, Pos newPos)
    {
      if (newPos.Y > shape.Pos.Y + GetShapeHeight(shape.ShapeType)) 
        return false;

      foreach (var pos1 in Flow.GetShapeElements(shape.ShapeType, shape.Pos))
        foreach (var pos2 in Flow.GetShapeElements(shapeType, newPos))
        {
          if (pos1 == pos2)
            return true;
        }
      return false;
    }

    private static int GetShapeWitdh(ShapeType shapeType)
    {
      return shapeType switch
      {
        ShapeType.Dash => 4,
        ShapeType.Plus => 3,
        ShapeType.L => 3,
        ShapeType.I => 1,
        ShapeType.Square => 2,
        _ => throw new ApplicationException("unexpected shape")
      }; ;
    }

    private static int GetShapeHeight(ShapeType shapeType)
    {
      return shapeType switch
      {
        ShapeType.Dash => 1,
        ShapeType.Plus => 3,
        ShapeType.L => 3,
        ShapeType.I => 4,
        ShapeType.Square => 2,
        _ => throw new ApplicationException("unexpected shape")
      }; ;
    }
    private static Pos GetNewPosFromMove(Pos pos, MoveType move)
    {
      return pos with { X = move == MoveType.Left ? pos.X - 1 : pos.X + 1 };
    }

    static internal int GetHeightAfterElements(string input, int numElements)
    {
      var chamber = new Chamber(input);
      for (int n = 0; n < numElements; ++n)
        chamber.DoStepUntilNextStopped();

      return chamber.CurrentTotalHeight;
    }
  }
}