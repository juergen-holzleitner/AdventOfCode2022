namespace _25_HotAir
{
  internal class Snafu
  {
    internal static long GetSumOf(string text)
    {
      long sum = 0;

      foreach (var line in text.Split('\n'))
      {
        if (!string.IsNullOrWhiteSpace(line))
        {
          sum += ParseNumber(line);
        }
      }

      return sum;
    }

    internal static long ParseDigit(char ch)
    {
      return ch switch
      {
        '0' => 0,
        '1' => 1,
        '2' => 2,
        '-' => -1,
        '=' => -2,
        _ => throw new ApplicationException()
      };
    }

    internal static long ParseNumber(string text)
    {
      var digits = new Stack<long>();
      foreach (var ch in text.Trim())
      {
        digits.Push(ParseDigit(ch));
      }

      long number = 0;
      long positionFactor = 1;
      while (digits.TryPop(out long digit))
      {
        number += digit * positionFactor;
        positionFactor *= 5;
      }
      return number;
    }

    internal static string ToSnafu(long number)
    {
      var numDigits = GetRequiredDigits(number);
      string snafu = string.Empty;
      for (long n = numDigits - 1; n >= 0; --n)
      {
        var positionalFactor = GetPositionalFactor(n);
        var maxRemaining = GetMaxPossibleNumberWithDigits(n);

        bool found = false;
        for (long digit = -2; digit <= 2; ++digit)
        {
          var currentAmount = digit * positionalFactor;
          var newNumber = number - currentAmount;
          if (newNumber <= maxRemaining && newNumber >= -maxRemaining)
          {
            number = newNumber;
            var snafuDigit = GetSnafuDigit(digit);
            snafu += snafuDigit;
            found = true;
            break;
          }
        }
        if (!found)
          throw new ApplicationException();
      }

      if (snafu.Length != numDigits)
        throw new ApplicationException();

      return snafu;
    }

    public static long GetMaxPossibleNumberWithDigits(long digits)
    {
      long maxNumber = 0;
      for (int n = 0; n < digits; ++n)
      {
        maxNumber += 2 * GetPositionalFactor(n);
      }
      return maxNumber;
    }

    private static long GetPositionalFactor(long position)
    {
      if (position < 0)
        return 0;

      long factor = 1;
      for (int n = 0; n < position; ++n)
        factor *= 5;
      return factor;
    }

    public static long GetRequiredDigits(long number)
    {
      long numDigits = 1;

      while (number > 2 * GetPositionalFactor(numDigits - 1) || number < -2 * GetPositionalFactor(numDigits - 1))
      {
        ++numDigits;
      }

      return numDigits;
    }

    private static char GetSnafuDigit(long number)
    {
      return number switch
      {
        0 => '0',
        1 => '1',
        2 => '2',
        -1 => '-',
        -2 => '=',
        _ => throw new ApplicationException()
      };
    }

    internal static string GetSumOfAsSnafu(string text)
    {
      var sum = GetSumOf(text);
      return ToSnafu(sum);
    }
  }
}