
namespace _22_MonkeyMap
{
  enum Field { Empty, Open, Block };

  record Board(Field[,] Field, int CubeSize)
  {
    internal int GetFace(Pos pos)
    {
      if (pos.Y < CubeSize)
        return 1;
      if (pos.Y < 2 * CubeSize)
      {
        if (pos.X < CubeSize)
          return 2;
        if (pos.X < 2 * CubeSize)
          return 3;
        return 4;
      }

      if (pos.X < 3 * CubeSize)
        return 5;
      return 6;
    }

    internal Pos GetNextPosition(Pos pos, Direction direction)
    {
      var nextPos = GetAdjacentPos(pos, direction);

      while (Field[nextPos.X, nextPos.Y] == _22_MonkeyMap.Field.Empty)
        nextPos = GetAdjacentPos(nextPos, direction);

      if (Field[nextPos.X, nextPos.Y] == _22_MonkeyMap.Field.Block)
        return pos;

      if (Field[nextPos.X, nextPos.Y] != _22_MonkeyMap.Field.Open)
        throw new ApplicationException("unexpected");

      return nextPos;
    }

    internal (Pos pos, Direction direction) GetNextPositionCube(Pos pos, Direction direction)
    {
      var (nextPos, nextDirection) = GetAdjacentPositionCube(pos, direction);

      if (Field[nextPos.X, nextPos.Y] == _22_MonkeyMap.Field.Empty)
        throw new ApplicationException("not expected");

      if (Field[nextPos.X, nextPos.Y] == _22_MonkeyMap.Field.Block)
        return (pos, direction);

      if (Field[nextPos.X, nextPos.Y] != _22_MonkeyMap.Field.Open)
        throw new ApplicationException("unexpected");

      return (nextPos, nextDirection);
    }

    internal (Pos nextPos, Direction nextDirection) GetAdjacentPositionCube(Pos pos, Direction direction)
    {
      var nextPos = GetNextPosition(pos, direction);
      var nextDirection = direction;

      var face = GetFace(pos);
      if (face == 1)
      {
        if (pos.Y == 0 && direction == Direction.Up)
        {
          nextDirection = Direction.Down;
          nextPos.Y = CubeSize;
          nextPos.X = CubeSize - 1 - (pos.X - 2 * CubeSize);
        }
        if (pos.X == 2 * CubeSize && direction == Direction.Left)
        {
          nextDirection = Direction.Down;
          nextPos.Y = CubeSize;
          nextPos.X = CubeSize + pos.Y;
        }
        if (pos.X == 3 * CubeSize - 1 && direction == Direction.Right)
        {
          nextDirection = Direction.Left;
          nextPos.X = 4 * CubeSize - 1;
          nextPos.Y = 3 * CubeSize - 1 - pos.Y;
        }
      }
      if (face == 2)
      {
        if (pos.X == 0 && direction == Direction.Left)
        {
          nextDirection = Direction.Up;
          nextPos.Y = 3 * CubeSize - 1;
          nextPos.X = 4 * CubeSize - 1 - (pos.Y - CubeSize);
        }
        if (pos.Y == CubeSize && direction == Direction.Up)
        {
          nextDirection = Direction.Down;
          nextPos.Y = 0;
          nextPos.X = 2 * CubeSize + (CubeSize - 1 - pos.X);
        }
        if (pos.Y == 2 * CubeSize - 1 && direction == Direction.Down)
        {
          nextDirection = Direction.Up;
          nextPos.Y = 3 * CubeSize - 1;
          nextPos.X = 3 * CubeSize - 1 - pos.X;
        }
      }
      if (face == 3)
      {
        if (pos.Y == CubeSize && direction == Direction.Up)
        {
          nextDirection = Direction.Right;
          nextPos.X = 2 * CubeSize;
          nextPos.Y = pos.X - CubeSize;
        }
        if (pos.Y == 2 * CubeSize - 1 && direction == Direction.Down)
        {
          nextDirection = Direction.Right;
          nextPos.X = 2 * CubeSize;
          nextPos.Y = 3 * CubeSize - 1 - (pos.X - CubeSize);
        }
      }
      if (face == 4)
      {
        if (pos.X == 3 * CubeSize - 1 && direction == Direction.Right)
        {
          nextDirection = Direction.Down;
          nextPos.Y = 2 * CubeSize;
          nextPos.X = 3 * CubeSize + (2 * CubeSize - 1 - pos.Y);
        }
      }
      if (face == 5)
      {
        if (pos.X == 2 * CubeSize && direction == Direction.Left)
        {
          nextDirection = Direction.Up;
          nextPos.Y = 2 * CubeSize - 1;
          nextPos.X = 2 * CubeSize - 1 - (pos.Y - 2 * CubeSize);
        }
        if (pos.Y == 3 * CubeSize - 1 && direction == Direction.Down)
        {
          nextDirection = Direction.Up;
          nextPos.Y = 2 * CubeSize - 1;
          nextPos.X = 3 * CubeSize - 1 - pos.X;
        }
      }
      if (face == 6)
      {
        if (pos.Y == 2 * CubeSize && direction == Direction.Up)
        {
          nextDirection = Direction.Left;
          nextPos.X = 3 * CubeSize - 1;
          nextPos.Y = CubeSize + (4 * CubeSize - 1 - pos.X);
        }
        if (pos.X == 4 * CubeSize - 1 && direction == Direction.Right)
        {
          nextDirection = Direction.Left;
          nextPos.X = 3 * CubeSize - 1;
          nextPos.Y = 3 * CubeSize - 1 - pos.Y;
        }
        if (pos.Y == 3 * CubeSize - 1 && direction == Direction.Down)
        {
          nextDirection = Direction.Right;
          nextPos.X = 0;
          nextPos.Y = CubeSize + (4 * CubeSize - 1 - pos.X);
        }
      }

      return (nextPos, nextDirection);
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
      var player = new Player(inp.Board);
      foreach (var instruction in inp.Instructions)
        player.DoInstruction(instruction, useCube);
      return player;
    }

    internal static int GetFinalScore(string input, int cubeSize, bool useCube)
    {
      var inp = ParseInput(input, cubeSize);
      var player = new Player(inp.Board);
      foreach (var instruction in inp.Instructions)
        player.DoInstruction(instruction, useCube);

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
  }
}