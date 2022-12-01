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

      sum.Should().Be(6000);
    }
  }
}