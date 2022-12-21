using System.Text.RegularExpressions;

namespace _21_MonkeyMath
{
  record Monkey(string Name, Job Job);

  record Job();

  record NumberJob(long Number) : Job;

  public enum Operation { Addition, Subtraction, Multiplication, Division };
  record MathOperationJob(string First, Operation Operation, string Second) : Job;

  internal partial class MonkeyMath
  {

    internal static long GetHumanValue(string monkeyName, string humanName, string input)
    {
      var monkeys = ParseInput(input);
      var monkeyJob = monkeys[monkeyName];

      return GetHumanValue(monkeys, (MathOperationJob)monkeyJob, humanName);
    }

    internal static long GetResultOf(string monkeyName, string input)
    {
      var monkeys = ParseInput(input);
      return GetResultOf(monkeys, monkeyName);
    }

    private static long GetResultOf(Dictionary<string, Job> monkeys, string monkeyName)
    {
      var job = monkeys[monkeyName];
      if (job is NumberJob n)
      {
        return n.Number;
      }

      var mathJob = (MathOperationJob)job;
      return CalculateMathJob(monkeys, mathJob);
    }

    private static long GetHumanValue(Dictionary<string, Job> monkeys, MathOperationJob monkeyJob, string humanName)
    {
      if (HasHuman(monkeys, monkeyJob.First, humanName))
      {
        var expectedValue = GetResultOf(monkeys, monkeyJob.Second);
        var enforcedValue = GetEnforcedValue(monkeys, monkeyJob.First, humanName, expectedValue);
        return enforcedValue;
      }
      else
      {
        var expectedValue = GetResultOf(monkeys, monkeyJob.Second);
        var enforcedValue = GetEnforcedValue(monkeys, monkeyJob.Second, humanName, expectedValue);
        return enforcedValue;
      }
    }

    internal static long GetEnforcedValue(Dictionary<string, Job> monkeys, string currentName, string humanName, long enforcedValue)
    {
      if (currentName == humanName)
        return enforcedValue;

      var mathJob = (MathOperationJob)monkeys[currentName];
      if (HasHuman(monkeys, mathJob.First, humanName))
      {
        var otherValue = GetResultOf(monkeys, mathJob.Second);
        var newEnforcedValue = GetNewEnforcedLeftValue(otherValue, mathJob.Operation, enforcedValue);
        return GetEnforcedValue(monkeys, mathJob.First, humanName, newEnforcedValue);
      }
      else
      {
        var otherValue = GetResultOf(monkeys, mathJob.First);
        var newEnforcedValue = GetNewEnforcedRightValue(otherValue, mathJob.Operation, enforcedValue);
        return GetEnforcedValue(monkeys, mathJob.Second, humanName, newEnforcedValue);
      }
    }

    public static long GetNewEnforcedLeftValue(long rightValue, Operation operation, long enforcedValue)
    {
      switch (operation)
      {
        case Operation.Addition:
          return enforcedValue - rightValue;
        case Operation.Subtraction:
          return enforcedValue + rightValue;
        case Operation.Multiplication:
          if (enforcedValue % rightValue != 0)
            throw new ApplicationException("division with remainder");
          return enforcedValue / rightValue;
        case Operation.Division:
          return enforcedValue * rightValue;
      }

      throw new ApplicationException("unexpected operation");
    }
    public static long GetNewEnforcedRightValue(long leftValue, Operation operation, long enforcedValue)
    {
      switch (operation)
      {
        case Operation.Addition:
          return enforcedValue - leftValue;
        case Operation.Subtraction:
          return leftValue - enforcedValue;
        case Operation.Multiplication:
          if (enforcedValue % leftValue != 0)
            throw new ApplicationException("division with remainder");
          return enforcedValue / leftValue;
        case Operation.Division:
          if (leftValue % enforcedValue != 0)
            throw new ApplicationException("division with remainder");
          return leftValue / enforcedValue;
      }

      throw new ApplicationException("unexpected operation");
    }

    internal static bool HasHuman(Dictionary<string, Job> monkeys, string current, string humanName)
    {
      if (current == humanName)
        return true;

      var job = monkeys[current];

      if (job is MathOperationJob mathJob)
      {
        if (HasHuman(monkeys, mathJob.First, humanName))
          return true;
        if (HasHuman(monkeys, mathJob.Second, humanName))
          return true;
      }

      return false;
    }

    private static long CalculateMathJob(Dictionary<string, Job> monkeys, MathOperationJob mathJob)
    {
      var first = GetResultOf(monkeys, mathJob.First);
      var second = GetResultOf(monkeys, mathJob.Second);

      return mathJob.Operation switch
      {
        Operation.Addition => first + second,
        Operation.Subtraction => first - second,
        Operation.Multiplication => first * second,
        Operation.Division => first / second,
        _ => throw new ApplicationException("invalid operation")
      };
    }

    internal static Dictionary<string, Job> ParseInput(string input)
    {
      var data = new Dictionary<string, Job>();
      foreach (var line in input.Split('\n'))
        if (!string.IsNullOrWhiteSpace(line))
        {
          var monkey = ParseLine(line);
          data.Add(monkey.Name, monkey.Job);
        }
      return data;
    }

    internal static Monkey ParseLine(string line)
    {
      var items = line.Split(':');
      if (long.TryParse(items[1].Trim(), out long number))
      {
        return new Monkey(items[0], new NumberJob(number));
      }

      var regEx = regexMonkey();
      var match = regEx.Match(line);
      if (!match.Success)
        throw new ApplicationException("Not expected");

      var first = match.Groups["first"].Value;
      var second = match.Groups["second"].Value;
      var operation = GetOperation(match.Groups["oper"].Value);
      return new Monkey(items[0], new MathOperationJob(first, operation, second));
    }

    private static Operation GetOperation(string value)
    {
      return value switch
      {
        "+" => Operation.Addition,
        "-" => Operation.Subtraction,
        "*" => Operation.Multiplication,
        "/" => Operation.Division,
        _ => throw new ApplicationException("Invalid operation")
      }; ;
    }

    [GeneratedRegex("(?<first>\\w+) (?<oper>\\+|-|\\*|/) (?<second>\\w+)")]
    private static partial Regex regexMonkey();
  }


}