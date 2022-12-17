using FluentAssertions;

namespace _17_PyroclasticFlow
{
  public class FlowTest
  {
    [Theory]
    [InlineData(1, ShapeType.Dash)]
    [InlineData(2, ShapeType.Plus)]
    [InlineData(3, ShapeType.L)]
    [InlineData(4, ShapeType.I)]
    [InlineData(5, ShapeType.Square)]
    [InlineData(6, ShapeType.Dash)]
    public void Can_get_next_shape(int numElement, ShapeType expectedShape)
    {
      var shapes = Flow.GetShapes();
      var enumerator = shapes.GetEnumerator();

      for (int n = 0; n < numElement; ++n)
        enumerator.MoveNext().Should().BeTrue();

      enumerator.Current.Should().Be(expectedShape);
    }

    [Theory]
    [InlineData(1, MoveType.Right)]
    [InlineData(2, MoveType.Left)]
    [InlineData(3, MoveType.Right)]
    [InlineData(4, MoveType.Right)]
    public void Can_get_next_move(int numElement, MoveType expectedMove)
    {
      var input = "><>\r\n";
      var moves = Flow.GetMoves(input);
      var enumerator = moves.GetEnumerator();

      for (int n = 0; n < numElement; ++n)
        enumerator.MoveNext().Should().BeTrue();
      
      enumerator.Current.Should().Be(expectedMove);
    }

    [Fact]
    public void Chamber_has_initial_state()
    {
      var input = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
      var sut = new Chamber(input);

      sut.CurrentShape.Should().BeNull();
      sut.CurrentTotalHeight.Should().Be(0);
    }

    [Theory]
    [InlineData(1, 3, 2)]
    [InlineData(2, 3, 1)]
    [InlineData(3, 3, 0)]
    [InlineData(5, 1, 3)]
    [InlineData(6, 2, 2)]
    [InlineData(7, 1, 1)]
    public void Chamber_can_make_a_step(int numSteps, int expectedX, int expectedY)
    {
      var input = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
      var chamber = new Chamber(input);

      for (int n = 0; n < numSteps; ++n)
        chamber.DoStep();

      chamber.CurrentShape.Should().NotBeNull();
      chamber.CurrentShape!.Pos.Should().Be(new Pos(expectedX, expectedY));
    }

    [Theory]
    [InlineData(4, 2, 0)]
    [InlineData(8, 2, 1)]
    public void Chamber_can_get_last_stopped(int numSteps, int expectedX, int expectedY)
    {
      var input = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
      var chamber = new Chamber(input);

      for (int n = 0; n < numSteps; ++n)
        chamber.DoStep();

      chamber.StoppedShapes.Last().Pos.Should().Be(new Pos(expectedX, expectedY));
    }

    [Fact]
    public void Can_enumerate_dash_elements()
    {
      var elements = Flow.GetShapeElements(ShapeType.Dash, new Pos(0, 0));

      elements.Should().BeEquivalentTo(new[] {new Pos(0, 0), new Pos(1, 0), new Pos(2, 0), new Pos(3, 0)});
    }

    [Fact]
    public void Can_enumerate_plus_elements()
    {
      var elements = Flow.GetShapeElements(ShapeType.Plus, new Pos(1, 1));

      elements.Should().BeEquivalentTo(new[] { new Pos(2, 1), new Pos(1, 2), new Pos(2, 2), new Pos(3, 2), new Pos(2, 3) });
    }

    [Fact]
    public void Can_enumerate_L_elements()
    {
      var elements = Flow.GetShapeElements(ShapeType.L, new Pos(2, 3));

      elements.Should().BeEquivalentTo(new[] { new Pos(2, 3), new Pos(3, 3), new Pos(4, 3), new Pos(4, 4), new Pos(4, 5) });
    }

    [Fact]
    public void Can_enumerate_I_elements()
    {
      var elements = Flow.GetShapeElements(ShapeType.I, new Pos(3, 3));

      elements.Should().BeEquivalentTo(new[] { new Pos(3, 3), new Pos(3, 4), new Pos(3, 5), new Pos(3, 6) });
    }

    [Fact]
    public void Can_enumerate_Square_elements()
    {
      var elements = Flow.GetShapeElements(ShapeType.Square, new Pos(4, 5));

      elements.Should().BeEquivalentTo(new[] { new Pos(4, 5), new Pos(5, 5), new Pos(4, 6), new Pos(5, 6) });
    }

    [Theory]
    [InlineData(1, ShapeType.Dash, 2, 0)]
    [InlineData(2, ShapeType.Plus, 2, 1)]
    [InlineData(3, ShapeType.L, 0, 3)]
    [InlineData(4, ShapeType.I, 4, 3)]
    [InlineData(5, ShapeType.Square, 4, 7)]
    [InlineData(6, ShapeType.Dash, 1, 9)]
    [InlineData(7, ShapeType.Plus, 1, 10)]
    [InlineData(8, ShapeType.L, 3, 12)]
    [InlineData(9, ShapeType.I, 4, 13)]
    [InlineData(10, ShapeType.Square, 0, 12)]
    public void Chamber_can_stop_at_elemenat(int numElement, ShapeType expectedLastStoppedShapeType, int expectedX, int expectedY)
    {
      var input = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
      var chamber = new Chamber(input);

      for (int n = 0; n < numElement; ++n)
        chamber.DoStepUntilNextStopped();

      chamber.StoppedShapes.Last().Should().Be(new Shape(expectedLastStoppedShapeType, new Pos(expectedX, expectedY)));
    }

    [Fact]
    public void Can_get_height_after_10_steps()
    {
      var input = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
      var height = Chamber.GetHeightAfterElements(input, 10);
      height.Should().Be(17);
    }

    [Fact]
    public void Can_get_height_of_sample()
    {
      var input = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
      var height = Chamber.GetHeightAfterElements(input, 2022);
      height.Should().Be(3068);
    }
  }
}