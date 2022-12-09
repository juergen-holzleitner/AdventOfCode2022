using FluentAssertions;

namespace _09_RopeBridge
{
  public class RopeBridgeTest
  {
    [Theory]
    [InlineData("R 4", Direction.Right, 4)]
    [InlineData("U 4", Direction.Up, 4)]
    [InlineData("L 3", Direction.Left, 3)]
    [InlineData("D 1", Direction.Down, 1)]
    public void Can_parse_input_line(string line, Direction expectedDirection, int expectedSteps)
    {
      var motion = RopeBridge.Parse(line);

      motion.Should().Be(new Motion(expectedDirection, expectedSteps));
    }

    [Fact]
    public void Get_exception_if_empty_line()
    {
      var line = "";
      var action = () => RopeBridge.Parse(line);
      action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Initial_head_position_is_zero()
    {
      var sut = new RopeBridge();

      var pos = sut.GetHeadPosition();

      pos.Should().Be(new Position(0, 0));
    }

    [Fact]
    public void Initial_tail_position_is_zero()
    {
      var sut = new RopeBridge();

      var pos = sut.GetTailPosition();

      pos.Should().Be(new Position(0, 0));
    }

    [Theory]
    [InlineData(Direction.Right, 1, 0)]
    [InlineData(Direction.Up, 0, -1)]
    [InlineData(Direction.Down, 0, 1)]
    [InlineData(Direction.Left, -1, 0)]
    public void Can_move(Direction direction, int expectedX, int expectedY)
    {
      var sut = new RopeBridge();

      sut.Move(direction);

      var pos = sut.GetHeadPosition();
      pos.Should().Be(new Position(expectedX, expectedY));
    }

    [Fact]
    public void Can_move_motion()
    {
      var line = "R 4";
      var motion = RopeBridge.Parse(line);
      var sut = new RopeBridge();

      sut.Move(motion);

      var pos = sut.GetHeadPosition();
      pos.Should().Be(new Position(4, 0));
    }

    [Fact]
    public void Can_move_input()
    {
      var input = "R 4\r\nU 4\r\nL 3\r\nD 1\r\nR 4\r\nD 1\r\nL 5\r\nR 2\r\n";
      var sut = new RopeBridge();

      sut.MoveInput(input.Split('\n'));

      var pos = sut.GetHeadPosition();
      pos.Should().Be(new Position(2, -2));
      pos = sut.GetTailPosition();
      pos.Should().Be(new Position(1, -2));
    }

    [Theory]
    [InlineData("R 2", 1, 0)]
    [InlineData("D 2", 0, 1)]
    public void Tail_follows_head(string input, int expectedX, int expectedY)
    {
      var sut = new RopeBridge();

      sut.MoveInput(input.Split('\n'));

      var pos = sut.GetTailPosition();
      pos.Should().Be(new Position(expectedX, expectedY));
    }

    [Fact]
    public void Visited_tail_positions_is_initially_the_start()
    {
      var sut = new RopeBridge();
      var visitedTailPositions = sut.GetVisitedTailPositions();
      visitedTailPositions.Should().BeEquivalentTo(new[] { new Position(0, 0) });
    }

    [Fact]
    public void Can_get_visited_tail_positions()
    {
      var sut = new RopeBridge();

      sut.MoveInput(new List<string>() { "R 2" });

      var visitedTailPositions = sut.GetVisitedTailPositions();
      visitedTailPositions.Should().BeEquivalentTo(new[] { new Position(0, 0), new Position(1, 0) });
    }

    [Fact]
    public void Can_process_sample_input()
    {
      var input = "R 4\r\nU 4\r\nL 3\r\nD 1\r\nR 4\r\nD 1\r\nL 5\r\nR 2\r\n";
      var sut = new RopeBridge();

      sut.MoveInput(input.Split('\n'));

      var visitedTailPositions = sut.GetVisitedTailPositions();

      visitedTailPositions.Should().HaveCount(13);
    }
  }
}