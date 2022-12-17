using System.Text.RegularExpressions;

namespace _16_ProboscideaVolcanium
{
  record System(Dictionary<string, Valve> Valves, bool WithElephant);

  record struct State(System System, Valve Valve, Valve ValveElephant, int TimeLeft, Dictionary<string, bool> ValveOpen, int TotalPressure) : IComparable<State>
  {
    public int CompareTo(State other)
    {
      if (TotalPressure < other.TotalPressure)
        return -1;
      if (TotalPressure > other.TotalPressure)
        return 1;

      if (TimeLeft < other.TimeLeft)
        return -1;
      if (TimeLeft > other.TimeLeft)
        return 1;


      foreach (var key in ValveOpen.Keys)
      {
        if (ValveOpen[key] != other.ValveOpen[key])
        {
          if (!ValveOpen[key])
            return -1;
          return 1;
        }
      }

      if (System.WithElephant)
      {
        var smaller = Valve.Id;
        var bigger = ValveElephant.Id;

        if (smaller.CompareTo(bigger) > 1)
        {
          smaller = ValveElephant.Id;
          bigger = Valve.Id;
        }

        var otherSmaller = other.Valve.Id;
        var otherBigger = other.ValveElephant.Id;
        if (otherSmaller.CompareTo(otherBigger) > 1)
        {
          otherSmaller = other.ValveElephant.Id;
          otherBigger = other.Valve.Id;
        }

        if (smaller == otherSmaller)
          return bigger.CompareTo(otherBigger);

        return smaller.CompareTo(otherSmaller);
      }
      else
      {
        return Valve.Id.CompareTo(other.Valve.Id);
      }
    }

    internal int GetMaxRemaining(bool withElephant)
    {
      if (TimeLeft <= 1)
        return 0;

      var valveOpen = ValveOpen;
      var remaining = from v in System.Valves
                      where v.Value.Rate > 0 && !valveOpen[v.Key]
                      let rate = v.Value.Rate
                      orderby rate descending
                      select rate;

      int sum = TotalPressure;
      var currentTimeLeft = TimeLeft - 1;
      int n = 0;
      foreach (var r in remaining)
      {
        sum += currentTimeLeft * r;
        ++n;
        if (!withElephant || (n % 2 == 0))
          currentTimeLeft -= 2;
        if (currentTimeLeft <= 0)
          break;
      }

      return sum;
    }

    internal IEnumerable<State> GetPossibleNextStates(int currentMaxPressure, bool withElephant)
    {
      if (TimeLeft <= 0)
        yield break;

      var valves = System.Valves;
      if (!ValveOpen.Any(v => !v.Value && valves[v.Key].Rate > 0))
        yield break;

      if (GetMaxRemaining(withElephant) <= currentMaxPressure)
        yield break;

      if (!ValveOpen[Valve.Id] && Valve.Rate > 0)
      {
        var newValveOpen = new Dictionary<string, bool>(ValveOpen)
        {
          [Valve.Id] = true
        };
        var additionaPressure = (TimeLeft - 1) * Valve.Rate;
        if (withElephant)
        {
          if (!newValveOpen[ValveElephant.Id] && ValveElephant.Rate > 0)
          {
            newValveOpen[ValveElephant.Id] = true;
            additionaPressure += (TimeLeft - 1) * ValveElephant.Rate;
            yield return new State(System, Valve, ValveElephant, TimeLeft - 1, newValveOpen, TotalPressure + additionaPressure);
          }
          else
          {
            foreach (var t in ValveElephant.Tunnels)
            {
              yield return new State(System, Valve, System.Valves[t], TimeLeft - 1, newValveOpen, TotalPressure + additionaPressure);
            }
          }
        }
        else
        {
          yield return new State(System, Valve, ValveElephant, TimeLeft - 1, newValveOpen, TotalPressure + additionaPressure);
        }
      }

      foreach (var t in Valve.Tunnels)
      {
        if (withElephant)
        {
          if (!ValveOpen[ValveElephant.Id] && ValveElephant.Rate > 0)
          {
            var newValveOpen = new Dictionary<string, bool>(ValveOpen)
            {
              [ValveElephant.Id] = true
            };

            var additionaPressure = (TimeLeft - 1) * ValveElephant.Rate;
            yield return new State(System, System.Valves[t], ValveElephant, TimeLeft - 1, newValveOpen, TotalPressure + additionaPressure);
          }
          else
          {
            foreach (var tElephant in ValveElephant.Tunnels)
            {
              yield return new State(System, System.Valves[t], System.Valves[tElephant], TimeLeft - 1, ValveOpen, TotalPressure);
            }
          }

        }
        else
        {
          yield return new State(System, System.Valves[t], ValveElephant, TimeLeft - 1, ValveOpen, TotalPressure);
        }
      }
    }
  }

  internal partial record Valve(string Id, int Rate, List<string> Tunnels)
  {
    internal static State GetInitialState(string input, bool withElephant)
    {
      var valves = ParseInput(input, withElephant);
      var valveOpen = new Dictionary<string, bool>();
      foreach (var id in valves.Valves.Keys)
        valveOpen.Add(id, false);

      var timeLeft = withElephant ? 26 : 30;
      return new State(valves, valves.Valves["AA"], valves.Valves["AA"], timeLeft, valveOpen, 0);
    }

    internal static int GetMaxTotalPressure(string input, bool withElephant)
    {
      var initialState = GetInitialState(input, withElephant);
      int bestPressure = initialState.TotalPressure;

      var visitedStates = new SortedSet<State>();

      var openStates = new PriorityQueue<State, int>();
      openStates.Enqueue(initialState, -initialState.TotalPressure);
      visitedStates.Add(initialState);

      while (openStates.TryDequeue(out State currentState, out int _))
      {
        if (currentState.TotalPressure > bestPressure)
          bestPressure = currentState.TotalPressure;

        foreach (var newState in currentState.GetPossibleNextStates(bestPressure, withElephant))
        {
          if (!visitedStates.Contains(newState))
          {
            openStates.Enqueue(newState, -newState.TotalPressure);
            visitedStates.Add(newState);
          }
        }
      }

      return bestPressure;
    }

    internal static System ParseInput(string input, bool withElephant)
    {
      var valves = new Dictionary<string, Valve>();
      foreach (var line in input.Split('\n'))
        if (!string.IsNullOrWhiteSpace(line))
        {
          var valve = ParseLine(line);
          valves.Add(valve.Id, valve);
        }
      return new System(valves, withElephant);
    }

    internal static Valve ParseLine(string line)
    {
      var regEx = RegExValve();
      var match = regEx.Match(line);
      if (match.Success)
      {
        var id = match.Groups["Id"].Value;
        var rate = int.Parse(match.Groups["Rate"].Value);
        var tunnels = new List<string>();
        foreach (var tunnel in match.Groups["Tunnels"].Value.Split(','))
          tunnels.Add(tunnel.Trim());
        return new Valve(id, rate, tunnels);
      }
      throw new ApplicationException("not expected");
    }

    [GeneratedRegex("Valve (?<Id>[A-Z]+) has flow rate=(?<Rate>\\d+); tunnels? leads? to valves? (?<Tunnels>[A-Z]+(, [A-Z]+)*)")]
    private static partial Regex RegExValve();
  }
}