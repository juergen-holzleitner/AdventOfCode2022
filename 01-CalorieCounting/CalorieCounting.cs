namespace _01_CalorieCounting
{
  internal class CalorieCounting
  {
    internal static int GetMaxSumOfBlock(string blocks)
    {
      return GetSumOfBlock(blocks).Max();
    }

    internal static IEnumerable<int> GetSumOfBlock(string block)
    {
      int? sum = null;
      foreach (var line in block.Split('\n'))
      {
        if (string.IsNullOrWhiteSpace(line))
        {
          if (sum.HasValue)
          {
            yield return sum.Value;
          }
          sum = null;
          continue;
        }

        var val = int.Parse(line.Trim());
        sum = (sum ?? 0) + val;
      }

      if (sum.HasValue)
      {
        yield return sum.Value;
      }
    }
  }
}