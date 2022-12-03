using FluentAssertions;

namespace _02_RockPaperScissors
{
  public class RockPaperScissorsTest
  {
    [Theory]
    [InlineData('A', Parser.Shape.Rock)]
    [InlineData('B', Parser.Shape.Paper)]
    [InlineData('C', Parser.Shape.Scissors)]
    public void Can_parse_Opponent(char inputShape, Parser.Shape result)
    {
      var shape = Parser.ParseOpponent(inputShape);

      shape.Should().Be(result);
    }

    [Theory]
    [InlineData('X', Parser.Shape.Rock)]
    [InlineData('Y', Parser.Shape.Paper)]
    [InlineData('Z', Parser.Shape.Scissors)]
    public void Can_parse_Player(char inputShape, Parser.Shape result)
    {
      var shape = Parser.ParsePlayer(inputShape);

      shape.Should().Be(result);
    }
  }
}