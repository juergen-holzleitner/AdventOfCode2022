using System.Text;

namespace _06_TuningTrouble
{
  internal class Device
  {
    internal Device(int numCharsToCheck = 4)
    {
      this.numCharsTocheck = numCharsToCheck;
    }

    StringBuilder state = new StringBuilder();
    int count = 0;
    private readonly int numCharsTocheck;

    internal void AddChar(char c)
    {
      state.Append(c);
      ++count;

      if (state.Length > numCharsTocheck)
        state.Remove(0, state.Length - numCharsTocheck);
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
      if (state.Length < numCharsTocheck)
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

    internal static int GetMessagePos(string input)
    {
      var device = new Device(14);
      foreach (var c in input)
      {
        device.AddChar(c);
        if (device.IsStartSequence())
          return device.GetCount();
      }
      throw new ApplicationException("no messagepos found");
    }

  }
}
