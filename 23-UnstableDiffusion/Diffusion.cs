using System.Reflection.Metadata.Ecma335;

namespace _23_UnstableDiffusion
{
  record struct Pos(int X, int Y)
  {
    internal Pos Move(Pos moveOffset)
    {
      return new Pos(X + moveOffset.X, Y + moveOffset.Y);
    }
  }

  record Elve(Pos CurrentPos, Pos ProposedPos)
  {
    public Pos CurrentPos { get; set; } = CurrentPos;
    public Pos ProposedPos { get; set; } = ProposedPos;
  }

  record Instruction(Pos MoveOffset, Pos[] CheckPositions);

  record InstructionList(List<Instruction> Instructions)
  {
    internal void ScheduleNext()
    {
      var instruction = Instructions[0];
      Instructions.RemoveAt(0);
      Instructions.Add(instruction);
    }
  }

  record BoundingBox(int Left, int Right, int Top, int Bottom)
  {
    public int Left { get; set; } = Left;
    public int Right { get; set; } = Right;
    public int Top { get; set; } = Top;
    public int Bottom { get; set; } = Bottom;

    public int Width { get => Right - Left + 1; }
    public int Height { get => Bottom - Top + 1; }
  }

  record ElveSetup(List<Elve> Elves)
  {
    internal void MoveProposal()
    {
      foreach (var elve in Elves)
      {
        var numElves = Elves.Where(e => e.ProposedPos == elve.ProposedPos).Count();
        if (numElves == 1)
          elve.CurrentPos = elve.ProposedPos;
      }
    }

    internal void ProposePosition(InstructionList instructions)
    {
      foreach (var elve in Elves)
      {
        elve.ProposedPos = elve.CurrentPos;
        if (HasNeighbours(elve.CurrentPos))
        {
          foreach (var instruction in instructions.Instructions)
          {
            if (AreAllCheckPositionsFree(elve.CurrentPos, instruction.CheckPositions))
            {
              elve.ProposedPos = elve.CurrentPos.Move(instruction.MoveOffset);
              break;
            }
          }
        }
      }
    }

    private bool HasNeighbours(Pos currentPos)
    {
      foreach (var pos in GetNeighbourPos(currentPos))
      {
        if (!IsPositionFree(pos))
          return true;
      }
      return false;
    }

    private static IEnumerable<Pos> GetNeighbourPos(Pos currentPos)
    {
      for (int x = currentPos.X - 1; x <= currentPos.X + 1; ++x)
        for (int y = currentPos.Y - 1; y <= currentPos.Y + 1; ++y)
          if (x != currentPos.X || y != currentPos.Y)
          {
            yield return new Pos(x, y);
          }
    }

    private bool AreAllCheckPositionsFree(Pos currentPos, Pos[] checkPositions)
    {
      foreach (var offset in checkPositions)
      {
        if (!IsPositionFree(currentPos.Move(offset)))
          return false;
      }
      return true;
    }

    private bool IsPositionFree(Pos pos)
    {
      return !Elves.Any(e => e.CurrentPos == pos);
    }

    internal BoundingBox GetBoundingBox()
    {
      var boundingBox = new BoundingBox(Elves.First().CurrentPos.X, Elves.First().CurrentPos.X, Elves.First().CurrentPos.Y, Elves.First().CurrentPos.Y);
      foreach (var elve in Elves)
      {
        if (elve.CurrentPos.X < boundingBox.Left)
          boundingBox.Left = elve.CurrentPos.X;
        if (elve.CurrentPos.X > boundingBox.Right)
          boundingBox.Right = elve.CurrentPos.X;
        if (elve.CurrentPos.Y < boundingBox.Top)
          boundingBox.Top = elve.CurrentPos.Y;
        if (elve.CurrentPos.Y > boundingBox.Bottom)
          boundingBox.Bottom = elve.CurrentPos.Y;
      }
      return boundingBox;
    }
  }

  internal class Diffusion
  {
    internal static InstructionList GetInstructions()
    {
      return new InstructionList(new()
      {
        new Instruction(new Pos( 0, -1), new Pos[]{ new Pos(0, -1), new Pos(1, -1), new Pos(-1, -1) }),
        new Instruction(new Pos( 0,  1), new Pos[]{ new Pos(0, 1), new Pos(1, 1), new Pos(-1, 1) }),
        new Instruction(new Pos(-1,  0), new Pos[]{ new Pos(-1, 0), new Pos(-1, -1), new Pos(-1, 1) }),
        new Instruction(new Pos( 1,  0), new Pos[]{ new Pos(1, 0), new Pos(1, -1), new Pos(1, 1) }),
      }
      );
    }

    internal static int GetNumFreePosAfter(string input, int numRounds)
    {
      var elves = ParseInput(input);
      var instructions = GetInstructions();

      for (int n = 0; n < numRounds; ++n)
      {
        elves.ProposePosition(instructions);
        elves.MoveProposal();
        instructions.ScheduleNext();
      }

      var boundingBox = elves.GetBoundingBox();
      return boundingBox.Width * boundingBox.Height - elves.Elves.Count;
    }

    internal static ElveSetup ParseInput(string input)
    {
      var elves = new List<Elve>();
      int y = 0;
      foreach (var line in input.Split('\n'))
      {
        int x = 0;
        foreach (var ch in line)
        {
          if (ch == '#')
            elves.Add(new Elve(new Pos(x, y), new Pos()));
          ++x;
        }
        ++y;
      }
      return new ElveSetup(elves);
    }
  }
}