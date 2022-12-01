using FluentAssertions;

namespace _01_CalorieCounting
{
  public class CalorieCountingTest
  {
    [Fact]
    public void Can_get_sum_of_a_block()
    {
      var block = "1000\r\n2000\r\n3000";
      var sum = CalorieCounting.GetSumOfBlock(block);

      sum.Should().BeEquivalentTo(new[] { 6000 });
    }

    [Fact]
    public void Can_get_sum_of_multiple_blocks()
    {
      var blocks = "1000\r\n2000\r\n3000\r\n\r\n4000\r\n";
      var sums = CalorieCounting.GetSumOfBlock(blocks);

      sums.Should().BeEquivalentTo(new[] { 6000, 4000 });
    }

    [Fact]
    public void Can_get_max_sum_of_multiple_blocks()
    {
      var blocks = "1000\r\n2000\r\n3000\r\n\r\n4000\r\n\r\n5000\r\n6000\r\n\r\n7000\r\n8000\r\n9000\r\n\r\n10000";
      var max = CalorieCounting.GetMaxSumOfBlock(blocks);

      max.Should().Be(24000);
    }

    [Fact]
    public void Can_get_sum_of_top_three()
    {
      var blocks = "1000\r\n2000\r\n3000\r\n\r\n4000\r\n\r\n5000\r\n6000\r\n\r\n7000\r\n8000\r\n9000\r\n\r\n10000";
      var max = CalorieCounting.GetSumOfTopThree(blocks);

      max.Should().Be(45000);
    }
  }
}