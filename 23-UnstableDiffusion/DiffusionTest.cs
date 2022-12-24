using FluentAssertions;
using System.Security.Cryptography;

namespace _23_UnstableDiffusion
{
  public class DiffusionTest
  {
    [Fact]
    public void Can_parse_input()
    {
      var input = "....#..\r\n..###.#\r\n#...#.#\r\n.#...##\r\n#.###..\r\n##.#.##\r\n.#..#..\r\n";
      
      var elves = Diffusion.ParseInput(input);

      elves.Elves.Values.Should().HaveCount(22);
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(4, 0));
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(6, 5));
    }

    [Fact]
    public void Can_get_initial_instructions()
    {
      var instructions = Diffusion.GetInstructions();
      instructions.Instructions.Should().HaveCount(4);
      instructions.Instructions[0].MoveOffset.Should().Be(new Pos(0, -1));
      instructions.Instructions[1].MoveOffset.Should().Be(new Pos(0, 1));
      instructions.Instructions[2].MoveOffset.Should().Be(new Pos(-1, 0));
      instructions.Instructions[3].MoveOffset.Should().Be(new Pos(1, 0));

      instructions.Instructions[0].CheckPositions.Should().BeEquivalentTo(new Pos[] {new Pos(-1, -1), new Pos(0, -1), new Pos(1, -1)});
      instructions.Instructions[1].CheckPositions.Should().BeEquivalentTo(new Pos[] {new Pos(-1, 1), new Pos(0, 1), new Pos(1, 1)});
      instructions.Instructions[2].CheckPositions.Should().BeEquivalentTo(new Pos[] {new Pos(-1, -1), new Pos(-1, 0), new Pos(-1, 1)});
      instructions.Instructions[3].CheckPositions.Should().BeEquivalentTo(new Pos[] {new Pos(1, -1), new Pos(1, 0), new Pos(1, 1)});
    }

    [Fact]
    public void Can_schedule_next_instructions()
    {
      var instructions = Diffusion.GetInstructions();
      
      instructions.ScheduleNext();

      instructions.Instructions[0].MoveOffset.Should().Be(new Pos(0, 1));
      instructions.Instructions[1].MoveOffset.Should().Be(new Pos(-1, 0));
      instructions.Instructions[2].MoveOffset.Should().Be(new Pos(1, 0));
      instructions.Instructions[3].MoveOffset.Should().Be(new Pos(0, -1));
    }

    [Fact]
    public void Can_propose_position()
    {
      var input = ".....\r\n..##.\r\n..#..\r\n.....\r\n..##.\r\n.....";
      var elves = Diffusion.ParseInput(input);
      var instructions = Diffusion.GetInstructions();

      elves.ProposePosition(instructions);

      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(2, 1) && e.ProposedPos == new Pos(2, 0));
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(3, 1) && e.ProposedPos == new Pos(3, 0));
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(2, 2) && e.ProposedPos == new Pos(2, 3));
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(2, 4) && e.ProposedPos == new Pos(2, 3));
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(3, 4) && e.ProposedPos == new Pos(3, 3));
    }

    [Fact]
    public void Can_move_after_propose()
    {
      var input = ".....\r\n..##.\r\n..#..\r\n.....\r\n..##.\r\n.....";
      var elves = Diffusion.ParseInput(input);
      var instructions = Diffusion.GetInstructions();
      elves.ProposePosition(instructions);
      
      elves.MoveProposal();

      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(2, 0));
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(3, 0));
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(2, 2));
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(3, 3));
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(2, 4));
    }

    [Fact]
    public void Can_move_three_steps()
    {
      var input = ".....\r\n..##.\r\n..#..\r\n.....\r\n..##.\r\n.....";
      var elves = Diffusion.ParseInput(input);
      var instructions = Diffusion.GetInstructions();

      for (int n = 0; n < 3; ++n)
      {
        elves.ProposePosition(instructions);
        elves.MoveProposal();
        instructions.ScheduleNext();
      }

      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(2, 0));
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(4, 1));
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(0, 2));
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(4, 3));
      elves.Elves.Values.Should().Contain(e => e.CurrentPos == new Pos(2, 5));
    }

    [Fact]
    public void Can_get_pos_after_ten_rounds()
    {
      var input = "....#..\r\n..###.#\r\n#...#.#\r\n.#...##\r\n#.###..\r\n##.#.##\r\n.#..#..\r\n";
      var elves = Diffusion.ParseInput(input);
      var instructions = Diffusion.GetInstructions();

      for (int n = 0; n < 10; ++n)
      {
        elves.ProposePosition(instructions);
        elves.MoveProposal();
        instructions.ScheduleNext();
      }

      var boundingBox = elves.GetBoundingBox();
      boundingBox.Width.Should().Be(12);
      boundingBox.Height.Should().Be(11);
    }

    [Fact]
    public void Can_get_num_free_pos_after_ten_rounds()
    {
      var input = "....#..\r\n..###.#\r\n#...#.#\r\n.#...##\r\n#.###..\r\n##.#.##\r\n.#..#..\r\n";

      var numFreePos = Diffusion.GetNumFreePosAfter(input, 10);

      numFreePos.Should().Be(110);
    }

    [Fact]
    public void Can_get_num_moves_until_stable()
    {
      var input = "....#..\r\n..###.#\r\n#...#.#\r\n.#...##\r\n#.###..\r\n##.#.##\r\n.#..#..\r\n";

      var numMoves = Diffusion.GetNumMovesUntilStable(input);

      numMoves.Should().Be(20);
    }
  }
}