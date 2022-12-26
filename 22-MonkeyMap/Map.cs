
namespace _22_MonkeyMap
{
  enum Field { Empty, Open, Block };

  record Board(Field[,] Field, int CubeSize)
  {
    internal Pos GetNextPosition(Pos pos, Direction direction)
    {
      var nextPos = GetAdjacentPos(pos, direction);

      while (Field[nextPos.X, nextPos.Y] == _22_MonkeyMap.Field.Empty)
        nextPos = GetAdjacentPos(nextPos, direction);

      if (Field[nextPos.X, nextPos.Y] == _22_MonkeyMap.Field.Block)
        nextPos = pos;

      if (Field[nextPos.X, nextPos.Y] != _22_MonkeyMap.Field.Open)
        throw new ApplicationException("unexpected");

      return nextPos;
    }

    internal Pos GetStartPosition()
    {
      var startPos = new Pos(0, 0);
      while (Field[startPos.X, startPos.Y] == _22_MonkeyMap.Field.Empty)
        startPos = startPos.Move(Direction.Right, 1);

      return startPos;
    }

    internal (Pos pos, Direction direction) GetNextPositionCube(Pos pos, Direction direction, CubeSetup cubeSetup)
    {
      var (nextPos, nextDirection) = GetAdjacentPositionCube(pos, direction, cubeSetup);

      if (Field[nextPos.X, nextPos.Y] == _22_MonkeyMap.Field.Empty)
        throw new ApplicationException("not expected");

      if (Field[nextPos.X, nextPos.Y] == _22_MonkeyMap.Field.Block)
      {
        nextPos = pos;
        nextDirection = direction;
      }

      if (Field[nextPos.X, nextPos.Y] != _22_MonkeyMap.Field.Open)
        throw new ApplicationException("unexpected");

      return (nextPos, nextDirection);
    }

    internal (Pos nextPos, Direction nextDirection) GetAdjacentPositionCube(Pos pos, Direction direction, CubeSetup cubeSetup)
    {
      var nextPos = pos.Move(direction, 1);
      var nextDirection = direction;

      var face = GetFaceAt(cubeSetup, pos);
      var localPos = GetLocalPos(pos, face);

      if (localPos.IsLocalBorder(direction, CubeSize))
      {
        var targetFaceVector = Map.RotateFaceVector(face.FaceVector, direction);
        var targetFace = GetFaceAt(cubeSetup, targetFaceVector.NormalVector);
        var transformedPos = localPos.TransformLocalBorder(direction, CubeSize);

        int numSteps = 0;
        while (targetFaceVector.X != targetFace.FaceVector.X)
        {
          if (targetFaceVector.Y == targetFace.FaceVector.Y)
            throw new ApplicationException();

          ++numSteps;
          if (numSteps >= 4)
            throw new ApplicationException();

          targetFaceVector = RotateFaceVectorClockwise(targetFaceVector);
          nextDirection = RotateDirectionCounterClockwise(nextDirection);
          transformedPos = RotatePosCounterClockwise(transformedPos);
        }

        nextPos = new Pos(transformedPos.X + targetFace.TopLeft2DPos.X, transformedPos.Y + targetFace.TopLeft2DPos.Y);
      }

      return (nextPos, nextDirection);
    }

    private Pos RotatePosCounterClockwise(Pos pos)
    {
      return new Pos(pos.Y, CubeSize - 1 - pos.X);
    }

    private static Direction RotateDirectionCounterClockwise(Direction direction)
    {
      return Player.GetNextDirection(direction, Direction.Left);
    }

    private static FaceVector RotateFaceVectorClockwise(FaceVector targetFaceVector)
    {
      var newX = targetFaceVector.Y;
      var newY = Map.NegateVector(targetFaceVector.X);
      return new FaceVector(targetFaceVector.NormalVector, newX, newY);
    }

    private static CubeFace GetFaceAt(CubeSetup cubeSetup, Vector normalVector)
    {
      return cubeSetup.Faces[normalVector];
    }

    private static Pos GetLocalPos(Pos pos, CubeFace face)
    {
      return new Pos(pos.X - face.TopLeft2DPos.X, pos.Y - face.TopLeft2DPos.Y);
    }

    private CubeFace GetFaceAt(CubeSetup cubeSetup, Pos pos)
    {
      return cubeSetup.Faces.Values.Where(f =>
      pos.X >= f.TopLeft2DPos.X
      && pos.Y >= f.TopLeft2DPos.Y
      && pos.X < f.TopLeft2DPos.X + CubeSize
      && pos.Y < f.TopLeft2DPos.Y + CubeSize
      ).Single();
    }

    private Pos GetAdjacentPos(Pos pos, Direction direction)
    {
      if (direction == Direction.Right)
      {
        ++pos.X;
        if (pos.X >= Field.GetLength(0))
          pos.X = 0;

      }
      else if (direction == Direction.Left)
      {
        --pos.X;
        if (pos.X < 0)
          pos.X = Field.GetLength(0) - 1;
      }
      else if (direction == Direction.Down)
      {
        ++pos.Y;
        if (pos.Y >= Field.GetLength(1))
          pos.Y = 0;
      }
      else if (direction == Direction.Up)
      {
        --pos.Y;
        if (pos.Y < 0)
          pos.Y = Field.GetLength(1) - 1;
      }

      return pos;
    }
  }

  public enum Direction { Left, Right, Up, Down };

  record Instruction();
  record MoveInstruction(int Num) : Instruction;
  record TurnInstruction(Direction Direction) : Instruction;

