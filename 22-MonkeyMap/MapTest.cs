using FluentAssertions;

namespace _22_MonkeyMap
{
  public class MapTest
  {
    [Fact]
    public void Can_parse_dimensions()
    {
      var input = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5\r\n";
      var dimensions = Map.GetDimensions(input);
      dimensions.Should().Be(new(16, 12));
    }

    [Fact]
    public void Can_parse_input()
    {
      var input = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5\r\n";
      var inp = Map.ParseInput(input);

      inp.Board.Field.GetLength(0).Should().Be(16);
      inp.Board.Field.GetLength(1).Should().Be(12);

      inp.Board.Field[0, 0].Should().Be(Field.Empty);
      inp.Board.Field[8, 0].Should().Be(Field.Open);

      inp.Instructions.Should().HaveCount(13);
    }

    [Fact]
    public void Can_parse_instructions()
    {
      var input = "10R5L5R10L4R5L5";
      var instructions = Map.ParseInstructions(input);
      instructions.Should().Equal(new Instruction[] {
        new MoveInstruction(10),
        new TurnInstruction(Direction.Right),
        new MoveInstruction(5),
        new TurnInstruction(Direction.Left),
        new MoveInstruction(5),
        new TurnInstruction(Direction.Right),
        new MoveInstruction(10),
        new TurnInstruction(Direction.Left),
        new MoveInstruction(4),
        new TurnInstruction(Direction.Right),
        new MoveInstruction(5),
        new TurnInstruction(Direction.Left),
        new MoveInstruction(5),
      });
    }

    [Theory]
    [InlineData(8, 0, Direction.Right, 9, 0)]
    [InlineData(0, 0, Direction.Down, 0, 4)]
    [InlineData(10, 0, Direction.Right, 10, 0)]
    [InlineData(9, 0, Direction.Down, 9, 0)]
    [InlineData(8, 1, Direction.Left, 11, 1)]
    [InlineData(0, 4, Direction.Up, 0, 7)]
    [InlineData(0, 5, Direction.Up, 0, 4)]
    [InlineData(0, 4, Direction.Left, 0, 4)]
    public void Can_get_next_position(int startX, int startY, Direction direction, int expectedX, int expectedY)
    {
      var input = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5\r\n";
      var inp = Map.ParseInput(input);

      var pos = inp.Board.GetNextPosition(new Pos(startX, startY), direction);
      pos.Should().Be(new Pos(expectedX, expectedY));
    }

    [Fact]
    public void Can_initialize_player()
    {
      var input = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5\r\n";
      var inp = Map.ParseInput(input);

      var player = new Player(inp.Board);
      player.Direction.Should().Be(Direction.Right);
      player.Pos.Should().Be(new Pos(8, 0));
    }

    [Fact]
    public void Player_can_move()
    {
      var input = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5\r\n";
      var inp = Map.ParseInput(input);

      var player = new Player(inp.Board);
      player.DoInstruction(new MoveInstruction(5));
      player.Pos.Should().Be(new Pos(10, 0));
    }

    [Theory]
    [InlineData(Direction.Left, Direction.Left, Direction.Down)]
    [InlineData(Direction.Left, Direction.Right, Direction.Up)]
    [InlineData(Direction.Right, Direction.Left, Direction.Up)]
    [InlineData(Direction.Right, Direction.Right, Direction.Down)]
    [InlineData(Direction.Up, Direction.Left, Direction.Left)]
    [InlineData(Direction.Up, Direction.Right, Direction.Right)]
    [InlineData(Direction.Down, Direction.Left, Direction.Right)]
    [InlineData(Direction.Down, Direction.Right, Direction.Left)]
    public void Can_turn_into_direction(Direction currentDirection, Direction turnDirection, Direction expectedDirection)
    {
      var newDirection = Player.GetNextDirection(currentDirection, turnDirection);

      newDirection.Should().Be(expectedDirection);
    }

    [Fact]
    public void Can_get_final_player()
    {
      var input = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5\r\n";
      var player = Map.GetFinalPlayer(input);
      player.Pos.Should().Be(new Pos(7, 5));
      player.Direction.Should().Be(Direction.Right);
    }

    [Theory]
    [InlineData(Direction.Right, 0)]
    [InlineData(Direction.Down, 1)]
    [InlineData(Direction.Left, 2)]
    [InlineData(Direction.Up, 3)]
    public void Can_get_Direction_score(Direction direction, int expectedScore)
    {
      var score = Player.GetDirectionScore(direction);
      score.Should().Be(expectedScore);
    }

    [Fact]
    public void Can_get_final_score()
    {
      var input = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5\r\n";
      var score = Map.GetFinalScore(input);
      score.Should().Be(6032);
    }
  }
}