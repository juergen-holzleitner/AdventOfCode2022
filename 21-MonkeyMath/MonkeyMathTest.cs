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

    [Theory]
    [InlineData("humn", true)]
    [InlineData("pppw", true)]
    [InlineData("sjmn", false)]
    public void Can_check_for_human(string startName, bool expectedResult)
    {
      var input = "root: pppw + sjmn\r\ndbpl: 5\r\ncczh: sllz + lgvd\r\nzczc: 2\r\nptdq: humn - dvpt\r\ndvpt: 3\r\nlfqf: 4\r\nhumn: 5\r\nljgn: 2\r\nsjmn: drzm * dbpl\r\nsllz: 4\r\npppw: cczh / lfqf\r\nlgvd: ljgn * ptdq\r\ndrzm: hmdt - zczc\r\nhmdt: 32\r\n";
      var monkeys = MonkeyMath.ParseInput(input);

      var hasHuman = MonkeyMath.HasHuman(monkeys, startName, "humn");

      hasHuman.Should().Be(expectedResult);
    }


    [Theory]
    [InlineData("humn", 150)]
    public void Can_get_enforced_value(string startName, long expectedValue)
    {
      var input = "root: pppw + sjmn\r\ndbpl: 5\r\ncczh: sllz + lgvd\r\nzczc: 2\r\nptdq: humn - dvpt\r\ndvpt: 3\r\nlfqf: 4\r\nhumn: 5\r\nljgn: 2\r\nsjmn: drzm * dbpl\r\nsllz: 4\r\npppw: cczh / lfqf\r\nlgvd: ljgn * ptdq\r\ndrzm: hmdt - zczc\r\nhmdt: 32\r\n";
      var monkeys = MonkeyMath.ParseInput(input);

      var enforcedValue = MonkeyMath.GetEnforcedValue(monkeys, startName, "humn", 150);

      enforcedValue.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(10, Operation.Addition, 15, 5)]
    [InlineData(20, Operation.Subtraction, 15, 35)]
    [InlineData(7, Operation.Multiplication, 35, 5)]
    [InlineData(3, Operation.Division, 2, 6)]
    public void Can_get_new_enforced_left_value(int rightValue, Operation operation, int resultValue, int expectedLeftValue)
    {
      var newEnforcedValue = MonkeyMath.GetNewEnforcedLeftValue(rightValue, operation, resultValue);
      newEnforcedValue.Should().Be(expectedLeftValue);
    }

    [Theory]
    [InlineData(10, Operation.Addition, 15, 5)]
    [InlineData(25, Operation.Subtraction, 15, 10)]
    [InlineData(7, Operation.Multiplication, 35, 5)]
    [InlineData(10, Operation.Division, 2, 5)]

    public void Can_get_new_enforced_right_value(int leftValue, Operation operation, int resultValue, int expectedRightValue)
    {
      var newEnforcedValue = MonkeyMath.GetNewEnforcedRightValue(leftValue, operation, resultValue);
      newEnforcedValue.Should().Be(expectedRightValue);
    }

    [Fact]
    public void Can_get_hmn_value()
    {
      var input = "root: pppw + sjmn\r\ndbpl: 5\r\ncczh: sllz + lgvd\r\nzczc: 2\r\nptdq: humn - dvpt\r\ndvpt: 3\r\nlfqf: 4\r\nhumn: 5\r\nljgn: 2\r\nsjmn: drzm * dbpl\r\nsllz: 4\r\npppw: cczh / lfqf\r\nlgvd: ljgn * ptdq\r\ndrzm: hmdt - zczc\r\nhmdt: 32\r\n";
      var result = MonkeyMath.GetHumanValue("root", "humn", input);

      result.Should().Be(301);
    }


  }
}