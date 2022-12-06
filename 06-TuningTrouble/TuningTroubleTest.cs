using FluentAssertions;

namespace _06_TuningTrouble
{
  public class TuningTroubleTest
  {
    [Fact]
    public void Can_add_char_to_device()
    {
      var sut = new Device();

      sut.AddChar('m');

      sut.GetState().Should().Be("m");
    }

    [Fact]
    public void Can_add_two_char_to_device()
    {
      var sut = new Device();

      sut.AddChar('m');
      sut.AddChar('j');

      sut.GetState().Should().Be("mj");
    }

    [Fact]
    public void Does_not_contain_more_than_4_chars()
    {
      var sut = new Device();

      sut.AddChar('m');
      sut.AddChar('j');
      sut.AddChar('q');
      sut.AddChar('j');
      sut.AddChar('p');

      sut.GetState().Should().Be("jqjp");
    }

    [Theory]
    [InlineData("", false)]
    [InlineData("abcd", true)]
    [InlineData("abbd", false)]
    public void Can_check_start_sequence(string sequence, bool expected)
    {
      var sut = new Device();
      foreach (var c in sequence)
        sut.AddChar(c);

      var isStartSequence = sut.IsStartSequence();

      isStartSequence.Should().Be(expected);
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData("a", 1)]
    public void Can_count_states(string sequence, int expectedCount)
    {
      var sut = new Device();
      foreach (var c in sequence)
        sut.AddChar(c);

      var count = sut.GetCount();

      count.Should().Be(expectedCount);
    }

    [Theory]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 6)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
    public void Can_get_start_pos(string input, int expectedStartPos)
    {
      var startPos = Device.GetStartPos(input);

      startPos.Should().Be(expectedStartPos);
    }

    [Theory]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 23)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 23)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)]
    public void Can_get_message_pos(string input, int expectedStartPos)
    {
      var startPos = Device.GetMessagePos(input);

      startPos.Should().Be(expectedStartPos);
    }

  }
}