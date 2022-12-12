using FluentAssertions;
using System.Reflection.Metadata.Ecma335;

namespace _11_Monkey
{
  public class MonkeyTest
  {
    [Theory]
    [InlineData("Monkey 0:", 0)]
    [InlineData("Monkey 2:", 2)]
    public void Can_parse_monkey_id(string line, int expectedMonkeyNumber)
    {
      var item = MonkeyStuff.ParseLine(line);

      item.Should().Be(new MonkeyItem(expectedMonkeyNumber));
    }

    [Fact]
    public void Can_parse_starting_items()
    {
      var line = "  Starting items: 79, 98";

      var item = MonkeyStuff.ParseLine(line);

      item.Should().BeEquivalentTo(new StartingItem(new() { 79, 98 }));
    }

    [Fact]
    public void Can_parse_mul_operation()
    {
      var line = "  Operation: new = old * 19";
      var item = MonkeyStuff.ParseLine(line) as OperationItem;

      for (ulong n = 0; n < 10; ++n)
      {
        item!.Func(n).Should().Be(n * 19);
      }
    }

    [Fact]
    public void Can_parse_add_operation()
    {
      var line = "  Operation: new = old + 6";
      var item = MonkeyStuff.ParseLine(line) as OperationItem;

      for (ulong n = 0; n < 10; ++n)
      {
        item!.Func(n).Should().Be(n + 6);
      }
    }

    [Fact]
    public void Can_parse_mul_by_itself_operation()
    {
      var line = "  Operation: new = old * old";
      var item = MonkeyStuff.ParseLine(line) as OperationItem;

      for (ulong n = 0; n < 10; ++n)
      {
        item!.Func(n).Should().Be(n * n);
      }
    }

    [Fact]
    public void Can_parse_divisible_test()
    {
      var line = "  Test: divisible by 13";
      var item = MonkeyStuff.ParseLine(line);
      item.Should().Be(new DivisibleTest(13));
    }

    [Fact]
    public void Can_parse_next_true_monkey()
    {
      var line = "    If true: throw to monkey 1";
      var item = MonkeyStuff.ParseLine(line);
      item.Should().Be(new MonkeyIfTrue(1));
    }

    [Fact]
    public void Can_parse_next_false_monkey()
    {
      var line = "    If false: throw to monkey 3";
      var item = MonkeyStuff.ParseLine(line);
      item.Should().Be(new MonkeyIfFalse(3));
    }

    [Fact]
    public void Can_parse_monkeys()
    {
      var input = "Monkey 0:\r\n  Starting items: 79, 98\r\n  Operation: new = old * 19\r\n  Test: divisible by 23\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 3\r\n";
      var monkeys = MonkeyStuff.Parse(input);
      monkeys.Should().HaveCount(1);
      monkeys[0].MonkeyItem.Should().Be(new MonkeyItem(0));
      monkeys[0].StartingItem.Should().BeEquivalentTo(new StartingItem(new() { 79, 98 }));
      for (ulong n = 10; n < 20; ++n)
      {
        monkeys[0].Operation!.Func(n).Should().Be(n * 19);
      }
      monkeys[0].DivisibleTest.Should().Be(new DivisibleTest(23));
      monkeys[0].MonkeyIfTrue.Should().Be(new MonkeyIfTrue(2));
      monkeys[0].MonkeyIfFalse.Should().Be(new MonkeyIfFalse(3));
    }

    [Fact]
    public void Can_parse_test_input()
    {
      var input = "Monkey 0:\r\n  Starting items: 79, 98\r\n  Operation: new = old * 19\r\n  Test: divisible by 23\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 3\r\n\r\nMonkey 1:\r\n  Starting items: 54, 65, 75, 74\r\n  Operation: new = old + 6\r\n  Test: divisible by 19\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 0\r\n\r\nMonkey 2:\r\n  Starting items: 79, 60, 97\r\n  Operation: new = old * old\r\n  Test: divisible by 13\r\n    If true: throw to monkey 1\r\n    If false: throw to monkey 3\r\n\r\nMonkey 3:\r\n  Starting items: 74\r\n  Operation: new = old + 3\r\n  Test: divisible by 17\r\n    If true: throw to monkey 0\r\n    If false: throw to monkey 1";
      var monkeys = MonkeyStuff.Parse(input);
      monkeys.Should().HaveCount(4);

      monkeys[3].MonkeyItem.Should().Be(new MonkeyItem(3));
      monkeys[3].StartingItem.Should().BeEquivalentTo(new StartingItem(new() { 74 }));
      for (ulong n = 10; n < 20; ++n)
      {
        monkeys[3].Operation!.Func(n).Should().Be(n + 3);
      }
      monkeys[3].DivisibleTest.Should().Be(new DivisibleTest(17));
      monkeys[3].MonkeyIfTrue.Should().Be(new MonkeyIfTrue(0));
      monkeys[3].MonkeyIfFalse.Should().Be(new MonkeyIfFalse(1));
    }

