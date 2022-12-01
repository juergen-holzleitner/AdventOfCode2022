namespace _01_CalorieCounting
{
  internal class CalorieCounting
  {
    internal static int GetSumOfBlock(string block)
    {
      int sum = 0;
      foreach (var line in block.Split('\n'))
      {
        var val = int.Parse(line.Trim());
        sum += val;
      }
      return sum;
    }
  }
}