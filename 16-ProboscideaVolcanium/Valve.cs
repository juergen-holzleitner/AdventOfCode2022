using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _16_ProboscideaVolcanium
{
  record System(Dictionary<string, Valve> Valves);

  record struct State(System System, Valve Valve, int TimeLeft, Dictionary<string, bool> ValveOpen, int TotalPressure)
  {
    internal int GetMaxRemaining()
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
      foreach (var r in remaining)
      {
        sum += currentTimeLeft * r;
        currentTimeLeft -= 2;
        if (currentTimeLeft <= 0)
          break;
      }

      return sum;
    }

    internal IEnumerable<State> GetPossibleNextStates(int currentMaxPressure)
    {
      if (TimeLeft <= 0)
        yield break;

      var valves = System.Valves;
      if (!ValveOpen.Any(v => !v.Value && valves[v.Key].Rate > 0))
        yield break;

      if (GetMaxRemaining() <= currentMaxPressure)
        yield break;

      if (!ValveOpen[Valve.Id] && Valve.Rate > 0)
      {
        var newValveOpen = new Dictionary<string, bool>(ValveOpen)
        {
          [Valve.Id] = true
        };
        var additionaPressure = (TimeLeft - 1) * Valve.Rate;
        yield return new State(System, Valve, TimeLeft - 1, newValveOpen, TotalPressure + additionaPressure);
      }

      foreach (var t in Valve.Tunnels)
      {
        yield return new State(System, System.Valves[t], TimeLeft - 1, ValveOpen, TotalPressure);
      }
    }
  }

  internal partial record Valve(string Id, int Rate, List<string> Tunnels)
  {
    internal static State GetInitialState(string input)
    {
      var valves = ParseInput(input);
      var valveOpen = new Dictionary<string, bool>();
      foreach (var id in valves.Valves.Keys)
        valveOpen.Add(id, false);

      return new State(valves, valves.Valves["AA"], 30, valveOpen, 0);
    }

    internal static int GetMaxTotalPressure(string input)
    {
      var initialState = GetInitialState(input);
      int bestPressure = initialState.TotalPressure;
      var openStates = new PriorityQueue<State, int>();
      openStates.Enqueue(initialState, -initialState.TotalPressure);

      while (openStates.TryDequeue(out State currentState, out int _))
      {
        if (currentState.TotalPressure > bestPressure)
          bestPressure = currentState.TotalPressure;

        foreach (var newState in currentState.GetPossibleNextStates(bestPressure))
          openStates.Enqueue(newState, -newState.TotalPressure);
      }

      return bestPressure;
    }

    internal static System ParseInput(string input)
    {
      var valves = new Dictionary<string, Valve>();
      foreach (var line in input.Split('\n'))
        if (!string.IsNullOrWhiteSpace(line))
        {
          var valve = ParseLine(line);
          valves.Add(valve.Id, valve);
        }
      return new System(valves);
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