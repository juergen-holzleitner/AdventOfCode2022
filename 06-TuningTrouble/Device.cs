using System.Text;

namespace _06_TuningTrouble
{
  internal class Device
  {
    StringBuilder state = new StringBuilder();
    int count = 0;

    internal void AddChar(char c)
    {
      const int numCharsToKeep = 4;

      state.Append(c);
      ++count;

      if (state.Length > numCharsToKeep)
        state.Remove(0, state.Length - numCharsToKeep);
    }

    internal int GetCount()
    {
      return count;
    }

    internal string GetState()
    {
      return state.ToString();
    }

    internal bool IsStartSequence()
    {
      if (state.Length < 4)
        return false;

      for (int n = 0; n < state.Length; ++n)
      {
        for (int m = n + 1; m < state.Length; ++m)
          if (state[n] == state[m])
            return false;
      }

      return true;
    }

    internal static int GetStartPos(string input)
    {
      var device = new Device();
      foreach (var c in input)
      {
        device.AddChar(c);
        if (device.IsStartSequence())
          return device.GetCount();
      }
      throw new ApplicationException("no startpos found");
    }

  }
}
