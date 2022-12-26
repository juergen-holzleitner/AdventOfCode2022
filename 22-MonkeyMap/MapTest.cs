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
      var inp = Map.ParseInput(input, 4);

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
      var inp = Map.ParseInput(input, 4);

      var pos = inp.Board.GetNextPosition(new Pos(startX, startY), direction);
      pos.Should().Be(new Pos(expectedX, expectedY));
    }

    [Fact]
    public void Can_initialize_player()
    {
      var input = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5\r\n";
      var inp = Map.ParseInput(input, 4);

      var player = new Player(inp.Board, inp.Board.GetStartPosition());
      player.Direction.Should().Be(Direction.Right);
      player.Pos.Should().Be(new Pos(8, 0));
    }

    [Fact]
    public void Player_can_move()
    {
      var input = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5\r\n";
      var inp = Map.ParseInput(input, 4);

      var player = new Player(inp.Board, inp.Board.GetStartPosition());
      player.DoInstruction(new MoveInstruction(5), false);
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
      var player = Map.GetFinalPlayer(input, 4, false);
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
      var score = Map.GetFinalScore(input, 4, false);
      score.Should().Be(6032);
    }

    [Fact]
    public void Can_get_final_player_cube()
    {
      var input = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5\r\n";
      var player = Map.GetFinalPlayer(input, 4, true);
      player.Pos.Should().Be(new Pos(6, 4));
      player.Direction.Should().Be(Direction.Up);
    }

    [Fact]
    public void Can_get_final_score_cube()
    {
      var input = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5\r\n";
      var score = Map.GetFinalScore(input, 4, true);
      score.Should().Be(5031);
    }

    [Fact]
    public void Can_fold_cube_from_example()
    {
      var text = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5\r\n";
      var input = Map.ParseInput(text, 4);
      var startPos = input.Board.GetStartPosition();
      startPos.Should().Be(new Pos(8, 0));

      var cubeSetup = Map.FoldToCube(input.Board, startPos);
      cubeSetup.Faces.Should().Contain(f => f.Key == new Vector(0, 1, 0) && f.Value.TopLeft2DPos == new Pos(8, 0)); // top
      cubeSetup.Faces.Should().Contain(f => f.Key == new Vector(0, 0, -1) && f.Value.TopLeft2DPos == new Pos(8, 4)); // front
      cubeSetup.Faces.Should().Contain(f => f.Key == new Vector(0, 0, 1) && f.Value.TopLeft2DPos == new Pos(0, 4)); // back
      cubeSetup.Faces.Should().Contain(f => f.Key == new Vector(0, -1, 0) && f.Value.TopLeft2DPos == new Pos(8, 8)); // bottom
      cubeSetup.Faces.Should().Contain(f => f.Key == new Vector(-1, 0, 0) && f.Value.TopLeft2DPos == new Pos(4, 4)); // left
      cubeSetup.Faces.Should().Contain(f => f.Key == new Vector(1, 0, 0) && f.Value.TopLeft2DPos == new Pos(12, 8)); // right
    }

    [Fact]
    public void Can_fold_cube_from_part2()
    {
      var text = "    ........\r\n    ........\r\n    ........\r\n    ........\r\n    ....\r\n    ....\r\n    ....\r\n    ....\r\n........\r\n........\r\n........\r\n........\r\n....\r\n....\r\n....\r\n....";
      var input = Map.ParseInput(text, 4);
      var player = new Player(input.Board, input.Board.GetStartPosition());
      player.Pos.Should().Be(new Pos(4, 0));

      var cubeSetup = Map.FoldToCube(input.Board, player.Pos);
      cubeSetup.Faces.Should().Contain(f => f.Key == new Vector(0, 1, 0) && f.Value.TopLeft2DPos == new Pos(4, 0)); // top
      cubeSetup.Faces.Should().Contain(f => f.Key == new Vector(0, 0, -1) && f.Value.TopLeft2DPos == new Pos(4, 4)); // front
      cubeSetup.Faces.Should().Contain(f => f.Key == new Vector(0, 0, 1) && f.Value.TopLeft2DPos == new Pos(0, 12)); // back
      cubeSetup.Faces.Should().Contain(f => f.Key == new Vector(0, -1, 0) && f.Value.TopLeft2DPos == new Pos(4, 8)); // bottom
      cubeSetup.Faces.Should().Contain(f => f.Key == new Vector(-1, 0, 0) && f.Value.TopLeft2DPos == new Pos(0, 8)); // left
      cubeSetup.Faces.Should().Contain(f => f.Key == new Vector(1, 0, 0) && f.Value.TopLeft2DPos == new Pos(8, 0)); // right
    }

    /*
          1111
          1111
          1111
          1111
    222233334444
    222233334444
    222233334444
    222233334444
          55556666
          55556666
          55556666
          55556666
    */
    [Theory]
    [InlineData(8, 0, Direction.Right, 9, 0, Direction.Right)]
    [InlineData(8, 3, Direction.Down, 8, 4, Direction.Down)]
    [InlineData(8, 0, Direction.Left, 4, 4, Direction.Down)]
    [InlineData(8, 0, Direction.Up, 3, 4, Direction.Down)]
    [InlineData(11, 2, Direction.Right, 15, 9, Direction.Left)]
    [InlineData(0, 5, Direction.Left, 14, 11, Direction.Up)]
    [InlineData(3, 4, Direction.Up, 8, 0, Direction.Down)]
    [InlineData(1, 7, Direction.Down, 10, 11, Direction.Up)]
    [InlineData(7, 4, Direction.Up, 8, 3, Direction.Right)]
    [InlineData(4, 7, Direction.Down, 8, 11, Direction.Right)]
    [InlineData(11, 5, Direction.Right, 14, 8, Direction.Down)]
    [InlineData(8, 8, Direction.Left, 7, 7, Direction.Up)]
    [InlineData(10, 11, Direction.Down, 1, 7, Direction.Up)]
    [InlineData(14, 8, Direction.Up, 11, 5, Direction.Left)]
    [InlineData(15, 10, Direction.Right, 11, 1, Direction.Left)]
    [InlineData(14, 11, Direction.Down, 0, 5, Direction.Right)]
    public void Can_get_next_cube_position_auto_fold_from_sample(int X, int Y, Direction direction, int expectedX, int expectedY, Direction expectedDirection)
    {
      var text = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5\r\n";
      var input = Map.ParseInput(text, 4);
      var pos = new Pos(X, Y);

      var cubeSetup = Map.FoldToCube(input.Board, input.Board.GetStartPosition());

      var (nextPos, nextDirection) = input.Board.GetAdjacentPositionCube(pos, direction, cubeSetup);

      nextPos.Should().Be(new Pos(expectedX, expectedY));
      nextDirection.Should().Be(expectedDirection);
    }

    /*
        11112222
        11112222
        11112222
        11112222
        3333
        3333
        3333
        3333
    44445555
    44445555
    44445555
    44445555
    6666
    6666
    6666
    6666
    */
    [Theory]
    [InlineData(4, 4, Direction.Left, 0, 8, Direction.Down)] // 3 left
    [InlineData(3, 8, Direction.Up, 4, 7, Direction.Right)] // 4 up
    [InlineData(8, 3, Direction.Down, 7, 4, Direction.Left)] // 2 down
    [InlineData(7, 7, Direction.Right, 11, 3, Direction.Up)] // 3 right
    [InlineData(4, 11, Direction.Down, 3, 12, Direction.Left)] // 5 down
    [InlineData(3, 15, Direction.Right, 7, 11, Direction.Up)] // 6 right
    [InlineData(4, 0, Direction.Left, 0, 11, Direction.Right)] // 1 left
    [InlineData(0, 8, Direction.Left, 4, 3, Direction.Right)] // 4 left
    [InlineData(4, 0, Direction.Up, 0, 12, Direction.Right)] // 1 up
    [InlineData(0, 15, Direction.Left, 7, 0, Direction.Down)] // 6 left
    [InlineData(0, 15, Direction.Down, 8, 0, Direction.Down)] // 6 down
    [InlineData(11, 0, Direction.Up, 3, 15, Direction.Up)] // 2 up
    [InlineData(11, 0, Direction.Right, 7, 11, Direction.Left)] // 2 right
    [InlineData(7, 8, Direction.Right, 11, 3, Direction.Left)] // 5 right
    public void Can_get_next_cube_position_auto_fold_from_real(int X, int Y, Direction direction, int expectedX, int expectedY, Direction expectedDirection)
    {
      var text = "    ........\r\n    ........\r\n    ........\r\n    ........\r\n    ....\r\n    ....\r\n    ....\r\n    ....\r\n........\r\n........\r\n........\r\n........\r\n....\r\n....\r\n....\r\n....";
      var input = Map.ParseInput(text, 4);
      var pos = new Pos(X, Y);

      var cubeSetup = Map.FoldToCube(input.Board, input.Board.GetStartPosition());

      var (nextPos, nextDirection) = input.Board.GetAdjacentPositionCube(pos, direction, cubeSetup);

      nextPos.Should().Be(new Pos(expectedX, expectedY));
      nextDirection.Should().Be(expectedDirection);
    }
  }
}