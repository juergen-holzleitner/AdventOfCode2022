
namespace _22_MonkeyMap
{
  enum Field { Empty, Open, Block };

  record Board(Field[,] Field)
  {
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

    internal static Player GetFinalPlayer(string input)
    {
      var inp = ParseInput(input);
      var player = new Player(inp.Board);
      foreach (var instruction in inp.Instructions)
        player.DoInstruction(instruction);
      return player;
    }

    internal static int GetFinalScore(string input)
    {
      var inp = ParseInput(input);
      var player = new Player(inp.Board);
      foreach (var instruction in inp.Instructions)
        player.DoInstruction(instruction);

      return player.GetScore();
    }

    internal static Input ParseInput(string input)
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

      return new Input(new Board(board), instructions);
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