    [Fact]
    public void Can_process_round()
    {
      var input = "Monkey 0:\r\n  Starting items: 79, 98\r\n  Operation: new = old * 19\r\n  Test: divisible by 23\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 3\r\n\r\nMonkey 1:\r\n  Starting items: 54, 65, 75, 74\r\n  Operation: new = old + 6\r\n  Test: divisible by 19\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 0\r\n\r\nMonkey 2:\r\n  Starting items: 79, 60, 97\r\n  Operation: new = old * old\r\n  Test: divisible by 13\r\n    If true: throw to monkey 1\r\n    If false: throw to monkey 3\r\n\r\nMonkey 3:\r\n  Starting items: 74\r\n  Operation: new = old + 3\r\n  Test: divisible by 17\r\n    If true: throw to monkey 0\r\n    If false: throw to monkey 1";
      var monkeys = MonkeyStuff.Parse(input);

      MonkeyStuff.ProcessRound(monkeys, true);

      monkeys[0].StartingItem.Should().BeEquivalentTo(new StartingItem(new() { 20, 23, 27, 26 }));
      monkeys[1].StartingItem.Should().BeEquivalentTo(new StartingItem(new() { 2080, 25, 167, 207, 401, 1046 }));
      monkeys[2].StartingItem!.Items.Should().BeEmpty();
      monkeys[3].StartingItem!.Items.Should().BeEmpty();
    }

    [Fact]
    public void Can_process_multiple_rounds()
    {
      var input = "Monkey 0:\r\n  Starting items: 79, 98\r\n  Operation: new = old * 19\r\n  Test: divisible by 23\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 3\r\n\r\nMonkey 1:\r\n  Starting items: 54, 65, 75, 74\r\n  Operation: new = old + 6\r\n  Test: divisible by 19\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 0\r\n\r\nMonkey 2:\r\n  Starting items: 79, 60, 97\r\n  Operation: new = old * old\r\n  Test: divisible by 13\r\n    If true: throw to monkey 1\r\n    If false: throw to monkey 3\r\n\r\nMonkey 3:\r\n  Starting items: 74\r\n  Operation: new = old + 3\r\n  Test: divisible by 17\r\n    If true: throw to monkey 0\r\n    If false: throw to monkey 1";
      var monkeys = MonkeyStuff.Parse(input);

      MonkeyStuff.ProcessRounds(monkeys, 20);

      monkeys[0].StartingItem.Should().BeEquivalentTo(new StartingItem(new() { 10, 12, 14, 26, 34 }));
      monkeys[1].StartingItem.Should().BeEquivalentTo(new StartingItem(new() { 245, 93, 53, 199, 115 }));
      monkeys[2].StartingItem!.Items.Should().BeEmpty();
      monkeys[3].StartingItem!.Items.Should().BeEmpty();
    }

    [Fact]
    public void Can_process_multiple_rounds_and_get_num_inspected()
    {
      var input = "Monkey 0:\r\n  Starting items: 79, 98\r\n  Operation: new = old * 19\r\n  Test: divisible by 23\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 3\r\n\r\nMonkey 1:\r\n  Starting items: 54, 65, 75, 74\r\n  Operation: new = old + 6\r\n  Test: divisible by 19\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 0\r\n\r\nMonkey 2:\r\n  Starting items: 79, 60, 97\r\n  Operation: new = old * old\r\n  Test: divisible by 13\r\n    If true: throw to monkey 1\r\n    If false: throw to monkey 3\r\n\r\nMonkey 3:\r\n  Starting items: 74\r\n  Operation: new = old + 3\r\n  Test: divisible by 17\r\n    If true: throw to monkey 0\r\n    If false: throw to monkey 1";
      var monkeys = MonkeyStuff.Parse(input);

      MonkeyStuff.ProcessRounds(monkeys, 20);

      monkeys[0].NumInspected.Should().Be(101);
      monkeys[1].NumInspected.Should().Be(95);
      monkeys[2].NumInspected.Should().Be(7);
      monkeys[3].NumInspected.Should().Be(105);
    }

