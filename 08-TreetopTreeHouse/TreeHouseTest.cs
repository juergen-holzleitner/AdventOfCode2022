using FluentAssertions;

namespace _08_TreetopTreeHouse
{
  public class TreeHouseTest
  {
    [Fact]
    public void Can_read_input_1()
    {
      var input = "1";
      var data = TreeHouse.Parse(input);
      data.Should().BeEquivalentTo(new int[1, 1] { { 1 } });
    }

    [Fact]
    public void Can_read_input_2()
    {
      var input = "2";
      var data = TreeHouse.Parse(input);
      data.Should().BeEquivalentTo(new int[1, 1] { { 2 } });
    }

    [Fact]
    public void Can_read_line_of_input()
    {
      var input = "234";
      var data = TreeHouse.Parse(input);
      data.Should().BeEquivalentTo(new int[1, 3] { { 2, 3, 4 } });
    }

    [Fact]
    public void Can_read_multiple_lines_of_input()
    {
      var input = "234\r\n567";
      var data = TreeHouse.Parse(input);
      data.Should().BeEquivalentTo(new int[2, 3] { { 2, 3, 4 }, { 5, 6, 7 } });
    }

    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(2, 2, false)]
    [InlineData(0, 1, true)]
    [InlineData(1, 4, true)]
    [InlineData(4, 2, true)]
    public void Can_check_visibility_from_left(int row, int col, bool expected)
    {
      var input = "30373\r\n25512\r\n65332\r\n33549\r\n35390";
      var data = TreeHouse.Parse(input);

      bool isVisible = TreeHouse.IsVisible(data, row, col);

      isVisible.Should().Be(expected);
    }

    [Fact]
    public void Can_get_num_visible_trees()
    {
      var input = "30373\r\n25512\r\n65332\r\n33549\r\n35390";
      var data = TreeHouse.Parse(input);

      var numVisible = TreeHouse.GetNumVisible(data);

      numVisible.Should().Be(21);

    }
  }
}