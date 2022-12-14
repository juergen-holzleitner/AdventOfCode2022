using FluentAssertions;

namespace _14_RegolithReservoir
{
  public class RegolithTest
  {
    [Fact]
    public void Can_parse_single_pos()
    {
      string line = "498,4";

      var walls = RegolithReservoir.ParseLine(line);

      walls.Positions.Should().BeEquivalentTo(new Pos[] { new Pos(498, 4) });
    }

    [Fact]
    public void Can_parse_single_line()
    {
      string line = "498,4 -> 498,6 -> 496,6";

      var walls = RegolithReservoir.ParseLine(line);

      walls.Positions.Should().BeEquivalentTo(new Pos[] { new Pos(498, 4), new Pos(498, 6), new Pos(496, 6) });
    }

    [Fact]
    public void Can_parse_cave()
    {
      var lines = "498,4 -> 498,6 -> 496,6\r\n503,4 -> 502,4 -> 502,9 -> 494,9";

      var cave = RegolithReservoir.ParseCave(lines);

      cave.Walls.Should().HaveCount(2);
      cave.Walls[1].Positions.Should().BeEquivalentTo(new Pos[] { new Pos(503, 4), new Pos(502, 4), new Pos(502, 9), new Pos(494, 9) });
    }

    [Fact]
    public void Can_get_max_vertical_position()
    {
      var lines = "498,4 -> 498,6 -> 496,6\r\n503,4 -> 502,4 -> 502,9 -> 494,9";
      var cave = RegolithReservoir.ParseCave(lines);

      var maxVerticalPosition = RegolithReservoir.GetMaxVerticalPosition(cave);

      maxVerticalPosition.Should().Be(9);
    }

    [Theory]
    [InlineData(500, 0, false)]
    [InlineData(497, 6, true)]
    [InlineData(498, 3, false)]
    [InlineData(502, 10, false)]
    [InlineData(495, 6, false)]
    [InlineData(499, 6, false)]
    public void Can_check_if_pos_is_blocked(int x, int y, bool expectedResult)
    {
      var lines = "498,4 -> 498,6 -> 496,6\r\n503,4 -> 502,4 -> 502,9 -> 494,9";
      var cave = RegolithReservoir.ParseCave(lines);
      var pos = new Pos(x, y);

      var isBlocked = RegolithReservoir.IsPositionBlockedByWall(pos, cave);

      isBlocked.Should().Be(expectedResult);
    }

    [Fact]
    public void Can_check_sand_position()
    {
      var sandPositions = new List<Pos>();
      var pos = new Pos(500, 8);

      var isBlocked = RegolithReservoir.IsPositionBlockedBySand(pos, sandPositions);

      isBlocked.Should().BeFalse();
    }

    [Fact]
    public void Can_check_sand_position_blocked()
    {
      var sandPositions = new List<Pos>
      {
        new Pos(500, 8)
      };

      var pos = new Pos(500, 8);

      var isBlocked = RegolithReservoir.IsPositionBlockedBySand(pos, sandPositions);

      isBlocked.Should().BeTrue();
    }

    [Fact]
    public void Can_add_sand()
    {
      var lines = "498,4 -> 498,6 -> 496,6\r\n503,4 -> 502,4 -> 502,9 -> 494,9";
      var cave = RegolithReservoir.ParseCave(lines);
      var sandPositions = new List<Pos>();

      bool wasAdded = RegolithReservoir.AddSand(cave, sandPositions, false, RegolithReservoir.GetMaxVerticalPosition(cave));

      wasAdded.Should().BeTrue();
      sandPositions.Should().BeEquivalentTo(new Pos[] { new Pos(500, 8) });
    }

    [Theory]
    [InlineData(1, 500, 8)]
    [InlineData(2, 499, 8)]
    [InlineData(3, 501, 8)]
    [InlineData(4, 500, 7)]
    [InlineData(5, 498, 8)]
    [InlineData(22, 500, 2)]
    [InlineData(23, 497, 5)]
    [InlineData(24, 495, 8)]
    public void Can_add_sand_multiple_times(int numSand, int lastX, int lastY)
    {
      var lines = "498,4 -> 498,6 -> 496,6\r\n503,4 -> 502,4 -> 502,9 -> 494,9";
      var cave = RegolithReservoir.ParseCave(lines);
      var sandPositions = new List<Pos>();

      for (int n = 0; n < numSand; ++n)
      {
        bool wasAdded = RegolithReservoir.AddSand(cave, sandPositions, false, RegolithReservoir.GetMaxVerticalPosition(cave));
        wasAdded.Should().BeTrue();
      }
      sandPositions.Should().HaveCount(numSand);
      sandPositions.Should().Contain(new Pos(lastX, lastY));
    }

    [Fact]
    public void Can_get_num_sands_added_without_blocking()
    {
      var lines = "498,4 -> 498,6 -> 496,6\r\n503,4 -> 502,4 -> 502,9 -> 494,9";

      var numSands = RegolithReservoir.GetNumSandsAdded(lines, false);

      numSands.Should().Be(24);
    }

    [Fact]
    public void Can_get_num_sands_added_with_blocking()
    {
      var lines = "498,4 -> 498,6 -> 496,6\r\n503,4 -> 502,4 -> 502,9 -> 494,9";

      var numSands = RegolithReservoir.GetNumSandsAdded(lines, true);

      numSands.Should().Be(93);
    }
  }
}