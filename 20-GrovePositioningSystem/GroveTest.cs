using FluentAssertions;

namespace _20_GrovePositioningSystem
{
  public class GroveTest
  {
    [Fact]
    public void Can_read_input()
    {
      var inputData = "1\r\n2\r\n-3\r\n3\r\n-2\r\n0\r\n4\r\n";
      var input = Grove.ParseInput(inputData, 1);

      input.Elements.Should().HaveCount(7);
      input.Elements[0].Value.Should().Be(1);
      input.Elements[0].Position.Should().Be(0);
    }

    [Fact]
    public void Can_move_element_with_value_zero()
    {
      var inputData = "1\r\n2\r\n-3\r\n3\r\n-2\r\n0\r\n4\r\n";
      var input = Grove.ParseInput(inputData, 1);
      Grove.MoveElement(input, 5);

      input.Elements[5].Value.Should().Be(0);
    }

    [Fact]
    public void Can_move_element_with_positive_value()
    {
      var inputData = "1\r\n2\r\n-3\r\n3\r\n-2\r\n0\r\n4\r\n";
      var input = Grove.ParseInput(inputData, 1);
      Grove.MoveElement(input, 0);

      input.Elements[1].Value.Should().Be(1);
    }

    [Fact]
    public void Can_move_element_with_negative_value()
    {
      var inputData = "1\r\n2\r\n-3\r\n3\r\n-2\r\n0\r\n4\r\n";
      var input = Grove.ParseInput(inputData, 1);
      Grove.MoveElement(input, 4);

      input.Elements[2].Value.Should().Be(-2);
    }

    [Fact]
    public void Can_move_element_with_positive_value_around()
    {
      var inputData = "1\r\n2\r\n-3\r\n3\r\n-2\r\n0\r\n4\r\n";
      var input = Grove.ParseInput(inputData, 1);
      Grove.MoveElement(input, 6);

      input.Elements[3].Value.Should().Be(4);
    }

    [Fact]
    public void Can_move_element_with_negative_value_around()
    {
      var inputData = "1\r\n2\r\n-3\r\n3\r\n-2\r\n0\r\n4\r\n";
      var input = Grove.ParseInput(inputData, 1);
      Grove.MoveElement(input, 2);

      input.Elements[6].Value.Should().Be(-3);
    }

    [Fact]
    public void Can_move_element_with_negative_value_around_two()
    {
      var inputData = "1\r\n-3\r\n2\r\n3\r\n-2\r\n0\r\n4";
      var input = Grove.ParseInput(inputData, 1);
      Grove.MoveElement(input, 1);

      input.Elements[5].Value.Should().Be(-3);
    }

    [Theory]
    [InlineData(1, new long[] { 2, 1, -3, 3, -2, 0, 4 })]
    [InlineData(2, new long[] { 1, -3, 2, 3, -2, 0, 4 })]
    [InlineData(3, new long[] { 4, 1, 2, 3, -2, -3, 0 })]
    public void Can_move_elements_for_n_steps(int numSteps, long[] expectedElements)
    {
      var inputData = "1\r\n2\r\n-3\r\n3\r\n-2\r\n0\r\n4\r\n";

      var elements = Grove.MoveAllElementsForNSteps(inputData, 1, numSteps);

      elements.Select(e => e.Value).Should().Equal(expectedElements);
    }

    [Fact]
    public void Can_move_all_elements()
    {
      var inputData = "1\r\n2\r\n-3\r\n3\r\n-2\r\n0\r\n4\r\n";

      var elements = Grove.MoveAllElements(inputData, 1, 1);

      elements.Select(e => e.Value).Should().ContainInOrder(-2, 1, 2, -3, 4, 0, 3);
    }

    [Fact]
    public void Can_get_grove_coordinate()
    {
      var inputData = "1\r\n2\r\n-3\r\n3\r\n-2\r\n0\r\n4\r\n";

      var groveCoordinate = Grove.GetGroveCoordinate(inputData, 1, 1);

      groveCoordinate.Should().Be(3);
    }

    [Fact]
    public void Can_get_grove_coordinate_par2()
    {
      var inputData = "1\r\n2\r\n-3\r\n3\r\n-2\r\n0\r\n4\r\n";

      var groveCoordinate = Grove.GetGroveCoordinate(inputData, 811589153, 10);

      groveCoordinate.Should().Be(1623178306);
    }
  }
}