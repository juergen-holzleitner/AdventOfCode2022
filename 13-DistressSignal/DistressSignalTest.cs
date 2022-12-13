using FluentAssertions;

namespace _13_DistressSignal
{
  public class DistressSignalTest
  {
    [Fact]
    public void Can_parse_empty_list()
    {
      var line = "[]";
      var value = Signal.Parse(line);
      value.Item.Items.Should().BeEmpty();
    }

    [Fact]
    public void Can_parse_list_with_single_value()
    {
      var line = "[9]";
      var value = Signal.Parse(line);
      value.Item.Items.Should().BeEquivalentTo(new[] { new ValueItem(9) });
    }

    [Fact]
    public void Can_parse_multiple_values()
    {
      var line = "[7,7,7]";
      var value = Signal.Parse(line);
      value.Item.Items.Should().BeEquivalentTo(new[] { new ValueItem(7), new ValueItem(7), new ValueItem(7) });
    }

    [Fact]
    public void Can_parse_nested_list()
    {
      var line = "[[1],4]";
      var value = Signal.Parse(line);
      value.Item.Items.Should().HaveCount(2);
      value.Item.Items[0].Should().BeEquivalentTo(new ListItem(new List<Item> { new ValueItem(1) }));
      value.Item.Items[1].Should().Be(new ValueItem(4));
    }

    [Fact]
    public void Can_parse_pair()
    {
      var line = "\r\n[[1],[2,3,4]]\r\n[[1],4]";

      var pairs = Signal.ParseInput(line.Split('\n'));

      pairs.Count().Should().Be(1);

      pairs.First().Left.Item.Items.Should().HaveCount(2);
      pairs.First().Right.Item.Items.Should().HaveCount(2);
      pairs.First().Right.Item.Items[1].Should().Be(new ValueItem(4));
    }

    [Fact]
    public void Can_parse_sample_pairs()
    {
      var line = "[1,1,3,1,1]\r\n[1,1,5,1,1]\r\n\r\n[[1],[2,3,4]]\r\n[[1],4]\r\n\r\n[9]\r\n[[8,7,6]]\r\n\r\n[[4,4],4,4]\r\n[[4,4],4,4,4]\r\n\r\n[7,7,7,7]\r\n[7,7,7]\r\n\r\n[]\r\n[3]\r\n\r\n[[[]]]\r\n[[]]\r\n\r\n[1,[2,[3,[4,[5,6,7]]]],8,9]\r\n[1,[2,[3,[4,[5,6,0]]]],8,9]";

      var pairs = Signal.ParseInput(line.Split('\n'));

      pairs.Count().Should().Be(8);
    }

    [Theory]
    [InlineData("[1,1,3,1,1]\r\n[1,1,5,1,1]", true)]
    [InlineData("[[1],[2,3,4]]\r\n[[1],4]", true)]
    [InlineData("[9]\r\n[[8,7,6]]", false)]
    [InlineData("[[4,4],4,4]\r\n[[4,4],4,4,4]", true)]
    [InlineData("[7,7,7,7]\r\n[7,7,7]", false)]
    [InlineData("[]\r\n[3]", true)]
    [InlineData("[[[]]]\r\n[[]]", false)]
    [InlineData("[1,[2,[3,[4,[5,6,7]]]],8,9]\r\n[1,[2,[3,[4,[5,6,0]]]],8,9]", false)]
    public void Can_compare_pair(string line, bool expectedResult)
    {
      var pairs = Signal.ParseInput(line.Split('\n'));

      var result = Signal.CheckPair(pairs.Single());

      result.Should().Be(expectedResult);
    }

    [Fact]
    public void Can_process_sample_data()
    {
      var input = "[1,1,3,1,1]\r\n[1,1,5,1,1]\r\n\r\n[[1],[2,3,4]]\r\n[[1],4]\r\n\r\n[9]\r\n[[8,7,6]]\r\n\r\n[[4,4],4,4]\r\n[[4,4],4,4,4]\r\n\r\n[7,7,7,7]\r\n[7,7,7]\r\n\r\n[]\r\n[3]\r\n\r\n[[[]]]\r\n[[]]\r\n\r\n[1,[2,[3,[4,[5,6,7]]]],8,9]\r\n[1,[2,[3,[4,[5,6,0]]]],8,9]";
      var lines = input.Split('\n');

      var sumOfIndices = Signal.GetSumOfIndices(lines);

      sumOfIndices.Should().Be(13);
    }

    [Fact]
    public void Can_get_result_of_part2()
    {
      var input = "[1,1,3,1,1]\r\n[1,1,5,1,1]\r\n\r\n[[1],[2,3,4]]\r\n[[1],4]\r\n\r\n[9]\r\n[[8,7,6]]\r\n\r\n[[4,4],4,4]\r\n[[4,4],4,4,4]\r\n\r\n[7,7,7,7]\r\n[7,7,7]\r\n\r\n[]\r\n[3]\r\n\r\n[[[]]]\r\n[[]]\r\n\r\n[1,[2,[3,[4,[5,6,7]]]],8,9]\r\n[1,[2,[3,[4,[5,6,0]]]],8,9]";
      var lines = input.Split('\n');

      var decoderKey = Signal.GetDecoderKey(lines);

      decoderKey.Should().Be(140);
    }
  }
}