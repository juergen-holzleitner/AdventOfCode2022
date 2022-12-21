using FluentAssertions;

namespace _21_MonkeyMath
{
  public class MonkeyMathTest
  {
    [Fact]
    public void Can_parse_line_with_number()
    {
      var line = "dbpl: 5";
      var monkey = MonkeyMath.ParseLine(line);

      monkey.Name.Should().Be("dbpl");
      monkey.Job.Should().Be(new NumberJob(5));
    }

    [Theory]
    [InlineData("root: pppw + sjmn", "root", "pppw", Operation.Addition, "sjmn")]
    [InlineData("ptdq: humn - dvpt", "ptdq", "humn", Operation.Subtraction, "dvpt")]
    [InlineData("sjmn: drzm * dbpl", "sjmn", "drzm", Operation.Multiplication, "dbpl")]
    [InlineData("pppw: cczh / lfqf", "pppw", "cczh", Operation.Division, "lfqf")]
    public void Can_parse_line_with_term(string line, string expectedName, string expectedFirst, Operation expectedOperation, string expectedSecond)
    {
      var monkey = MonkeyMath.ParseLine(line);

      monkey.Name.Should().Be(expectedName);
      monkey.Job.Should().Be(new MathOperationJob(expectedFirst, expectedOperation, expectedSecond));
    }

    [Fact]
    public void Can_parse_complete_input()
    {
      var input = "root: pppw + sjmn\r\ndbpl: 5\r\ncczh: sllz + lgvd\r\nzczc: 2\r\nptdq: humn - dvpt\r\ndvpt: 3\r\nlfqf: 4\r\nhumn: 5\r\nljgn: 2\r\nsjmn: drzm * dbpl\r\nsllz: 4\r\npppw: cczh / lfqf\r\nlgvd: ljgn * ptdq\r\ndrzm: hmdt - zczc\r\nhmdt: 32\r\n";

      var setup = MonkeyMath.ParseInput(input);

      setup.Should().HaveCount(15);
    }

    [Fact]
    public void Can_get_result()
    {
      var input = "root: pppw + sjmn\r\ndbpl: 5\r\ncczh: sllz + lgvd\r\nzczc: 2\r\nptdq: humn - dvpt\r\ndvpt: 3\r\nlfqf: 4\r\nhumn: 5\r\nljgn: 2\r\nsjmn: drzm * dbpl\r\nsllz: 4\r\npppw: cczh / lfqf\r\nlgvd: ljgn * ptdq\r\ndrzm: hmdt - zczc\r\nhmdt: 32\r\n";
      var result = MonkeyMath.GetResultOf("root", input);

      result.Should().Be(152);
    }


  }
}