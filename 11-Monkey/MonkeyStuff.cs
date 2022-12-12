using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _11_Monkey
{
  record Parsable();

  record MonkeyItem(int Number) : Parsable;

  record StartingItem(List<int> Items) : Parsable;

  record OperationItem(Func<int, int> Func) : Parsable;

  record DivisibleTest(int Divisor) : Parsable;

  record MonkeyIfTrue(int Monkey) : Parsable;

  record MonkeyIfFalse(int Monkey) : Parsable;

  record Monkey(MonkeyItem MonkeyItem)
  {
    public StartingItem? StartingItem { get; set; }
    public OperationItem? Operation { get; set; }
    public DivisibleTest? DivisibleTest { get; set; }
    public MonkeyIfTrue? MonkeyIfTrue { get; set; }
    public MonkeyIfFalse? MonkeyIfFalse { get; set; }
    public int NumInspected { get; set; } = 0;
  }

  internal partial class MonkeyStuff
  {
    internal static Parsable ParseLine(string line)
    {
      var regexMonkey = RegExMonkey();
      var matchMonkey = regexMonkey.Match(line);

      var regexStartingItem = RegExStartingItem();
      var matchStartingItems = regexStartingItem.Match(line);

      var regexOperation = RegExOperation();
      var matchOperation = regexOperation.Match(line);

      var regexDivisibleTest = RegExDivisibleTest();
      var matchDivisibleTest = regexDivisibleTest.Match(line);

      var regExTrueMonkey = RegExMonkeyIfTrue();
      var matchTrueMonkey = regExTrueMonkey.Match(line);

      var regExFalseMonkey = RegExMonkeyIfFalse();
      var matchFalseMonkey = regExFalseMonkey.Match(line);

      if (matchMonkey.Success)
      {
        var number = int.Parse(matchMonkey.Groups["number"].Value);
        return new MonkeyItem(number);
      }
      
      if (matchStartingItems.Success)
      {
        var items = matchStartingItems.Groups["items"].Value;
        var itemList = new List<int>();
        foreach (var i in items.Split(','))
        {
          itemList.Add(int.Parse(i));
        }

        return new StartingItem(itemList);
      }

      if (matchOperation.Success)
      {
        if (matchOperation.Groups["arg"].Value == "old")
        {
          if (matchOperation.Groups["op"].Value == "*")
            return new OperationItem(x => x * x);
        }
        else
        {
          int val = int.Parse(matchOperation.Groups["arg"].Value);
          switch (matchOperation.Groups["op"].Value)
          {
            case "+":
              return new OperationItem(x => x + val);
            case "*":
              return new OperationItem(x => x * val);
          }
        }
      }

      if (matchDivisibleTest.Success)
      {
        var val = int.Parse(matchDivisibleTest.Groups["divisor"].Value);
        return new DivisibleTest(val);
      }

      if (matchTrueMonkey.Success)
      {
        var val = int.Parse(matchTrueMonkey.Groups["monkey"].Value);
        return new MonkeyIfTrue(val);
      }

      if (matchFalseMonkey.Success)
      {
        var val = int.Parse(matchFalseMonkey.Groups["monkey"].Value);
        return new MonkeyIfFalse(val);
      }

      throw new ApplicationException($"can't parse {line}");
    }

    [GeneratedRegex("Monkey (?<number>\\d+):")]
    private static partial Regex RegExMonkey();
    [GeneratedRegex("Starting items: (?<items>\\d+(, \\d+)*)")]
    private static partial Regex RegExStartingItem();
    [GeneratedRegex("Operation: new = old (?<op>[+\\-*/]) (?<arg>(\\d+|old))")]
    private static partial Regex RegExOperation();
    [GeneratedRegex("Test: divisible by (?<divisor>\\d+)")]
    private static partial Regex RegExDivisibleTest();
    [GeneratedRegex("If true: throw to monkey (?<monkey>\\d+)")]
    private static partial Regex RegExMonkeyIfTrue();
    [GeneratedRegex("If false: throw to monkey (?<monkey>\\d+)")]
    private static partial Regex RegExMonkeyIfFalse();

    internal static List<Monkey> Parse(string input)
    {
      var monkeys = new List<Monkey>();

      foreach (var line in input.Split('\n'))
      {
        if (!string.IsNullOrWhiteSpace(line))
        {
          var item = ParseLine(line);
          if (item is MonkeyItem monkeyItem)
          {
            monkeys.Add(new Monkey(monkeyItem));
          }
          if (item is StartingItem startingItem)
          {
            monkeys[^1].StartingItem = startingItem;
          }
          if (item is OperationItem operation)
          {
            monkeys[^1].Operation = operation;
          }
          if (item is DivisibleTest divisibleTest)
          {
            monkeys[^1].DivisibleTest = divisibleTest;
          }
          if (item is MonkeyIfTrue monkeyIfTrue)
          {
            monkeys[^1].MonkeyIfTrue = monkeyIfTrue;
          }
          if (item is MonkeyIfFalse monkeyIfFalse)
          {
            monkeys[^1].MonkeyIfFalse = monkeyIfFalse;
          }
        }
      }

      return monkeys;
    }

    internal static void ProcessRound(List<Monkey> monkeys)
    {
      foreach (var monkey in monkeys)
      {
        var items = monkey.StartingItem!.Items;
        monkey.StartingItem = new StartingItem(new());
        foreach (var item in items)
        {
          ++monkey.NumInspected;

          var worryLevel = monkey.Operation!.Func(item);
          worryLevel /= 3;
          if (worryLevel % monkey.DivisibleTest!.Divisor == 0)
            monkeys[monkey.MonkeyIfTrue!.Monkey].StartingItem!.Items.Add(worryLevel);
          else
            monkeys[monkey.MonkeyIfFalse!.Monkey].StartingItem!.Items.Add(worryLevel);
        }
      }
    }

    internal static void ProcessRounds(List<Monkey> monkeys, int rounds)
    {
      for (int n = 0; n < rounds; ++n)
        ProcessRound(monkeys);
    }

    internal static int GetMonkeyBusiness(List<Monkey> monkeys, int rounds)
    {
      ProcessRounds(monkeys, rounds);

      var monkeyBusiness = (from m in monkeys orderby m.NumInspected descending select m.NumInspected).Take(2).Aggregate((a,x) => a*x);
      return monkeyBusiness;
    }
  }
}
