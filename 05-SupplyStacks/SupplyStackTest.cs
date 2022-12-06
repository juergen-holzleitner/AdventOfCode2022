using FluentAssertions;

namespace _05_SupplyStacks
{
  public class SupplyStackTest
  {
    [Theory]
    [InlineData("[D]", 'D')]
    [InlineData("[N]", 'N')]
    public void Can_parse_crate(string crate, char expectedSupply)
    {
      var supply = SupplyStack.ParseCrate(crate);
      supply.Should().Be(expectedSupply);
    }

    [Fact]
    public void Can_parse_line()
    {
      var input = "[N] [C]";
      var supplies = SupplyStack.ParseLine(input);
      supplies.Should().BeEquivalentTo(new char[] { 'N', 'C'});
    }

    [Theory]
    [InlineData("[A]", 0, 'A')]
    [InlineData("[A]", 1, null)]
    [InlineData("[A]", -1, null)]
    [InlineData(" A]", 0, null)]
    [InlineData("[A ", 0, null)]
    public void Can_get_supply_at_position(string line, int position, char? expectedResult)
    {
      var result = SupplyStack.GetSupplyAt(line, position);
      result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(" 1 ", true)]
    [InlineData("[A]", false)]
    public void Can_detect_separator_line(string line, bool expectedResult)
    {
      var isSeparator = SupplyStack.IsSeparatorLine(line);
      isSeparator.Should().Be(expectedResult);
    }

    [Fact]
    public void Can_get_lines_until_separator()
    {
      var input = "    [D]    \r\n[N] [C]    \r\n[Z] [M] [P]\r\n 1   2   3";

      var lines = SupplyStack.GetLinesUntilSeparator(input);
      
      lines.Should().BeEquivalentTo(new string[] { "    [D]    ", "[N] [C]    ", "[Z] [M] [P]" });
    }

    [Fact]
    public void Can_build_initial_stack()
    {
      var input = "    [D]    \r\n[N] [C]    \r\n[Z] [M] [P]\r\n 1   2   3";

      var initialStack = SupplyStack.ParseInitialStack(input);

      initialStack[0].Should().BeEquivalentTo(new char[] { 'Z', 'N' });
      initialStack[1].Should().BeEquivalentTo(new char[] { 'M', 'C', 'D' });
      initialStack[2].Should().BeEquivalentTo(new char[] { 'P' });
      for (int n = 3; n < initialStack.Length; ++n)
      {
        initialStack[n].Should().BeEmpty();
      }
    }

    [Theory]
    [InlineData("move 1 from 2 to 1", 1, 2, 1)]
    [InlineData("move 3 from 1 to 3", 3, 1, 3)]
    [InlineData("move 2 from 2 to 1", 2, 2, 1)]
    [InlineData("move 1 from 1 to 2", 1, 1, 2)]
    public void Can_get_single_instruction(string instruction, int expectedNum, int expectedFrom, int expectedTo)
    {
      var input = SupplyStack.ParseInstruction(instruction);

      input.Should().Be(new SupplyStack.Instruction(expectedNum, expectedFrom, expectedTo));
    }

    [Fact]
    public void Can_get_final_stack()
    {
      var input = "    [D]    \r\n[N] [C]    \r\n[Z] [M] [P]\r\n 1   2   3 \r\n\r\nmove 1 from 2 to 1\r\nmove 3 from 1 to 3\r\nmove 2 from 2 to 1\r\nmove 1 from 1 to 2";
      var stack = SupplyStack.ProcessInput(input);
      stack[0].Should().BeEquivalentTo(new char[] { 'C' });
      stack[1].Should().BeEquivalentTo(new char[] { 'M' });
      stack[2].Should().BeEquivalentTo(new char[] { 'P', 'D', 'N', 'Z' });
    }

    [Fact]
    public void Can_get_final_result()
    {
      var input = "    [D]    \r\n[N] [C]    \r\n[Z] [M] [P]\r\n 1   2   3 \r\n\r\nmove 1 from 2 to 1\r\nmove 3 from 1 to 3\r\nmove 2 from 2 to 1\r\nmove 1 from 1 to 2";
      var result = SupplyStack.GetFinalResult(input);
      result.Should().Be("CMZ");
    }

  }
}