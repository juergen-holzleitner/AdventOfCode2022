using FluentAssertions;

namespace _24_BlizzardBasin
{
  public class BasinTest
  {
    private static void CheckBlizzardContained(BlizzardMap blizzards, Pos pos, Direction direction)
    {
      blizzards.Blizzards.Should().Contain(b => b.Key == pos && b.Value.Contains(direction));
    }

    [Fact]
    public void Can_parse_input()
    {
      var text = "#.#####\r\n#.....#\r\n#>....#\r\n#.....#\r\n#...v.#\r\n#.....#\r\n#####.#\r\n";

      var blizzardMap = Blizzard.Parse(text);

      blizzardMap.StartPos.Should().Be(new Pos(0, -1));
      blizzardMap.EndPos.Should().Be(new Pos(4, 5));

      blizzardMap.Size.Should().Be(new Pos(5, 5));

      blizzardMap.Blizzards.Should().HaveCount(2);
      CheckBlizzardContained(blizzardMap, new Pos(0, 1), Direction.Right);
      CheckBlizzardContained(blizzardMap, new Pos(3, 3), Direction.Down);
    }

    [Fact]
    public void Can_move_blizzards()
    {
      var text = "#.#####\r\n#.....#\r\n#>....#\r\n#.....#\r\n#...v.#\r\n#.....#\r\n#####.#\r\n";
      var blizzardMap = Blizzard.Parse(text);

      blizzardMap.SingleStep();

      CheckBlizzardContained(blizzardMap, new Pos(1, 1), Direction.Right);
      CheckBlizzardContained(blizzardMap, new Pos(3, 4), Direction.Down);
    }

    [Fact]
    public void Can_move_blizzards_around_board()
    {
      var text = "#.#####\r\n#.....#\r\n#>....#\r\n#.....#\r\n#...v.#\r\n#.....#\r\n#####.#\r\n";
      var blizzardMap = Blizzard.Parse(text);

      blizzardMap.SingleStep();
      blizzardMap.SingleStep();

      CheckBlizzardContained(blizzardMap, new Pos(2, 1), Direction.Right);
      CheckBlizzardContained(blizzardMap, new Pos(3, 0), Direction.Down);
    }

    [Fact]
    public void Can_move_blizzards_initial_sample()
    {
      var text = "#.#####\r\n#.....#\r\n#>....#\r\n#.....#\r\n#...v.#\r\n#.....#\r\n#####.#\r\n";
      var blizzardMap = Blizzard.Parse(text);

      for (int step = 0; step < 5; ++step)
        blizzardMap.SingleStep();

      CheckBlizzardContained(blizzardMap, new Pos(0, 1), Direction.Right);
      CheckBlizzardContained(blizzardMap, new Pos(3, 3), Direction.Down);
    }

    [Fact]
    public void Can_get_follow_up_states()
    {
      var text = "#E######\r\n#>>.<^<#\r\n#.<..<<#\r\n#>v.><>#\r\n#<^v^^>#\r\n######.#";
      var blizzardMap = Blizzard.Parse(text);

      var playerPositions = new HashSet<Pos> { blizzardMap.StartPos };

      blizzardMap.SingleStep();
      playerPositions = blizzardMap.GetNextPlayerPositions(playerPositions, blizzardMap);

      playerPositions.Should().BeEquivalentTo(new[] { new Pos(0, -1), new Pos(0, 0) });
    }

    [Fact]
    public void Can_get_num_steps_for_first_part()
    {
      var text = "#E######\r\n#>>.<^<#\r\n#.<..<<#\r\n#>v.><>#\r\n#<^v^^>#\r\n######.#";

      var numSteps = Blizzard.GetNumStepsToExit(text);

      numSteps.Should().Be(18);
    }

    [Fact]
    public void Can_get_num_steps_for_second_part()
    {
      var text = "#E######\r\n#>>.<^<#\r\n#.<..<<#\r\n#>v.><>#\r\n#<^v^^>#\r\n######.#";

      var numSteps = Blizzard.GetNumStepsToExitBackAndExitAgain(text);

      numSteps.Should().Be(54);
    }

  }
}