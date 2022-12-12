using FluentAssertions;

namespace _12_HillClimbing
{
  public class HillClimbingTest
  {
    [Fact]
    public void Can_parse_high_map()
    {
      var inputString = "Sabqponm\r\nabcryxxl\r\naccszExk\r\nacctuvwj\r\nabdefghi";
      var input = HillClimbing.Parse(inputString);
      input.Highmap.Count.Should().Be(5);
      foreach (var row in input.Highmap)
      {
        row.Count.Should().Be(8);
      }
      input.Highmap[0][0].Should().Be(0);
      input.Highmap[2][5].Should().Be(25);
      input.Highmap[0][7].Should().Be(12);
      
      input.Start.Should().Be(new Pos(0, 0));
    }

    [Fact]
    public void Can_get_min_steps()
    {
      var inputString = "Sabqponm\r\nabcryxxl\r\naccszExk\r\nacctuvwj\r\nabdefghi";
      var input = HillClimbing.Parse(inputString);

      var steps = HillClimbing.GetMinStepsFromStartToEnd(input);

      steps.Should().Be(31);
    }
  }
}