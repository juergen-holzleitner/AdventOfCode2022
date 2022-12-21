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