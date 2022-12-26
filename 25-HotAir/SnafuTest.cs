using FluentAssertions;

namespace _25_HotAir
{
  public class SnafuTest
  {
    [Theory]
    [InlineData('0', 0)]
    [InlineData('1', 1)]
    [InlineData('2', 2)]
    [InlineData('-', -1)]
    [InlineData('=', -2)]
    public void Can_convert_digit(char ch, long expectedNumber)
    {
      var n = Snafu.ParseDigit(ch);
      n.Should().Be(expectedNumber);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("-", -1)]
    [InlineData("2=", 8)]
    [InlineData("2=-01", 976)]
    [InlineData("            1", 1)]
    [InlineData("            2", 2)]
    [InlineData("           1=", 3)]
    [InlineData("           1-", 4)]
    [InlineData("           10", 5)]
    [InlineData("           11", 6)]
    [InlineData("           12", 7)]
    [InlineData("           2=", 8)]
    [InlineData("           2-", 9)]
    [InlineData("           20", 10)]
    [InlineData("          1=0", 15)]
    [InlineData("          1-0", 20)]
    [InlineData("       1=11-2", 2022)]
    [InlineData("      1-0---0", 12345)]
    [InlineData("1121-1110-1=0", 314159265)]
    public void Can_read_number(string text, long expectedNumber)
    {
      var n = Snafu.ParseNumber(text);
      n.Should().Be(expectedNumber);
    }

    [Fact]
    public void Can_get_sum_of_sample_numbers()
    {
      var text = "1=-0-2\r\n 12111\r\n  2=0=\r\n    21\r\n  2=01\r\n   111\r\n 20012\r\n   112\r\n 1=-1=\r\n  1-12\r\n    12\r\n    1=\r\n   122\r\n";
      var sum = Snafu.GetSumOf(text);
      sum.Should().Be(4890);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(-1, 1)]
    [InlineData(-2, 1)]
    [InlineData(3, 2)]
    [InlineData(4, 2)]
    [InlineData(5, 2)]
    [InlineData(6, 2)]
    [InlineData(7, 2)]
    [InlineData(8, 2)]
    [InlineData(9, 2)]
    [InlineData(10, 2)]
    [InlineData(15, 3)]
    [InlineData(20, 3)]
    [InlineData(2022, 6)]
    [InlineData(12345, 7)]
    [InlineData(314159265, 13)]
    public void Can_get_required_digits_for_snafu_number(long number, int requiredDigits)
    {
      var digits = Snafu.GetRequiredDigits(number);
      digits.Should().Be(requiredDigits);
    }

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(2, "2")]
    [InlineData(-1, "-")]
    [InlineData(-2, "=")]
    [InlineData(3, "1=")]
    [InlineData(4, "1-")]
    [InlineData(5, "10")]
    [InlineData(6, "11")]
    [InlineData(7, "12")]
    [InlineData(8, "2=")]
    [InlineData(9, "2-")]
    [InlineData(10, "20")]
    [InlineData(15, "1=0")]
    [InlineData(20, "1-0")]
    [InlineData(2022, "1=11-2")]
    [InlineData(12345, "1-0---0")]
    [InlineData(314159265, "1121-1110-1=0")]
    public void Can_get_snafu_number(long number, string expectedSnafu)
    {
      var snafu = Snafu.ToSnafu(number);
      snafu.Should().Be(expectedSnafu);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 2)]
    [InlineData(2, 12)]
    [InlineData(3, 62)]
    [InlineData(4, 312)]
    public void Can_get_max_possible_number_with_digits(int digits, long expectedMaxNumber)
    {
      var maxNumber = Snafu.GetMaxPossibleNumberWithDigits(digits);
      maxNumber.Should().Be(expectedMaxNumber);
    }

    [Fact]
    public void Can_get_part1_sample()
    {
      var text = "1=-0-2\r\n12111\r\n2=0=\r\n21\r\n2=01\r\n111\r\n20012\r\n112\r\n1=-1=\r\n1-12\r\n12\r\n1=\r\n122";
      var sum = Snafu.GetSumOfAsSnafu(text);
      sum.Should().Be("2=-1=0");
    }

  }
}