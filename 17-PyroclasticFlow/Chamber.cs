using System.Diagnostics.CodeAnalysis;

namespace _17_PyroclasticFlow
{
  public record struct StateProfile(ShapeType LastShape, int InputIndex, int[] HeightMap);

  public class StateProfileComparer : IEqualityComparer<StateProfile>
  {

    public bool Equals(StateProfile x, StateProfile y)
    {
      if (x.InputIndex != y.InputIndex)
        return false;

      if (x.LastShape != y.LastShape) return false;

      for (int n = 0; n < x.HeightMap.Length; ++n)
        if (x.HeightMap[n] != y.HeightMap[n])
          return false;

      return true;
    }

    public int GetHashCode([DisallowNull] StateProfile obj)
    {
      var val = obj.InputIndex.GetHashCode() ^ obj.LastShape.GetHashCode();
      for (int n = 0; n < obj.HeightMap.Length; ++n)
        val ^= obj.HeightMap[n];
      return val;
    }
  }

  internal class Chamber
  {
    private readonly IEnumerator<MoveType> nextMoves;
    private readonly IEnumerator<ShapeType> shapeTypes;

    public Shape? CurrentShape { get; private set; }

    public List<Shape> StoppedShapes { get; private init; } = new();

    public long CurrentTotalHeight { get; private set; }

    public long NumStepsProcessed { get; private set; }

    private const int chamberWidth = 7;

    private readonly string initialInput;

    public Chamber(string input)
    {
      initialInput = input.Trim();

      nextMoves = Flow.GetMoves(initialInput).GetEnumerator();
      shapeTypes = Flow.GetShapes().GetEnumerator();
    }


    internal void DoStepUntilNextStopped()
    {
      while (!DoStep()) ;
    }

    internal bool DoStep()
    {
      ++NumStepsProcessed;

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
        long newHight = CurrentShape.Pos.Y + GetShapeHeight(CurrentShape.ShapeType);
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

    private static bool IsShapeCollision(Shape shape, ShapeType shapeType, Pos newPos)
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

    private bool IsPosInElement(Pos pos)
    {
      foreach (var shape in StoppedShapes)
      {
        foreach (var shapePos in Flow.GetShapeElements(shape.ShapeType, shape.Pos))
        {
          if (pos == shapePos)
            return true;
        }
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

    internal static long GetHeightAfterElements(string input, long numElements)
    {
      var chamber = new Chamber(input);
      for (long n = 0; n < numElements; ++n)
        chamber.DoStepUntilNextStopped();

      return chamber.CurrentTotalHeight;
    }

    internal static int GetInputCycleLength(string input)
    {
      return input.Trim().Length;
    }

    internal static (long initial, long loopSize) GetInputCycle(string input)
    {
      var stateCache = new Dictionary<StateProfile, long>(new StateProfileComparer());

      var chamber = new Chamber(input);
      for (; ; )
      {
        chamber.DoStepUntilNextStopped();
        var profile = chamber.GetStateProfile();

        if (stateCache.ContainsKey(profile))
        {
          long initial = stateCache[profile];
          long loopSize = chamber.NumStepsProcessed - initial;
          return new(initial, loopSize);
        }
        else
        {
          stateCache.Add(profile, chamber.NumStepsProcessed);
        }
      }
    }

    internal StateProfile GetStateProfile()
    {
      long inputCycleLength = GetInputCycleLength(initialInput);
      var highMap = new int[chamberWidth];
      for (int n = 0; n < highMap.Length; ++n)
      {
        highMap[n] = GetHeight(n);
      }

      return new StateProfile(StoppedShapes.Last().ShapeType, (int)(NumStepsProcessed % inputCycleLength), highMap);
    }

    private int GetHeight(int n)
    {
      for (long y = CurrentTotalHeight - 1; y >= 0; --y)
      {
        var pos = new Pos(n, y);
        if (IsPosInElement(pos))
          return (int)(CurrentTotalHeight - 1 - y);
      }

      return (int)CurrentTotalHeight;
    }

    private record StateInfo(long NumSteps, long Height, long NumElements);

    internal static long GetHeightAfterElementsPart2(string input, long numElements)
    {

      var stateCache = new Dictionary<StateProfile, StateInfo>(new StateProfileComparer());

      var chamber = new Chamber(input);
      for (; ; )
      {
        chamber.DoStepUntilNextStopped();
        var profile = chamber.GetStateProfile();

        if (stateCache.ContainsKey(profile))
        {
          var initial = stateCache[profile];

          var looping = new StateInfo(chamber.NumStepsProcessed, chamber.CurrentTotalHeight, chamber.StoppedShapes.Count);
          return GetCalculatedHeight(input, initial, looping, numElements);
        }
        else
        {
          var stateInfo = new StateInfo(chamber.NumStepsProcessed, chamber.CurrentTotalHeight, chamber.StoppedShapes.Count);
          stateCache.Add(profile, stateInfo);
        }
      }
    }

    private static long GetCalculatedHeight(string input, StateInfo initial, StateInfo looping, long numElements)
    {
      var chamber = new Chamber(input);
      for (int n = 0; n < initial.NumElements; n++)
      {
        chamber.DoStepUntilNextStopped();
      }

      var numRemaining = numElements - initial.NumElements;

      var numElementsPerLoop = looping.NumElements - initial.NumElements;

      var numLoops = numRemaining / numElementsPerLoop;

      numRemaining %= numElementsPerLoop;
      for (int n = 0; n < numRemaining; n++)
      {
        chamber.DoStepUntilNextStopped();
      }

      var heightPerLoop = looping.Height - initial.Height;

      return chamber.CurrentTotalHeight + numLoops * heightPerLoop;
    }
  }
}