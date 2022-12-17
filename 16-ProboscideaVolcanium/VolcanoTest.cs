using FluentAssertions;

namespace _16_ProboscideaVolcanium
{
  public class VolcanoTest
  {
    [Fact]
    public void Can_scan_valve()
    {
      var line = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB";

      var valve = Valve.ParseLine(line);

      valve.Id.Should().Be("AA");
      valve.Rate.Should().Be(0);
      valve.Tunnels.Should().BeEquivalentTo(new[] { "DD", "II", "BB" });
    }

    [Fact]
    public void Can_scan_another_valve()
    {
      var line = "Valve HH has flow rate=22; tunnel leads to valve GG";

      var valve = Valve.ParseLine(line);

      valve.Id.Should().Be("HH");
      valve.Rate.Should().Be(22);
      valve.Tunnels.Should().BeEquivalentTo(new[] { "GG" });
    }

    [Fact]
    public void Can_parse_sample()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";

      var state = Valve.ParseInput(input, false);

      state.Valves.Should().HaveCount(10);
    }

    [Fact]
    public void Can_have_initial_state()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, false);

      initialState.Valve.Id.Should().Be("AA");
      initialState.TimeLeft.Should().Be(30);
      initialState.TotalPressure.Should().Be(0);
      foreach (var key in initialState.System.Valves.Keys)
        initialState.ValveOpen[key].Should().BeFalse();
    }

    [Fact]
    public void Can_have_initial_state_with_elephant()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, true);

      initialState.Valve.Id.Should().Be("AA");
      initialState.ValveElephant.Id.Should().Be("AA");
      initialState.TimeLeft.Should().Be(26);
      initialState.TotalPressure.Should().Be(0);
      foreach (var key in initialState.System.Valves.Keys)
        initialState.ValveOpen[key].Should().BeFalse();
    }

    [Fact]
    public void Can_get_possible_next_states()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, false);

      var nextStates = initialState.GetPossibleNextStates(0, false);

      nextStates.Should().HaveCount(3);
      // pressure is 0: nextStates.Should().Contain(s => s.TimeLeft == 29 && s.Valve.Id == "AA" && s.ValveOpen["AA"] == true);
      nextStates.Should().Contain(s => s.TimeLeft == 29 && s.Valve.Id == "DD" && s.ValveOpen.All(s => s.Value == false) && s.TotalPressure == 0);
      nextStates.Should().Contain(s => s.TimeLeft == 29 && s.Valve.Id == "II" && s.ValveOpen.All(s => s.Value == false) && s.TotalPressure == 0);
      nextStates.Should().Contain(s => s.TimeLeft == 29 && s.Valve.Id == "BB" && s.ValveOpen.All(s => s.Value == false) && s.TotalPressure == 0);

      initialState.TimeLeft.Should().Be(30);
      initialState.Valve.Id.Should().Be("AA");
      initialState.ValveOpen.Should().OnlyContain(s => s.Value == false);
      initialState.TotalPressure.Should().Be(0);
    }

    [Fact]
    public void Can_get_possible_next_states_with_elephant()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, false);

      var nextStates = initialState.GetPossibleNextStates(0, true);

      nextStates.Should().HaveCount(9);

      nextStates.Should().Contain(s => s.TimeLeft == 29 && s.Valve.Id == "DD" && s.ValveElephant.Id == "DD" && s.ValveOpen.All(s => s.Value == false) && s.TotalPressure == 0);
      nextStates.Should().Contain(s => s.TimeLeft == 29 && s.Valve.Id == "II" && s.ValveElephant.Id == "DD" && s.ValveOpen.All(s => s.Value == false) && s.TotalPressure == 0);
      nextStates.Should().Contain(s => s.TimeLeft == 29 && s.Valve.Id == "BB" && s.ValveElephant.Id == "DD" && s.ValveOpen.All(s => s.Value == false) && s.TotalPressure == 0);

      nextStates.Should().Contain(s => s.TimeLeft == 29 && s.Valve.Id == "DD" && s.ValveElephant.Id == "II" && s.ValveOpen.All(s => s.Value == false) && s.TotalPressure == 0);
      nextStates.Should().Contain(s => s.TimeLeft == 29 && s.Valve.Id == "II" && s.ValveElephant.Id == "II" && s.ValveOpen.All(s => s.Value == false) && s.TotalPressure == 0);
      nextStates.Should().Contain(s => s.TimeLeft == 29 && s.Valve.Id == "BB" && s.ValveElephant.Id == "II" && s.ValveOpen.All(s => s.Value == false) && s.TotalPressure == 0);

      nextStates.Should().Contain(s => s.TimeLeft == 29 && s.Valve.Id == "DD" && s.ValveElephant.Id == "BB" && s.ValveOpen.All(s => s.Value == false) && s.TotalPressure == 0);
      nextStates.Should().Contain(s => s.TimeLeft == 29 && s.Valve.Id == "II" && s.ValveElephant.Id == "BB" && s.ValveOpen.All(s => s.Value == false) && s.TotalPressure == 0);
      nextStates.Should().Contain(s => s.TimeLeft == 29 && s.Valve.Id == "BB" && s.ValveElephant.Id == "BB" && s.ValveOpen.All(s => s.Value == false) && s.TotalPressure == 0);

      initialState.TimeLeft.Should().Be(30);
      initialState.Valve.Id.Should().Be("AA");
      initialState.ValveElephant.Id.Should().Be("AA");
      initialState.ValveOpen.Should().OnlyContain(s => s.Value == false);
      initialState.TotalPressure.Should().Be(0);
    }

    [Fact]
    public void Dont_get_any_states_if_time_ran_out()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, false);
      initialState.TimeLeft = 0;

      var nextStates = initialState.GetPossibleNextStates(0, false);

      nextStates.Should().BeEmpty();
    }

    [Fact]
    public void Dont_get_any_states_if_time_ran_out_with_Elephant()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, true);
      initialState.TimeLeft = 0;

      var nextStates = initialState.GetPossibleNextStates(0, true);

      nextStates.Should().BeEmpty();
    }

    [Fact]
    public void Dont_get_any_states_if_all_valves_are_open_or_have_no_pressure()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, false);
      foreach (var k in initialState.ValveOpen.Keys)
        if (initialState.System.Valves[k].Rate > 0)
          initialState.ValveOpen[k] = true;

      var nextStates = initialState.GetPossibleNextStates(0, false);

      nextStates.Should().BeEmpty();
    }

    [Fact]
    public void Dont_get_any_states_if_all_valves_are_open_or_have_no_pressure_with_elephant()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, true);
      foreach (var k in initialState.ValveOpen.Keys)
        if (initialState.System.Valves[k].Rate > 0)
          initialState.ValveOpen[k] = true;

      var nextStates = initialState.GetPossibleNextStates(0, true);

      nextStates.Should().BeEmpty();
    }

    [Fact]
    public void Can_calc_total_pressure()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, false);

      var nextStates = initialState.GetPossibleNextStates(0, false);
      nextStates = nextStates.Where(s => s.Valve.Id == "BB").Single().GetPossibleNextStates(0, false);

      nextStates.Should().Contain(s => s.TimeLeft == 28 && s.Valve.Id == "BB" && s.ValveOpen["BB"] == true && s.TotalPressure == 364);
    }

    [Fact]
    public void Can_calc_total_pressure_with_elephant()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, true);

      var nextStates = initialState.GetPossibleNextStates(0, true);
      nextStates = nextStates.Where(s => s.Valve.Id == "BB" && s.ValveElephant.Id == "DD").Single().GetPossibleNextStates(0, true);

      nextStates.Should().Contain(s => s.TimeLeft == 24 && s.ValveOpen["BB"] == true && s.ValveOpen["DD"] == true && s.TotalPressure == 792);
    }

    [Fact]
    public void Can_get_max_possible_remaining()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, false);

      var maxRemaining = initialState.GetMaxRemaining(false);

      maxRemaining.Should().Be(2105);
    }

    [Fact]
    public void Can_get_max_possible_remaining_with_elephant()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, true);

      var maxRemaining = initialState.GetMaxRemaining(true);

      maxRemaining.Should().Be(1939);
    }

    [Fact]
    public void Dont_get_any_if_higher_pressure_is_not_possible()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, false);

      var nextStates = initialState.GetPossibleNextStates(2105, false);

      nextStates.Should().BeEmpty();
    }

    [Fact]
    public void Dont_get_any_if_higher_pressure_is_not_possible_with_elephant()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var initialState = Valve.GetInitialState(input, true);

      var nextStates = initialState.GetPossibleNextStates(1939, true);

      nextStates.Should().BeEmpty();
    }

    [Fact]
    public void Can_get_maximum_total_pressure()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var maxTotalPressure = Valve.GetMaxTotalPressure(input, false);
      maxTotalPressure.Should().Be(1651);
    }

    [Fact]
    public void Can_get_maximum_total_pressure_with_elephant()
    {
      var input = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II\r\n";
      var maxTotalPressure = Valve.GetMaxTotalPressure(input, true);
      maxTotalPressure.Should().Be(1707);
    }

  }
}