  record Input(Board Board, List<Instruction> Instructions);

  internal class Map
  {
    internal static (int X, int Y) GetDimensions(string input)
    {
      int X = 0; int Y = 0;
      foreach (var l in input.Split('\n'))
      {
        var line = l.TrimEnd();
        if (string.IsNullOrEmpty(line))
          break;

        X = Math.Max(X, line.Length);
        ++Y;
      }

      return (X, Y);
    }

    internal static Player GetFinalPlayer(string input, int cubeSize, bool useCube)
    {
      var inp = ParseInput(input, cubeSize);
      var player = new Player(inp.Board, inp.Board.GetStartPosition());
      foreach (var instruction in inp.Instructions)
        player.DoInstruction(instruction, useCube);
      return player;
    }

    internal static int GetFinalScore(string input, int cubeSize, bool useCube)
    {
      var player = GetFinalPlayer(input, cubeSize, useCube);
      return player.GetScore();
    }

    internal static Input ParseInput(string input, int cubeSize)
    {
      var (X, Y) = GetDimensions(input);
      var board = new Field[X, Y];

      var instructions = new List<Instruction>();

      bool hasSeenEmptyLine = false;

      int y = 0;
      foreach (var l in input.Split('\n'))
      {
        var line = l.TrimEnd();

        if (hasSeenEmptyLine)
        {
          instructions = ParseInstructions(line);
          break;
        }

        if (string.IsNullOrEmpty(line))
        {
          hasSeenEmptyLine = true;
          continue;
        }

        for (int x = 0; x < line.Length; ++x)
          board[x, y] = ParseField(line[x]);

        ++y;
      }

      return new Input(new Board(board, cubeSize), instructions);
    }

    internal static List<Instruction> ParseInstructions(string input)
    {
      var instructions = new List<Instruction>();

      var enumerator = input.GetEnumerator();
      if (!enumerator.MoveNext())
        return instructions;

      for (; ; )
      {
        if (enumerator.Current == 'L')
        {
          instructions.Add(new TurnInstruction(Direction.Left));
          if (!enumerator.MoveNext())
            break;
        }
        else if (enumerator.Current == 'R')
        {
          instructions.Add(new TurnInstruction(Direction.Right));
          if (!enumerator.MoveNext())
            break;
        }
        else if (char.IsDigit(enumerator.Current))
        {
          int number = int.Parse(enumerator.Current.ToString());
          bool hasEnded = false;

          for (; ; )
          {
            if (!enumerator.MoveNext())
            {
              hasEnded = true;
              break;
            }
            if (!char.IsDigit(enumerator.Current))
              break;

            number *= 10;
            number += int.Parse(enumerator.Current.ToString());
          }
          instructions.Add(new MoveInstruction(number));
          if (hasEnded)
            break;
        }
        else
          throw new ApplicationException("unexpected char " + enumerator.Current);
      }

      return instructions;
    }

    private static Field ParseField(char field)
    {
      return field switch
      {
        ' ' => Field.Empty,
        '.' => Field.Open,
        '#' => Field.Block,
        _ => throw new ApplicationException("unexpected char " + field)
      };
    }

    internal static CubeSetup FoldToCube(Board board, Pos topLeft2DPos)
    {
      var faces = new Dictionary<Vector, CubeFace>();
      var faceVector = new FaceVector(new Vector(0, 1, 0), new Vector(1, 0, 0), new Vector(0, 0, -1));

      AddCubeFace(faces, topLeft2DPos, faceVector, board);

      return new CubeSetup(faces);
    }

    private static void AddCubeFace(Dictionary<Vector, CubeFace> faces, Pos topLeft2DPos, FaceVector faceVector, Board board)
    {
      if (faces.ContainsKey(faceVector.NormalVector))
        return;

      if (topLeft2DPos.X < 0 || topLeft2DPos.X >= board.Field.GetLength(0))
        return;

      if (topLeft2DPos.Y < 0 || topLeft2DPos.Y >= board.Field.GetLength(1))
        return;

      if (board.Field[topLeft2DPos.X, topLeft2DPos.Y] == Field.Empty)
        return;

      faces.Add(faceVector.NormalVector, new CubeFace(topLeft2DPos, faceVector));

      foreach (var direction in Enum.GetValues<Direction>())
      {
        var nextFaceVector = RotateFaceVector(faceVector, direction);
        AddCubeFace(faces, topLeft2DPos.Move(direction, board.CubeSize), nextFaceVector, board);
      }
    }

    public static FaceVector RotateFaceVector(FaceVector faceVector, Direction direction)
    {
      return direction switch
      {
        Direction.Left => new FaceVector(NegateVector(faceVector.X), faceVector.NormalVector, faceVector.Y),
        Direction.Right => new FaceVector(faceVector.X, NegateVector(faceVector.NormalVector), faceVector.Y),
        Direction.Up => new FaceVector(NegateVector(faceVector.Y), faceVector.X, faceVector.NormalVector),
        Direction.Down => new FaceVector(faceVector.Y, faceVector.X, NegateVector(faceVector.NormalVector)),
        _ => throw new ApplicationException()
      };
    }

    public static Vector NegateVector(Vector x)
    {
      return new Vector(-x.X, -x.Y, -x.Z);
    }
  }

  record CubeFace(Pos TopLeft2DPos, FaceVector FaceVector);

  record struct Vector(int X, int Y, int Z);

  record struct FaceVector(Vector NormalVector, Vector X, Vector Y);

  record CubeSetup(Dictionary<Vector, CubeFace> Faces);

}