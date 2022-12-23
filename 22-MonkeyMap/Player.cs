using System.Reflection.Metadata.Ecma335;

namespace _22_MonkeyMap
{
  record struct Pos(int X, int Y)
  {
    public int X { get; set; } = X;
    public int Y { get; set; } = Y;

    internal bool IsBorder(Direction direction, int cubeSize)
    {
      return direction switch
      {
        Direction.Up when Y % cubeSize == 0 => true,
        Direction.Left when X % cubeSize == 0 => true,
        Direction.Down when (Y + 1) % cubeSize == 0 => true,
        Direction.Right when (X + 1) % cubeSize == 0 => true,
        _ => false
      };
    }
  }

  internal class Player
  {
    private Board board;

    public Player(Board board)
    {
      this.board = board;
      Pos = board.GetNextPosition(new Pos(0, 0), Direction.Right);
    }

    public Direction Direction { get; private set; } = Direction.Right;

    public Pos Pos { get; private set; }

    internal void DoInstruction(Instruction instruction, bool useCube)
    {
      if (instruction is MoveInstruction move)
      {
        for (int n = 0; n < move.Num; ++n)
        {
          if (useCube)
          {
            (Pos, Direction) = board.GetNextPositionCube(Pos, Direction);
          }
          else
            Pos = board.GetNextPosition(Pos, Direction);
        }
      }
      else if (instruction is TurnInstruction turn)
      {
        Direction = GetNextDirection(Direction, turn.Direction);
      }
      else
        throw new ApplicationException("unexpected");
    }

    public static Direction GetNextDirection(Direction currentDirection, Direction turnDirection)
    {
      return currentDirection switch
      {
        Direction.Left => turnDirection == Direction.Left ? Direction.Down : Direction.Up,
        Direction.Right => turnDirection == Direction.Left ? Direction.Up : Direction.Down,
        Direction.Up => turnDirection == Direction.Left ? Direction.Left : Direction.Right,
        Direction.Down => turnDirection == Direction.Left ? Direction.Right : Direction.Left,
        _ => throw new ApplicationException("unexpected")
      };
    }

    internal static int GetDirectionScore(Direction direction)
    {
      return direction switch
      {
        Direction.Right => 0,
        Direction.Down => 1,
        Direction.Left => 2,
        Direction.Up => 3,
        _ => throw new ApplicationException("unexpected")
      };
    }

    internal int GetScore()
    {
      var score = 1000 * (Pos.Y + 1) + 4 * (Pos.X + 1) + GetDirectionScore(Direction);
      return score;
    }

  }
}