    [Fact]
    public void Can_get_monkey_business()
    {
      var input = "Monkey 0:\r\n  Starting items: 79, 98\r\n  Operation: new = old * 19\r\n  Test: divisible by 23\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 3\r\n\r\nMonkey 1:\r\n  Starting items: 54, 65, 75, 74\r\n  Operation: new = old + 6\r\n  Test: divisible by 19\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 0\r\n\r\nMonkey 2:\r\n  Starting items: 79, 60, 97\r\n  Operation: new = old * old\r\n  Test: divisible by 13\r\n    If true: throw to monkey 1\r\n    If false: throw to monkey 3\r\n\r\nMonkey 3:\r\n  Starting items: 74\r\n  Operation: new = old + 3\r\n  Test: divisible by 17\r\n    If true: throw to monkey 0\r\n    If false: throw to monkey 1";
      var monkeys = MonkeyStuff.Parse(input);

      var monkeyBusiness = MonkeyStuff.GetMonkeyBusiness(monkeys, 20);

      monkeyBusiness.Should().Be(10605);
    }

    [Theory]
    [InlineData(1, 2, 4, 3, 6)]
    [InlineData(20, 99, 97, 8, 103)]
    [InlineData(10000, 52166, 47830, 1938, 52013)]
    public void Can_process_multiple_rounds_and_get_num_inspected_for_part_2(int rounds, ulong expected0, ulong expected1, ulong expected2, ulong expected3)
    {
      var input = "Monkey 0:\r\n  Starting items: 79, 98\r\n  Operation: new = old * 19\r\n  Test: divisible by 23\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 3\r\n\r\nMonkey 1:\r\n  Starting items: 54, 65, 75, 74\r\n  Operation: new = old + 6\r\n  Test: divisible by 19\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 0\r\n\r\nMonkey 2:\r\n  Starting items: 79, 60, 97\r\n  Operation: new = old * old\r\n  Test: divisible by 13\r\n    If true: throw to monkey 1\r\n    If false: throw to monkey 3\r\n\r\nMonkey 3:\r\n  Starting items: 74\r\n  Operation: new = old + 3\r\n  Test: divisible by 17\r\n    If true: throw to monkey 0\r\n    If false: throw to monkey 1";
      var monkeys = MonkeyStuff.Parse(input);

      MonkeyStuff.ProcessRoundsWithoutDiv(monkeys, rounds);

      monkeys[0].NumInspected.Should().Be(expected0);
      monkeys[1].NumInspected.Should().Be(expected1);
      monkeys[2].NumInspected.Should().Be(expected2);
      monkeys[3].NumInspected.Should().Be(expected3);
    }

    [Fact]
    public void Can_get_monkey_business_part_2()
    {
      var input = "Monkey 0:\r\n  Starting items: 79, 98\r\n  Operation: new = old * 19\r\n  Test: divisible by 23\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 3\r\n\r\nMonkey 1:\r\n  Starting items: 54, 65, 75, 74\r\n  Operation: new = old + 6\r\n  Test: divisible by 19\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 0\r\n\r\nMonkey 2:\r\n  Starting items: 79, 60, 97\r\n  Operation: new = old * old\r\n  Test: divisible by 13\r\n    If true: throw to monkey 1\r\n    If false: throw to monkey 3\r\n\r\nMonkey 3:\r\n  Starting items: 74\r\n  Operation: new = old + 3\r\n  Test: divisible by 17\r\n    If true: throw to monkey 0\r\n    If false: throw to monkey 1";
      var monkeys = MonkeyStuff.Parse(input);

      var monkeyBusiness = MonkeyStuff.GetMonkeyBusinessWithoutDiv(monkeys, 10000);

      monkeyBusiness.Should().Be(2713310158);
    }

  }
}