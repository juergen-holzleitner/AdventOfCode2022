namespace _08_TreetopTreeHouse
{
  internal class TreeHouse
  {
    internal static bool IsVisible(int[,] data, int row, int col)
    {
      return IsVisibleFromLeft(data, row, col)
        || IsVisibleFromTop(data, row, col)
        || IsVisibleFromRight(data, row, col)
        || IsVisibleFromBottom(data, row, col);
    }

    private static bool IsVisibleFromBottom(int[,] data, int row, int col)
    {
      int height = data[row, col];
      for (int r = row + 1; r < data.GetLength(0); ++r)
        if (data[r, col] >= height)
          return false;

      return true;
    }

    private static bool IsVisibleFromRight(int[,] data, int row, int col)
    {
      int height = data[row, col];
      for (int c = col + 1; c < data.GetLength(1); ++c)
        if (data[row, c] >= height)
          return false;

      return true;
    }

    private static bool IsVisibleFromLeft(int[,] data, int row, int col)
    {
      int height = data[row, col];
      for (int c = col - 1; c >= 0; --c)
        if (data[row, c] >= height)
          return false;

      return true;
    }
    private static bool IsVisibleFromTop(int[,] data, int row, int col)
    {
      int height = data[row, col];
      for (int r = row - 1; r >= 0; --r)
        if (data[r, col] >= height)
          return false;

      return true;
    }

    internal static int GetNumVisible(int[,] data)
    {
      int numVisible = 0;

      for (int row = 0; row < data.GetLength(0); ++row)
        for (int col = 0; col < data.GetLength(1); ++col)
          if (IsVisible(data, row, col))
            ++numVisible;

      return numVisible;
    }

    internal static int[,] Parse(string input)
    {
      var rows = input.Split('\n');
      var numRows = rows.Where(r => !string.IsNullOrEmpty(r)).Count();

      var data = new int[numRows, rows[0].Trim('\r').Length];
      for (int row = 0; row < data.GetLength(0); ++row)
        for (int col = 0; col < data.GetLength(1); ++col)
        {
          data[row, col] = int.Parse(rows[row][col].ToString());
        }
      return data;
    }
  }
}
